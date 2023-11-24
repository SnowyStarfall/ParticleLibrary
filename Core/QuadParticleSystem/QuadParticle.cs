using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// The base for a GParticle.
	/// </summary>
	public class QuadParticle : GPUParticle
	{
		/// <summary>
		/// Sets the starting color for each corner of the particle.
		/// </summary>
		public RenderQuad StartQuad { get; set; } = null;
		/// <summary>
		/// Sets the ending color for each corner of the particle.
		/// </summary>
		public RenderQuad EndQuad { get; set; } = null;

		/// <summary>
		/// The start color
		/// </summary>
		public Color StartColor { get; set; } = Color.White;
		/// <summary>
		/// The end color
		/// </summary>
		public Color EndColor { get; set; } = Color.White;

		/// <summary>
		/// How much velocity changes over time.
		/// </summary>
		public Vector2 VelocityDeviation { get; set; } = Vector2.Zero;
		/// <summary>
		/// How much velocity should accelerate over time. (multiplicative)
		/// </summary>
		public Vector2 VelocityAcceleration { get; set; } = Vector2.Zero;

		/// <summary>
		/// The scale of the particle.
		/// </summary>
		public Vector2 Scale { get; set; } = Vector2.One;
		/// <summary>
		/// How much scale changes over time
		/// </summary>
		public Vector2 ScaleVelocity { get; set; } = Vector2.Zero;

		/// <summary>
		/// The rotation of the particle
		/// </summary>
		public float Rotation { get; set; } = 0f;
		/// <summary>
		/// How much rotation changes over time
		/// </summary>
		public float RotationVelocity { get; set; } = 0f;

		/// <summary>
		/// The depth of the particle. Default is 1f. Changes the strength of the parallax effect, making the particle seem closest at higher values (2f) or farthest at lower values (0f)
		/// </summary>
		public float Depth { get; set; } = 1f;
		/// <summary>
		/// How much the depth changes over time. Can result in the particle completely disappearing (clipping beyond the visual field of the "camera")
		/// </summary>
		public float DepthVelocity { get; set; } = 0f;
	}
}
