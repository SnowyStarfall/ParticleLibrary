using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using System;

namespace ParticleLibrary.Core
{
    public class GParticleSystemSettings
    {
        public Texture2D Texture { get; }
        public int MaxParticles { get; }
        public int Lifespan { get; internal set; }
        public Layer Layer { get; }
        public BlendState BlendState { get; }
        public bool Fade { get; internal set; }
        public float Gravity { get; internal set; }
        public float TerminalGravity { get; internal set; }

        public GParticleSystemSettings(Texture2D texture, int maxParticles, int lifespan, Layer layer = Layer.BeforeDust, BlendState? blendState = null, bool fade = true, float gravity = 0f, float terminalGravity = 0)
        {
            if (texture is null)
                throw new ArgumentNullException(nameof(texture), "Texture cannot be null.");

            Texture = texture;
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
