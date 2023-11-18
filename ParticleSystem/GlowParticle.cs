using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using static ParticleLibrary.ParticleManager;

namespace ParticleLibrary.ParticleSystem
{
	[Obsolete("This type is obsolete")]
	public class GlowParticle : Particle
	{
		public override void SetDefaults()
		{
			width = 1;
			height = 1;
			timeLeft = 300;
			tileCollide = true;
			SpawnAction = Spawn;
			gravity = 0f;
		}
		public override void AI()
		{
			Scale = timeLeft / 300f;
		}
		public void Spawn()
		{
			Scale *= 0.125f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 drawPos, Color lightColor)
		{
			spriteBatch.Draw(texture, layer == Layer.AfterMainMenu ? position : drawPos, new Rectangle(0, 0, 128, 128), new Color(1f, 1f, 1f, 0f) * Scale, rotation, new Vector2(64, 64), 0.1f * Scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void TileCollision(Vector2 oldVelocity)
		{
		}
	}
}
