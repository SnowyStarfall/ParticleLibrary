using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.Core.Systems.ParticleSystem;
using ReLogic.Content;
using System;
using System.IO;
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
		private EffectParameter _time;
		private EffectParameter _screenPosition;

		private readonly Texture2D _texture;
		private EffectParameter _textureParameter;

		private readonly int _lifespan;
		private EffectParameter _lifespanParameter;

		// Buffers
		private DynamicVertexBuffer _vertexBuffer;
		private DynamicIndexBuffer _indexBuffer;

		private GParticleVertex[] _vertices;
		private int[] _indices;

		private int _currentParticleIndex;
		private int _currentTime;

		private bool _needDataSet;

		private int _maxParticles = 1000000;

		public GParticleSystem(GParticleSystemSettings settings)
		{
			if (settings.Texture is null)
				throw new ArgumentNullException(nameof(settings.Texture), "Texture cannot be null.");

			_texture = settings.Texture;
			_maxParticles = settings.MaxParticles;
			_lifespan = settings.Lifespan;

			Main.QueueMainThreadAction(() =>
			{
				LoadEffect();

				_vertexBuffer = new(Device, typeof(GParticleVertex), _maxParticles * 4, BufferUsage.WriteOnly);
				_indexBuffer = new(Device, IndexElementSize.ThirtyTwoBits, _maxParticles * 6, BufferUsage.WriteOnly);

				_vertices = new GParticleVertex[_maxParticles * 4];
				_indices = new int[_maxParticles * 6];
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

					_vertexBuffer = new(Device, typeof(GParticleVertex), _maxParticles * 4, BufferUsage.WriteOnly);
					_indexBuffer = new(Device, IndexElementSize.ThirtyTwoBits, _maxParticles * 6, BufferUsage.WriteOnly);

					_vertices = new GParticleVertex[_maxParticles * 4];
					_indices = new int[_maxParticles * 6];

					_currentParticleIndex = 0;
					_currentTime = 0;

					_needDataSet = false;
				});

				ReloadEffect();
			}

			// Batched data transfer
			if (_needDataSet)
			{
				_vertexBuffer.SetData(_vertices, 0, _vertices.Length, SetDataOptions.Discard);
				_indexBuffer.SetData(_indices, 0, _indices.Length, SetDataOptions.Discard);
			}

			// Update the system's time
			_currentTime++;
			_time.SetValue(_currentTime);
			_screenPosition.SetValue(Main.screenPosition);
		}

		private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
		{
			orig(self);

			// Safeguard
			if (_effect is null)
			{
				LoadEffect();
				return;
			}

			// Set blend state
			var previousBlendState = Device.BlendState;
			Device.BlendState = BlendState.AlphaBlend;

			// Set buffers
			Device.SetVertexBuffer(_vertexBuffer);
			Device.Indices = _indexBuffer;

			// Do particle pass
			_effect.CurrentTechnique.Passes["Particles"].Apply();
			Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _currentParticleIndex * 4, 0, _currentParticleIndex * 2);

			// Reset blend state
			Device.BlendState = previousBlendState;
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
			_effect = ModContent.Request<Effect>(Assets.Effects.GParticleShader, AssetRequestMode.ImmediateLoad).Value.Clone();

			SetParameters();
		}

		private void ReloadEffect()
		{
			// Create shader
			string additionalPath = @"";
			string fileName = "GParticleShader";
			string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			FileStream stream = new(documents + $@"\My Games\Terraria\tModLoader\ModSources\ParticleLibrary\Assets\Effects\{additionalPath}{fileName}.xnb", FileMode.Open, FileAccess.Read);
			_effect = Main.Assets.CreateUntracked<Effect>(stream, $"{fileName}.xnb", AssetRequestMode.ImmediateLoad).Value;

			SetParameters();
		}

		private void SetParameters()
		{
			_transformMatrixParameter = _effect.Parameters["TransformMatrix"];
			_time = _effect.Parameters["Time"];
			_screenPosition = _effect.Parameters["ScreenPosition"];
			_textureParameter = _effect.Parameters["Texture"];
			_lifespanParameter = _effect.Parameters["Lifespan"];

			ResolutionChanged(Main.ScreenSize.ToVector2());
			_textureParameter.SetValue(_texture);
			_lifespanParameter.SetValue(_lifespan);
		}

		public void AddParticle(Vector2 position, Vector2 velocity, GParticleSettings settings)
		{
			if (_currentParticleIndex >= _maxParticles)
				return;

			Vector2 size = new(_texture.Width * settings.Scale.X, _texture.Height * settings.Scale.Y);
			Vector4 random = new(Main.rand.NextFloat(1f + float.Epsilon), Main.rand.NextFloat(1f + float.Epsilon), Main.rand.NextFloat(1f + float.Epsilon), Main.rand.NextFloat(1f + float.Epsilon));

			_vertices[_currentParticleIndex * 4] = new GParticleVertex()
			{
				Position = new Vector4(position.X, position.Y, 0f, 1f),
				Color = settings.StartColor,
				TexCoord = new Vector2(),
				Velocity = velocity,
				TimeOfAdd = _currentTime
			};
			_vertices[_currentParticleIndex * 4 + 1] = new GParticleVertex()
			{
				Position = new Vector4(position.X, position.Y + size.Y, 0f, 1f),
				Color = settings.StartColor,
				TexCoord = new Vector2(0f, 1f),
				Velocity = velocity,
				TimeOfAdd = _currentTime
			};
			_vertices[_currentParticleIndex * 4 + 2] = new GParticleVertex()
			{
				Position = new Vector4(position.X + size.X, position.Y, 0f, 1f),
				Color = settings.StartColor,
				TexCoord = new Vector2(1f, 0f),
				Velocity = velocity,
				TimeOfAdd = _currentTime
			};
			_vertices[_currentParticleIndex * 4 + 3] = new GParticleVertex()
			{
				Position = new Vector4(position.X + size.X, position.Y + size.Y, 0f, 1f),
				Color = settings.StartColor,
				TexCoord = new Vector2(1f),
				Velocity = velocity,
				TimeOfAdd = _currentTime
			};

			int vertexIndex = _currentParticleIndex * 4;

			// _currentParticleIndex is 0
			// vertexIndex would be 0
			// vertices would be 0, 1, 2, 3
			// indices would be 0, 2, 3, 0, 3, 1

			// _currentParticleIndex is 1
			// vertexIndex would be 4
			// vertices would be 4, 5, 6, 7
			// indices would be 4, 6, 7, 4, 7, 5

			_indices[_currentParticleIndex * 6] = vertexIndex;
			_indices[_currentParticleIndex * 6 + 1] = vertexIndex + 2;
			_indices[_currentParticleIndex * 6 + 2] = vertexIndex + 3;
			_indices[_currentParticleIndex * 6 + 3] = vertexIndex;
			_indices[_currentParticleIndex * 6 + 4] = vertexIndex + 3;
			_indices[_currentParticleIndex * 6 + 5] = vertexIndex + 1;

			//_vertexBuffer.SetData(_vertices[(_currentParticleIndex * 4)..(_currentParticleIndex * 4 + 4)], _currentParticleIndex * 4, 4, SetDataOptions.None);
			//_indexBuffer.SetData(_indices[(_currentParticleIndex * 6)..(_currentParticleIndex * 6 + 6)], _currentParticleIndex * 6, 6, SetDataOptions.None);

			_currentParticleIndex++;
			_needDataSet = true;
		}
	}
}
