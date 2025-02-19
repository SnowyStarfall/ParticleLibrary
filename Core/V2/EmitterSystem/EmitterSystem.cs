﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// Manages the emitters in the world
	/// </summary>
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.EmitterManagerV3 instead")]
	public class EmitterSystem : ModSystem
	{
		/// <summary>
		/// List of emitters.
		/// </summary>
		public static FastList<Emitter> Emitters { get; private set; }

		/// <summary>
		/// Shorthand for screen location as rectangle
		/// </summary>
		public Rectangle ScreenLocation => new((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

		public override void Load()
		{
			Emitters = new();
			On_Main.DrawDust += Draw;
		}

		public override void Unload()
		{
			Emitters = null;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			Emitters ??= new();

			var emitters = tag.Get<List<EmitterSerializer>>("Emitters").ToList()
				.ConvertAll((o) => o.Emitter)
				.Where((x) => x is not null);

			Emitters.AddRange(emitters);
		}

		public override void SaveWorldData(TagCompound tag)
		{
			Emitters ??= new();

			List<EmitterSerializer> c = Emitters.Buffer.Where(x => x is not null)
				.ToList()
				.ConvertAll<EmitterSerializer>((o) => new(o));

			tag.Add("Emitters", c);

			Emitters.Clear();
		}

		public override void PreUpdateWorld()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				foreach (var emitter in Emitters.Buffer)
				{
					if (emitter is null)
						return;

					if (Main.LocalPlayer?.active != true)
						continue;

					if (ScreenLocation.Intersects(emitter.Bounds))
						emitter.Update();
				}
			}
		}

		private void Draw(On_Main.orig_DrawDust orig, Main self)
		{
			orig(self);

			if (Main.netMode != NetmodeID.Server)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

				foreach (var emitter in Emitters.Buffer)
				{
					emitter?.Draw(Main.spriteBatch, emitter.EmitterSettings.Position - Main.screenPosition);
				}

				Main.spriteBatch.End();
			}
		}

		/// <summary>
		/// Spawns a new emitter at the desired position.
		/// </summary>
		/// <typeparam name="T">The emitter.</typeparam>
		/// <param name="settings">Emitter settings.</param>
		/// <param name="particle">Particle spawn settings.</param>
		/// <param name="color">Particle color settings.</param>
		/// <returns>The resulting emitter.</returns>
		public static Emitter NewEmitter<T>(EmitterSettings settings = null, EmitterParticleSettings particle = null, EmitterColorSettings color = null) where T : Emitter
		{
			Emitter emitter = (Emitter)Activator.CreateInstance(typeof(T), settings, particle, color);
			return NewEmitter(emitter);
		}

		/// <summary>
		/// Spawns a new emitter at the desired position.
		/// </summary>
		/// <param name="emitter">Premade emitter.</param>
		/// <returns>The resulting emitter.</returns>
		public static Emitter NewEmitter(Emitter emitter)
		{
			Emitter e = emitter ?? (Emitter)Activator.CreateInstance(emitter.GetType());
			Emitters?.Add(e);
			return e;
		}

		/// <summary>
		/// Kills a specified emitter.
		/// </summary>
		/// <param name="emitter"></param>
		public static void Remove(Emitter emitter) => Emitters.Remove(emitter);

		/// <summary>
		/// Kills all emitters that fulfill the conditions.
		/// </summary>
		/// <param name="predicate"></param>
		public static void Remove(Predicate<Emitter> predicate)
		{
			for (int i = 0; i < Emitters.Length; i++)
			{
				if (predicate(Emitters[i]))
				{
					Emitters.RemoveAt(i);
					i--;
				}
			}
		}
	}
}
