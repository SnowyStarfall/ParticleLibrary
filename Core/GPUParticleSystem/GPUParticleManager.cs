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
	public class GPUParticleManager : ModSystem
	{
		/// <summary>
		/// Access to the particle config
		/// </summary>
		public static ParticleLibraryConfig Config => ParticleLibraryConfig.Instance;
		/// <summary>
		/// All registered <see cref="QuadParticleSystem"/>
		/// </summary>
		public static FastList<QuadParticleSystem> QuadSystems { get; private set; }
		/// <summary>
		/// All registered <see cref="QuadParticleSystem"/>
		/// </summary>
		public static FastList<PointParticleSystem> PointSystems { get; private set; }

		/// <summary>
		/// The maximum amount of Quad particles allowed
		/// </summary>
		public static int MaximumQuadParticleBudget => Config.MaxQuadParticles;
		/// <summary>
		/// The current Quad particle budget, taking into account the requirements of all registered systems
		/// </summary>
		public static int FreeQuadParticleBudget { get; private set; } = Config.MaxQuadParticles;

		/// <summary>
		/// The maximum amount of Point particles allowed
		/// </summary>
		public static int MaximumPointParticleBudget => Config.MaxQuadParticles;
		/// <summary>
		/// The current Point particle budget, taking into account the requirements of all registered systems
		/// </summary>
		public static int FreePointParticleBudget { get; private set; } = Config.MaxQuadParticles;

		internal static QuadParticleSystem TestQuadParticleSystem;
		internal static PointParticleSystem TestPointParticleSystem;

		public override void Load()
		{
			QuadSystems = new();
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

		internal static GPUParticleSystem<TSettings, TParticle, TVertex> AddSystem<TSettings, TParticle, TVertex>(GPUParticleSystem<TSettings, TParticle, TVertex> system)
			where TSettings : GPUSystemSettings
			where TParticle : GPUParticle
			where TVertex : IVertexType
		{
			if (system is null)
			{
				throw new ArgumentNullException(nameof(system));
			}

			if (system is QuadParticleSystem g)
			{
				QuadSystems.Add(g);
			}
			else if (system is PointParticleSystem p)
			{
				PointSystems.Add(p);
			}

			return system;
		}

		internal static void RemoveSystem<TSettings, TParticle, TVertex>(GPUParticleSystem<TSettings, TParticle, TVertex> system)
			where TSettings : GPUSystemSettings
			where TParticle : GPUParticle
			where TVertex : IVertexType
		{
			if (system is null)
			{
				throw new ArgumentNullException(nameof(system));
			}

			if (system is QuadParticleSystem g)
			{
				QuadSystems.Remove(g);
			}
			else if (system is PointParticleSystem p)
			{
				PointSystems.Remove(p);
			}
		}
	}
}
