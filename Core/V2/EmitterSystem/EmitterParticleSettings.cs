using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core
{
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Emission instead")]
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
		/// The minimum velocity deviation to spawn a particle with.
		/// </summary>
		public float MinimumScalarVelocityDeviation { get; set; } = 0f;
		/// <summary>
		/// The maximum velocity deviation to spawn a particle with.
		/// </summary>
		public float MaximumScalarVelocityDeviation { get; set; } = 0f;
		/// <summary>
		/// The minimum velocity acceleration to spawn a particle with.
		/// </summary>
		public float MinimumScalarVelocityAcceleration { get; set; } = 1f;
		/// <summary>
		/// The maximum velocity acceleration to spawn a particle with.
		/// </summary>
		public float MaximumScalarVelocityAcceleration { get; set; } = 1f;
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
		public Vector2 MinimumDirectionalVelocityDeviation { get; set; } = Vector2.Zero;
		/// <summary>
		/// The maximum velocity acceleration to spawn a particle with.
		/// </summary>
		public Vector2 MaximumDirectionalVelocityDeviation { get; set; } = Vector2.Zero;
		/// <summary>
		/// The minimum velocity acceleration to spawn a particle with.
		/// </summary>
		public Vector2 MinimumDirectionalVelocityAcceleration { get; set; } = Vector2.One;
		/// <summary>
		/// The maximum velocity acceleration to spawn a particle with.
		/// </summary>
		public Vector2 MaximumDirectionalVelocityAcceleration { get; set; } = Vector2.One;

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
		/// The minimum scale velocity to spawn a particle with.
		/// </summary>
		public Vector2 MinimumScaleAcceleration { get; set; } = Vector2.One;
		/// <summary>
		/// The maximum scale velocity to spawn a particle with.
		/// </summary>
		public Vector2 MaximumScaleAcceleration { get; set; } = Vector2.One;

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
		/// The minimum rotation speed to spawn a particle with. Defaults to zero, as that is more performant.
		/// </summary>
		public float MinimumRotationAcceleration { get; set; } = 1;
		/// <summary>
		/// The maximum rotation speed to spawn a particle with. Defaults to zero, as that is more performant.
		/// </summary>
		public float MaximumRotationAcceleration { get; set; } = 1;

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
			tag.Set("ScalarVelocity", new Vector2(MinimumScalarVelocity, MaximumScalarVelocity));
			tag.Set("ScalarVelocityDeviation", new Vector2(MinimumScalarVelocityDeviation, MaximumScalarVelocityDeviation));
			tag.Set("ScalarVelocityAcceleration", new Vector2(MinimumScalarVelocityAcceleration, MaximumScalarVelocityAcceleration));
			tag.Set("Radians", new Vector2(MinimumRadians, MaximumRadians));
			tag.Set("MinimumDirectionalVelocity", MinimumDirectionalVelocity);
			tag.Set("MaximumDirectionalVelocity", MaximumDirectionalVelocity);
			tag.Set("MinimumDirectionalVelocityDeviation", MinimumDirectionalVelocityDeviation);
			tag.Set("MaximumDirectionalVelocityDeviation", MaximumDirectionalVelocityDeviation);
			tag.Set("MinimumDirectionalVelocityAcceleration", MinimumDirectionalVelocityAcceleration);
			tag.Set("MaximumDirectionalVelocityAcceleration", MaximumDirectionalVelocityAcceleration);
			tag.Set("MinimumScale", MinimumScale);
			tag.Set("MaximumScale", MaximumScale);
			tag.Set("MinimumScaleVelocity", MinimumScaleVelocity);
			tag.Set("MaximumScaleVelocity", MaximumScaleVelocity);
			tag.Set("MinimumScaleAcceleration", MinimumScaleAcceleration);
			tag.Set("MaximumScaleAcceleration", MaximumScaleAcceleration);
			tag.Set("Rotation", new Vector2(MinimumRotation, MaximumRotation));
			tag.Set("RotationVelocity", new Vector2(MinimumRotationVelocity, MaximumRotationVelocity));
			tag.Set("RotationAcceleration", new Vector2(MinimumRotationAcceleration, MaximumRotationAcceleration));
			tag.Set("Depth", new Vector2(MinimumDepth, MaximumDepth));
			tag.Set("DepthVelocity", new Vector2(MinimumDepthVelocity, MaximumDepthVelocity));
		}

		internal void LoadData(TagCompound tag)
		{
			UseScalarVelocity = tag.GetBool("UseScalarVelocity");

			Vector2 scalar = tag.Get<Vector2>("ScalarVelocity");
			MinimumScalarVelocity = scalar.X;
			MaximumScalarVelocity = scalar.Y;

			Vector2 scalarDeviation = tag.Get<Vector2>("ScalarVelocityDeviation");
			MinimumScalarVelocityDeviation = scalarDeviation.X;
			MaximumScalarVelocityDeviation = scalarDeviation.Y;

			Vector2 scalarAcceleration = tag.Get<Vector2>("ScalarVelocityAcceleration");
			MinimumScalarVelocityAcceleration = scalarAcceleration.X;
			MaximumScalarVelocityAcceleration = scalarAcceleration.Y;

			Vector2 radians = tag.Get<Vector2>("Radians");
			MinimumRadians = radians.X;
			MaximumRadians = radians.Y;

			MinimumDirectionalVelocity = tag.Get<Vector2>("MinimumDirectionalVelocity");
			MaximumDirectionalVelocity = tag.Get<Vector2>("MaximumDirectionalVelocity");
			MinimumDirectionalVelocityDeviation = tag.Get<Vector2>("MinimumDirectionalVelocityDeviation");
			MaximumDirectionalVelocityDeviation = tag.Get<Vector2>("MaximumDirectionalVelocityDeviation");
			MinimumDirectionalVelocityAcceleration = tag.Get<Vector2>("MinimumDirectionalVelocityAcceleration");
			MaximumDirectionalVelocityAcceleration = tag.Get<Vector2>("MaximumDirectionalVelocityAcceleration");

			MinimumScale = tag.Get<Vector2>("MinimumScale");
			MaximumScale = tag.Get<Vector2>("MaximumScale");
			MinimumScaleVelocity = tag.Get<Vector2>("MinimumScaleVelocity");
			MaximumScaleVelocity = tag.Get<Vector2>("MaximumScaleVelocity");
			MinimumScaleAcceleration = tag.Get<Vector2>("MinimumScaleAcceleration");
			MaximumScaleAcceleration = tag.Get<Vector2>("MaximumScaleAcceleration");

			Vector2 rotation = tag.Get<Vector2>("Rotation");
			MinimumRotation = rotation.X;
			MaximumRotation = rotation.Y;

			Vector2 rotationVelocity = tag.Get<Vector2>("RotationVelocity");
			MinimumRotationVelocity = rotationVelocity.X;
			MaximumRotationVelocity = rotationVelocity.Y;

			Vector2 rotationAcceleration = tag.Get<Vector2>("RotationAcceleration");
			MinimumRotationAcceleration = rotationAcceleration.X;
			MaximumRotationAcceleration = rotationAcceleration.Y;

			Vector2 depth = tag.Get<Vector2>("Depth");
			MinimumDepth = depth.X;
			MaximumDepth = depth.Y;

			Vector2 depthVelocity = tag.Get<Vector2>("DepthVelocity");
			MinimumDepthVelocity = depthVelocity.X;
			MaximumDepthVelocity = depthVelocity.Y;
		}
	}
}
