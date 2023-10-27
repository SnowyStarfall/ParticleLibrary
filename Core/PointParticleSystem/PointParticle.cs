using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleLibrary.Core.PointParticleSystem
{
	public class PointParticle
	{
		public Color StartColor { get; set; } = Color.White;
		public Color EndColor { get; set; } = Color.White;

		public Vector2 VelocityAcceleration { get; set; } = Vector2.Zero;

		public float Size { get; set; } = 1f;

		public float Depth { get; set; } = 1f;
		public float DepthVelocity { get; set; } = 0f;
	}
}
