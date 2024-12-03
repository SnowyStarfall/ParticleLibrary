using Microsoft.Xna.Framework;
using ParticleLibrary.Core.V3.Emission;
using ParticleLibrary.Core.V3.Particles;
using ParticleLibrary.Utilities;
using System;
using Terraria;
using SysVector2 = System.Numerics.Vector2;

namespace ParticleLibrary.Examples.V3
{
	public class ExampleEmitter(Vector2 position, Point size) : Emitter(position, size)
	{
		public override void Initialize()
		{
			// Perform one-time code here.
		}

		public override void Update()
		{
			// Do your update logic here.

			// Previous emitters calculated a lot of it for you, but that was
			// discarded in favor of a much slimmer data profile and more flexible
			// implementation. You can still stora fields in the emitter.
			// However, these fields are not saved with the world, and serialization
			// overrides are no longer provided.

			//SpawnBox();
			//SpawnCircle();
			//SpawnHollowBox();
			//SpawnHollowCircle();
		}

		/// <summary>
		/// Spawns particles in a box
		/// </summary>
		private void SpawnBox()
		{
			float width = 32f;
			float height = 32f;
			SysVector2 position = new(
				Main.rand.NextFloat(-width / 2f, width / 2f),
				Main.rand.NextFloat(-height / 2f, height / 2f)
			);

			ExampleParticleSystemManagerV3.ExampleParticleBuffer.Create(new ParticleInfo(
				position,
				SysVector2.Zero,
				0f,
				SysVector2.One,
				new Color(1f, 1f, 1f, 0f),
				60
			));
		}

		/// <summary>
		/// Spawns particles in a circle
		/// </summary>
		private void SpawnCircle()
		{
			float radius = 16f;

			float distance = Main.rand.NextFloat(0f, radius + float.Epsilon);
			float radians = Main.rand.NextFloat(0f, MathF.Tau + float.Epsilon);

			SysVector2 position = new SysVector2(distance, 0f).RotatedBy(radians);

			ExampleParticleSystemManagerV3.ExampleParticleBuffer.Create(new ParticleInfo(
				position,
				SysVector2.Zero,
				0f,
				SysVector2.One,
				new Color(1f, 1f, 1f, 0f),
				60
			));
		}

		/// <summary>
		/// Spawns particles on the edges of a box
		/// </summary>
		private void SpawnHollowBox()
		{
			float width = 32f;
			float height = 32f;

			SysVector2 position = Position.ToNumerics();

			int edge = Main.rand.Next(0, 4);
			switch (edge)
			{
				case 0: // Top edge
					position.X += Main.rand.NextFloat(-(width / 2f), (width / 2f) + float.Epsilon);
					position.Y -= height / 2f;
					break;
				case 1: // Right edge
					position.X += width / 2f;
					position.Y += Main.rand.NextFloat(-(height / 2f), (height / 2f) + float.Epsilon);
					break;
				case 2: // Bottom edge
					position.X += Main.rand.NextFloat(-(width / 2f), (width / 2f) + float.Epsilon);
					position.Y += height / 2f;
					break;
				case 3: // Left edge
					position.X -= width / 2f;
					position.Y += Main.rand.NextFloat(-(height / 2f), (height / 2f) + float.Epsilon);
					break;
				default:
					throw new InvalidOperationException("Unexpected edge value.");
			}

			ExampleParticleSystemManagerV3.ExampleParticleBuffer.Create(new ParticleInfo(
				position,
				SysVector2.Zero,
				0f,
				SysVector2.One,
				new Color(1f, 1f, 1f, 0f),
				60
			));
		}

		/// <summary>
		/// Spawns particles on the edges of a circle
		/// </summary>
		private void SpawnHollowCircle()
		{
			float radius = 16f;
			float radians = Main.rand.NextFloat(0f, MathF.Tau + float.Epsilon);

			SysVector2 position = new SysVector2(radius, 0f).RotatedBy(radians);

			ExampleParticleSystemManagerV3.ExampleParticleBuffer.Create(new ParticleInfo(
				position,
				SysVector2.Zero,
				0f,
				SysVector2.One,
				new Color(1f, 1f, 1f, 0f),
				60
			));
		}
	}
}
