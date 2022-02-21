using Terraria.ModLoader;

namespace ParticleLibrary
{
	/// <summary>
	/// </summary>
	public class ParticleLibrary : Mod
	{
		public override void Unload()
		{
			ParticleLibraryConfig.Instance = null;
		}
		public override void PreSaveAndQuit()
		{
			ParticleManager.Dispose();
		}
	}
}