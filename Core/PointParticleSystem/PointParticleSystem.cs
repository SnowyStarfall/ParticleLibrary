﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ParticleLibrary.Utilities;
using ReLogic.Content;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ModLoader;
using static ParticleLibrary.Resources;

namespace ParticleLibrary.Core.PointParticleSystem
{
	public class PointParticleSystem : IDisposable
	{
		public GraphicsDevice Device => Main.graphics.GraphicsDevice;

		public Matrix Projection { get; private set; }
		public Matrix View { get; private set; }
		public Matrix WorldViewProjection { get; private set; }

		// Settings
		public int MaxParticles { get; }

		public int Lifespan
		{
			get => _lifespan;
			set
			{
				if (value < 0)
					value = 1;

				_lifespan = value;
				_eLifespan.SetValue(value);
			}
		}
		private int _lifespan;

		public Layer Layer
		{
			get => _layer;
			set
			{
				// We can't unhook a layer if we've just initialized
				Unhook(_layer);
				_layer = value;
				Hook(_layer);
			}
		}
		private Layer _layer;

		public BlendState BlendState
		{
			get => _blendState;
			set
			{
				if (value is null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				_blendState = value;
			}
		}
		private BlendState _blendState;

		public bool Fade
		{
			get => _fade;
			set
			{
				_fade = value;
				_eFade.SetValue(value);
			}
		}
		private bool _fade;

		public float Gravity
		{
			get => _gravity;
			set
			{
				_gravity = value;
				_eGravity.SetValue(value);
			}
		}
		private float _gravity;

		public float TerminalGravity
		{
			get => _terminalGravity;
			set
			{
				_terminalGravity = value;
				_eTerminalGravity.SetValue(value);
			}
		}
		private float _terminalGravity;

		// Effect
		private Effect _effect;
		private EffectParameter _eTransformMatrixParameter;
		private EffectParameter _eTime;
		private EffectParameter _eScreenPosition;
		private EffectParameter _eFade;
		private EffectParameter _eLifespan;
		private EffectParameter _eGravity;
		private EffectParameter _eTerminalGravity;
		private EffectParameter _eOffset;

		// Buffer
		private DynamicVertexBuffer _vertexBuffer;
		private PointParticleVertex[] _vertices;

		// Misc
		private int _currentTime;
		private int _lastParticleTime;
		private bool _disposedValue;

		// Buffer management
		private int _currentParticleIndex;
		private bool _setBuffers;
		private int _startIndex;

		public PointParticleSystem(int maxParticles, int lifespan)
		{
			if (maxParticles < 1)
				throw new ArgumentOutOfRangeException(nameof(maxParticles), "Must be greater than 0");

			MaxParticles = maxParticles;
			_lifespan = lifespan;
			Layer = Layer.BeforeDust;
			_blendState = BlendState.AlphaBlend;
			_fade = true;
			_gravity = 0f;
			_terminalGravity = 0f;

			Main.QueueMainThreadAction(() =>
			{
				LoadEffect();

				_vertexBuffer = new(Device, typeof(PointParticleVertex), MaxParticles, BufferUsage.WriteOnly);
				_vertices = new PointParticleVertex[MaxParticles];
			});

			Main.OnResolutionChanged += ResolutionChanged;
			On_Dust.UpdateDust += On_Dust_UpdateDust;
		}

		public PointParticleSystem(PointParticleSystemSettings settings)
		{
			MaxParticles = settings.MaxParticles;
			_lifespan = settings.Lifespan;
			Layer = settings.Layer;
			_blendState = settings.BlendState;
			_fade = settings.Fade;
			_gravity = settings.Gravity;
			_terminalGravity = settings.TerminalGravity;

			Main.QueueMainThreadAction(() =>
			{
				LoadEffect();

				_vertexBuffer = new(Device, typeof(PointParticleVertex), MaxParticles, BufferUsage.WriteOnly);
				_vertices = new PointParticleVertex[MaxParticles];
			});

			Main.OnResolutionChanged += ResolutionChanged;
			On_Dust.UpdateDust += On_Dust_UpdateDust;
		}

		// Function

		protected void Draw()
		{
			// Don't draw or perform calculations if the most recent particle has expired, making the system idle
			if (_lastParticleTime >= Lifespan)
				return;

			_effect.CurrentTechnique.Passes["Particles"].Apply();
			Device.DrawPrimitives(PrimitiveType.PointListEXT, 0, MaxParticles);
		}

		public void AddParticle(Vector2 position, Vector2 velocity, PointParticle particle)
		{
			_vertices[_currentParticleIndex] = new PointParticleVertex()
			{
				Position = new Vector4(position.X - particle.Size / 2f, position.Y - particle.Size / 2f, 0f, 1f),

				StartColor = particle.StartColor,
				EndColor = particle.EndColor,

				Velocity = ParticleUtils.Vec4From2Vec2(velocity, particle.VelocityAcceleration),
				Size = particle.Size,

				DepthTime = new Vector3(particle.Depth, particle.DepthVelocity, _currentTime),
			};

			// This means that we just started adding particles since the last batch
			if (_startIndex == -1)
			{
				_startIndex = _currentParticleIndex;
			}

			// We wrap back to zero and immediately send our batch of new particles without waiting
			if (++_currentParticleIndex >= MaxParticles)
			{
				SetBuffer();

				// We reset since we batched
				_currentParticleIndex = 0; // This effectively means that particles will be overwritten
				_startIndex = 0;

				_lastParticleTime = 0;
				_setBuffers = false;
				return;
			}

			_lastParticleTime = 0;
			_setBuffers = true;
		}

		// Setters

		public void SetLifespan(int value)
		{
			if (value < 0)
				value = 1;

			Lifespan = value;
			_eLifespan.SetValue(value);
		}

		public void SetFade(bool value)
		{
			Fade = value;
			_eFade.SetValue(value);
		}

		public void SetGravity(float value)
		{
			Gravity = value;
			_eGravity.SetValue(value);
		}

		public void SetTerminalGravity(float value)
		{
			TerminalGravity = value;
			_eTerminalGravity.SetValue(value);
		}

		private void SetBuffer()
		{
			_vertexBuffer.SetData(PointParticleVertex.SizeInBytes * _startIndex, _vertices, _startIndex, (_currentParticleIndex - _startIndex), PointParticleVertex.SizeInBytes, SetDataOptions.NoOverwrite);
		}


		// Internal utilities

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
				_eTransformMatrixParameter?.SetValue(WorldViewProjection);
			});
		}

		private void LoadEffect()
		{
			_effect = ModContent.Request<Effect>(Assets.Effects.PointParticleShader, AssetRequestMode.ImmediateLoad).Value.Clone();

			SetParameters();
		}

		private void ReloadEffect()
		{
			// Create shader
			string additionalPath = @"";
			string fileName = "PointParticleShader";
			string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			FileStream stream = new(documents + $@"\My Games\Terraria\tModLoader\ModSources\ParticleLibrary\Assets\Effects\{additionalPath}{fileName}.xnb", FileMode.Open, FileAccess.Read);
			_effect = Main.Assets.CreateUntracked<Effect>(stream, $"{fileName}.xnb", AssetRequestMode.ImmediateLoad).Value;

			SetParameters();
		}

		private void SetParameters()
		{
			_eTransformMatrixParameter = _effect.Parameters["TransformMatrix"];
			_eTime = _effect.Parameters["Time"];
			_eScreenPosition = _effect.Parameters["ScreenPosition"];
			_eLifespan = _effect.Parameters["Lifespan"];
			_eFade = _effect.Parameters["Fade"];
			_eGravity = _effect.Parameters["Gravity"];
			_eTerminalGravity = _effect.Parameters["TerminalGravity"];
			_eOffset = _effect.Parameters["Offset"];

			ResolutionChanged(Main.ScreenSize.ToVector2());
			_eLifespan.SetValue(Lifespan);
			_eFade.SetValue(Fade);
			_eGravity.SetValue(Gravity);
			_eTerminalGravity.SetValue(TerminalGravity);
		}

		private void Hook(Layer layer)
		{
			switch (layer)
			{
				case Layer.BeforeWalls:
					On_Main.DoDraw_WallsAndBlacks += DrawParticles_BeforeWalls;
					break;
				case Layer.BeforeNonSolidTiles:
					On_Main.DoDraw_Tiles_NonSolid += DrawParticles_BeforeNonSolidTiles;
					break;
				case Layer.BeforeNPCsBehindTiles:
					On_Main.DrawNPCs += DrawParticles_BeforeNPCs;
					break;
				case Layer.BeforeTiles:
					On_Main.DoDraw_Tiles_Solid += DrawParticles_BeforeSolidTiles;
					break;
				case Layer.BeforePlayersBehindNPCs:
					On_Main.DrawPlayers_BehindNPCs += DrawParticles_BeforePlayersBehindNPCs;
					break;
				case Layer.BeforeNPCs:
					On_Main.DrawNPCs += DrawParticles_BeforeNPCs;
					break;
				case Layer.BeforeProjectiles:
					On_Main.DrawProjectiles += DrawParticles_BeforeProjectiles;
					break;
				case Layer.BeforePlayers:
					On_Main.DrawPlayers_AfterProjectiles += DrawParticles_BeforePlayers;
					break;
				case Layer.BeforeItems:
					On_Main.DrawItems += DrawParticles_BeforeItems;
					break;
				case Layer.BeforeRain:
					On_Main.DrawRain += DrawParticles_BeforeRain;
					break;
				case Layer.BeforeGore:
					On_Main.DrawGore += DrawParticles_BeforeGore;
					break;
				case Layer.BeforeDust:
					On_Main.DrawDust += DrawParticles_BeforeDust;
					break;
				case Layer.BeforeWater:
					On_Main.DrawWaters += DrawParticles_BeforeWater;
					break;
				case Layer.BeforeUI:
					On_Main.DrawInterface += DrawParticles_OnInterface;
					break;
				case Layer.AfterUI:
					On_Main.DrawInterface += DrawParticles_OnInterface;
					break;
				case Layer.BeforeMainMenu:
					On_Main.DrawMenu += DrawParticles_OnMainMenu;
					break;
				case Layer.AfterMainMenu:
					On_Main.DrawMenu += DrawParticles_OnMainMenu;
					break;
			}
		}

		private void Unhook(Layer layer)
		{
			switch (layer)
			{
				case Layer.BeforeWalls:
					On_Main.DoDraw_WallsAndBlacks -= DrawParticles_BeforeWalls;
					break;
				case Layer.BeforeNonSolidTiles:
					On_Main.DoDraw_Tiles_NonSolid -= DrawParticles_BeforeNonSolidTiles;
					break;
				case Layer.BeforeNPCsBehindTiles:
					On_Main.DrawNPCs -= DrawParticles_BeforeNPCs;
					break;
				case Layer.BeforeTiles:
					On_Main.DoDraw_Tiles_Solid -= DrawParticles_BeforeSolidTiles;
					break;
				case Layer.BeforePlayersBehindNPCs:
					On_Main.DrawPlayers_BehindNPCs -= DrawParticles_BeforePlayersBehindNPCs;
					break;
				case Layer.BeforeNPCs:
					On_Main.DrawNPCs -= DrawParticles_BeforeNPCs;
					break;
				case Layer.BeforeProjectiles:
					On_Main.DrawProjectiles -= DrawParticles_BeforeProjectiles;
					break;
				case Layer.BeforePlayers:
					On_Main.DrawPlayers_AfterProjectiles -= DrawParticles_BeforePlayers;
					break;
				case Layer.BeforeItems:
					On_Main.DrawItems -= DrawParticles_BeforeItems;
					break;
				case Layer.BeforeRain:
					On_Main.DrawRain -= DrawParticles_BeforeRain;
					break;
				case Layer.BeforeGore:
					On_Main.DrawGore -= DrawParticles_BeforeGore;
					break;
				case Layer.BeforeDust:
					On_Main.DrawDust -= DrawParticles_BeforeDust;
					break;
				case Layer.BeforeWater:
					On_Main.DrawWaters -= DrawParticles_BeforeWater;
					break;
				case Layer.BeforeUI:
					On_Main.DrawInterface -= DrawParticles_OnInterface;
					break;
				case Layer.AfterUI:
					On_Main.DrawInterface -= DrawParticles_OnInterface;
					break;
				case Layer.BeforeMainMenu:
					On_Main.DrawMenu -= DrawParticles_OnMainMenu;
					break;
				case Layer.AfterMainMenu:
					On_Main.DrawMenu -= DrawParticles_OnMainMenu;
					break;
			}
		}

		// Updating
		private void On_Dust_UpdateDust(On_Dust.orig_UpdateDust orig)
		{
			orig();

			// Safeguard
			if (_effect is null)
			{
				LoadEffect();
				return;
			}

#if DEBUG
			if (Main.keyState.IsKeyDown(Keys.RightControl))
			{
				Main.QueueMainThreadAction(() =>
				{
					_vertexBuffer.Dispose();
					_vertexBuffer = new(Device, typeof(PointParticleVertex), MaxParticles, BufferUsage.WriteOnly);

					_vertices = new PointParticleVertex[MaxParticles];

					_currentParticleIndex = 0;
					_currentTime = 0;

					_setBuffers = false;
				});

				ReloadEffect();
			}
#endif

			// Batched data transfer
			if (_setBuffers)
			{
				SetBuffer();

				// We reset since we batched
				_startIndex = 0;
				_setBuffers = false;
			}

			// Update the system's time
			_currentTime++;
			if (_lastParticleTime < Lifespan)
			{
				_lastParticleTime++;
			}

			// Set effect values
			_eTime.SetValue(_currentTime);
			_eScreenPosition.SetValue(Main.screenPosition);
		}

		// Drawing

		private void DrawParticles_BeforeWalls(On_Main.orig_DoDraw_WallsAndBlacks orig, Main self)
		{
			Main.spriteBatch.End();
			DrawParticles_OnLayer(Layer.BeforeWalls);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticles_BeforeNonSolidTiles(On_Main.orig_DoDraw_Tiles_NonSolid orig, Main self)
		{
			Main.spriteBatch.End();
			DrawParticles_OnLayer(Layer.BeforeNonSolidTiles);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticles_BeforeSolidTiles(On_Main.orig_DoDraw_Tiles_Solid orig, Main self)
		{
			DrawParticles_OnLayer(Layer.BeforeTiles);

			orig(self);
		}

		private void DrawParticles_BeforePlayersBehindNPCs(On_Main.orig_DrawPlayers_BehindNPCs orig, Main self)
		{
			DrawParticles_OnLayer(Layer.BeforePlayersBehindNPCs);

			orig(self);
		}

		private void DrawParticles_BeforeNPCs(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
		{
			if (behindTiles)
			{
				Main.spriteBatch.End();
				DrawParticles_OnLayer(Layer.BeforeNPCsBehindTiles);
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			}
			else
			{
				Main.spriteBatch.End();
				DrawParticles_OnLayer(Layer.BeforeNPCs);
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			}

			orig(self, behindTiles);
		}

		private void DrawParticles_BeforeProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
		{
			DrawParticles_OnLayer(Layer.BeforeProjectiles);

			orig(self);
		}

		private void DrawParticles_BeforePlayers(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
		{
			DrawParticles_OnLayer(Layer.BeforePlayers);

			orig(self);
		}

		private void DrawParticles_BeforeItems(On_Main.orig_DrawItems orig, Main self)
		{
			Main.spriteBatch.End();
			DrawParticles_OnLayer(Layer.BeforeItems);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticles_BeforeRain(On_Main.orig_DrawRain orig, Main self)
		{
			Main.spriteBatch.End();
			DrawParticles_OnLayer(Layer.BeforeRain);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticles_BeforeGore(On_Main.orig_DrawGore orig, Main self)
		{
			Main.spriteBatch.End();
			DrawParticles_OnLayer(Layer.BeforeGore);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticles_BeforeDust(On_Main.orig_DrawDust orig, Main self)
		{
			DrawParticles_OnLayer(Layer.BeforeDust);

			orig(self);
		}

		private void DrawParticles_BeforeWater(On_Main.orig_DrawWaters orig, Main self, bool isBackground)
		{
			Main.spriteBatch.End();
			DrawParticles_OnLayer(Layer.BeforeWater);
			Main.spriteBatch.Begin();

			orig(self, isBackground);
		}

		private void DrawParticles_OnInterface(On_Main.orig_DrawInterface orig, Main self, GameTime gameTime)
		{
			DrawParticles_OnLayer(Layer.BeforeUI);

			orig(self, gameTime);

			DrawParticles_OnLayer(Layer.AfterUI);
		}

		private void DrawParticles_OnMainMenu(On_Main.orig_DrawMenu orig, Main self, GameTime gameTime)
		{
			// TODO: Move this
			//if (Main.gameMenu && Main.hasFocus)
			//	UpdateParticles();

			Main.spriteBatch.End();
			DrawParticles_OnLayer(Layer.BeforeMainMenu);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

			orig(self, gameTime);
			DrawParticles_OnLayer(Layer.AfterMainMenu);
		}

		private void DrawParticles_OnLayer(Layer layer)
		{
			// Safeguard
			if (_effect is null)
			{
				LoadEffect();
				return;
			}

			if (layer is Layer.BeforeWater)
			{
				_eOffset.SetValue(new Vector2(Main.offScreenRange));
			}
			else
			{
				_eOffset.SetValue(Vector2.Zero);
			}

			// Set blend state
			var previousBlendState = Device.BlendState;
			Device.BlendState = BlendState;

			// Set buffers
			Device.SetVertexBuffer(_vertexBuffer);
			Device.Indices = null;

			// Do particle pass
			Draw();

			// Reset blend state
			Device.BlendState = previousBlendState;
		}

		protected void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					_effect.Dispose();
					_vertexBuffer.Dispose();
				}

				_disposedValue = true;
			}
		}

		//~PointParticleSystem()
		//{
		//	// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//	Dispose(disposing: false);
		//}

		void IDisposable.Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}