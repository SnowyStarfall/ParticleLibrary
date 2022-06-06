using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.EmitterSystem;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ParticleLibrary
{
	public class EmitterManager : ModSystem
	{
		public delegate Emitter NewEmitterCreated(Vector2 Position, Emitter Emitter, string Data = null, float CullDistance = -1f);
		public static event NewEmitterCreated OnNewEmitter;
		/// <summary>
		/// List of emitters.
		/// </summary>
		public static List<Emitter> emitters;
		public override void Load()
		{
			emitters = new();
			On.Terraria.Main.DrawDust += DrawEmitters;
		}
		public override void Unload()
		{
			emitters = null;
			On.Terraria.Main.DrawDust -= DrawEmitters;
		}
		public override void LoadWorldData(TagCompound tag)
		{
			List<EmitterSerializer> e = tag.Get<List<EmitterSerializer>>("ParticleLibrary: Emitters");
			e.RemoveAll(x => x == null);
			emitters = e.ConvertAll((o) => o.emitter);

			if (emitters == null)
				emitters = new();
		}
		public override void SaveWorldData(TagCompound tag)
		{
			if (emitters == null)
				emitters = new();
			
			List<EmitterSerializer> e =  emitters.ConvertAll<EmitterSerializer>((o) => new(o));
			e.RemoveAll(x => x == null);
			tag.Add("ParticleLibrary: Emitters", e);
		}
		public override void PreUpdateWorld()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < emitters.Count; i++)
				{
					if (Main.LocalPlayer == null || !Main.LocalPlayer.active)
						continue;
					if (Main.LocalPlayer.Distance(emitters[i].position) <= emitters[i].CullDistance)
						emitters[i].AI();
				}
			}
		}
		private void DrawEmitters(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
				for (int i = 0; i < emitters?.Count; i++)
				{
					Emitter emitter = emitters[i];
					if (emitter != null)
						emitter.Draw(Main.spriteBatch, emitter.VisualPosition, Lighting.GetColor((int)(emitter.position.X / 16), (int)(emitter.position.Y / 16)));
				}
				Main.spriteBatch.End();
			}
			orig(self);
		}
		/// <summary>
		/// Creates a new instance of an emitter Type.
		/// </summary>
		/// <typeparam name="T">The emitter.</typeparam>
		/// <returns>A new instance of this emitter</returns>
		public static Emitter NewInstance<T>() where T : Emitter
		{
			return Activator.CreateInstance<T>();
		}
		/// <summary>
		/// Spawns a new emitter at the desired position.
		/// </summary>
		/// <typeparam name="T">The emitter.</typeparam>
		/// <param name="Position">The emitter's position.</param>
		/// <param name="Data">Custom string data.</param>
		/// <param name="CullDistance">The maximum distance before the emitter no longer runs.</param>
		public static Emitter NewEmitter<T>(Vector2 Position, string Data = null, float CullDistance = -1f) where T : Emitter
		{
			Emitter Emitter = Activator.CreateInstance<T>();
			return NewEmitter(Position, Emitter, Data, CullDistance);
		}
		/// <summary>
		/// Spawns a new emitter at the desired position.
		/// </summary>
		/// <param name="Position">The emitter's position.</param>
		/// <param name="Emitter">The emitter type.</param>
		/// <param name="Data">Defaults to "ModName: EmitterName". If the mod can't be found by the Assembly name, then defaults to "AssemblyName: EmitterName".</param>
		/// <param name="CullDistance">Defaults to the largest screen dimension.</param>
		public static Emitter NewEmitter(Vector2 Position, Emitter Emitter, string Data = null, float CullDistance = -1f)
		{
			Emitter type = (Emitter)Activator.CreateInstance(Emitter.GetType());

			type.position = Position;
			if (Data != null)
				type.Data = Data;
			else
			{
				bool parsed = ModLoader.TryGetMod(type.GetType().Assembly.GetName().Name, out Mod mod);
				if (parsed == true)
					type.Data = mod.Name + ": " + type.GetType().Name;
				else
					type.Data = type.GetType().Assembly.GetName().Name + ": " + type.GetType().Name;
				Data = type.Data;
			}
			if (CullDistance != -1)
				type.CullDistance = CullDistance;
			emitters?.Add(type);

			OnNewEmitter?.Invoke(Position, type, Data, CullDistance);
			return type;
		}

		/// Search Utilities

		/// <summary>
		/// Kills a specified emitter.
		/// </summary>
		/// <param name="emitter"></param>
		public static void Remove(Emitter emitter) => emitters.Remove(emitter);
		/// <summary>
		/// Kills all emitters with matching data.
		/// </summary>
		/// <param name="Data"></param>
		public static void Remove(string Data) => emitters.RemoveAll(x => x.Data == Data);
		/// <summary>
		/// Kills all emitters that fulfill the conditions.
		/// </summary>
		/// <param name="predicate"></param>
		public static void Remove(Predicate<Emitter> predicate) => emitters.RemoveAll(predicate);
		/// <summary>
		/// Returns an emitter with matching data.
		/// </summary>
		/// <param name="Data"></param>
		public static Emitter Find(string Data) => emitters.Find(x => x.Data == Data);
		/// <summary>
		/// Returns all emitters that fulfill the conditions.
		/// </summary>
		/// <param name="predicate"></param>
		public static List<Emitter> Find(Predicate<Emitter> predicate) => emitters.FindAll(predicate);
	}
}
