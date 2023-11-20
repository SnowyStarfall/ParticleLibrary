
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary.Examples
{
	/// <summary>
	/// This class demonstrates creating a custom <see cref="Core.Particle"/>
	/// In the 2.0 API, <see cref="Core.Particle"/>s recieved a major rewrite and became much more efficient.
	/// This means that a lot of familiar things have changed.
	/// <see cref="Core.Particle.Spawn"/> used to be Initialize and SpawnAction, but it's now condensed into one lifecycle method
	/// <see cref="Core.Particle.Update"/> used to be AI, but has since been renamed
	/// <see cref="Core.Particle.Draw(SpriteBatch, Vector2)"/> used to be PreDraw, Draw, and PostDraw, but has since been condensed and had its parameters simplified to only the necessities
	/// <see cref="Core.Particle.Death"/> used to be DeathAction, but is now overridable instead of being a field
	/// </summary>
	public class ExampleParticle : Core.Particle
	{
		/// <summary>
		/// You can override the default texture fetching just like with items.
		/// The <see cref="Resources"/> class is created by Resources.tt, which is a tool I borrowed from Nez, a Monogame library
		/// I customized it and made it more suited for Terraria modding. It autogenerates string paths to resources in your project.
		/// The tool recreates the paths at compile time, meaning you will never have an incorrect path on accident.
		/// See: <see href="https://github.com/prime31/Nez/blob/master/FAQs/ContentManagement.md#auto-generating-content-paths"/>
		/// </summary>
		public override string Texture => Resources.Examples.ExampleParticle;
		//public override string Texture => "ParticleLibrary/Examples/ExampleParticle";


		/// <summary>
		/// Runs when the particle is created
		/// </summary>
		public override void Spawn()
		{
			TimeLeft = 120;
			Scale *= 0.125f;
		}

		/// <summary>
		/// Runs every full frame (tick)
		/// </summary>
		public override void Update()
		{
			Scale = (120 - TimeLeft) / 120;
			Velocity *= 0.96f;
		}

		/// <summary>
		/// Runs every draw frame (interval depends on <see cref="Main.FrameSkipMode"/>)
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to use.</param>
		/// <param name="location">The visual location, already taking into account <see cref="Main.screenPosition"/></param>
		public override void Draw(SpriteBatch spriteBatch, Vector2 location)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			spriteBatch.Draw(texture, location, texture.Bounds, new Color(0.05f, 0f, 0.1f, 0f), 0f, texture.Size() * 0.5f, 0.1f, SpriteEffects.None, 0f);
		}

		/// <summary>
		/// Runs when <see cref="Core.Particle.TimeLeft"/> reaches 0 or when <see cref="Core.Particle.Kill"/> is called.
		/// </summary>
		public override void Death()
		{
		}
	}
}
