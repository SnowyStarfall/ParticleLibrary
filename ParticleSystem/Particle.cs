using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using static ParticleLibrary.ParticleManager;

namespace ParticleLibrary
{
	/// <summary>
	/// Base class for all particles. Inherit this class to create your own particle.
	/// </summary>
	public abstract class Particle : Entity
	{
		protected Particle()
		{
			SetDefaults();
			SetPrivateDefaults();
		}

		/// <summary>
		/// Texture path for a particle. Override this to specify a custom path.
		/// </summary>
		public virtual string Texture { get; set; }
		/// <summary>
		/// The visual position taking into account Main.screenPosition;
		/// </summary>
		public override Vector2 VisualPosition => AnchorPosition + position - Main.screenPosition;

		public enum Layer
		{
			/// <summary>
			/// ** (Broken) ** The surface background. You will have to do parallaxing yourself for now.
			/// </summary>
			BeforeSurfaceBackground,
			/// <summary>
			/// Walls.
			/// </summary>
			BeforeWalls,
			/// <summary>
			/// Trees, flowers, rocks, etc.
			/// </summary>
			BeforeNonSolidTiles,
			/// <summary>
			/// Worm enemies.
			/// </summary>
			BeforeNPCsBehindTiles,
			/// <summary>
			/// Tiles.
			/// </summary>
			BeforeTiles,
			/// <summary>
			/// Player details drawn behind NPCs.
			/// </summary>
			BeforePlayersBehindNPCs,
			/// <summary>
			/// NPCs.
			/// </summary>
			BeforeNPCs,
			/// <summary>
			/// Projectiles.
			/// </summary>
			BeforeProjectiles,
			/// <summary>
			/// Players.
			/// </summary>
			BeforePlayers,
			/// <summary>
			/// Items dropped in world.
			/// </summary>
			BeforeItems,
			/// <summary>
			/// Rain.
			/// </summary>
			BeforeRain,
			/// <summary>
			/// Gore.
			/// </summary>
			BeforeGore,
			/// <summary>
			/// Dust.
			/// </summary>
			BeforeDust,
			/// <summary>
			/// Water. Adjust draw position by new Vector2(Main.offScreenRange, Main.offScreenRange).
			/// </summary>
			BeforeWater,
			/// <summary>
			/// Before UI.
			/// </summary>
			BeforeUI,
			/// <summary>
			/// After UI.
			/// </summary>
			AfterUI,
			/// <summary>
			/// Before Main Menu.
			/// </summary>
			BeforeMainMenu,
			/// <summary>
			/// After Main Menu.
			/// </summary>
			AfterMainMenu,
		}
		/// <summary>
		/// Where the particle should be drawn.
		/// </summary>
		public Layer layer = Layer.BeforeDust;
		/// <summary>
		/// The reference position used for this particle when calculating its position. Defaults to (0, 0).
		/// </summary>
		public Vector2 AnchorPosition;
		/// <summary>
		/// The color of this particle.
		/// </summary>
		public Color color;
		/// <summary>
		/// The frame of this particle.
		/// </summary>
		public Rectangle frame;
		/// <summary>
		/// The texture for this particle.
		/// </summary>
		public Texture2D texture;
		/// <summary>
		/// Method to run when this particle is spawned.
		/// </summary>
		public Action SpawnAction;
		/// <summary>
		/// Method to run when this particle is despawned.
		/// </summary>
		public Action DeathAction;

		/// <summary>
		/// Whether this particle should collide with tiles.
		/// </summary>
		public bool tileCollide;
		/// <summary>
		/// The scale of this particle.
		/// </summary>
		public float Scale { get => scale.X; set => scale = new Vector2(value, value); }
		public Vector2 scale;
		/// <summary>
		/// How much the scale increases each frame.
		/// </summary>
		public Vector2 scaleVelocity;
		/// <summary>
		/// How much the scale velocity increases each frame.
		/// </summary>
		public Vector2 scaleAcceleration;
		/// <summary>
		/// The rotation of this particle.
		/// </summary>
		public float rotation;
		/// <summary>
		/// How much the rotation changes each frame.
		/// </summary>
		public float rotationVelocity;
		/// <summary>
		/// How much the rotation velocity changes each frame.
		/// </summary>
		public float rotationAcceleration;
		/// <summary>
		/// How much the velocity changes each frame.
		/// </summary>
		public Vector2 velocityAcceleration;
		/// <summary>
		/// The opacity of this particle.
		/// </summary>
		public float opacity = 1f;
		/// <summary>
		/// The gravity to apply to this particle's movement.
		/// </summary>
		public float gravity;
		/// <summary>
		/// The amount of ticks (frames) this particle has left in its lifetime.
		/// </summary>
		public int timeLeft;

		/// <summary>
		/// An array of floats used to pass data to the particle on spawn.
		/// </summary>
		public float[] ai;
		/// <summary>
		/// An array of old positions for this particle. Only used when instantiated.
		/// </summary>
		public Vector2[] oldPos;
		/// <summary>
		/// An array of old rotations for this particle. Only used when instantiated.
		/// </summary>
		public float[] oldRot;
		/// <summary>
		/// An array of old centers, taking into account the width and height of this particle. Only used when instantiated.
		/// </summary>
		public Vector2[] oldCen;
		/// <summary>
		/// An array of old velocities for this particle. Only used when instantiated.
		/// </summary>
		public Vector2[] oldVel;

		public virtual void SetDefaults()
		{
		}

		private void SetPrivateDefaults()
		{
			if (texture == null)
			{
				string filePath = Texture?.Length == 0 || Texture == null ? GetType().Namespace.Replace(".", "/") + "/" + GetType().Name : Texture;
				try { texture = ModContent.Request<Texture2D>(filePath).Value; }
				catch (Exception) { texture = ParticleLibrary.EmptyPixel; }
			}
		}

		/// <summary>
		/// This method runs before the particle is updated.
		/// </summary>
		public virtual void PreAI()
		{
		}

		/// <summary>
		/// This method runs as the particle is updated.
		/// </summary>
		public virtual void AI()
		{
		}

		/// <summary>
		/// This method runs after the particle is updated.
		/// </summary>
		public virtual void PostAI()
		{
		}

		/// <summary>
		/// This method runs before Draw. Return false to keep the Particle Manager from drawing your particle.
		/// </summary>
		/// <param name="spriteBatch">Provided SpriteBatch.</param>
		/// <param name="drawPos">Draw position of the particle. This factors in Main.screenPosition.</param>
		/// <param name="lightColor">The light color of the tile beneath this particle.</param>
		/// <returns>bool</returns>
		public virtual bool PreDraw(SpriteBatch spriteBatch, Vector2 drawPos, Color lightColor)
		{
			return true;
		}

		/// <summary>
		/// This method runs if PreDraw returns true.
		/// </summary>
		public void Draw(SpriteBatch spriteBatch, Vector2 drawPos, Color lightColor)
		{
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, width, height), Color.Multiply(color != default ? color : lightColor, opacity), rotation, new Vector2(width, height) * 0.5f, Scale, SpriteEffects.None, 0f);
		}

		/// <summary>
		/// This method runs after Draw is called.
		/// <param name="spriteBatch">Provided SpriteBatch.</param>
		/// <param name="drawPos">Draw position of the particle. This factors in Main.screenPosition.</param>
		/// <param name="lightColor">The light color of the tile beneath this particle.</param>
		/// </summary>
		public virtual void PostDraw(SpriteBatch spriteBatch, Vector2 drawPos, Color lightColor)
		{
		}

		/// <summary>
		/// Runs when the particle collides with a tile.
		/// </summary>
		/// <param name="oldVelocity">The old velocity of the particle.</param>
		public virtual void TileCollision(Vector2 oldVelocity)
		{
		}

		/// <summary>
		/// Kills a particle.
		/// </summary>
		public void Kill(bool deathAction = true)
		{
			if (deathAction)
				DeathAction?.Invoke();
			ParticleManager.particles?.Remove(this);
		}
	}
}
