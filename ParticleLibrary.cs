using log4net;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;
using ParticleLibrary.Core.V3.Emission;
using Terraria.ModLoader;

namespace ParticleLibrary
{
	public class ParticleLibrary : Mod
	{
		public static ParticleLibrary Instance { get; private set; }

		/// <summary>
		/// Empty 1x1 texture
		/// </summary>
		public static Texture2D EmptyPixel { get; private set; }

		/// <summary>
		/// White 1x1 texture
		/// </summary>
		public static Texture2D WhitePixel { get; private set; }

		public static bool Debug
		{
			get {
#if DEBUG
				return true;
#else
				return false;
#endif
			}
		}

		internal static ILog Log => Instance.Logger;

		public override void Load()
		{
			Instance = this;

			EmptyPixel = ModContent.Request<Texture2D>(Resources.Assets.Textures.EmptyPixel, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			WhitePixel = ModContent.Request<Texture2D>(Resources.Assets.Textures.WhitePixel, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			EmitterSettings.Load();
		}

		public override void Unload()
		{
			Instance = null;

			EmptyPixel = null;
			WhitePixel = null;

			EmitterSettings.Unload();
			ParticleLibraryConfig.Unload();
		}
	}
}