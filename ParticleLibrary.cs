using Terraria.ModLoader;

namespace ParticleLibrary
{
	/// <summary>
	/// </summary>
	public class ParticleLibrary : Mod
	{
		public override void Load()
		{
			ParticleLibraryDetours.Load();
			ParticleManager.Load();
		}
		public override void Unload()
		{
			ParticleLibraryDetours.Unload();
			ParticleManager.Unload();
		}
		
	}
}