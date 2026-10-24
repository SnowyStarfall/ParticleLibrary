using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ParticleLibrary
{
	public class ParticleLibraryConfig : ModConfig
	{
		public static ParticleLibraryConfig Instance;

		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("Particles")]
		[DefaultValue(15000)]
		[Range(0, 50000)]
		[Increment(1000)]
		public int MaxParticles;

		[DefaultValue(50000)]
		[Range(0, 250000)]
		[Increment(1000)]
		public int MaxQuadParticles;

		[DefaultValue(100000)]
		[Range(0, 500000)]
		[Increment(1000)]
		public int MaxPointParticles;

		[DefaultValue(ParticleLimit.High)]
		[DrawTicks]
		[ReloadRequired]
		public ParticleLimit ParticleLimit;
	}

	public enum ParticleLimit
	{
		None,
		Low,
		Medium,
		High,
		Unlimited
	}
}
