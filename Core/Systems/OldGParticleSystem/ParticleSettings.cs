using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ParticleLibrary.Core.Systems
{
    public abstract class ParticleSettings
    {
        /// <summary>
        /// The texture used for this particle.
        /// </summary>
        public Texture2D Texture { get; set; } = ModContent.Request<Texture2D>(Resources.WhitePixel).Value;

        /// <summary>
        /// The maximum amount of this particle allowed.
        /// </summary>
        public int MaxParticles { get; set; } = 1000;

        /// <summary>
        /// How long this particle stays alive, in frames. Minimum 1 frame.
        /// </summary>
        public int Duration { get; set; } = 60;

        /// <summary>
        /// How many frames the duration will deviate from the original value.
        /// </summary>
        public int DurationRandomness { get; set; } = 0;

        /// <summary>
        /// How much the particle velocity is affected by the emitter that created them.
        /// </summary>
        public float EmitterVelocityShare { get; set; } = 0f;

        /// <summary>
        /// The minimum amount of velocity to give the particle.
        /// </summary>
        public Vector2 MinimumVelocity { get; set; } = new(0f, 0f);
        /// <summary>
        /// The maximum amount of velocity to give the particle.
        /// </summary>
        public Vector2 MaximumVelocity { get; set; } = new(8f, 8f);

        /// <summary>
        /// The strength of the gravity that affects the particles.
        /// </summary>
        public Vector2 Gravity { get; set; } = Vector2.Zero;

        /// <summary>
        /// The factor of the remaining velocity at the end of a particle's lifetime.
        /// </summary>
        public float EndVelocity { get; set; } = 1f;

        /// <summary>
        /// The minimum starting color to create a particle with.
        /// </summary>
        public Color MinimumStartColor { get; set; } = Color.White;
        /// <summary>
        /// The maximum starting color to create a particle with. 
        /// </summary>
        public Color MaximumStartColor { get; set; } = Color.White;

        /// <summary>
        /// The minimum ending color to create a particle with.
        /// </summary>
        public Color MinimumEndColor { get; set; } = Color.White;
        /// <summary>
        /// The maximum ending color to create a particle with.
        /// </summary>
        public Color MaximumEndColor { get; set; } = new Color(0f, 0f, 0f, 0f);

        /// <summary>
        /// The minimum rotation speed to spawn a particle with. Defaults to zero, as that is more performant.
        /// </summary>
        public float MinimumRotationSpeed { get; set; } = 0;
        /// <summary>
        /// The maximum rotation speed to spawn a particle with. Defaults to zero, as that is more performant.
        /// </summary>
        public float MaximumRotationSpeed { get; set; } = 0;

        /// <summary>
        /// The minimum starting size of a particle.
        /// </summary>
        public Vector2 MinimumStartSize { get; set; } = new(1f, 1f);
        /// <summary>
        /// The maximum starting size of a particle.
        /// </summary>
        public Vector2 MaximumStartSize { get; set; } = new(1f, 1f);

        /// <summary>
        /// The minimum ending size of a particle.
        /// </summary>
        public Vector2 MinimumEndSize { get; set; } = new(0f, 0f);
        /// <summary>
        /// The maximum ending size of a particle.
        /// </summary>
        public Vector2 MaximumEndSize { get; set; } = new(0f, 0f);
    }
}
