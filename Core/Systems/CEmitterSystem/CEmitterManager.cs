using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core.Systems.OldEmitterSystem
{
	public class CEmitterManager : ModSystem
    {
        public delegate CEmitter NewEmitterCreated(Vector2 Position, CEmitter Emitter, string Data = null, float CullDistance = -1f);
        public static event NewEmitterCreated OnNewEmitter;

        /// <summary>
        /// List of emitters.
        /// </summary>
        public static List<CEmitter> emitters;

        public override void Load()
        {
            emitters = new();
            On_Main.DrawDust += DrawEmitters;
        }

        public override void Unload()
        {
            emitters = null;
            OnNewEmitter = null;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            List<CEmitterSerializer> e = tag.Get<List<CEmitterSerializer>>("ParticleLibrary: Emitters");
            e.RemoveAll(x => x == null);
            emitters = e.ConvertAll((o) => o.emitter) ?? new();
        }

        public override void SaveWorldData(TagCompound tag)
        {
            emitters ??= new();

            List<CEmitterSerializer> e = emitters.ConvertAll<CEmitterSerializer>((o) => new(o));
            e.RemoveAll(x => x == null);
            tag.Add("ParticleLibrary: Emitters", e);
        }

        public override void PreUpdateWorld()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < emitters.Count; i++)
                {
                    if (Main.LocalPlayer?.active != true)
                        continue;
                    if (Main.LocalPlayer.Distance(emitters[i].position) <= emitters[i].CullDistance)
                        emitters[i].AI();
                }
            }
        }

        private void DrawEmitters(On_Main.orig_DrawDust orig, Main self)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                for (int i = 0; i < emitters?.Count; i++)
                {
                    CEmitter emitter = emitters[i];
                    emitter?.Draw(Main.spriteBatch, emitter.VisualPosition, Lighting.GetColor((int)(emitter.position.X / 16), (int)(emitter.position.Y / 16)));
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
        public static CEmitter NewInstance<T>() where T : CEmitter
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
        public static CEmitter NewEmitter<T>(Vector2 Position, string Data = null, float CullDistance = -1f) where T : CEmitter
        {
            CEmitter Emitter = Activator.CreateInstance<T>();
            return NewEmitter(Position, Emitter, Data, CullDistance);
        }

        /// <summary>
        /// Spawns a new emitter at the desired position.
        /// </summary>
        /// <param name="Position">The emitter's position.</param>
        /// <param name="Emitter">The emitter type.</param>
        /// <param name="Data">Defaults to "ModName: EmitterName". If the mod can't be found by the Assembly name, then defaults to "AssemblyName: EmitterName".</param>
        /// <param name="CullDistance">Defaults to the largest screen dimension.</param>
        public static CEmitter NewEmitter(Vector2 Position, CEmitter Emitter, string Data = null, float CullDistance = -1f)
        {
            CEmitter type = (CEmitter)Activator.CreateInstance(Emitter.GetType());

            type.position = Position;
            if (Data != null)
            {
                type.Data = Data;
            }
            else
            {
                bool parsed = ModLoader.TryGetMod(type.GetType().Assembly.GetName().Name, out Mod mod);
                if (parsed)
                {
                    type.Data = mod.Name + ": " + type.GetType().Name;
                }
                else
                {
                    type.Data = type.GetType().Assembly.GetName().Name + ": " + type.GetType().Name;
                }
                Data = type.Data;
            }
            if (CullDistance != -1)
                type.CullDistance = CullDistance;
            emitters?.Add(type);

            OnNewEmitter?.Invoke(Position, type, Data, CullDistance);
            return type;
        }

        /// <summary>
        /// Kills a specified emitter.
        /// </summary>
        /// <param name="emitter"></param>
        public static void Remove(CEmitter emitter) => emitters.Remove(emitter);

        /// <summary>
        /// Kills all emitters with matching data.
        /// </summary>
        /// <param name="Data"></param>
        public static void Remove(string Data) => emitters.RemoveAll(x => x.Data == Data);

        /// <summary>
        /// Kills all emitters that fulfill the conditions.
        /// </summary>
        /// <param name="predicate"></param>
        public static void Remove(Predicate<CEmitter> predicate) => emitters.RemoveAll(predicate);

        /// <summary>
        /// Returns an emitter with matching data.
        /// </summary>
        /// <param name="Data"></param>
        public static CEmitter Find(string Data) => emitters.Find(x => x.Data == Data);

        /// <summary>
        /// Returns all emitters that fulfill the conditions.
        /// </summary>
        /// <param name="predicate"></param>
        public static List<CEmitter> Find(Predicate<CEmitter> predicate) => emitters.FindAll(predicate);
    }
}
