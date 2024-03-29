﻿using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public static IReadOnlyCollection<QuadParticleSystem> QuadSystems { get => _quadSystems.Buffer.ToList().AsReadOnly(); }
		private static FastList<QuadParticleSystem> _quadSystems;
		/// <summary>
		/// All registered <see cref="QuadParticleSystem"/>
		/// </summary>
		public static IReadOnlyCollection<PointParticleSystem> PointSystems { get => _pointSystems.Buffer.ToList().AsReadOnly(); }
		private static FastList<PointParticleSystem> _pointSystems;

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
			_quadSystems = new();
			_pointSystems = new();

			// Testing purposes
			//GParticleSystemSettings gs = new(ModContent.Request<Texture2D>(Resources.Assets.Textures.Star, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, 10000, 180/*, 1000*/);
			//PointParticleSystemSettings ps = new(100000, 180);

			//TestGParticleSystem = new(gs);
			//TestPParticleSystem = new(ps);
		}

		public override void OnWorldUnload()
		{
			foreach (var system in _quadSystems.Buffer)
			{
				system?.Clear();
			}

			foreach (var system in _pointSystems.Buffer)
			{
				system?.Clear();
			}
		}

		public override void Unload()
		{
			_quadSystems = null;
			_pointSystems = null;
		}

		internal static void AddQuadSystem(QuadParticleSystem system)
		{
			if (system is null)
			{
				throw new ArgumentNullException(nameof(system));
			}

			_quadSystems.Add(system);
			FreeQuadParticleBudget -= system.MaxParticles;
		}

		internal static void RemoveQuadSystem(QuadParticleSystem system)
		{
			if (system is null)
			{
				throw new ArgumentNullException(nameof(system));
			}

			_quadSystems.Remove(system);
			FreeQuadParticleBudget += system.MaxParticles;
		}

		internal static void AddPointSystem(PointParticleSystem system)
		{
			if (system is null)
			{
				throw new ArgumentNullException(nameof(system));
			}

			_pointSystems.Add(system);
			FreePointParticleBudget -= system.MaxParticles;
		}

		internal static void RemovePointSystem(PointParticleSystem system)
		{
			if (system is null)
			{
				throw new ArgumentNullException(nameof(system));
			}

			_pointSystems.Remove(system);
			FreePointParticleBudget += system.MaxParticles;
		}
	}
}
