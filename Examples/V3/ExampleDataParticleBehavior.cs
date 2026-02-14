using Microsoft.Xna.Framework;
using ParticleLibrary.Core.V3.Particles;
using ParticleLibrary.Utilities;
using System;
using Terraria;

namespace ParticleLibrary.Examples.V3
{
    public class ExampleDataParticleBehavior : Behavior<ParticleInfo>
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

			// Here we retrieve the color we packed into our data array.
			// This may look a little confusing at first, but the underlying logic is quite simple.

			// First, we once more use the BitConverter to convert our color from float to uint.
			// Then, since Color doesn't have a constructor to do this for us, we perform some
			// bit logic to extract each of the color channels from the uint.

			// As a quick explainer: binary reads from right to left. The first bit starts
			// at 1, and each consecutive bit is a power of 2.
			// The operator ">>" means to shift all of the bits to the right by some amount.
			// The operator "&" is the logical AND operator. It operates on two different values and
			// retrieves only active bits (1s) that have the same index between the two values.
			// Example: (1111 1010) & (0110 1001) = (0110 1000).
			// The value "0xFF" is hexadecimal for byte.MaxValue, or 255, with all bits active.

			// Keep in mind that Color stores its channels in ABGR format.
			// With all of that in mind, the logic is simple:
			// For R, we simply take the lowest 8 bits of the uint.
			// For G, we shift to the right by 8 bits (skipping over R), and then take the lowest 8 bits.
			// For B, we shift to the right by 16 bits (skipping over RG), and then take the lowest 8 bits.
			// For A, we shift to the right by 24 bits (skipping over RGB), and then take the lowest 8 bits.
			// Now we have our color values and have created our color.
			// Whew...
			// Now that we understand this, you can either use the below code or the utility
			// function provided in LibUtilities. It accepts either a uint or a float.

			//uint packed = BitConverter.SingleToUInt32Bits(info.Data[0]);
			//var otherColor = new Color(
			//  r: (byte)(packed & 0xFF),
			//  g: (byte)((packed >> 8) & 0xFF),
			//  b: (byte)((packed >> 16) & 0xFF),
			//  alpha: (byte)((packed >> 24) & 0xFF)
			//);

			var otherColor = LibUtilities.FromPackedValue(info.Data[0]);

			// "Initial" values are read-only and cannot be changed.
			// They only serve as a reference point for things like scaling down size over lifetime.
			info.Color = Color.Lerp(info.InitialColor, otherColor, mult);
			info.Scale = info.InitialScale * mult;

			// The particle will be marked inactive when Time <= 0.
			info.Time--;
		}
	}
}
