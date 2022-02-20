using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using Terraria.ModLoader;

namespace Embers
{
	internal class ParticleLibraryDetours : ModSystem
	{
		public override void OnModLoad()
		{
			On.Terraria.Main.DrawDust += Main_DrawDust;
		}
		public override void Unload()
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
