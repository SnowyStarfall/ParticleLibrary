using Microsoft.Xna.Framework;
using ParticleLibrary.Utilities;

namespace ParticleLibrary.UI.Themes
{
	public class DarkTheme : Theme
	{
		public override Color Low => LibUtilities.FromHex(0x1a1823ff);
		public override Color Mid => LibUtilities.FromHex(0x292736ff);
		public override Color High => LibUtilities.FromHex(0x363446ff);

		public override Color LowAccent => LibUtilities.FromHex(0x4c4960ff);
		public override Color MidAccent => LibUtilities.FromHex(0x55409aff);
		public override Color HighAccent => LibUtilities.FromHex(0x9ebae2ff);

		public override Color Error => LibUtilities.FromHex(0x9ebae2ff);
	}
}
