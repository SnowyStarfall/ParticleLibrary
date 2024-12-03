using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace ParticleLibrary.Core.Data
{
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Emission instead")]
	public struct VisualParameters
	{
		public Color StartColor;
		public Color EndColor;

		public VisualParameters(EmitterColorSettings settings)
		{
			if (settings.UseHSLA)
			{
				StartColor = Main.hslToRgb(
					Main.rand.NextFloat(settings.MinimumStartHSLA.X, settings.MaximumStartHSLA.X + float.Epsilon),
					Main.rand.NextFloat(settings.MinimumStartHSLA.Y, settings.MaximumStartHSLA.Y + float.Epsilon),
					Main.rand.NextFloat(settings.MinimumStartHSLA.Z, settings.MaximumStartHSLA.Z + float.Epsilon),
					(byte)(255 * Main.rand.NextFloat(settings.MinimumStartHSLA.W, settings.MaximumStartHSLA.W + float.Epsilon)));

				EndColor = Main.hslToRgb(
					Main.rand.NextFloat(settings.MinimumEndHSLA.X, settings.MaximumEndHSLA.X + float.Epsilon),
					Main.rand.NextFloat(settings.MinimumEndHSLA.Y, settings.MaximumEndHSLA.Y + float.Epsilon),
					Main.rand.NextFloat(settings.MinimumEndHSLA.Z, settings.MaximumEndHSLA.Z + float.Epsilon),
					(byte)(255 * Main.rand.NextFloat(settings.MinimumEndHSLA.W, settings.MaximumEndHSLA.W + float.Epsilon)));
			}
			else
			{
				StartColor = Color.Lerp(settings.MinimumStartColor, settings.MaximumStartColor, Main.rand.NextFloat(0f, 1f + float.Epsilon));
				EndColor = Color.Lerp(settings.MinimumEndColor, settings.MaximumEndColor, Main.rand.NextFloat(0f, 1f + float.Epsilon));
			}
		}
	}
}
