using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;
using ParticleLibrary.Core.V3;
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
			if (Main.netMode is NetmodeID.Server)
			{
				return;
			}

			ExampleParticleBuffer = new(262144);
			ParticleManagerV3.RegisterUpdatable(ExampleParticleBuffer);
			ParticleManagerV3.RegisterRenderable(Layer.BeforeSolidTiles, ExampleParticleBuffer);
		}

		public override void PostUpdatePlayers()
		{
			for (int i = 0; i < 2048; i++)
			{
				ExampleParticleBuffer.Create(new ParticleInfo(
					position: Main.LocalPlayer.position,
					velocity: Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 4f + float.Epsilon),
					rotation: MathF.PI / 4f,
					scale: new Vector2(32f),
					color: new Color(1f,1f,1f,0f),
					duration: 120
				));
			}
		}

		public override void Unload()
		{
		}
	}
}
