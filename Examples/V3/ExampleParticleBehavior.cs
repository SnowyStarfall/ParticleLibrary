using MonoMod.Logs;
using ParticleLibrary.Core.V3.Particles;
using ParticleLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ParticleLibrary.Examples.V3
{
    public class ExampleParticleBehavior : Behavior<ParticleInfo>
	{
		// This is the texture that will be used for your particle.
		public override string Texture { get; } = Resources.Assets.Textures.Star;

		public override void Update(ref ParticleInfo info)
		{
			// Particle Library no longer updates values for you.
			// You must manually update values such as velocity and position.
			// This allows you to optimize for performance by only updating what you need.
			info.Velocity *= 0.99f;
			info.Position += info.Velocity;
			info.Rotation = info.Position.AngleTo(Main.LocalPlayer.position.ToNumerics());

			// This calculates the scalar (0~1) of the particle's lifetime.
			// The value will be 1 at the beginning, then slowly interpolate to 0.
			float mult = info.Time / (float)info.Duration;

			// "Initial" values are read-only and cannot be changed.
			// They only serve as a reference point for things like scaling down size over lifetime.
			info.Color = info.InitialColor * mult;
			info.Scale = info.InitialScale * mult;

			// The particle will be marked inactive when Time <= 0.
			info.Time--;
		}
	}
}
