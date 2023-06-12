using log4net;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.EmitterSystem;
using Terraria.ModLoader;

namespace ParticleLibrary
{
	public class ParticleLibrary : Mod
	{
		/// <summary>
		/// Empty 1x1 texture
		/// </summary>
		public static Texture2D EmptyPixel;

		public override void Load()
		{
			EmptyPixel = ModContent.Request<Texture2D>("ParticleLibrary/EmptyPixel", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void Unload()
		{
			EmptyPixel = null;
		}
	}
}