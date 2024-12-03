using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core.Data;
using System;
using Terraria;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// Base class for all emitters. Inherit this class to create your own emitter.
	/// </summary>
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Emission instead")]
	public abstract class Emitter
	{
		/// <summary>
		/// Originating mod.
		/// </summary>
		internal string Assembly { get; }
		/// <summary>
		/// Originating type.
		/// </summary>
		internal string Type { get; }

		public EmitterSettings EmitterSettings { get; set; }
		public EmitterParticleSettings ParticleSettings { get; set; }
		public EmitterColorSettings ColorSettings { get; set; }

		public Rectangle Bounds => EmitterSettings.Bounds;

		// Function fields
		protected int Timer;
		protected int SpawnTime;
		protected int SpawnCount;

		/// <summary>
		/// Creates a new emitter. You must call <see cref="EmitterSystem.NewEmitter(Emitter)"/> or <see cref="EmitterSystem.NewEmitter{T}(EmitterSettings, EmitterParticleSettings, EmitterColorSettings)"/> to add it to the system.
		/// </summary>
		/// <param name="emitterSettings"></param>
		/// <param name="particleSettings"></param>
		/// <param name="colorSettings"></param>
		public Emitter(EmitterSettings emitterSettings = null, EmitterParticleSettings particleSettings = null, EmitterColorSettings colorSettings = null)
		{
			Assembly = GetType().Assembly.GetName().Name;
			Type = GetType().FullName;

			EmitterSettings = emitterSettings ?? new();
			ParticleSettings = particleSettings ?? new();
			ColorSettings = colorSettings ?? new();

			Initialize();
		}

		/// <summary>
		/// Runs on instantiation.
		/// </summary>
		public virtual void Initialize()
		{
		}

		/// <summary>
		/// Runs on PreUpdateWorld.
		/// </summary>
		public virtual void Update()
		{
			// Spawn logic
			if (Timer >= SpawnTime)
			{
				Vector2 position = EmitterSettings.Position;

				for (int i = 0; i < SpawnCount; i++)
				{
					// Calculate parameters
					position = EmitterSettings.Shape.Solve(position, EmitterSettings.Origin, EmitterSettings.Width, EmitterSettings.Height);
					SpatialParameters spatial = new(ParticleSettings);
					VisualParameters visual = new(ColorSettings);

					// Spawn the particle
					SpawnParticle(position, spatial, visual);
				}

				// Reset for next spawn interval
				Timer = 0;
				SpawnTime = Main.rand.Next(EmitterSettings.MinimumInterval, EmitterSettings.MaximumInterval + 1);
				SpawnCount = Main.rand.Next(EmitterSettings.MinimumSpawns, EmitterSettings.MaximumSpawns + 1);
			}

			// Update time
			Timer++;
		}

		/// <summary>
		/// Spawns a particle with the provided parameters.
		/// </summary>
		public virtual void SpawnParticle(Vector2 position, in SpatialParameters spatial, in VisualParameters visual)
		{
		}

		/// <summary>
		/// Runs on DrawDust.
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch to use.</param>
		/// <param name="location">Visual location of the emitter, taking into account Main.ScreenPosition.</param>
		public virtual void Draw(SpriteBatch spriteBatch, Vector2 location)
		{
		}

		/// <summary>
		/// Kills this emitter.
		/// </summary>
		public void Kill()
		{
			EmitterSystem.Emitters?.Remove(this);
		}

		/// <summary>
		/// Allows saving custom emitter data. In most cases you won't need this.
		/// </summary>
		/// <param name="tag"></param>
		public virtual void SaveData(TagCompound tag)
		{
		}

		/// <summary>
		/// Allows reading custom emitter data. In most casts you won't need this.
		/// </summary>
		/// <param name="tag"></param>
		public virtual void LoadData(TagCompound tag)
		{
		}
	}
}
