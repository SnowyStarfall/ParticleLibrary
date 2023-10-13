using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core.Systems.GEmitterSystem
{
	public class GEmitterParticleSettings
	{
		/// <summary>
		/// Whether to use scalar velocity, or to use directional velocity.
		/// </summary>
		public bool UseScalarVelocity { get; set; } = true;

		/// <summary>
		/// The minimum velocity to spawn a particle with.
		/// </summary>
		public float MinimumScalarVelocity { get; set; } = 0f;
		/// <summary>
		/// The maximum velocity to spawn a particle with.
		/// </summary>
		public float MaximumScalarVelocity { get; set; } = 0f;
		/// <summary>
		/// The minimum velocity acceleration to spawn a particle with.
		/// </summary>
		public float MinimumScalarVelocityAcceleration { get; set; } = 0f;
		/// <summary>
		/// The maximum velocity acceleration to spawn a particle with.
		/// </summary>
		public float MaximumScalarVelocityAcceleration { get; set; } = 0f;
		/// <summary>
		/// The minimum radians to rotate a particle's velocity by.
		/// </summary>
		public float MinimumRadians { get; set; } = 0f;
		/// <summary>
		/// The maximum radians to rotate a particle's velocity by.
		/// </summary>
		public float MaximumRadians { get; set; } = MathHelper.TwoPi;

		/// <summary>
		/// The minimum velocity to spawn a particle with.
		/// </summary>
		public Vector2 MinimumDirectionalVelocity { get; set; } = Vector2.One;
		/// <summary>
		/// The maximum velocity to spawn a particle with.
		/// </summary>
		public Vector2 MaximumDirectionalVelocity { get; set; } = Vector2.One;
		/// <summary>
		/// The minimum velocity acceleration to spawn a particle with.
		/// </summary>
		public Vector2 MinimumVelocityAcceleration { get; set; } = Vector2.Zero;
		/// <summary>
		/// The maximum velocity acceleration to spawn a particle with.
		/// </summary>
		public Vector2 MaximumVelocityAcceleration { get; set; } = Vector2.Zero;

		/// <summary>
		/// The minimum scale acceleration to spawn a particle with.
		/// </summary>
		public Vector2 MinimumScale { get; set; } = Vector2.One;
		/// <summary>
		/// The maximum scale acceleration to spawn a particle with.
		/// </summary>
		public Vector2 MaximumScale { get; set; } = Vector2.One;
		/// <summary>
		/// The minimum scale velocity to spawn a particle with.
		/// </summary>
		public Vector2 MinimumScaleVelocity { get; set; } = Vector2.Zero;
		/// <summary>
		/// The maximum scale velocity to spawn a particle with.
		/// </summary>
		public Vector2 MaximumScaleVelocity { get; set; } = Vector2.Zero;

		/// <summary>
		/// The minimum rotation to spawn a particle with. Defaults to zero, as that is more performant.
		/// </summary>
		public float MinimumRotation { get; set; } = 0;
		/// <summary>
		/// The maximum rotation to spawn a particle with. Defaults to zero, as that is more performant.
		/// </summary>
		public float MaximumRotation { get; set; } = 0;
		/// <summary>
		/// The minimum rotation speed to spawn a particle with. Defaults to zero, as that is more performant.
		/// </summary>
		public float MinimumRotationSpeed { get; set; } = 0;
		/// <summary>
		/// The maximum rotation speed to spawn a particle with. Defaults to zero, as that is more performant.
		/// </summary>
		public float MaximumRotationSpeed { get; set; } = 0;

		/// <summary>
		/// The minimum depth to spawn a particle with.
		/// </summary>
		public float MinimumDepth { get; set; } = 1f;
		/// <summary>
		/// The maximum depth to spawn a particle with.
		/// </summary>
		public float MaximumDepth { get; set; } = 1f;
		/// <summary>
		/// The minimum depth velocity to spawn a particle with.
		/// </summary>
		public float MinimumDepthVelocity { get; set; } = 0f;
		/// <summary>
		/// The maximum depth velocity to spawn a particle with.
		/// </summary>
		public float MaximumDepthVelocity { get; set; } = 0f;
	}
}
