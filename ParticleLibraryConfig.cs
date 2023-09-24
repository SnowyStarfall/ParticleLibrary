using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ParticleLibrary
{
	public class ParticleLibraryConfig : ModConfig
	{
		public static ParticleLibraryConfig Instance;

		public override void OnLoaded()
		{
			Instance = this;
		}

		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("Particles")]
		[DefaultValue(6000)]
		[Range(0, 50000)]
		[Increment(100)]
		public int MaxParticles;

		[Header("Debug")]
		[DefaultValue(false)]
		public bool DebugUI;
	}
}
