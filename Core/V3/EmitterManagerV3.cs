using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using EmitterV3 = ParticleLibrary.Core.V3.Emission.Emitter;
using EmitterSerializerV3 = ParticleLibrary.Core.V3.Emission.EmitterSerializer;

namespace ParticleLibrary.Core.V3
{
	/// <summary>
	/// Manages the emitters in the world.
	/// </summary>
	public class EmitterManagerV3 : ModSystem
	{
		private const string EMITTER_TAG_COMPOUND = "ParticleLibrary.V3.Emitters";

		/// <summary>
		/// List of emitters.
		/// </summary>
		private static FastList<EmitterV3> _emitters;

		/// <summary>
		/// Shorthand for screen location as rectangle
		/// </summary>
		public Rectangle ScreenLocation => new((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

		public override void Load()
		{
			_emitters = new();
		}

		public override void Unload()
		{
			_emitters = null;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			Mod.Logger.Info("Loading emitter data...");

			_emitters ??= new();

			var emitters = tag.Get<List<EmitterSerializerV3>>(EMITTER_TAG_COMPOUND).ToList()
				.ConvertAll((o) => o.Emitter)
				.Where((x) => x is not null);

			_emitters.AddRange(emitters);

			Mod.Logger.Info("...Loading complete");
		}

		public override void SaveWorldData(TagCompound tag)
		{
			Mod.Logger.Info("Saving emitter data...");

			_emitters ??= new();

			List<EmitterSerializerV3> c = _emitters.Buffer.Where(x => x is not null)
				.ToList()
				.ConvertAll<EmitterSerializerV3>((o) => new(o));

			tag.Add(EMITTER_TAG_COMPOUND, c);

			_emitters.Clear();

			Mod.Logger.Info("...Saving complete");
		}

		public override void PreUpdateWorld()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				foreach (var emitter in _emitters.Buffer)
				{
					if (Main.LocalPlayer?.active != true)
					{
						continue;
					}

					if (emitter is null)
					{
						return;
					}

					if (ScreenLocation.Intersects(new Rectangle((int)emitter.Position.X - emitter.Size.X / 2, (int)emitter.Position.Y - emitter.Size.Y / 2, emitter.Size.X, emitter.Size.Y)))
					{
						emitter.Update();
					}
				}
			}
		}

		/// <summary>
		/// Spawns a new emitter at the desired position.
		/// </summary>
		/// <typeparam name="T">The emitter.</typeparam>
		/// <param name="position">Emitter position.</param>
		/// <param name="size">Emitter size.</param>
		/// <returns>The resulting emitter.</returns>
		public static EmitterV3 NewEmitter<T>(in Vector2 position, in Point size) where T : EmitterV3, new()
		{
			EmitterV3 emitter = new T()
			{
				Position = position,
				Size = size
			};

			_emitters.Add(emitter);
			return emitter;
		}

		/// <summary>
		/// Adds an emitter to the emitter manager.
		/// </summary>
		public static void NewEmitter(EmitterV3 emitter)
		{
			_emitters.Add(emitter);
		}

		/// <summary>
		/// Kills a specified emitter.
		/// </summary>
		/// <param name="emitter"></param>
		public static void Remove(EmitterV3 emitter) => _emitters.Remove(emitter);

		/// <summary>
		/// Kills all emitters that fulfill the conditions.
		/// </summary>
		/// <param name="predicate"></param>
		public static void Remove(Predicate<EmitterV3> predicate)
		{
			for (int i = 0; i < _emitters.Length; i++)
			{
				if (predicate(_emitters[i]))
				{
					_emitters.RemoveAt(i);
					i--;
				}
			}
		}
	}
}
