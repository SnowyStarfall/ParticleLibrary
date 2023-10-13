using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System;

namespace ParticleLibrary.Core.Systems.Test
{
	public class GParticleSystemSettings
	{
		public Texture2D Texture { get; }
		public int MaxParticles { get; }
		public int Lifespan { get; internal set; }
		public bool Fade { get; internal set; }
		public float Gravity { get; internal set; }
		public float TerminalGravity { get; internal set; }

		public GParticleSystemSettings(Texture2D texture, int maxParticles, int lifespan, bool fade = true, float gravity = 0f, float terminalGravity = 0)
		{
			if (texture is null)
				throw new ArgumentNullException(nameof(texture), "Texture cannot be null.");

			Texture = texture;
			MaxParticles = maxParticles;
			Lifespan = lifespan;
			Fade = fade;
			Gravity = gravity;
			TerminalGravity = terminalGravity;
		}
	}
}
