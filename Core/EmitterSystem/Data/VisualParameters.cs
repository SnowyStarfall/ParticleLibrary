using Microsoft.Xna.Framework;
using Terraria;

namespace ParticleLibrary.Core.EmitterSystem.Data
{
	public class VisualParameters
	{
		public Color StartColor;
		public Color EndColor;

		public static VisualParameters Calculate(EmitterColorSettings settings)
		{
			VisualParameters visual = new();

			if (settings.UseHSLA)
			{
				visual.StartColor = Main.hslToRgb(
					Main.rand.NextFloat(settings.MinimumStartHSLA.X, settings.MaximumStartHSLA.X + float.Epsilon),
					Main.rand.NextFloat(settings.MinimumStartHSLA.Y, settings.MaximumStartHSLA.Y + float.Epsilon),
					Main.rand.NextFloat(settings.MinimumStartHSLA.Z, settings.MaximumStartHSLA.Z + float.Epsilon),
					(byte)(255 * Main.rand.NextFloat(settings.MinimumStartHSLA.W, settings.MaximumStartHSLA.W + float.Epsilon)));

				visual.EndColor = Main.hslToRgb(
					Main.rand.NextFloat(settings.MinimumEndHSLA.X, settings.MaximumEndHSLA.X + float.Epsilon),
					Main.rand.NextFloat(settings.MinimumEndHSLA.Y, settings.MaximumEndHSLA.Y + float.Epsilon),
					Main.rand.NextFloat(settings.MinimumEndHSLA.Z, settings.MaximumEndHSLA.Z + float.Epsilon),
					(byte)(255 * Main.rand.NextFloat(settings.MinimumEndHSLA.W, settings.MaximumEndHSLA.W + float.Epsilon)));
			}
			else
			{
				visual.StartColor = Color.Lerp(settings.MinimumStartColor, settings.MaximumStartColor, Main.rand.NextFloat(0f, 1f + float.Epsilon));
				visual.EndColor = Color.Lerp(settings.MinimumEndColor, settings.MaximumEndColor, Main.rand.NextFloat(0f, 1f + float.Epsilon));
			}

			return visual;
		}
	}
}
