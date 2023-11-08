using Microsoft.Xna.Framework;

namespace ParticleLibrary.UI.Themes
{
	public abstract class Theme
	{
		public abstract Color Low { get; }
		public abstract Color Mid { get; }
		public abstract Color High { get; }

		public abstract Color LowAccent { get; }
		public abstract Color MidAccent { get; }
		public abstract Color HighAccent { get; }

		public abstract Color Error { get; }
	}
}
