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
		[DefaultValue(15000)]
		[Range(0, 50000)]
		[Increment(1000)]
		public int MaxCPUParticles;

		[DefaultValue(50000)]
		[Range(0, 250000)]
		[Increment(1000)]
		public int MaxGPUParticles;

		[Header("Debug")]
		[DefaultValue(false)]
		public bool DebugUI;
	}
}
