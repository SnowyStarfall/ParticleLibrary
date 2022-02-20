using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary
{
	public class Particle : Entity
	{
		public Particle()
		{
			SetDefaults();
			SetPrivateDefaults();
		}
		public virtual string Texture { get; set; }
		public override Vector2 VisualPosition => position - Main.screenPosition;

		public Color color;
		public Rectangle frame;
		public Texture2D texture;
		public Action SpawnAction;
		public Action DeathAction;

		public bool tileCollide = false;
		public float scale = 1f;
		public float rotation = 0f;
		public float opacity = 1f;
		public float gravity = 0f;
		public int timeLeft = 0;

		public float[] ai;
		public Vector2[] oldPos;
		public float[] oldRot;
		public Vector2[] oldCen;
		public Vector2[] oldVel;

		public virtual void SetDefaults()
		{
		}
		private void SetPrivateDefaults()
		{
			if (texture == null)
			{
				string filePath = Texture == string.Empty || Texture == null ? GetType().Namespace.Replace(".", "/") + "/" + GetType().Name : Texture;
				texture = ModContent.Request<Texture2D>(filePath).Value;
				if (texture == null)
					throw new NullReferenceException($"Texture was null for {GetType().Name}.");
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
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, width, height), color != default ? color : lightColor, rotation, new Vector2(width, height) * 0.5f, scale, SpriteEffects.None, 0f);
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
