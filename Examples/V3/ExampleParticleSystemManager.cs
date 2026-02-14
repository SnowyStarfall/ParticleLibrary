using Microsoft.Xna.Framework;
using ParticleLibrary.Core;
using ParticleLibrary.Core.V3;
using ParticleLibrary.Core.V3.Particles;
using System;
using Terraria;
using Terraria.ModLoader;
using SystemVector2 = System.Numerics.Vector2;

namespace ParticleLibrary.Examples.V3
{
	internal class ExampleParticleSystemManagerV3 : ModSystem
	{
		internal static ParticleBuffer<ExampleParticleBehavior> ExampleParticleBuffer { get; private set; }

		private static ParticleBuffer<ExampleDataParticleBehavior> _dataParticleBuffer;

		private static ParticleCollection _exampleMultiLayerParticleBuffer;

		public override bool IsLoadingEnabled(Mod mod)
		{
			// Never create particle buffers on the server.
			// Also, only create example stuff if we're debugging.
			return !Main.dedServ && ParticleLibrary.Debug;
		}

		public override void OnModLoad()
		{
			// Instantiate our buffers with a max particle size of 512.
			// Remember, the smaller your buffer, the less memory it uses.
			// It's important that you choose a buffer size that isn't too large.
			// Think about how many particles you expect to have at any given
			// moment and make that the size your buffer.

			ExampleParticleBuffer = new(256);
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

			_dataParticleBuffer = new(256);
			ParticleManagerV3.RegisterUpdatable(_dataParticleBuffer);
			ParticleManagerV3.RegisterRenderable(Layer.BeforeSolidTiles, _dataParticleBuffer);

			_exampleMultiLayerParticleBuffer = new();
			foreach (Layer value in Enum.GetValuesAsUnderlyingType<Layer>())
			{
				_exampleMultiLayerParticleBuffer.Add(
					new ParticleBuffer<ExampleParticleBehavior>(32),
					value
				);
			}
		}

		public override void PostUpdatePlayers()
		{
			// You may notice that I'm using ToNumerics(). This is because particles use System.Numerics vectors.
			// System.Numerics vectors use SIMD, or Single Instruction, Multiple Data. They are more performant because of this.
			// The utility to convert from XNA vectors is in LibUtilities for you to use.

			//for (int i = 0; i < 2; i++)
			//{
			//	ExampleParticleBuffer.Create(new ParticleInfo(
			//		position: Main.LocalPlayer.position.ToNumerics(),
			//		velocity: (Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 4f + float.Epsilon)).ToNumerics(),
			//		rotation: Main.GlobalTimeWrappedHourly,
			//		scale: new Vector2(64f, 16f).ToNumerics(),
			//		depth: 1f,
			//		color: new Color(1f, 1f, 1f, 0f),
			//		duration: 120
			//	));
			//}

			//for (int i = 0; i < 2; i++)
			//{
			//	CreateDataParticle(
			//		position: Main.LocalPlayer.position.ToNumerics(),
			//		velocity: (Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 4f + float.Epsilon)).ToNumerics(),
			//		rotation: Main.GlobalTimeWrappedHourly,
			//		scale: new Vector2(64f, 16f).ToNumerics(),
			//		depth: 1f,
			//		color: new Color(175, 137, 241, 0),
			//		duration: 120,
			//		myOtherColor: new Color(107, 87, 210, 0)
			//	);
			//}

			//var buffers = _exampleMultiLayerParticleBuffer.ParticleBuffers;
			//var values = Enum.GetValuesAsUnderlyingType<Layer>();

			//buffers[14].Create(new ParticleInfo(
			//	position: Main.LocalPlayer.position.ToNumerics(),
			//	velocity: Vector2.Zero.ToNumerics(),
			//	rotation: Main.GlobalTimeWrappedHourly,
			//	scale: new Vector2(64f, 64f).ToNumerics(),
			//	depth: 1f,
			//	color: new Color(1f, 1f, 1f, 0f),
			//	duration: 120
			//));

			//for (int i = 0; i < buffers.Count; i++)
			//{
			//	if (i == 14)
			//	{
			//		for (int k = 0; k < 8; k++)
			//		{
			//			buffers[i].Create(new ParticleInfo(
			//				position: Main.LocalPlayer.position.ToNumerics(),
			//				velocity: Main.rand.NextVector2Unit().ToNumerics() * 0f,
			//				rotation: Main.GlobalTimeWrappedHourly,
			//				scale: new Vector2(64f, 64f).ToNumerics(),
			//				depth: 1f,
			//				color: new Color(1f, 1f, 1f, 0f),
			//				duration: 120
			//			));
			//		}
			//	}
			//}
		}

		// We create this method to expose access to _dataParticleBuffer's Create() method.
		// Here, we can assure that the particle's Data field is always instantiated and always
		// has values. This ensure our code will never throw a null reference and will prevent
		// unexpected behavior.
		internal static void CreateDataParticle(SystemVector2 position, SystemVector2 velocity, float rotation, SystemVector2 scale, float depth, Color color, int duration, Color myOtherColor)
		{
			// Never create particles on the server.
			if (Main.dedServ)
			{
				return;
			}

			// Here we demonstrate how to efficiently pass a second color into the particle.
			// To understand what we're doing, we only need to understand a bit about bits.
			// A color is made up of 4 bytes: R, G, B, and A. Each byte is 8 bits.
			// A float is a 32-bit value type. This means it's made up of 4 bytes.
			// Since a Color is exactly 32 bits in size like a Color, we can store a Color into a float's data structure.
			// We use the BitConverter here for ease of conversion. Color already contains a
			// PackedValue uint32 property that contains each color channel stored in its 32 bits.
			// All we have to do is convert this to a single (float) and pass it in as one of our data values.
			float otherColor = BitConverter.UInt32BitsToSingle(myOtherColor.PackedValue);

			_dataParticleBuffer.Create(new ParticleInfo(position, velocity, rotation, scale, depth, color, duration, otherColor));
		}
	}
}
