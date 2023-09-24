using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core.Systems.ParticleSystem
{
    public class ColorSettings
    {
        public bool UseHSL { get; set; } = false;

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
        public Color MaximumEndColor { get; set; } = Color.White;

        /// <summary>
        /// The minimum starting HSL to create a particle with. Only used with <see cref="UseHSL"/>.
        /// </summary>
        public Vector3 MinimumStartHSL { get; set; } = new(0f, 0f, 1f);
        /// <summary>
        /// The maximum starting HSL to create a particle with. Only used with <see cref="UseHSL"/>.
        /// </summary>
        public Vector3 MaximumStartHSL { get; set; } = new(1f, 0f, 1f);

        /// <summary>
        /// The minimum ending HSL to create a particle with. Only used with <see cref="UseHSL"/>.
        /// </summary>
        public Vector3 MinimumEndHSL { get; set; } = new(0f, 0f, 0f);
        /// <summary>
        /// The maximum ending HSL to create a particle with. Only used with <see cref="UseHSL"/>.
        /// </summary>
        public Vector3 MaximumEndHSL { get; set; } = new(1f, 0f, 0f);
    }
}
