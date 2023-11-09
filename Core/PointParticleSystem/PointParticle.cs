﻿using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core
{
	public class PointParticle
	{
		public Color StartColor { get; set; } = Color.White;
		public Color EndColor { get; set; } = Color.White;

		public Vector2 VelocityAcceleration { get; set; } = Vector2.Zero;

		public float Depth { get; set; } = 1f;
		public float DepthVelocity { get; set; } = 0f;
	}
}
