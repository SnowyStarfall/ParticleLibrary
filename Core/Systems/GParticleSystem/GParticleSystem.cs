using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.Core.Systems;
using ParticleLibrary.Utilities;
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
		private GParticleSystemSettings _settings;

		private Effect _effect;
		private EffectParameter _transformMatrixParameter;
		private EffectParameter _time;
		private EffectParameter _screenPosition;
		private EffectParameter _fade;
		private EffectParameter _lifespan;
		private EffectParameter _size;
		private EffectParameter _scaleVelocity;
		private EffectParameter _rotationVelocity;
		private EffectParameter _texture;

		// Buffers
		private DynamicVertexBuffer _vertexBuffer;
		private DynamicIndexBuffer _indexBuffer;

		private GParticleVertex[] _vertices;
		private int[] _indices;

		private int _currentParticleIndex;
		private int _currentTime;
		private bool _needDataSet;

		public GParticleSystem(GParticleSystemSettings settings)
		{
			if (settings is null)
				throw new ArgumentNullException(nameof(settings), "Settings cannot be null.");

			if (settings.Texture is null)
				throw new ArgumentNullException(nameof(settings.Texture), "Texture cannot be null.");

			_settings = settings;

			Main.QueueMainThreadAction(() =>
			{
				LoadEffect();

				_vertexBuffer = new(Device, typeof(GParticleVertex), _settings.MaxParticles * 4, BufferUsage.WriteOnly);
				_indexBuffer = new(Device, IndexElementSize.ThirtyTwoBits, _settings.MaxParticles * 6, BufferUsage.WriteOnly);

				_vertices = new GParticleVertex[_settings.MaxParticles * 4];
				_indices = new int[_settings.MaxParticles * 6];
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

					_vertexBuffer = new(Device, typeof(GParticleVertex), _settings.MaxParticles * 4, BufferUsage.WriteOnly);
					_indexBuffer = new(Device, IndexElementSize.ThirtyTwoBits, _settings.MaxParticles * 6, BufferUsage.WriteOnly);

					_vertices = new GParticleVertex[_settings.MaxParticles * 4];
					_indices = new int[_settings.MaxParticles * 6];

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
			_lifespan = _effect.Parameters["Lifespan"];
			_fade = _effect.Parameters["Fade"];
			_size = _effect.Parameters["Size"];
			_scaleVelocity = _effect.Parameters["ScaleVelocity"];
			_rotationVelocity = _effect.Parameters["RotationVelocity"];
			_texture = _effect.Parameters["Texture"];

			ResolutionChanged(Main.ScreenSize.ToVector2());
			_lifespan.SetValue(_settings.Lifespan);
			_fade.SetValue(_settings.Fade);
			_size.SetValue(_settings.Texture.Size());
			_scaleVelocity.SetValue(_settings.ScaleVelocity);
			_rotationVelocity.SetValue(_settings.RotationVelocity);
			_texture.SetValue(_settings.Texture);
		}

		public void AddParticle(Vector2 position, Vector2 velocity, GParticleSettings settings)
		{
			if (_currentParticleIndex >= _settings.MaxParticles)
				return;

			Vector2 size = new(_settings.Texture.Width * settings.Scale.X, _settings.Texture.Height * settings.Scale.Y);
			Vector4 random = new(Main.rand.NextFloat(1f + float.Epsilon), Main.rand.NextFloat(1f + float.Epsilon), Main.rand.NextFloat(1f + float.Epsilon), Main.rand.NextFloat(1f + float.Epsilon));

			_vertices[_currentParticleIndex * 4] = new GParticleVertex()
			{
				Position = new Vector4(position.X - size.X / 2f, position.Y - size.Y / 2f, 0f, 1f),
				TexCoord = new Vector2(),

				StartColor = settings.StartColor,
				EndColor = settings.EndColor,

				Velocity = ParticleUtils.Vec4From2Vec2(velocity, settings.VelocityAcceleration),
				Size = new Vector2(_settings.Texture.Width, _settings.Texture.Height),
				Scale = ParticleUtils.Vec4From2Vec2(settings.Scale, settings.ScaleVelocity),
				Rotation = new Vector4(-0.5f, -0.5f, settings.Rotation, settings.RotationVelocity),

				DepthTime = new Vector3(settings.Depth, settings.DepthVelocity, _currentTime),
				Random = new Color(random)
			};
			_vertices[_currentParticleIndex * 4 + 1] = new GParticleVertex()
			{
				Position = new Vector4(position.X - size.X / 2f, position.Y + size.Y / 2f, 0f, 1f),
				TexCoord = new Vector2(0f, 1f),

				StartColor = settings.StartColor,
				EndColor = settings.EndColor,

				Velocity = ParticleUtils.Vec4From2Vec2(velocity, settings.VelocityAcceleration),
				Size = new Vector2(_settings.Texture.Width, _settings.Texture.Height),
				Scale = ParticleUtils.Vec4From2Vec2(settings.Scale, settings.ScaleVelocity),
				Rotation = new Vector4(-0.5f, 0.5f, settings.Rotation, settings.RotationVelocity),

				DepthTime = new Vector3(settings.Depth, settings.DepthVelocity, _currentTime),
				Random = new Color(random)
			};
			_vertices[_currentParticleIndex * 4 + 2] = new GParticleVertex()
			{
				Position = new Vector4(position.X + size.X / 2f, position.Y - size.Y / 2f, 0f, 1f),
				TexCoord = new Vector2(1f, 0f),

				StartColor = settings.StartColor,
				EndColor = settings.EndColor,

				Velocity = ParticleUtils.Vec4From2Vec2(velocity, settings.VelocityAcceleration),
				Size = new Vector2(_settings.Texture.Width, _settings.Texture.Height),
				Scale = ParticleUtils.Vec4From2Vec2(settings.Scale, settings.ScaleVelocity),
				Rotation = new Vector4(0.5f, -0.5f, settings.Rotation, settings.RotationVelocity),

				DepthTime = new Vector3(settings.Depth, settings.DepthVelocity, _currentTime),
				Random = new Color(random)
			};
			_vertices[_currentParticleIndex * 4 + 3] = new GParticleVertex()
			{
				Position = new Vector4(position.X + size.X / 2f, position.Y + size.Y / 2f, 0f, 1f),
				TexCoord = new Vector2(1f),

				StartColor = settings.StartColor,
				EndColor = settings.EndColor,

				Velocity = ParticleUtils.Vec4From2Vec2(velocity, settings.VelocityAcceleration),
				Size = new Vector2(_settings.Texture.Width, _settings.Texture.Height),
				Scale = ParticleUtils.Vec4From2Vec2(settings.Scale, settings.ScaleVelocity),
				Rotation = new Vector4(0.5f, 0.5f, settings.Rotation, settings.RotationVelocity),

				DepthTime = new Vector3(settings.Depth, settings.DepthVelocity, _currentTime),
				Random = new Color(random)
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
