using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleLibrary
{
	/// <summary>
	/// </summary>
	public class ParticleLibrary : Mod
	{
		public static Texture2D emptyPixel;
		public override void Load()
		{
			ParticleManager.Load();
			emptyPixel = ModContent.GetTexture("ParticleLibrary/EmptyPixel");
		}
		public override void Unload()
		{
			ParticleManager.Unload();
			ParticleLibraryConfig.Instance = null;
		}
		public override void PreSaveAndQuit()
		{
			ParticleManager.Dispose();
		}
	}
}