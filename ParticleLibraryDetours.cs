using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary
{
	internal class ParticleLibraryDetours
	{
		public static void Load()
		{
			On.Terraria.Main.DrawDust += Main_DrawDust;
		}
		public static void Unload()
		{
			On.Terraria.Main.DrawDust -= Main_DrawDust;
		}
		private static void Main_DrawDust(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			ParticleManager.PreUpdate();
			ParticleManager.Update(Main.spriteBatch);
			ParticleManager.PostUpdate(Main.spriteBatch);
			Main.spriteBatch.End();
			orig(self);
		}
	}
}
