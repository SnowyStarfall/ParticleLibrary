using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary.Core
{
	public class Primitive : ModSystem
	{
		public static GraphicsDevice GraphicsDevice => Main.graphics.GraphicsDevice;

		public static Matrix Projection { get; private set; }
		public static Matrix View { get; private set; }
		public static Matrix WorldViewProjection { get; private set; }

		public static BasicEffect Effect { get; private set; }

		public static event Action<Matrix> OnResolutionChanged;

		public override void Load()
		{
			Main.OnResolutionChanged += ResolutionChanged;

			ResolutionChanged(Main.ScreenSize.ToVector2());

			Effect = new(GraphicsDevice)
			{
				VertexColorEnabled = true,
				Projection = WorldViewProjection
			};
		}

		public override void Unload()
		{
		}

		private void ResolutionChanged(Vector2 size)
		{
			int width = Main.graphics.GraphicsDevice.Viewport.Width;
			int height = Main.graphics.GraphicsDevice.Viewport.Height;
			Vector2 zoom = Main.GameViewMatrix.Zoom;
			View = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
			Projection = Matrix.CreateOrthographic(width, height, 0, 1000);
			WorldViewProjection = View * Projection;

			Main.QueueMainThreadAction(() =>
			{
				OnResolutionChanged(WorldViewProjection);
			});
		}
	}
}
