using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ParticleLibrary.Core.PointParticleSystem
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
			ParticleSystem = new(100000, 3600);
		}

		public override void Unload()
		{
		}
	}
}
