﻿using Microsoft.Xna.Framework;
using System;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// The base for a PointParticle
	/// </summary>
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Particles instead")]
	public class PointParticle : GPUParticle
	{
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
		public Vector2 VelocityAcceleration { get; set; } = Vector2.One;

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
