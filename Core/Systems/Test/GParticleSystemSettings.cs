using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleLibrary.Core.Systems.Test
{
	public class GParticleSystemSettings
	{
		public Texture2D Texture { get; }
		public int MaxParticles { get; }
		public int Lifespan { get; }

		public GParticleSystemSettings(Texture2D texture, int maxParticles, int lifespan)
		{
			if (texture is null)
				throw new ArgumentNullException(nameof(texture), "Texture cannot be null.");

			Texture = texture;
			MaxParticles = maxParticles;
			Lifespan = lifespan;
		}
	}
}
