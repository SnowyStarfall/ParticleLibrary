﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.Core.Systems.ParticleSystem;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
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
		private EffectParameter _time;
		private EffectParameter _screenPosition;
		private readonly Texture2D _texture;
		private EffectParameter _textureParameter;

		// Buffers
		private DynamicVertexBuffer _vertexBuffer;
		private DynamicIndexBuffer _indexBuffer;

		private GParticleVertex[] _vertices;
		private short[] _indices;

		private int _currentParticleIndex;
		private int _currentTime;

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

				_vertices = new GParticleVertex[400000];
				_indices = new short[600000];
			});

			Main.OnResolutionChanged += ResolutionChanged;
			On_Dust.UpdateDust += On_Dust_UpdateDust;
			On_Main.DrawDust += On_Main_DrawDust;
		}

		private void On_Dust_UpdateDust(On_Dust.orig_UpdateDust orig)
		{
			orig();

			// Safeguard
			if (_effect is null)
			{
				LoadEffect();
				return;
			}

			if (Main.keyState.IsKeyDown(Keys.RightAlt))
			{
				Main.QueueMainThreadAction(() =>
				{
					_vertexBuffer.Dispose();
					_indexBuffer.Dispose();

					_vertexBuffer = new(Device, typeof(GParticleVertex), 400000, BufferUsage.WriteOnly);
					_indexBuffer = new(Device, IndexElementSize.SixteenBits, 600000, BufferUsage.WriteOnly);

					_vertices = new GParticleVertex[400000];
					_indices = new short[600000];

					_currentParticleIndex = 0;
					_currentTime = 0;
				});

				ReloadEffect();
			}

			// Update the system's time
			_currentTime++;
			_time.SetValue(_currentTime);
			_screenPosition.SetValue(Main.screenPosition);
		}

		private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
		{
			Main.NewText("Draw:" + _currentParticleIndex);

			orig(self);

			// Safeguard
			if (_effect is null)
			{
				LoadEffect();
				return;
			}

			// Set buffers
			Device.SetVertexBuffer(_vertexBuffer);
			Device.Indices = _indexBuffer;

			// Do particle pass
			_effect.CurrentTechnique.Passes["Particles"].Apply();
			Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _currentParticleIndex * 4, 0, _currentParticleIndex * 2);
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
			_time = _effect.Parameters["Time"];
			_screenPosition = _effect.Parameters["ScreenPosition"];
			_textureParameter = _effect.Parameters["Texture"];

			ResolutionChanged(Main.ScreenSize.ToVector2());
			_textureParameter.SetValue(_texture);
		}

		private void ReloadEffect()
		{
			// Create shader
			string additionalPath = @"";
			string fileName = "ParticleShader";
			string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			FileStream stream = new(documents + $@"\My Games\Terraria\tModLoader\ModSources\ParticleLibrary\Assets\Effects\{additionalPath}{fileName}.xnb", FileMode.Open, FileAccess.Read);
			_effect = Main.Assets.CreateUntracked<Effect>(stream, $"{fileName}.xnb", AssetRequestMode.ImmediateLoad).Value;

			// Set parameters
			_transformMatrixParameter = _effect.Parameters["TransformMatrix"];
			_time = _effect.Parameters["Time"];
			_screenPosition = _effect.Parameters["ScreenPosition"];
			_textureParameter = _effect.Parameters["Texture"];

			ResolutionChanged(Main.ScreenSize.ToVector2());
			_textureParameter.SetValue(_texture);
		}

		public void AddParticle(Vector2 position, Vector2 velocity)
		{
			_vertices[_currentParticleIndex * 4] = new GParticleVertex()
			{
				Position = new Vector4(position.X, position.Y, 0f, 1f),
				Color = new Color(255, 255, 0, 255),
				TexCoord = new Vector2(),
				Velocity = velocity,
				TimeOfAdd = _currentTime
			};
			_vertices[_currentParticleIndex * 4 + 1] = new GParticleVertex()
			{
				Position = new Vector4(position.X, position.Y + _texture.Height, 0f, 1f),
				Color = new Color(255, 0, 255, 255),
				TexCoord = new Vector2(0f, 1f),
				Velocity = velocity,
				TimeOfAdd = _currentTime
			};
			_vertices[_currentParticleIndex * 4 + 2] = new GParticleVertex()
			{
				Position = new Vector4(position.X + _texture.Width, position.Y, 0f, 1f),
				Color = new Color(0, 255, 0, 255),
				TexCoord = new Vector2(1f, 0f),
				Velocity = velocity,
				TimeOfAdd = _currentTime
			};
			_vertices[_currentParticleIndex * 4 + 3] = new GParticleVertex()
			{
				Position = new Vector4(position.X + _texture.Width, position.Y + _texture.Height, 0f, 1f),
				Color = new Color(0, 0, 255, 255),
				TexCoord = new Vector2(1f),
				Velocity = velocity,
				TimeOfAdd = _currentTime
			};

			short vertexIndex = (short)(_currentParticleIndex * 4);

			// _currentParticleIndex is 0
			// vertexIndex would be 0
			// vertices would be 0, 1, 2, 3
			// indices would be 0, 2, 3, 0, 3, 1

			// _currentParticleIndex is 1
			// vertexIndex would be 4
			// vertices would be 4, 5, 6, 7
			// indices would be 4, 6, 7, 4, 7, 5

			_indices[_currentParticleIndex * 6] = vertexIndex;
			_indices[_currentParticleIndex * 6 + 1] = (short)(vertexIndex + 2);
			_indices[_currentParticleIndex * 6 + 2] = (short)(vertexIndex + 3);
			_indices[_currentParticleIndex * 6 + 3] = vertexIndex;
			_indices[_currentParticleIndex * 6 + 4] = (short)(vertexIndex + 3);
			_indices[_currentParticleIndex * 6 + 5] = (short)(vertexIndex + 1);

			_vertexBuffer.SetData(_vertices[(_currentParticleIndex * 4)..(_currentParticleIndex * 4 + 4)], _currentParticleIndex * 4, 4, SetDataOptions.None);
			_indexBuffer.SetData(_indices[(_currentParticleIndex * 6)..(_currentParticleIndex * 6 + 6)], _currentParticleIndex * 6, 6, SetDataOptions.None);

			_currentParticleIndex++;
			if (_currentParticleIndex > 99999)
				_currentParticleIndex = 0;

			Main.NewText("Add:" + _currentParticleIndex);
		}
	}
}
