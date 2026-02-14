using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// This file uses <see href="https://github.com/krafs/Publicizer"/> to publicize the private fields set by <see cref="SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix)"/>.<br/>
	/// This allows them to be restored later using <see cref="Apply(SpriteBatch, in SpriteBatchState)"/>. The private field <see cref="SpriteBatch.beginCalled"/> is also publicized and is used to ensure runtime safety for <see cref="Apply(SpriteBatch, in SpriteBatchState)"/>.
	/// </summary>
	/// <param name="spriteBatch"></param>
	public readonly struct SpriteBatchState(SpriteBatch spriteBatch)
	{
		public readonly bool BeginCalled = spriteBatch.beginCalled;
		public readonly SpriteSortMode SortMode = spriteBatch.sortMode;
		public readonly BlendState BlendState = spriteBatch.blendState;
		public readonly SamplerState SamplerState = spriteBatch.samplerState;
		public readonly DepthStencilState DepthStencilState = spriteBatch.depthStencilState;
		public readonly RasterizerState RasterizerState = spriteBatch.rasterizerState;
		public readonly Effect Effect = spriteBatch.customEffect;
		public readonly Matrix Matrix = spriteBatch.transformMatrix;

		/// <summary>
		/// Applies the provided <see cref="SpriteBatchState"/>'s values to the provided <see cref="SpriteBatch"/>.
		/// This method <b>will end</b> the provided <see cref="SpriteBatch"/> if Begin has already been called.
		/// </summary>
		/// <param name="spriteBatch">The <see cref="SpriteBatch"/> to apply to.</param>
		/// <param name="spriteBatchState">The <see cref="SpriteBatchState"/> to reference the values of.</param>
		public static void Apply(SpriteBatch spriteBatch, in SpriteBatchState spriteBatchState)
		{
			if (spriteBatch.beginCalled)
			{
				spriteBatch.End();
			}

			spriteBatch.Begin(
				sortMode: spriteBatchState.SortMode,
				blendState: spriteBatchState.BlendState,
				samplerState: spriteBatchState.SamplerState,
				depthStencilState: spriteBatchState.DepthStencilState,
				rasterizerState: spriteBatchState.RasterizerState,
				effect: spriteBatchState.Effect,
				transformMatrix: spriteBatchState.Matrix
			);
		}
	}
}
