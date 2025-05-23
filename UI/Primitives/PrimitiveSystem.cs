﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ParticleLibrary.UI.Primitives
{
	public class PrimitiveSystem : ModSystem
	{
		public static GraphicsDevice GraphicsDevice => Main.graphics.GraphicsDevice;

		public static Matrix WorldViewProjection { get; private set; }

		public static BasicEffect WorldEffect { get; private set; }
		public static BasicEffect InterfaceEffect { get; private set; }

		public static event Action<Matrix> OnResolutionChanged;

		public override void Load()
		{
			if (Main.netMode is NetmodeID.Server)
			{
				return;
			}

			Main.OnPreDraw += OnPreDraw;
			Main.OnResolutionChanged += ResolutionChanged;

			Main.QueueMainThreadAction(Load_MainThread);
		}

		private void Load_MainThread()
		{
			WorldEffect = new(GraphicsDevice)
			{
				VertexColorEnabled = true
			};

			InterfaceEffect = new(GraphicsDevice)
			{
				VertexColorEnabled = true
			};

			ResolutionChanged(Main.ScreenSize.ToVector2());
		}

		public override void Unload()
		{
			if (Main.netMode is NetmodeID.Server)
			{
				return;
			}

			OnResolutionChanged = null;
			Main.OnResolutionChanged -= ResolutionChanged;

			Main.QueueMainThreadAction(Unload_MainThread);
		}

		private void Unload_MainThread()
		{
			WorldEffect.Dispose();
			WorldEffect = null;
			InterfaceEffect.Dispose();
			InterfaceEffect = null;
		}

		private Matrix _oldZoomMatrix;
		private void OnPreDraw(GameTime obj)
		{
			if (Main.netMode is NetmodeID.Server)
			{
				return;
			}

			if (_oldZoomMatrix != Main.GameViewMatrix.ZoomMatrix)
			{
				_oldZoomMatrix = Main.GameViewMatrix.ZoomMatrix;
				ResolutionChanged(Vector2.Zero);
			}
		}

		private void ResolutionChanged(Vector2 size)
		{
			int width = Main.graphics.GraphicsDevice.Viewport.Width;
			int height = Main.graphics.GraphicsDevice.Viewport.Height;
			Vector2 zoom = Main.GameViewMatrix.Zoom;

			Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) *
				Matrix.CreateTranslation(width / 2, height / -2, 0) *
				Matrix.CreateRotationZ(MathHelper.Pi) *
				Matrix.CreateScale(zoom.X, zoom.Y, 1f);

			Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);

			WorldViewProjection = view * projection;

			if (WorldEffect is not null)
			{
				WorldEffect.Projection = WorldViewProjection;
			}

			if (InterfaceEffect is not null)
			{
				InterfaceEffect.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);
			}

			Main.QueueMainThreadAction(OnResolutionChanged_MainThread);
		}

		private void OnResolutionChanged_MainThread()
		{
			OnResolutionChanged?.Invoke(WorldViewProjection);
		}
	}
}
