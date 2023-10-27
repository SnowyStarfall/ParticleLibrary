using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleLibrary.Core.PointParticleSystem
{
	public class PointParticleSystemSettings
	{
		public int MaxParticles { get; }
		public int Lifespan { get; internal set; }
		public Layer Layer { get; }
		public BlendState BlendState { get; }
		public bool Fade { get; internal set; }
		public float Gravity { get; internal set; }
		public float TerminalGravity { get; internal set; }

		public PointParticleSystemSettings(int maxParticles, int lifespan, Layer layer = Layer.BeforeDust, BlendState? blendState = null, bool fade = true, float gravity = 0f, float terminalGravity = 0)
		{
			if (maxParticles < 1)
				throw new ArgumentOutOfRangeException(nameof(maxParticles), "Must be greater than 0");

			MaxParticles = maxParticles;
			Lifespan = lifespan;
			Layer = layer;
			BlendState = blendState;
			Fade = fade;
			Gravity = gravity;
			TerminalGravity = terminalGravity;
		}
	}
}
