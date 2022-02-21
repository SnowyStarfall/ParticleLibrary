using Terraria.ModLoader;

namespace ParticleLibrary
{
	/// <summary>
	/// </summary>
	public class ParticleLibrary : Mod
	{
		public override void Load()
		{
			ParticleManager.Load();
		}
		public override void Unload()
		{
			ParticleManager.Unload();
		}
		
	}
}