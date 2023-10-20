using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core
{
	public class EmitterColorSettings
	{
		public bool UseHSLA { get; set; } = false;

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
		/// The minimum starting HSLA to create a particle with. Only used with <see cref="UseHSLA"/>.
		/// </summary>
		public Vector4 MinimumStartHSLA { get; set; } = new(0f, 0f, 1f, 1f);
		/// <summary>
		/// The maximum starting HSLA to create a particle with. Only used with <see cref="UseHSLA"/>.
		/// </summary>
		public Vector4 MaximumStartHSLA { get; set; } = new(1f, 0f, 1f, 1f);

		/// <summary>
		/// The minimum ending HSLA to create a particle with. Only used with <see cref="UseHSLA"/>.
		/// </summary>
		public Vector4 MinimumEndHSLA { get; set; } = new(0f, 0f, 0f, 1f);
		/// <summary>
		/// The maximum ending HSLA to create a particle with. Only used with <see cref="UseHSLA"/>.
		/// </summary>
		public Vector4 MaximumEndHSLA { get; set; } = new(1f, 0f, 0f, 1f);

		internal void SaveData(TagCompound tag)
		{
			tag.Set("UseHSL", UseHSLA);
			tag.Set("MinimumStartColor", MinimumStartColor);
			tag.Set("MaximumStartColor", MaximumStartColor);
			tag.Set("MinimumEndColor", MinimumEndColor);
			tag.Set("MaximumEndColor", MaximumEndColor);
			tag.Set("MinimumStartHSLA", MinimumStartHSLA);
			tag.Set("MaximumStartHSLA", MaximumStartHSLA);
			tag.Set("MinimumEndHSLA", MinimumEndHSLA);
			tag.Set("MaximumEndHSLA", MaximumEndHSLA);
		}

		internal void LoadData(TagCompound tag)
		{
			UseHSLA = tag.GetBool("UseHSLA");
			MinimumStartColor = tag.Get<Color>("MinimumStartColor");
			MaximumStartColor = tag.Get<Color>("MaximumStartColor");
			MinimumEndColor = tag.Get<Color>("MinimumEndColor");
			MaximumEndColor = tag.Get<Color>("MaximumEndColor");
			MinimumStartHSLA = tag.Get<Vector4>("MinimumStartHSLA");
			MaximumStartHSLA = tag.Get<Vector4>("MaximumStartHSLA");
			MinimumEndHSLA = tag.Get<Vector4>("MinimumEndHSLA");
			MaximumEndHSLA = tag.Get<Vector4>("MaximumEndHSLA");
		}
	}
}
