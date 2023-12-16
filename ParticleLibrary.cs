using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;
using Terraria.ModLoader;

namespace ParticleLibrary
{
	public class ParticleLibrary : Mod
	{
		/// <summary>
		/// Empty 1x1 texture
		/// </summary>
		public static Texture2D EmptyPixel { get; private set; }

		/// <summary>
		/// White 1x1 texture
		/// </summary>
		public static Texture2D WhitePixel { get; private set; }

		public override void Load()
		{
			EmptyPixel = ModContent.Request<Texture2D>(Resources.Assets.Textures.EmptyPixel, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			WhitePixel = ModContent.Request<Texture2D>(Resources.Assets.Textures.WhitePixel, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			EmitterSettings.Load();
		}

		public override void Unload()
		{
			EmptyPixel = null;

			EmitterSettings.Unload();
			ParticleLibraryConfig.Unload();
		}
	}
}