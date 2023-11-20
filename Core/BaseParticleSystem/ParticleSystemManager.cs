using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ParticleLibrary.Core
{
	public class ParticleSystemManager : ModSystem
	{
		/// <summary>
		/// Access to the particle config
		/// </summary>
		public static ParticleLibraryConfig Config => ParticleLibraryConfig.Instance;
		/// <summary>
		/// All registered <see cref="GParticleSystem"/>
		/// </summary>
		public static FastList<GParticleSystem> GPUSystems { get; private set; }
		/// <summary>
		/// All registered <see cref="GParticleSystem"/>
		/// </summary>
		public static FastList<PointParticleSystem> PointSystems { get; private set; }

		/// <summary>
		/// The maximum amount of GPU particles allowed
		/// </summary>
		public static int MaximumParticleBudget => Config.MaxGPUParticles;
		/// <summary>
		/// The current GPU particle budget, taking into account the requirements of all registered systems
		/// </summary>
		public static int FreeParticleBudget { get; private set; } = Config.MaxGPUParticles;

		internal static GParticleSystem TestGParticleSystem;
		internal static PointParticleSystem TestPParticleSystem;

		public override void Load()
		{
			PointSystems = new();

			// Testing purposes
			//GParticleSystemSettings gs = new(ModContent.Request<Texture2D>(Resources.Assets.Textures.Star, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, 10000, 180/*, 1000*/);
			//PointParticleSystemSettings ps = new(100000, 180);

			//TestGParticleSystem = new(gs);
			//TestPParticleSystem = new(ps);
		}

		public override void Unload()
		{
		}

		internal static BaseParticleSystem<TSettings, TParticle, TVertex> AddSystem<TSettings, TParticle, TVertex>(BaseParticleSystem<TSettings, TParticle, TVertex> system)
			where TSettings : BaseSystemSettings
			where TParticle : BaseParticle
			where TVertex : IVertexType
		{
			if (system is null)
			{
				throw new ArgumentNullException(nameof(system));
			}

			if (system is GParticleSystem g)
			{
				GPUSystems.Add(g);
			}
			else if (system is PointParticleSystem p)
			{
				PointSystems.Add(p);
			}

			return system;
		}

		internal static void RemoveSystem<TSettings, TParticle, TVertex>(BaseParticleSystem<TSettings, TParticle, TVertex> system)
			where TSettings : BaseSystemSettings
			where TParticle : BaseParticle
			where TVertex : IVertexType
		{
			if (system is null)
			{
				throw new ArgumentNullException(nameof(system));
			}

			if (system is GParticleSystem g)
			{
				GPUSystems.Remove(g);
			}
			else if (system is PointParticleSystem p)
			{
				PointSystems.Remove(p);
			}
		}
	}
}
