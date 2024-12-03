using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace ParticleLibrary.Core
{
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Particles instead")]
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
			ArgumentNullException.ThrowIfNull(system);

			_quadSystems.Add(system);
		}

		internal static void RemoveQuadSystem(QuadParticleSystem system)
		{
			ArgumentNullException.ThrowIfNull(system);

			_quadSystems.Remove(system);
		}

		internal static void AddPointSystem(PointParticleSystem system)
		{
			ArgumentNullException.ThrowIfNull(system);

			_pointSystems.Add(system);
		}

		internal static void RemovePointSystem(PointParticleSystem system)
		{
			ArgumentNullException.ThrowIfNull(system);

			_pointSystems.Remove(system);
		}

		internal static int GetAdjustedMaxParticles(int maxParticles)
		{
			return ParticleLibraryConfig.Instance.ParticleLimit switch
			{
				ParticleLimit.None => 0,
				ParticleLimit.Low => maxParticles / 8,
				ParticleLimit.Medium => maxParticles / 4,
				ParticleLimit.High => maxParticles / 2,
				ParticleLimit.Unlimited => maxParticles,
				_ => maxParticles / 4
			};
		}
	}
}
