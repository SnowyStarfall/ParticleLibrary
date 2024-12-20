﻿using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// Represents the settings for a <see cref="PointParticleSystem"/>
	/// </summary>
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Particles instead")]
	public class PointParticleSystemSettings : GPUParticleSystemSettings
	{
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
		/// Creates a new settings present for a <see cref="PointParticleSystem"/>
		/// </summary>
		/// <param name="maxParticles">The maximum amount of particles</param>
		/// <param name="lifespan">The lifespan of the particles</param>
		/// <param name="layer">The layer the particles are drawn on</param>
		/// <param name="blendState">The BlendState the particles are drawn with</param>
		/// <param name="fade">Whether the particles should fade over their lifespan</param>
		/// <exception cref="ArgumentNullException">Ensure that texture is not null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Ensure that max particles is greater than 0</exception>
		public PointParticleSystemSettings(int maxParticles, int lifespan, Layer layer = Layer.BeforeDust, BlendState blendState = null, bool fade = true)
		{
			if (maxParticles < 1)
				throw new ArgumentOutOfRangeException(nameof(maxParticles), "Must be greater than 0");

			MaxParticles = maxParticles;
			Lifespan = lifespan;
			Layer = layer;
			BlendState = blendState ?? BlendState.AlphaBlend;
			Fade = fade;
		}
	}
}
