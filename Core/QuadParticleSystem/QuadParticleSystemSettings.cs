using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// Represents the settings for a <see cref="QuadParticleSystem"/>
	/// </summary>
	public class QuadParticleSystemSettings : GPUParticleSystemSettings
	{
		/// <summary>
		/// The texture of the particles
		/// </summary>
		public override Texture2D Texture { get; }
		/// <summary>
		/// The maximum amount of particles
		/// </summary>
		public override int MaxParticles { get; }
		/// <summary>
		/// The lifespan of the particles
		/// </summary>
		public override int Lifespan { get; internal set; }
		/// <summary>
		/// The size of the batching buffer. Currently unimplemented for now
		/// </summary>
		public override int BufferSize { get; }
		/// <summary>
		/// The layer the particles are drawn on
		/// </summary>
		public override Layer Layer { get; }
		/// <summary>
		/// The BlendState the particles are drawn with
		/// </summary>
		public override BlendState BlendState { get; }
		/// <summary>
		/// Whether the particles should fade over their lifespan
		/// </summary>
		public override bool Fade { get; internal set; }

		/// <summary>
		/// Creates a new settings preset for a <see cref="QuadParticleSystem"/>
		/// </summary>
		/// <param name="texture">The texture of the particles</param>
		/// <param name="maxParticles">The maximum amount of particles</param>
		/// <param name="lifespan">The lifespan of the particles</param>
		/// <param name="layer">The layer the particles are drawn on</param>
		/// <param name="blendState">The BlendState the particles are drawn with</param>
		/// <param name="fade">Whether the particles should fade over their lifespan</param>
		/// <exception cref="InvalidOperationException">Ensure that this <b>IS NOT called on a server</b></exception>
		/// <exception cref="ArgumentNullException">Ensure that texture is not null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Ensure that max particles is greater than 0</exception>
		public QuadParticleSystemSettings(Texture2D texture, int maxParticles, int lifespan, /*int bufferSize,*/ Layer layer = Layer.BeforeDust, BlendState blendState = null, bool fade = true)
		{
			if (Main.netMode is NetmodeID.Server)
				throw new InvalidOperationException("Cannot perform particle operations on a server.");

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
		}
	}
}
