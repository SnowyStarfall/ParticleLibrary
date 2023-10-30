using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core
{
	public class EmitterParticleSettings
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
		public Vector2 MinimumDirectionalVelocityAcceleration { get; set; } = Vector2.Zero;
		/// <summary>
		/// The maximum velocity acceleration to spawn a particle with.
		/// </summary>
		public Vector2 MaximumDirectionalVelocityAcceleration { get; set; } = Vector2.Zero;

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
		public float MinimumRotationVelocity { get; set; } = 0;
		/// <summary>
		/// The maximum rotation speed to spawn a particle with. Defaults to zero, as that is more performant.
		/// </summary>
		public float MaximumRotationVelocity { get; set; } = 0;

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

		internal void SaveData(TagCompound tag)
		{
			tag.Set("UseScalarVelocity", UseScalarVelocity);
			tag.Set("ScalarVelocity", new Vector4(MinimumScalarVelocity, MaximumScalarVelocity, MinimumScalarVelocityAcceleration, MaximumScalarVelocityAcceleration));
			tag.Set("Radians", new Vector2(MinimumRadians, MaximumRadians));
			tag.Set("MinimumDirectionalVelocity", MinimumDirectionalVelocity);
			tag.Set("MaximumDirectionalVelocity", MaximumDirectionalVelocity);
			tag.Set("MinimumDirectionalVelocityAcceleration", MinimumDirectionalVelocityAcceleration);
			tag.Set("MaximumDirectionalVelocityAcceleration", MaximumDirectionalVelocityAcceleration);
			tag.Set("MinimumScale", MinimumScale);
			tag.Set("MaximumScale", MaximumScale);
			tag.Set("MinimumScaleVelocity", MinimumScaleVelocity);
			tag.Set("MaximumScaleVelocity", MaximumScaleVelocity);
			tag.Set("Rotation", new Vector4(MinimumRotation, MaximumRotation, MinimumRotationVelocity, MaximumRotationVelocity));
			tag.Set("Depth", new Vector4(MinimumDepth, MaximumDepth, MinimumDepthVelocity, MaximumDepthVelocity));
		}

		internal void LoadData(TagCompound tag)
		{
			UseScalarVelocity = tag.GetBool("UseScalarVelocity");

			Vector4 scalar = tag.Get<Vector4>("ScalarVelocity");
			MinimumScalarVelocity = scalar.X;
			MaximumScalarVelocity = scalar.Y;
			MinimumScalarVelocityAcceleration = scalar.Z;
			MaximumScalarVelocityAcceleration = scalar.W;

			Vector2 radians = tag.Get<Vector2>("Radians");
			MinimumRadians = radians.X;
			MaximumRadians = radians.Y;

			MinimumDirectionalVelocity = tag.Get<Vector2>("MinimumDirectionalVelocity");
			MaximumDirectionalVelocity = tag.Get<Vector2>("MaximumDirectionalVelocity");
			MinimumDirectionalVelocityAcceleration = tag.Get<Vector2>("MinimumDirectionalVelocityAcceleration");
			MaximumDirectionalVelocityAcceleration = tag.Get<Vector2>("MaximumDirectionalVelocityAcceleration");
			MinimumScale = tag.Get<Vector2>("MinimumScale");
			MaximumScale = tag.Get<Vector2>("MaximumScale");
			MinimumScaleVelocity = tag.Get<Vector2>("MinimumScaleVelocity");
			MaximumScaleVelocity = tag.Get<Vector2>("MaximumScaleVelocity");

			Vector4 rotation = tag.Get<Vector4>("Rotation");
			MinimumRotation = rotation.X;
			MaximumRotation = rotation.Y;
			MinimumRotationVelocity = rotation.Z;
			MaximumRotationVelocity = rotation.W;

			Vector4 depth = tag.Get<Vector4>("Depth");
			MinimumDepth = depth.X;
			MaximumDepth = depth.Y;
			MinimumDepthVelocity = depth.Z;
			MaximumDepthVelocity = depth.W;
		}
	}
}
