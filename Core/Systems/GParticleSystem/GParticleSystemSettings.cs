using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleLibrary.Core.Systems.Test
{
	public class GParticleSystemSettings
	{
		public Texture2D Texture { get; }
		public int MaxParticles { get; }
		public int Lifespan { get; }
		public bool Fade { get; }
		public Vector2 Gravity { get; }

		public GParticleSystemSettings(Texture2D texture, int maxParticles, int lifespan, bool fade = true, Vector2? gravity = null)
		{
			if (texture is null)
				throw new ArgumentNullException(nameof(texture), "Texture cannot be null.");

			Texture = texture;
			MaxParticles = maxParticles;
			Lifespan = lifespan;
			Fade = fade;
			Gravity = gravity ?? Vector2.Zero;
		}
	}
}
