using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;
using ParticleLibrary.Core.Data;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Diagnostics;
using TerraCompendium.Core.Utilities;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using SystemVector2 = System.Numerics.Vector2;

namespace ParticleLibrary.Utilities
{
	public static class LibUtilities
	{
		public static SpriteBatchSettings DefaultUISettings { get; }
		public static SpriteBatchSettings ClarityUISettings { get; }

		public static RasterizerState OverflowHiddenRasterizerState { get; }

		static LibUtilities()
		{
			DefaultUISettings = new(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, ui: true);
			ClarityUISettings = new(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, ui: true);

			OverflowHiddenRasterizerState = new RasterizerState
			{
				CullMode = CullMode.None,
				ScissorTestEnable = true
			};
		}

		public static SystemVector2 ToNumerics(this in Vector2 v) => new(v.X, v.Y);

		public static SystemVector2 DirectionTo(this in SystemVector2 from, in SystemVector2 to)
		{
			return SystemVector2.Normalize(to - from);
		}

		public static float AngleTo(this in SystemVector2 from, in SystemVector2 to)
		{
			var v = to - from;
			return (float)Math.Atan2(v.Y, v.X);
		}

		public static float ToRotation(this in SystemVector2 v)
		{
			return (float)Math.Atan2(v.Y, v.X);
		}

		public static SystemVector2 RotatedBy(this in SystemVector2 v, float radians)
		{
			float cos = MathF.Cos(radians);
			float sin = MathF.Sin(radians);
			return new SystemVector2(
				v.X * cos - v.Y * sin,
				v.X * sin + v.Y * cos
			);
		}

		public static Vector4 Vec4From2Vec2(Vector2 xy, Vector2 zw) => new(xy, zw.X, zw.Y);

		public static Color FromPackedValue(float packedValue)
		{
			uint packed = BitConverter.SingleToUInt32Bits(packedValue);
			return new Color(
			  r: (byte)(packed & 0xFF),
			  g: (byte)((packed >> 8) & 0xFF),
			  b: (byte)((packed >> 16) & 0xFF),
			  alpha: (byte)((packed >> 24) & 0xFF)
			);
		}

		public static Color FromPackedValue(uint packedValue)
		{
			return new Color(
			  r: (byte)(packedValue & 0xFF),
			  g: (byte)((packedValue >> 8) & 0xFF),
			  b: (byte)((packedValue >> 16) & 0xFF),
			  alpha: (byte)((packedValue >> 24) & 0xFF)
			);
		}

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

		public static Rectangle GetClippingRectangle(this SpriteBatch spriteBatch, UIElement element)
		{
			return Rectangle.Intersect(element.GetClippingRectangle(spriteBatch), spriteBatch.GraphicsDevice.ScissorRectangle);
		}

		public static Texture2D GetTexture(string path) => ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad).Value;

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

		public static void BeginScissorIf(this SpriteBatch spriteBatch, bool condition, SpriteBatchSettings spriteBatchSettings, Rectangle scissorRectangle, bool useMatrix, /*out RasterizerState oldRasterizerState, */out Rectangle oldScissorRectangle)
		{
			if (condition)
			{
				spriteBatch.BeginScissor(spriteBatchSettings, scissorRectangle, useMatrix, out Rectangle old);
				oldScissorRectangle = old;
			}
			else
			{
				spriteBatch.Begin(spriteBatchSettings, useMatrix);
				oldScissorRectangle = Rectangle.Empty;
			}
		}

		public static void EndScissorIf(this SpriteBatch spriteBatch, bool condition,/* RasterizerState oldRasterizerState,*/ in Rectangle oldScissorRectangle)
		{
			if (condition)
			{
				spriteBatch.EndScissor(oldScissorRectangle);
			}
			else
			{
				spriteBatch.End();
			}
		}

		public static void BeginScissor(this SpriteBatch spriteBatch, SpriteBatchSettings spriteBatchSettings, Rectangle scissorRectangle, bool useMatrix, /*out RasterizerState oldRasterizerState, */out Rectangle oldScissorRectangle)
		{
			//oldRasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
			oldScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;

			spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
			spriteBatch.GraphicsDevice.RasterizerState = OverflowHiddenRasterizerState;

			if (useMatrix)
			{
				spriteBatch.Begin(spriteBatchSettings.SortMode, spriteBatchSettings.BlendState, spriteBatchSettings.SamplerState, spriteBatchSettings.DepthStencilState, OverflowHiddenRasterizerState, spriteBatchSettings.Effect, spriteBatchSettings.UI ? spriteBatchSettings.UIScaleMatrix : spriteBatchSettings.TransformationMatrix);
			}
			else
			{
				spriteBatch.Begin(spriteBatchSettings.SortMode, spriteBatchSettings.BlendState, spriteBatchSettings.SamplerState, spriteBatchSettings.DepthStencilState, OverflowHiddenRasterizerState, spriteBatchSettings.Effect);
			}
		}

		public static void EndScissor(this SpriteBatch spriteBatch,/* RasterizerState oldRasterizerState,*/ in Rectangle oldScissorRectangle)
		{
			var rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;

			spriteBatch.End();

			spriteBatch.GraphicsDevice.ScissorRectangle = oldScissorRectangle;
			spriteBatch.GraphicsDevice.RasterizerState = rasterizerState;
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

		public static QuadParticle FromEmitter(this QuadParticle particle, SpatialParameters spatial, VisualParameters visual)
		{
			particle.StartColor = visual.StartColor;
			particle.EndColor = visual.EndColor;
			particle.VelocityAcceleration = spatial.VelocityAcceleration;
			particle.Scale = spatial.Scale;
			particle.ScaleVelocity = spatial.ScaleVelocity;
			particle.Rotation = spatial.Rotation;
			particle.RotationVelocity = spatial.RotationVelocity;
			particle.Depth = spatial.Depth;
			particle.DepthVelocity = spatial.DepthVelocity;

			return particle;
		}

		public static PointParticle FromEmitter(this PointParticle particle, SpatialParameters spatial, VisualParameters visual)
		{
			particle.StartColor = visual.StartColor;
			particle.EndColor = visual.EndColor;
			particle.VelocityAcceleration = spatial.VelocityAcceleration;
			particle.Depth = spatial.Depth;
			particle.DepthVelocity = spatial.DepthVelocity;

			return particle;
		}
	}
}
