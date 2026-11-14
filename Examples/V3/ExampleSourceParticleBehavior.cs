using System;
using Microsoft.Xna.Framework;
using ParticleLibrary.Core.V3.Particles;
using Terraria;
using Rectangle = ParticleLibrary.Core.V3.Rectangle;

namespace ParticleLibrary.Examples.V3;

public class ExampleSourceParticleBehavior : Behavior<SourceParticleInfo>
{
	public override string Texture => Assets.Textures.Star.KEY;

	public override void Update(ref SourceParticleInfo info)
	{
		// Particle Library no longer updates values for you.
		// You must manually update values such as velocity and position.
		// This allows you to optimize for performance by only updating what you need.
		info.Velocity *= 0.99f;
		info.Position += info.Velocity;

		// This calculates the scalar (0~1) of the particle's lifetime.
		// The value will be 1 at the beginning, then slowly interpolate to 0.
		float mult = info.Time / (float)info.Duration;

		// This adjusts the source rectangle to sample only a specific region
		// of the provided texture, like how SpriteBatch handles source rectangles.
		Vector2 size = Assets.Textures.Star.Asset.Value.Size();
		float cos = MathF.Cos(Main.GlobalTimeWrappedHourly + 1f) * 0.5f;
		float sin = MathF.Sin(Main.GlobalTimeWrappedHourly + 1f) * 0.5f;
		info.Source = new Rectangle(
			x: size.X / 2f * cos,
			y: size.Y / 2f * sin,
			width: size.X / 2f + size.X / 2f * cos,
			height: size.Y / 2f + size.Y / 2f * sin
		);
		
		// "Initial" values are read-only and cannot be changed.
		// They only serve as a reference point for things like scaling down size over lifetime.
		info.Color = info.InitialColor * mult;
		info.Scale = info.InitialScale * mult;

		// The particle will be marked inactive when Time <= 0.
		info.Time--;
	}
}