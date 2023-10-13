using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ParticleLibrary.Core.Systems.Test
{
	public class GParticleManager : ModSystem
	{
		public GParticleManager Instance { get; private set; }

		public static GParticleSystem ParticleSystem;

		public override void Load()
		{
			Instance = this;

			GParticleSystemSettings settings = new(ModContent.Request<Texture2D>(Resources.Assets.Textures.Star, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, 100000, 180);
			ParticleSystem = new(settings);
		}

		public override void Unload()
		{
			Instance = null;
		}

		// TODO: Implement AddSystem and system management
		public static GParticleSystem AddSystem(Texture2D texture)
		{
			return null;
		}
	}
}
