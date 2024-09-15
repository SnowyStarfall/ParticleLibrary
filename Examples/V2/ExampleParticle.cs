
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;
using Terraria;

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
		public override string Texture => Resources.Examples.V2.ExampleParticle;
		//public override string Texture => "ParticleLibrary/Examples/ExampleParticle";

		/// <summary>
		/// You can override the bounds to skip draw calls for this particle if it wouldn't be visible.
		/// Currently, the library already calculates whether these bounds would intersect the player's vision,
		/// This means you only need to specify where the particle is and how much space it's using.
		/// Alternatively, you can leave this as null to prevent culling altogether.
		/// You can also set it to something like Rectangle.Empty to prevent the particle from being drawn.
		/// </summary>
		public override Rectangle? Bounds => new Rectangle((int)Position.X - Sprite.Width / 2, (int)Position.Y - Sprite.Height / 2, Sprite.Width, Sprite.Height);

		public float VelocityMult { get; init; }

		/// <summary>
		/// With the removal of the AI array, you can no longer pass data into a particle via <see cref="Core.ParticleSystem.NewParticle(Vector2, Vector2, Core.Particle, Color, Vector2, Layer)"/>
		/// Instead, with recent fixes, you can instantiate the particle's constructor and pass it in that way. This way, code is much more readable.
		/// <para>
		/// NOTE: If you plan on allowing your particle to be automatically instantiated with the NewParticle(T) methods, you MUST have a parameterless constructor
		/// </para>
		/// </summary>
		/// <param name="timeLeft"></param>
		/// <param name="velocityMult"></param>
		public ExampleParticle(int timeLeft, float velocityMult = 0.99f)
		{
			TimeLeft = timeLeft;
			VelocityMult = velocityMult;
			VelocityAcceleration.X = VelocityMult;
			VelocityAcceleration.Y = VelocityMult;
		}
		/// <summary>
		/// This parameterless constructor allows us to use our particle in the NewParticle(T) methods without errors
		/// It's a good idea to provide default values for your parameter constructor unless it's not necessary
		/// </summary>
		public ExampleParticle() : this(120, 0.99f) { }

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
			Scale = TimeLeft / 120f;
		}

		/// <summary>
		/// Runs every draw frame (interval depends on <see cref="Main.FrameSkipMode"/>)
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to use.</param>
		/// <param name="location">The visual location, already taking into account <see cref="Main.screenPosition"/></param>
		public override void Draw(SpriteBatch spriteBatch, Vector2 location)
		{
			spriteBatch.Draw(Sprite, location, Sprite.Bounds, Color, 0f, Sprite.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
		}

		/// <summary>
		/// Runs when <see cref="Core.Particle.TimeLeft"/> reaches 0 or when <see cref="Core.Particle.Kill"/> is called.
		/// </summary>
		public override void Death()
		{
		}
	}
}
