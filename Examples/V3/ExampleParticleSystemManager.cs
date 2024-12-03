using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;
using ParticleLibrary.Core.V3;
using ParticleLibrary.Core.V3.Particles;
using ParticleLibrary.Utilities;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ParticleLibrary.Examples.V3
{
    public class ExampleParticleSystemManagerV3 : ModSystem
	{
		public static ParticleBuffer<ExampleParticleBehavior> ExampleParticleBuffer { get; private set; }

		public override void OnModLoad()
		{
			// Never create particle buffers on the server
			if (Main.netMode is NetmodeID.Server)
			{
				return;
			}

			// Instantiate our buffer with a max particle size of 512.
			// Remember, the smaller your buffer, the less memory it uses.
			// It's important that you choose a buffer size that isn't too large.
			// Think about how many particles you expect to have at any given
			// moment and make that the size your buffer.
			ExampleParticleBuffer = new(512);
			ParticleManagerV3.RegisterUpdatable(ExampleParticleBuffer);
			ParticleManagerV3.RegisterRenderable(Layer.BeforeSolidTiles, ExampleParticleBuffer);
		}

		public override void PostUpdatePlayers()
		{
			// You may notice that I'm using ToNumerics(). This is because particles use System.Numerics vectors.
			// System.Numerics vectors use SIMD, or Single Instruction, Multiple Data. They are more performant because of this.
			// The utility to convert from XNA vectors is in LibUtilities for you to use.
			for (int i = 0; i < 3; i++)
			{
				ExampleParticleBuffer.Create(new ParticleInfo(
					position: Main.LocalPlayer.position.ToNumerics(),
					velocity: (Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 4f + float.Epsilon)).ToNumerics(),
					rotation: Main.rand.NextFloat(0f, MathF.Tau + float.Epsilon),
					scale: new Vector2(32f).ToNumerics(),
					color: new Color(1f,1f,1f,0f),
					duration: 120
				));
			}
		}
	}
}
