using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.Graphics;

namespace ParticleLibrary.Core.EmitterSystem.Data
{
	public class SpatialParameters
	{
		public Vector2 Velocity = new();
		public Vector2 VelocityAcceleration = new();
		public Vector2 Scale = new();
		public Vector2 ScaleVelocity = new();
		public float Rotation;
		public float RotationVelocity;
		public float Depth;
		public float DepthVelocity;

		public static SpatialParameters Calculate(EmitterParticleSettings settings)
		{
			SpatialParameters spatial = new();

			// Velocity
			if (settings.UseScalarVelocity)
			{
				float scalar = Main.rand.NextFloat(0f, 1f + float.Epsilon);

				spatial.Velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(settings.MinimumScalarVelocity, settings.MaximumScalarVelocity + float.Epsilon) * scalar;
				spatial.VelocityAcceleration = Main.rand.NextVector2Unit() * Main.rand.NextFloat(settings.MinimumScalarVelocityAcceleration, settings.MaximumScalarVelocityAcceleration + float.Epsilon) * scalar;
			}
			else
			{
				spatial.Velocity.X = Main.rand.NextFloat(settings.MinimumDirectionalVelocity.X, settings.MaximumDirectionalVelocity.X + float.Epsilon);
				spatial.Velocity.Y = Main.rand.NextFloat(settings.MinimumDirectionalVelocity.Y, settings.MaximumDirectionalVelocity.Y + float.Epsilon);

				spatial.VelocityAcceleration.X = Main.rand.NextFloat(settings.MinimumDirectionalVelocityAcceleration.X, settings.MinimumDirectionalVelocityAcceleration.X + float.Epsilon);
				spatial.VelocityAcceleration.Y = Main.rand.NextFloat(settings.MinimumDirectionalVelocityAcceleration.Y, settings.MinimumDirectionalVelocityAcceleration.Y + float.Epsilon);
			}

			// Scale
			spatial.Scale.X = Main.rand.NextFloat(settings.MinimumScale.X, settings.MaximumScale.X + float.Epsilon);
			spatial.Scale.Y = Main.rand.NextFloat(settings.MinimumScale.Y, settings.MaximumScale.Y + float.Epsilon);

			spatial.ScaleVelocity.X = Main.rand.NextFloat(settings.MinimumScaleVelocity.X, settings.MaximumScaleVelocity.X + float.Epsilon);
			spatial.ScaleVelocity.Y = Main.rand.NextFloat(settings.MinimumScaleVelocity.Y, settings.MaximumScaleVelocity.Y + float.Epsilon);

			// Rotation
			spatial.Rotation = Main.rand.NextFloat(settings.MinimumRotation, settings.MaximumRotation + float.Epsilon);
			spatial.RotationVelocity = Main.rand.NextFloat(settings.MinimumRotationVelocity, settings.MaximumRotationVelocity + float.Epsilon);

			// Depth
			spatial.Depth = Main.rand.NextFloat(settings.MinimumDepth, settings.MaximumDepth + float.Epsilon);
			spatial.DepthVelocity = Main.rand.NextFloat(settings.MinimumDepthVelocity, settings.MaximumDepthVelocity + float.Epsilon);

			return spatial;
		}
	}
}
