using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace TerraCompendium.Core.Utilities
{
	public class SpriteBatchSettings
	{
		public SpriteSortMode SortMode { get; set; }
		public BlendState BlendState { get; set; }
		public SamplerState SamplerState { get; set; }
		public DepthStencilState DepthStencilState { get; set; }
		public RasterizerState RasterizerState { get; set; }
		public Effect Effect { get; set; }
		public bool UI { get; set; }

		public Matrix TransformationMatrix => Main.GameViewMatrix.TransformationMatrix;
		public Matrix UIScaleMatrix => Main.UIScaleMatrix;

		public SpriteBatchSettings(SpriteSortMode? sortMode = null, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, bool ui = false)
		{
			SortMode = sortMode ?? SpriteSortMode.Immediate;
			BlendState = blendState ?? BlendState.AlphaBlend;
			SamplerState = samplerState ?? SamplerState.AnisotropicClamp;
			DepthStencilState = depthStencilState ?? DepthStencilState.Default;
			RasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
			Effect = effect;
			UI = ui;
		}
	}
}
