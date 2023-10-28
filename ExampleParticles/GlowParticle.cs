
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary.ExampleParticles
{
	public class GlowParticle : CParticle
	{
		public override void Spawn()
		{
			TimeLeft = 120;
			Scale *= 0.125f;
		}

		public override void Update()
		{
			Scale = (120 - TimeLeft) / 120;
			Velocity *= 0.96f;
		}

		public override void Draw(SpriteBatch spriteBatch, Vector2 location)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			spriteBatch.Draw(texture, location, texture.Bounds, new Color(0.05f, 0f, 0.1f, 0f), 0f, texture.Size() * 0.5f, 0.1f, SpriteEffects.None, 0f);
		}
	}
}
