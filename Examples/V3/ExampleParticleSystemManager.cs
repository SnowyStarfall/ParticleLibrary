using Microsoft.Xna.Framework;
using ParticleLibrary.Core;
using ParticleLibrary.Core.V3;
using ParticleLibrary.Core.V3.Particles;
using ParticleLibrary.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SystemVector2 = System.Numerics.Vector2;

namespace ParticleLibrary.Examples.V3
{
    public class ExampleParticleSystemManagerV3 : ModSystem
	{
		public static ParticleBuffer<ExampleParticleBehavior> ExampleParticleBuffer { get; private set; }

		private static ParticleBuffer<ExampleParticleBehavior> _dataParticleBuffer;

		public override void OnModLoad()
		{
			// Never create particle buffers on the server
			if (Main.netMode is NetmodeID.Server)
			{
				return;
			}

			// Instantiate our buffers with a max particle size of 512.
			// Remember, the smaller your buffer, the less memory it uses.
			// It's important that you choose a buffer size that isn't too large.
			// Think about how many particles you expect to have at any given
			// moment and make that the size your buffer.

			ExampleParticleBuffer = new(512);
			ParticleManagerV3.RegisterUpdatable(ExampleParticleBuffer);
			ParticleManagerV3.RegisterRenderable(Layer.BeforeSolidTiles, ExampleParticleBuffer);

			// ParticleInfo has an undeclared field of an array of floats named Data.
			// We'll use _dataParticleBuffer to showcase how we can safely handle
			// particles that depend on passing in data to determine their behavior.

			// Firstly, we ensure that the buffer field is private. This is to
			// prevent unwanted direct access to the buffer's Create() method, as
			// that will effectively bypass our assumptions that Data will always have
			// a value and will never be null.

			// Now we continue in CreateDataParticle().

			_dataParticleBuffer = new(512);
			ParticleManagerV3.RegisterUpdatable(_dataParticleBuffer);
			ParticleManagerV3.RegisterRenderable(Layer.BeforeSolidTiles, _dataParticleBuffer);
		}

		public override void PostUpdatePlayers()
		{
			// You may notice that I'm using ToNumerics(). This is because particles use System.Numerics vectors.
			// System.Numerics vectors use SIMD, or Single Instruction, Multiple Data. They are more performant because of this.
			// The utility to convert from XNA vectors is in LibUtilities for you to use.

			//for (int i = 0; i < 3; i++)
			//{
			//	ExampleParticleBuffer.Create(new ParticleInfo(
			//		position: Main.LocalPlayer.position.ToNumerics(),
			//		velocity: (Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 4f + float.Epsilon)).ToNumerics(),
			//		rotation: Main.rand.NextFloat(0f, MathF.Tau + float.Epsilon),
			//		scale: new Vector2(32f).ToNumerics(),
			//		color: new Color(1f, 1f, 1f, 0f),
			//		duration: 120
			//	));
			//}
		}

		// We create this method to expose access to _dataParticleBuffer's Create() method.
		// Here, we can assure that the particle's Data field is always instantiated and always
		// has values. This ensure our code will never throw a null reference and will prevent
		// unexpected behavior.
		public static void CreateDataParticle(in SystemVector2 position, in SystemVector2 velocity, float rotation, in SystemVector2 scale, in Color color, int duration, float myFirstValue, float mySecondValue, float myThirdValue, float myFourthValue)
		{
			_dataParticleBuffer.Create(new ParticleInfo(position, velocity, rotation, scale, color, duration, myFirstValue, mySecondValue, myThirdValue, myFourthValue));
		}
	}
}
