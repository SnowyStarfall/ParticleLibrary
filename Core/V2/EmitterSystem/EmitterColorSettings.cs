using Microsoft.Xna.Framework;
using ParticleLibrary.Utilities;
using System;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core
{
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Emission instead")]
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
			tag.Set("MinimumStartHSLAXY", new Vector2(MinimumStartHSLA.X, MinimumStartHSLA.Y));
			tag.Set("MinimumStartHSLAZW", new Vector2(MinimumStartHSLA.Z, MinimumStartHSLA.W));
			tag.Set("MaximumStartHSLAXY", new Vector2(MaximumStartHSLA.X, MaximumStartHSLA.Y));
			tag.Set("MaximumStartHSLAZW", new Vector2(MaximumStartHSLA.Z, MaximumStartHSLA.W));
			tag.Set("MinimumEndHSLAXY", new Vector2(MinimumEndHSLA.X, MinimumEndHSLA.Y));
			tag.Set("MinimumEndHSLAZW", new Vector2(MinimumEndHSLA.Z, MinimumEndHSLA.Y));
			tag.Set("MaximumEndHSLAXY", new Vector2(MaximumEndHSLA.X, MaximumEndHSLA.Y));
			tag.Set("MaximumEndHSLAZW", new Vector2(MaximumEndHSLA.Z, MaximumEndHSLA.Y));
		}

		internal void LoadData(TagCompound tag)
		{
			UseHSLA = tag.GetBool("UseHSLA");
			MinimumStartColor = tag.Get<Color>("MinimumStartColor");
			MaximumStartColor = tag.Get<Color>("MaximumStartColor");
			MinimumEndColor = tag.Get<Color>("MinimumEndColor");
			MaximumEndColor = tag.Get<Color>("MaximumEndColor");
			MinimumStartHSLA = LibUtilities.Vec4From2Vec2(tag.Get<Vector2>("MinimumStartHSLAXY"), tag.Get<Vector2>("MinimumStartHSLAZW"));
			MaximumStartHSLA = LibUtilities.Vec4From2Vec2(tag.Get<Vector2>("MaximumStartHSLAXY"), tag.Get<Vector2>("MaximumStartHSLAZW"));
			MinimumEndHSLA = LibUtilities.Vec4From2Vec2(tag.Get<Vector2>("MinimumEndHSLAXY"), tag.Get<Vector2>("MinimumEndHSLAZW"));
			MaximumEndHSLA = LibUtilities.Vec4From2Vec2(tag.Get<Vector2>("MaximumEndHSLAXY"), tag.Get<Vector2>("MaximumEndHSLAZW"));
		}
	}
}
