using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// Base class for all emitters. Inherit this class to create your own emitter.
	/// </summary>
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

		public EmitterSettings EmitterSettings { get; private set; }
		public EmitterParticleSettings ParticleSettings { get; private set; }
		public EmitterColorSettings ColorSettings { get; private set; }

		public RectangleF Bounds => EmitterSettings.Bounds;

		/// <summary>
		/// Creates a new emitter. You must call <see cref="EmitterManager.NewEmitter(Emitter)"/> or <see cref="EmitterManager.NewEmitter{T}(EmitterSettings, EmitterParticleSettings, EmitterColorSettings)"/> to add it to the system.
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

		public Emitter() : this(null, null, null)
		{
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
		}

		/// <summary>
		/// Spawns a particle with the provided parameters.
		/// </summary>
		public virtual void SpawnParticle()
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
			EmitterManager.Emitters?.Remove(this);
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
