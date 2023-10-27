using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using System.Diagnostics;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace ParticleLibrary.Core
{
    public class GParticleManager : ModSystem
    {
        public static ParticleLibraryConfig Config => ParticleLibraryConfig.Instance;
        public static FastList<GParticleSystem> Systems { get; private set; }

		internal static GParticleSystem ParticleSystem;

        public override void Load()
        {
            Systems = new();

            ParticleSystem = new(ModContent.Request<Texture2D>(Resources.Assets.Textures.Star, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, 100000, 180);
        }

        public override void Unload()
        {
        }

		public static GParticleSystem AddSystem(GParticleSystem system)
        {
            if(system is null)
                throw new ArgumentNullException(nameof(system));



            return system;
        }
    }
}
