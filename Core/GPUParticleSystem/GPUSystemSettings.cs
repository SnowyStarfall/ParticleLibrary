using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleLibrary.Core
{
	public abstract class GPUSystemSettings
	{
		/// <summary>
		/// The texture of the particles
		/// </summary>
		public virtual Texture2D Texture { get; }
		/// <summary>
		/// The maximum amount of particles
		/// </summary>
		public abstract int MaxParticles { get; }
		/// <summary>
		/// The lifespan of the particles
		/// </summary>
		public abstract int Lifespan { get; internal set; }
		/// <summary>
		/// The size of the batching buffer. Currently unimplemented for now
		/// </summary>
		public abstract int BufferSize { get; }
		/// <summary>
		/// The layer the particles are drawn on
		/// </summary>
		public abstract Layer Layer { get; }
		/// <summary>
		/// The BlendState the particles are drawn with
		/// </summary>
		public abstract BlendState BlendState { get; }
		/// <summary>
		/// Whether the particles should fade over their lifespan
		/// </summary>
		public abstract bool Fade { get; internal set; }
		///// <summary>
		///// How much gravity to apply to the particles
		///// </summary>
		//public abstract float Gravity { get; internal set; }
		///// <summary>
		///// The maximum amount of velocity a particle should recieve from gravity/ Currently unimplemented for now
		///// </summary>
		//public abstract float TerminalGravity { get; internal set; }
	}
}
