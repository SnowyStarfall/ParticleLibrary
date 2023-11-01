using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using TerraCompendium.Core.Utilities;
using Terraria.GameContent;

namespace ParticleLibrary.Utilities
{
	public static class LibUtilities
	{
		public static SpriteBatchSettings DefaultUISettings { get; private set; }
		public static SpriteBatchSettings CustomUISettings { get; private set; }

		static LibUtilities()
		{
			DefaultUISettings = new(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, ui: true);
			CustomUISettings = new(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, ui: true);
		}

		public static Vector4 Vec4From2Vec2(Vector2 xy, Vector2 zw) => new(xy, zw.X, zw.Y);

		public static Color FromHex(uint packedValue)
		{
			return new Color()
			{
				R = (byte)(packedValue >> 24),
				G = (byte)(packedValue >> 16),
				B = (byte)(packedValue >> 8),
				A = (byte)packedValue
			};
		}

		public static Color WithAlpha(this Color color, float alpha)
		{
			return new Color(color.R / 255f, color.G / 255f, color.B / 255f, alpha);
		}

		public static void Begin(this SpriteBatch spriteBatch, SpriteBatchSettings spriteBatchSettings, bool useMatrix = true)
		{
			if (useMatrix)
			{
				spriteBatch.Begin(spriteBatchSettings.SortMode, spriteBatchSettings.BlendState, spriteBatchSettings.SamplerState, spriteBatchSettings.DepthStencilState, spriteBatchSettings.RasterizerState, spriteBatchSettings.Effect, spriteBatchSettings.UI ? spriteBatchSettings.UIScaleMatrix : spriteBatchSettings.TransformationMatrix);
			}
			else
			{
				spriteBatch.Begin(spriteBatchSettings.SortMode, spriteBatchSettings.BlendState, spriteBatchSettings.SamplerState, spriteBatchSettings.DepthStencilState, spriteBatchSettings.RasterizerState, spriteBatchSettings.Effect);
			}
		}

		public static void DrawText(this SpriteBatch spriteBatch, string text, Vector2 position, Color color)
		{
			spriteBatch.DrawText(text, 0, position, color, Color.Black);
		}

		public static void DrawText(this SpriteBatch spriteBatch, string text, int thickness, Vector2 position, Color textColor, Color shadowColor, Vector2 origin = default, float scale = 1f)
		{
			for (int i = -thickness; i <= thickness; i++)
			{
				for (int k = -thickness; k <= thickness; k++)
				{
					if (i == 0 && k == 0)
					{
						continue;
					}

					float alpha = MathHelper.Lerp(1f, 0f, Math.Abs((i + k) / 2f));
					spriteBatch.DrawString(FontAssets.MouseText.Value, text, position + new Vector2(i, k), Color.Multiply(shadowColor, alpha), 0f, origin, scale, SpriteEffects.None, 0f);
				}
			}

			spriteBatch.DrawString(FontAssets.MouseText.Value, text, position, textColor, 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
		{
			spriteBatch.DrawLine(start, MathF.Atan2(end.Y - start.Y, end.X - start.X), Vector2.Distance(start, end), color, thickness);
		}

		public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, float radians, float length, Color color, float thickness)
		{
			spriteBatch.Draw(ParticleLibrary.WhitePixel, start, ParticleLibrary.WhitePixel.Bounds, color, radians, new Vector2(0f, 0.5f), new Vector2(length, thickness), SpriteEffects.None, 0);
		}
	}
}
