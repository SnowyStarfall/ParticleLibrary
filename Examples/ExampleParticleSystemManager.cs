using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;
using ParticleLibrary.Utilities;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary.Examples
{
	/// <summary>
	/// This class demonstrates a good way to manage your GPU particle systems.
	/// It's a good idea to centralize your GPU particle systems, as you'll want to reuse them as much as possible.
	/// </summary>
	public class ExampleParticleSystemManager : ModSystem
	{
		public static QuadParticleSystem ExampleQuadSystem { get; private set; }
		public static QuadParticleSystemSettings ExampleQuadSettings { get; private set; }
		public static QuadParticle ExampleQuadParticle { get; private set; }

		public static PointParticleSystem ExamplePointSystem { get; private set; }
		public static PointParticleSystemSettings ExamplePointSettings { get; private set; }
		public static PointParticle ExamplePointParticle { get; private set; }

		public static ExampleParticleSystemWrapper<QuadParticle> ExampleWrappedQuadParticleSystem { get; private set; }

		public override void OnModLoad()
		{
			// Demonstrates creating a Quad particle system.
			ExampleQuadSettings = new(ModContent.Request<Texture2D>(Resources.Assets.Textures.Star, AssetRequestMode.ImmediateLoad).Value, 500, 300, blendState: BlendState.AlphaBlend);
			ExampleQuadSystem = new QuadParticleSystem(ExampleQuadSettings);
			ExampleQuadParticle = new()
			{
				StartColor = Color.White.WithAlpha(0f),
				EndColor = Color.Black.WithAlpha(0f),
				Scale = new Vector2(1f),
				Rotation = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi + float.Epsilon),
				RotationVelocity = Main.rand.NextFloat(-0.1f, 0.1f + float.Epsilon),
				Depth = 1f + Main.rand.NextFloat(-0.1f, 0.1f + float.Epsilon),
				DepthVelocity = Main.rand.NextFloat(-0.001f, 0.001f + float.Epsilon)
			};

			// Demonstrates creating a Point particle system.
			ExamplePointSettings = new(500, 300);
			ExamplePointSystem = new PointParticleSystem(ExamplePointSettings);
			ExamplePointParticle = new()
			{
				StartColor = Color.White.WithAlpha(0f),
				EndColor = Color.Black.WithAlpha(0f),
				Depth = 1f + Main.rand.NextFloat(-0.1f, 0.1f + float.Epsilon),
				DepthVelocity = Main.rand.NextFloat(-0.001f, 0.001f + float.Epsilon)
			};

			// Demonstrates creating a wrapped particle system from a Quad particle system.
			// This can be useful for implementing custom functionality, such as embedding your system into a RenderTarget2D.
			ExampleWrappedQuadParticleSystem = new(ExampleQuadSystem, ExampleQuadSettings);
		}

		public override void Unload()
		{
			// Always make sure to dispose GPU particle systems when you're done with them!
			ExampleQuadSystem.Dispose();
			ExamplePointSystem.Dispose();
		}
	}
}
