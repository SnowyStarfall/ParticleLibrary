using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.UI.Primitives;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using static ParticleLibrary.Resources;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core
{
	public abstract class GPUParticleSystem<TSettings, TParticle, TVertex> : IGPUParticleSystem<TParticle>, IDisposable
		where TSettings : GPUSystemSettings
		where TParticle : GPUParticle
		where TVertex : IVertexType
	{
		public GraphicsDevice Device => Main.graphics.GraphicsDevice;

		/// <summary>
		/// The texture of the particles
		/// </summary>
		public Texture2D Texture { get; private set; }

		/// <summary>
		/// The maximum amount of particles. Cannot be changed after creation
		/// </summary>
		public int MaxParticles { get; }

		/// <summary>
		/// The lifespan of the particles
		/// </summary>
		public int Lifespan { get; private set; }

		/// <summary>
		/// The size of the batching buffer. Unused for now
		/// </summary>
		public int BufferSize { get; }

		/// <summary>
		/// The layer the particles will draw on
		/// </summary>
		public Layer Layer { get; private set; }

		/// <summary>
		/// The BlendState used for the particles
		/// </summary>
		public BlendState BlendState { get; private set; }

		/// <summary>
		/// Whether the particles should fade over their lifespan
		/// </summary>
		public bool Fade { get; private set; }

		/// <summary>
		/// How much gravity should be applied to the particles
		/// </summary>
		public float Gravity { get; private set; }

		/// <summary>
		/// The maximum velocity from gravity a particle should recieve. Currently unimplemented
		/// </summary>
		public float TerminalGravity { get; private set; }

		// Effect
		protected Effect Effect { get; private set; }
		protected EffectPass Pass { get; set; }

		protected EffectParameter TransformMatrixParameter { get; private set; }
		protected EffectParameter TimeParameter { get; private set; }
		protected EffectParameter ScreenPositionParameter { get; private set; }
		protected EffectParameter FadeParameter { get; private set; }
		protected EffectParameter LifespanParameter { get; private set; }
		protected EffectParameter GravityParameter { get; private set; }
		protected EffectParameter TerminalGravityParameter { get; private set; }
		protected EffectParameter TextureParameter { get; private set; }
		protected EffectParameter OffsetParameter { get; private set; }

		// Buffers
		protected abstract DynamicVertexBuffer VertexBuffer { get; set; }
		protected abstract DynamicIndexBuffer IndexBuffer { get; set; }

		protected abstract TVertex[] Vertices { get; set; }
		protected abstract int[] Indices { get; set; }

		// Misc
		protected abstract int CurrentTime { get; set; }
		protected abstract int LastParticleTime { get; set; }

		// Buffer management
		protected abstract int CurrentParticleIndex { get; set; }
		protected abstract int CurrentBufferIndex { get; set; }
		protected abstract bool SendBatch { get; set; }
		protected abstract int StartIndex { get; set; }

		public GPUParticleSystem(TSettings settings)
		{
			Texture = settings.Texture;
			MaxParticles = settings.MaxParticles;
			Lifespan = settings.Lifespan;
			Layer = settings.Layer;
			BlendState = settings.BlendState;
			Fade = settings.Fade;
			Gravity = settings.Gravity;
			TerminalGravity = settings.TerminalGravity;

			Main.QueueMainThreadAction(() =>
			{
				LoadEffect();
				SetParameters();
				CreateBuffers();

				DrawHooks.Hook(Layer, Draw);
			});

			Primitive.OnResolutionChanged += ResolutionChanged;
			DrawHooks.OnUpdateDust += Update;
		}

		// Function
		private void Update()
		{
			// Safeguard
			if (Effect is null)
			{
				LoadEffect();
				return;
			}

			// Batched data transfer
			if (SendBatch)
			{
				SetBuffers();

				// We reset since we batched
				StartIndex = 0;
				SendBatch = false;
			}

			// Update the system's time
			CurrentTime++;
			if (LastParticleTime < Lifespan)
			{
				LastParticleTime++;
			}

			// Set effect values
			TimeParameter.SetValue(CurrentTime);
			ScreenPositionParameter.SetValue(Main.screenPosition);
		}

		public abstract void Draw(Layer layer = Layer.None);

		public abstract void AddParticle(Vector2 position, Vector2 velocity, TParticle particle);

		// Setters
		/// <summary>
		/// Sets the texture to use for the particles.
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public void SetTexture(Texture2D value)
		{
			if (value is null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			Texture = value;
			TextureParameter.SetValue(value);
		}

		/// <summary>
		/// Sets the lifespan of the particles.
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public void SetLifespan(int value)
		{
			if (value < -1)
			{
				throw new ArgumentOutOfRangeException(nameof(value));
			}

			Lifespan = value;
			LifespanParameter.SetValue(value);
		}

		/// <summary>
		/// Sets the position in the draw order to draw the particles on.
		/// </summary>
		/// <param name="value"></param>
		public void SetLayer(Layer value)
		{
			DrawHooks.UnHook(Layer, Draw);
			Layer = value;
			DrawHooks.Hook(Layer, Draw);
		}

		/// <summary>
		/// Sets the BlendState to use when drawing particles.
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public void SetBlendState(BlendState value)
		{
			if (value is null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			BlendState = value;
		}

		/// <summary>
		/// Sets whether the particles should fade over their lifespan
		/// </summary>
		/// <param name="value"></param>
		public void SetFade(bool value)
		{
			Fade = value;
			FadeParameter.SetValue(value);
		}

		/// <summary>
		/// Sets the gravity to apply to the particles
		/// </summary>
		/// <param name="value"></param>
		public void SetGravity(float value)
		{
			Gravity = value;
			GravityParameter.SetValue(value);
		}

		/// <summary>
		/// Sets the maximum amount of velocity a particle should recieve from gravity. Currently unused for now
		/// </summary>
		/// <param name="value"></param>
		public void SetTerminalGravity(float value)
		{
			TerminalGravity = value;
			TerminalGravityParameter.SetValue(value);
		}

		// Effect
		protected void LoadEffect()
		{
			Effect = ModContent.Request<Effect>(Assets.Effects.Particle, AssetRequestMode.ImmediateLoad).Value.Clone();
		}

		protected void ReloadEffect()
		{
			// Create shader
			string additionalPath = @"";
			string fileName = "GParticleShader";
			string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			FileStream stream = new(documents + $@"\My Games\Terraria\tModLoader\ModSources\ParticleLibrary\Assets\Effects\{additionalPath}{fileName}.xnb", FileMode.Open, FileAccess.Read);
			Effect = Main.Assets.CreateUntracked<Effect>(stream, $"{fileName}.xnb", AssetRequestMode.ImmediateLoad).Value;

			SetParameters();
		}

		protected void SetParameters()
		{
			SetPass();

			TransformMatrixParameter = Effect.Parameters["TransformMatrix"];
			TimeParameter = Effect.Parameters["Time"];
			ScreenPositionParameter = Effect.Parameters["ScreenPosition"];
			LifespanParameter = Effect.Parameters["Lifespan"];
			FadeParameter = Effect.Parameters["Fade"];
			GravityParameter = Effect.Parameters["Gravity"];
			TerminalGravityParameter = Effect.Parameters["TerminalGravity"];
			TextureParameter = Effect.Parameters["Texture"];
			OffsetParameter = Effect.Parameters["Offset"];

			TransformMatrixParameter.SetValue(Primitive.WorldViewProjection);
			TextureParameter.SetValue(Texture);
			LifespanParameter.SetValue(Lifespan);
			FadeParameter.SetValue(Fade);
			GravityParameter.SetValue(Gravity);
			TerminalGravityParameter.SetValue(TerminalGravity);
		}

		protected abstract void CreateBuffers();

		protected abstract void SetBuffers();

		protected abstract void SetPass();

		// Event
		private void ResolutionChanged(Matrix transformationMatrix)
		{
			TransformMatrixParameter.SetValue(transformationMatrix);
		}

		// Disposing
		private bool _disposedValue;
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					Texture = null;
					Effect?.Dispose();
					VertexBuffer?.Dispose();
					IndexBuffer?.Dispose();
					GPUParticleManager.RemoveSystem(this);
				}

				_disposedValue = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
