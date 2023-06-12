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
		[Label("Max Particles")]
		[Tooltip("Maximum particles allowed at once.\nNote: Performance is tested with 6000 as max.\nAny higher cannot guarantee a (mostly) lag-free experience.\nA value of 0 will disable the system.")]
		[DefaultValue(6000)]
		[Range(0, 50000)]
		[Increment(100)]
		public int MaxParticles;

		[Header("Debug")]
		[Label("Debug UI")]
		[Tooltip("Whether the Debug UI is enabled. Currently a WIP. Emitter debugging is not fully functional. The UI IS and WILL BE buggy. Texture errors are normal.")]
		[DefaultValue(false)]
		public bool DebugUI;
	}
}
