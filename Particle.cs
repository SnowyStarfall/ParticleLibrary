using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary
{
	/// <summary>
	/// Base class for all particles. Inherit this class to create your own particle.
	/// </summary>
	public class Particle : Entity
	{
		/// <summary>
		/// </summary>
		public Particle()
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
		public override Vector2 VisualPosition => position - Main.screenPosition;
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
		public bool tileCollide = false;
		/// <summary>
		/// The scale of this particle.
		/// </summary>
		public float scale = 1f;
		/// <summary>
		/// The rotation of this particle.
		/// </summary>
		public float rotation = 0f;
		/// <summary>
		/// The opacity of this particle.
		/// </summary>
		public float opacity = 1f;
		/// <summary>
		/// The gravity to apply to this particle's movement.
		/// </summary>
		public float gravity = 0f;
		/// <summary>
		/// The amount of ticks (frames) this particle has left in its lifetime.
		/// </summary>
		public int timeLeft = 0;

		/// <summary>
		/// An array of floats used to pass data to the particle on spawn.
		/// </summary>
		public float[] ai;
		/// <summary>
		/// An array of old positions for this particle. Only used when instantiated in SetDefaults().
		/// </summary>
		public Vector2[] oldPos;
		/// <summary>
		/// An array of old rotations for this particle. Only used when instantiated in SetDefaults().
		/// </summary>
		public float[] oldRot;
		/// <summary>
		/// An array of old centers, taking into account the width and height of this particle. Only used when instantiated in SetDefaults().
		/// </summary>
		public Vector2[] oldCen;
		/// <summary>
		/// An array of old velocities for this particle. Only used when instantiated in SetDefaults().
		/// </summary>
		public Vector2[] oldVel;

		/// <summary>
		/// </summary>
		public virtual void SetDefaults()
		{
		}
		private void SetPrivateDefaults()
		{
			if (texture == null)
			{
				string filePath = Texture == string.Empty || Texture == null ? GetType().Namespace.Replace(".", "/") + "/" + GetType().Name : Texture;
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
		/// <param name="spriteBatch">Provided SpriteBatch.</param>
		/// <param name="drawPos">Draw position of the particle. This factors in Main.screenPosition.</param>
		/// <param name="lightColor">The light color of the tile beneath this particle.</param>
		/// </summary>
		public void Draw(SpriteBatch spriteBatch, Vector2 drawPos, Color lightColor)
		{
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, width, height), Color.Multiply(color != default ? color : lightColor, opacity), rotation, new Vector2(width, height) * 0.5f, scale, SpriteEffects.None, 0f);
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
		/// Kills a particle.
		/// </summary>
		public void Kill() => ParticleManager.particles.Remove(this);
	}
}
