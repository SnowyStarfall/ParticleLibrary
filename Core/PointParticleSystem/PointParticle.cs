using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// The base for a PointParticle
	/// </summary>
	public class PointParticle : BaseParticle
	{
		/// <summary>
		/// The start color
		/// </summary>
		public override Color StartColor { get; set; } = Color.White;
		/// <summary>
		/// The end color
		/// </summary>
		public override Color EndColor { get; set; } = Color.White;

		/// <summary>
		/// How much velocity should accelerate over time (additive)
		/// </summary>
		public override Vector2 VelocityAcceleration { get; set; } = Vector2.Zero;

		/// <summary>
		/// The depth of the particle. Default is 1f. Changes the strength of the parallax effect, making the particle seem closest at higher values (2f) or farthest at lower values (0f)
		/// </summary>
		public override float Depth { get; set; } = 1f;
		/// <summary>
		/// How much the depth changes over time. Can result in the particle completely disappearing (clipping beyond the visual field of the "camera")
		/// </summary>
		public override float DepthVelocity { get; set; } = 0f;
	}
}
