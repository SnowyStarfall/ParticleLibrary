using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace ParticleLibrary.Core.Data
{
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Emission instead")]
	public struct SpatialParameters
	{
		public Vector2 Velocity = new();
		public Vector2 VelocityAcceleration = new();
		public Vector2 Scale = new();
		public Vector2 ScaleVelocity = new();
		public Vector2 ScaleAcceleration = new();
		public float Rotation;
		public float RotationVelocity;
		public float RotationAcceleration;
		public float Depth;
		public float DepthVelocity;

		public SpatialParameters(EmitterParticleSettings settings)
		{
			// Velocity
			if (settings.UseScalarVelocity)
			{
				float scalar = Main.rand.NextFloat(0f, 1f + float.Epsilon);

				Velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(settings.MinimumScalarVelocity, settings.MaximumScalarVelocity + float.Epsilon) * scalar;
				VelocityAcceleration = Main.rand.NextVector2Unit() * Main.rand.NextFloat(settings.MinimumScalarVelocityAcceleration, settings.MaximumScalarVelocityAcceleration + float.Epsilon) * scalar;
			}
			else
			{
				Velocity.X = Main.rand.NextFloat(settings.MinimumDirectionalVelocity.X, settings.MaximumDirectionalVelocity.X + float.Epsilon);
				Velocity.Y = Main.rand.NextFloat(settings.MinimumDirectionalVelocity.Y, settings.MaximumDirectionalVelocity.Y + float.Epsilon);

				VelocityAcceleration.X = Main.rand.NextFloat(settings.MinimumDirectionalVelocityAcceleration.X, settings.MinimumDirectionalVelocityAcceleration.X + float.Epsilon);
				VelocityAcceleration.Y = Main.rand.NextFloat(settings.MinimumDirectionalVelocityAcceleration.Y, settings.MinimumDirectionalVelocityAcceleration.Y + float.Epsilon);
			}

			// Scale
			Scale.X = Main.rand.NextFloat(settings.MinimumScale.X, settings.MaximumScale.X + float.Epsilon);
			Scale.Y = Main.rand.NextFloat(settings.MinimumScale.Y, settings.MaximumScale.Y + float.Epsilon);

			ScaleVelocity.X = Main.rand.NextFloat(settings.MinimumScaleVelocity.X, settings.MaximumScaleVelocity.X + float.Epsilon);
			ScaleVelocity.Y = Main.rand.NextFloat(settings.MinimumScaleVelocity.Y, settings.MaximumScaleVelocity.Y + float.Epsilon);

			ScaleAcceleration.X = Main.rand.NextFloat(settings.MinimumScaleAcceleration.X, settings.MaximumScaleAcceleration.X + float.Epsilon);
			ScaleAcceleration.Y = Main.rand.NextFloat(settings.MinimumScaleAcceleration.Y, settings.MaximumScaleAcceleration.Y + float.Epsilon);

			// Rotation
			Rotation = Main.rand.NextFloat(settings.MinimumRotation, settings.MaximumRotation + float.Epsilon);
			RotationVelocity = Main.rand.NextFloat(settings.MinimumRotationVelocity, settings.MaximumRotationVelocity + float.Epsilon);
			RotationAcceleration = Main.rand.NextFloat(settings.MinimumRotationAcceleration, settings.MaximumRotationAcceleration + float.Epsilon);

			// Depth
			Depth = Main.rand.NextFloat(settings.MinimumDepth, settings.MaximumDepth + float.Epsilon);
			DepthVelocity = Main.rand.NextFloat(settings.MinimumDepthVelocity, settings.MaximumDepthVelocity + float.Epsilon);
		}
	}
}
