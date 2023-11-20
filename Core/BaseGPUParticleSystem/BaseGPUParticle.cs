using Microsoft.Xna.Framework;
using SteelSeries.GameSense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleLibrary.Core
{
	public abstract class BaseGPUParticle
	{
		/// <summary>
		/// The start color
		/// </summary>
		public abstract Color StartColor { get; set; } 
		/// <summary>
		/// The end color
		/// </summary>
		public abstract Color EndColor { get; set; } 

		/// <summary>
		/// How much velocity accelerates over time (additive)
		/// </summary>
		public abstract Vector2 VelocityAcceleration { get; set; } 

		/// <summary>
		/// The scale of the particle.
		/// </summary>
		public virtual Vector2 Scale { get; set; }
		/// <summary>
		/// How much scale changes over time
		/// </summary>
		public virtual Vector2 ScaleVelocity { get; set; }

		/// <summary>
		/// The rotation of the particle
		/// </summary>
		public virtual float Rotation { get; set; }
		/// <summary>
		/// How much rotation changes over time
		/// </summary>
		public virtual float RotationVelocity { get; set; }

		/// <summary>
		/// The depth of the particle. Default is 1f. Changes the strength of the parallax effect, making the particle seem closest at higher values (2f) or farthest at lower values (0f)
		/// </summary>
		public abstract float Depth { get; set; }
		/// <summary>
		/// How much the depth changes over time. Can result in the particle completely disappearing (clipping beyond the visual field of the "camera")
		/// </summary>
		public abstract float DepthVelocity { get; set; }
	}
}
