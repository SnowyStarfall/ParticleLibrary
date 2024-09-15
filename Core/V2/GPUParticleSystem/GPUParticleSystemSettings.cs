using Microsoft.Xna.Framework.Graphics;

namespace ParticleLibrary.Core
{
	public abstract class GPUParticleSystemSettings
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
	}
}
