using ParticleLibrary.Utilities;
using Terraria.ModLoader;

namespace ParticleLibrary.Core
{
	internal class PointParticleManager : ModSystem
	{
		/// <summary>
		/// Access to the particle config
		/// </summary>
		public static ParticleLibraryConfig Config => ParticleLibraryConfig.Instance;
		/// <summary>
		/// All registered <see cref="GParticleSystem"/>s
		/// </summary>
		public static FastList<PointParticleSystem> Systems { get; private set; }

		/// <summary>
		/// The maximum amount of GPU particles allowed
		/// </summary>
		public static int MaximumParticleBudget => Config.MaxGPUParticles;
		/// <summary>
		/// The current GPU particle budget, taking into account the requirements of all registered systems
		/// </summary>
		public static int FreeParticleBudget { get; private set; } = Config.MaxGPUParticles;

		internal static PointParticleSystem ParticleSystem;

		public override void Load()
		{
			Systems = new();

			// Testing purposes
			ParticleSystem = new(100000, 180);
		}

		public override void Unload()
		{
		}
	}
}
