using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// Represents the settings for a <see cref="GParticleSystem"/>
	/// </summary>
	public class GParticleSystemSettings
	{
		/// <summary>
		/// The texture of the particles
		/// </summary>
		public Texture2D Texture { get; }
		/// <summary>
		/// The maximum amount of particles
		/// </summary>
		public int MaxParticles { get; }
		/// <summary>
		/// The lifespan of the particles
		/// </summary>
		public int Lifespan { get; internal set; }
		/// <summary>
		/// The size of the batching buffer. Currently unimplemented for now
		/// </summary>
		public int BufferSize { get; }
		/// <summary>
		/// The layer the particles are drawn on
		/// </summary>
		public Layer Layer { get; }
		/// <summary>
		/// The BlendState the particles are drawn with
		/// </summary>
		public BlendState BlendState { get; }
		/// <summary>
		/// Whether the particles should fade over their lifespan
		/// </summary>
		public bool Fade { get; internal set; }
		/// <summary>
		/// How much gravity to apply to the particles
		/// </summary>
		public float Gravity { get; internal set; }
		/// <summary>
		/// The maximum amount of velocity a particle should recieve from gravity/ Currently unimplemented for now
		/// </summary>
		public float TerminalGravity { get; internal set; }

		/// <summary>
		/// Creates a new settings present for a <see cref="GParticleSystem"/>
		/// </summary>
		/// <param name="texture">The texture of the particles</param>
		/// <param name="maxParticles">The maximum amount of particles</param>
		/// <param name="lifespan">The lifespan of the particles</param>
		/// <param name="layer">The layer the particles are drawn on</param>
		/// <param name="blendState">The BlendState the particles are drawn with</param>
		/// <param name="fade">Whether the particles should fade over their lifespan</param>
		/// <param name="gravity">How much gravity to apply to the particles</param>
		/// <param name="terminalGravity">The maximum amount of velocity a particle should recieve from gravity/ Currently unimplemented for now</param>
		/// <exception cref="ArgumentNullException">Ensure that texture is not null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Ensure that max particles is greater than 0</exception>
		public GParticleSystemSettings(Texture2D texture, int maxParticles, int lifespan, /*int bufferSize,*/ Layer layer = Layer.BeforeDust, BlendState blendState = null, bool fade = true, float gravity = 0f, float terminalGravity = 0)
		{
			if (texture is null)
				throw new ArgumentNullException(nameof(texture), "Texture cannot be null.");

			if (maxParticles < 1)
				throw new ArgumentOutOfRangeException(nameof(maxParticles), "Must be greater than 0");

			Texture = texture;
			MaxParticles = maxParticles;
			Lifespan = lifespan;
			//BufferSize = bufferSize;
			Layer = layer;
			BlendState = blendState ?? BlendState.AlphaBlend;
			Fade = fade;
			Gravity = gravity;
			TerminalGravity = terminalGravity;
		}
	}
}
