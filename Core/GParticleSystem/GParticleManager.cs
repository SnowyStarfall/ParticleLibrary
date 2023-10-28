using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using Terraria.ModLoader;

namespace ParticleLibrary.Core
{
	public class GParticleManager : ModSystem
	{
		/// <summary>
		/// Access to the particle config
		/// </summary>
		public static ParticleLibraryConfig Config => ParticleLibraryConfig.Instance;
		/// <summary>
		/// All registered <see cref="GParticleSystem"/>s
		/// </summary>
		public static FastList<GParticleSystem> Systems { get; private set; }

		/// <summary>
		/// The maximum amount of GPU particles allowed
		/// </summary>
		public static int MaximumParticleBudget => Config.MaxGPUParticles;
		/// <summary>
		/// The current GPU particle budget, taking into account the requirements of all registered systems
		/// </summary>
		public static int FreeParticleBudget { get; private set; } = Config.MaxGPUParticles;

		internal static GParticleSystem ParticleSystem;

		public override void Load()
		{
			Systems = new();

			// Testing purposes
			ParticleSystem = new(ModContent.Request<Texture2D>(Resources.Assets.Textures.Star, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, 100000, 180);
		}

		public override void Unload()
		{
		}

		internal static GParticleSystem AddSystem(GParticleSystem system)
		{
			if (system is null)
				throw new ArgumentNullException(nameof(system));

			return system;
		}
	}
}
