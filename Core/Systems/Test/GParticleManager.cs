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

			ParticleSystem = new(ModContent.Request<Texture2D>(Resources.Debug.Plus, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		}

		public override void Unload()
		{
			Instance = null;
		}

		public static GParticleSystem AddSystem(Texture2D texture)
		{
			return null;
		}
	}
}
