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
			EmitterManager.Load();
			emptyPixel = ModContent.GetTexture("ParticleLibrary/EmptyPixel");
		}
		public override void Unload()
		{
			ParticleManager.Unload();
			EmitterManager.Unload();
			emptyPixel = null;
			ParticleLibraryConfig.Instance = null;
		}
		public override void PreUpdateEntities()
		{
			EmitterManager.PreUpdateWorld();
		}
		public override void PreSaveAndQuit()
		{
			ParticleManager.Dispose();
		}
	}
}