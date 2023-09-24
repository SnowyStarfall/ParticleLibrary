using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core.Systems.ParticleSystem;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static ParticleLibrary.Resources;

namespace ParticleLibrary.Core.Systems.Test
{
	public class GParticleSystem
	{
		public GraphicsDevice Device => Main.graphics.GraphicsDevice;

		// Matrices
		public Matrix Projection { get; private set; }
		public Matrix View { get; private set; }
		public Matrix WorldViewProjection { get; private set; }

		// Effect
		private Effect _effect;
		private EffectParameter _transformMatrixParameter;
		private readonly Texture2D _texture;
		private EffectParameter _textureParameter;

		// Buffers
		private DynamicVertexBuffer _vertexBuffer;
		private DynamicIndexBuffer _indexBuffer;

		private int _index;

		public GParticleSystem(Texture2D texture)
		{
			if (texture is null)
				throw new ArgumentNullException(nameof(texture), "Texture cannot be null.");

			_texture = texture;

			Main.QueueMainThreadAction(() =>
			{
				LoadEffect();

				_vertexBuffer = new(Device, typeof(GParticleVertex), 400000, BufferUsage.WriteOnly);
				_indexBuffer = new(Device, IndexElementSize.SixteenBits, 600000, BufferUsage.WriteOnly);
			});

			Main.OnResolutionChanged += ResolutionChanged;
			On_Dust.UpdateDust += On_Dust_UpdateDust;
			On_Main.DrawDust += On_Main_DrawDust;
		}

		private void On_Dust_UpdateDust(On_Dust.orig_UpdateDust orig)
		{
			orig();

			if (_effect is null)
			{
				LoadEffect();
				return;
			}

			// Do update pass here
			Device.SetVertexBuffer(_vertexBuffer);
			Device.Indices = _indexBuffer;

			_effect.CurrentTechnique.Passes["Update"].Apply();
			Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _index * 4, 0, _index * 2);
		}
		
		private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
		{
			orig(self);

			if (_effect is null)
			{
				LoadEffect();
				return;
			}

			// Do draw pass here
			Device.SetVertexBuffer(_vertexBuffer);
			Device.Indices = _indexBuffer;

			_effect.CurrentTechnique.Passes["Render"].Apply();
			Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _index * 4, 0, _index * 2);
		}

		private void ResolutionChanged(Vector2 obj)
		{
			int width = Main.graphics.GraphicsDevice.Viewport.Width;
			int height = Main.graphics.GraphicsDevice.Viewport.Height;
			Vector2 zoom = Main.GameViewMatrix.Zoom;
			View = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
			Projection = Matrix.CreateOrthographic(width, height, 0, 1000);
			WorldViewProjection = View * Projection;

			Main.QueueMainThreadAction(() =>
			{
				_transformMatrixParameter?.SetValue(WorldViewProjection);
			});
		}

		private void LoadEffect()
		{
			_effect = ModContent.Request<Effect>(Assets.Effects.ParticleShader, AssetRequestMode.ImmediateLoad).Value.Clone();

			_transformMatrixParameter = _effect.Parameters["TransformMatrix"];
			_textureParameter = _effect.Parameters["Texture"];

			ResolutionChanged(Main.ScreenSize.ToVector2());
			_textureParameter.SetValue(_texture);
		}

		public void AddParticle(Vector2 position, Vector2 velocity)
		{
			GParticleVertex[] vertices = new GParticleVertex[]
			{
				new GParticleVertex()
				{
					Position = new Vector4(position.X - Main.screenPosition.X, position.Y- Main.screenPosition.Y, 0f, 1f),
					Color = new Color(255, 255, 0, 255),
					TexCoord = new Vector2(),
					Velocity = velocity
				},
				new GParticleVertex()
				{
					Position = new Vector4(position.X - Main.screenPosition.X, position.Y + _texture.Height - Main.screenPosition.Y, 0f, 1f),
					Color = new Color(255, 0, 255, 255),
					TexCoord = new Vector2(0f, 1f),
					Velocity = velocity
				},
				new GParticleVertex()
				{
					Position = new Vector4(position.X + _texture.Width- Main.screenPosition.X, position.Y- Main.screenPosition.Y, 0f, 1f),
					Color = new Color(0, 255, 0, 255),
					TexCoord = new Vector2(1f, 0f),
					Velocity = velocity
				},
				new GParticleVertex()
				{
					Position = new Vector4(position.X + _texture.Width - Main.screenPosition.X, position.Y + _texture.Height - Main.screenPosition.Y, 0f, 1f),
					Color = new Color(0, 0, 255, 255),
					TexCoord = new Vector2(1f),
					Velocity = velocity
				}
			};

			short[] indices = new short[]
			{
				0, 2, 3, 0, 3, 1
			};

			_vertexBuffer.SetData(vertices, _index * 4, vertices.Length, SetDataOptions.Discard);
			_indexBuffer.SetData(indices, _index * 6, indices.Length, SetDataOptions.Discard);

			_index++;
			if (_index > 99999)
				_index = 0;
		}
	}
}
