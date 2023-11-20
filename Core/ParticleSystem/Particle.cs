using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// Base class for all particles. Inherit this class to create your own particle.
	/// </summary>
	public abstract class Particle
	{
		protected Particle()
		{
			Spawn();
			PrivateSpawn();
		}

		/// <summary>
		/// Texture path for a particle. Override this to specify a custom path.
		/// </summary>
		public virtual string Texture => GetType().Namespace.Replace(".", "/") + "/" + GetType().Name;

		/// <summary>
		/// The expected visual bounds for this particle. Used for visual culling.
		/// X and Y are used for position offset. Width and Height are the size of the bounds.
		/// Defaults to null. The particle will never be culled.
		/// </summary>
		public virtual Rectangle? Bounds { get; set; }

		/// <summary>
		/// The visual position taking into account Main.screenPosition;
		/// </summary>
		public Vector2 VisualPosition => AnchorPosition + Position - Main.screenPosition;

		/// <summary>
		/// The location of the particle.
		/// </summary>
		public Vector2 Position;
		/// <summary>
		/// How fast <see cref="Position"/> in world coordinates per frame.
		/// </summary>
		public Vector2 Velocity;
		/// <summary>
		/// How much <see cref="Velocity"/> changes each frame.
		/// </summary>
		public Vector2 VelocityAcceleration;
		/// <summary>
		/// The reference position used for this particle when calculating its position. Defaults to (0, 0).
		/// </summary>
		public Vector2 AnchorPosition;

		public float Scale { get => Scale2D.X; set => Scale2D = new Vector2(value, value); }
		/// <summary>
		/// The scale of this particle.
		/// </summary>
		public Vector2 Scale2D;
		/// <summary>
		/// How much <see cref="Scale2D"/> increases each frame.
		/// </summary>
		public Vector2 ScaleVelocity;
		/// <summary>
		/// How much <see cref="ScaleVelocity"/> increases each frame.
		/// </summary>
		public Vector2 ScaleAcceleration;

		/// <summary>
		/// The rotation of this particle.
		/// </summary>
		public float Rotation;
		/// <summary>
		/// How much <see cref="Rotation"/> changes each frame.
		/// </summary>
		public float RotationVelocity;
		/// <summary>
		/// How much <see cref="RotationVelocity"/> changes each frame.
		/// </summary>
		public float RotationAcceleration;

		/// <summary>
		/// Where the particle should be drawn.
		/// </summary>
		public Layer Layer = Layer.BeforeDust;
		/// <summary>
		/// The color of this particle.
		/// </summary>
		public Color Color;
		/// <summary>
		/// The frame of this particle.
		/// </summary>
		public Rectangle Frame;
		/// <summary>
		/// Whether this particle should collide with tiles.
		/// </summary>
		public bool TileCollide;
		/// <summary>
		/// The amount of frames this particle has left in its lifetime.
		/// </summary>
		public int TimeLeft;

		private Texture2D _texture;


		private void PrivateSpawn()
		{
			if (Main.netMode is not NetmodeID.Server && _texture is null)
			{
				try { _texture = ModContent.Request<Texture2D>(Texture).Value; }
				catch { _texture = ParticleLibrary.EmptyPixel; }

				if (!Bounds.HasValue && _texture is not null)
				{
					Bounds = new Rectangle(0, 0, _texture.Width, _texture.Height);
				}

			}
		}

		/// <summary>
		/// Runs when the particle spawns.
		/// </summary>
		public virtual void Spawn()
		{
		}

		/// <summary>
		/// Runs when the particle is updated.
		/// </summary>
		public virtual void Update()
		{
		}

		/// <summary>
		/// Renders the particle.
		/// </summary>
		/// <param name="spriteBatch">Provided SpriteBatch.</param>
		/// <param name="location">Draw position of the particle. This factors in Main.screenPosition.</param>
		/// <returns>bool</returns>
		public virtual void Draw(SpriteBatch spriteBatch, Vector2 location)
		{
			spriteBatch.Draw(_texture, location, Frame, Color, Rotation, _texture.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
		}

		/// <summary>
		/// Runs when the particle dies.
		/// </summary>
		public virtual void Death()
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
		public void Kill()
		{
			Death();
			NewParticleManager._particlesToRemove?.Add(this);
			NewParticleManager.ParticleCount--;
		}
	}
}
