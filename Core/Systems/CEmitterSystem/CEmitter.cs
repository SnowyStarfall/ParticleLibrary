using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace ParticleLibrary.Core.Systems.OldEmitterSystem
{
    /// <summary>
    /// Base class for all emitters. Inherit this class to create your own emitter.
    /// </summary>
    public abstract class CEmitter : Entity
    {
        /// <summary>
        /// Originating mod.
        /// </summary>
        internal string Assembly;
        /// <summary>
        /// Originating type.
        /// </summary>
        internal string Type;
        /// <summary>
        /// Custom string Data for this emitter.
        /// </summary>
        public string Data;
        /// <summary>
        /// Whether this emitter should save when the world is exited.
        /// </summary>
        public bool Save = true;
        /// <summary>
        /// Minumum distance before AI is run. Measured by distance from Main.localPlayer to Emitter.
        /// </summary>
        public float CullDistance = MathHelper.Max(Main.screenWidth, Main.screenHeight);

        /// <summary>
        /// </summary>
        protected CEmitter()
        {
            SetDefaults();
            Assembly = GetType().Assembly.GetName().Name;
            Type = GetType().FullName;
        }

        /// <summary>
        /// Runs on instantiation.
        /// </summary>
        public virtual void SetDefaults()
        {
        }
        /// <summary>
        /// Runs on PreUpdateWorld.
        /// </summary>
        public virtual void AI()
        {
        }
        /// <summary>
        /// Runs on DrawDust.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 drawPos, Color lightColor)
        {
        }
        /// <summary>
        /// Kills this emitter.
        /// </summary>
        public void Kill()
        {
            CEmitterManager.emitters?.Remove(this);
        }
    }
}
