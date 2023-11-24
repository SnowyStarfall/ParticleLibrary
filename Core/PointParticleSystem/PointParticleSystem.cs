using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.UI.Primitives;
using ParticleLibrary.Utilities;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static ParticleLibrary.Resources;
using static ParticleLibrary.Resources.Assets;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// Represents a point particle system. Do not forget to call <see cref="Dispose(bool)"/> when no longer using it
	/// </summary>
	public class PointParticleSystem : GPUParticleSystem<PointParticleSystemSettings, PointParticle, PointParticleVertex>, IDisposable
	{
		// Buffers
		protected override DynamicVertexBuffer VertexBuffer { get; set; }
		protected override DynamicIndexBuffer IndexBuffer { get; set; }

		protected override PointParticleVertex[] Vertices { get; set; }
		protected override int[] Indices { get; set; }

		// Misc
		protected override int CurrentTime { get; set; }
		protected override int LastParticleTime { get; set; }
		protected override int LastParticleLifespan { get; set; }

		// Buffer management
		protected override int CurrentParticleIndex { get; set; }
		protected override int CurrentBufferIndex { get; set; }
		protected override bool SendBatch { get; set; }
		protected override int StartIndex { get; set; }

		public PointParticleSystem(PointParticleSystemSettings settings) : base(settings)
		{
			GPUParticleManager.AddSystem(this);
		}

		// Function
		public override void Draw(Layer layer = Layer.None)
		{
			// Safeguard
			if (Effect is null)
			{
				LoadEffect();
				return;
			}

			// Don't draw or perform calculations if the most recent particle has expired, making the system idle
			if (LastParticleTime < LastParticleLifespan)
			{
				// Ensure offsets
				if (layer is Layer.BeforeWater)
				{
					OffsetParameter.SetValue(new Vector2(Main.offScreenRange));
				}
				else
				{
					OffsetParameter.SetValue(Vector2.Zero);
				}

				// Set blend state
				var previousBlendState = Device.BlendState;
				Device.BlendState = BlendState;

				// Set buffers
				Device.SetVertexBuffer(VertexBuffer);
				Device.Indices = null;

				// Do particle pass
				Pass.Apply();
				Device.DrawPrimitives(PrimitiveType.PointListEXT, 0, MaxParticles);

				// Reset blend state
				Device.BlendState = previousBlendState;
			}
		}

		public override void NewParticle(Vector2 position, Vector2 velocity, PointParticle particle, int? lifespan = null)
		{
			int lifeSpan = lifespan ?? Lifespan;

			Vertices[CurrentParticleIndex] = new PointParticleVertex()
			{
				Position = new Vector4(position, 0f, 1f),

				StartColor = particle.StartColor,
				EndColor = particle.EndColor,

				Velocity = LibUtilities.Vec4From2Vec2(velocity, particle.VelocityDeviation),
				Acceleration = particle.VelocityAcceleration,

				DepthTime = new Vector4(particle.Depth, particle.DepthVelocity, CurrentTime, lifeSpan),
			};

			// This means that we just started adding particles since the last batch
			if (StartIndex == -1)
			{
				StartIndex = CurrentParticleIndex;
			}

			// Set idle checking parameters
			if (LastParticleTime != -1 && LastParticleLifespan - LastParticleTime < lifeSpan)
			{
				LastParticleTime = 0;
				LastParticleLifespan = lifeSpan;
			}

			// We wrap back to zero and immediately send our batch of new particles without waiting
			if (++CurrentParticleIndex >= MaxParticles)
			{
				SetBuffers();

				// We reset since we batched
				CurrentParticleIndex = 0; // This effectively means that particles will be overwritten
				return;
			}

			SendBatch = true;
		}

		public override void Clear()
		{
			Main.QueueMainThreadAction(() =>
			{
				VertexBuffer.SetData(Array.Empty<QuadParticleVertex>(), SetDataOptions.Discard);
			});
		}

		// Setters
		protected override void CreateBuffers()
		{
			VertexBuffer = new(Device, typeof(PointParticleVertex), MaxParticles, BufferUsage.WriteOnly);
			Vertices = new PointParticleVertex[MaxParticles];
		}

		protected override void SetBuffers()
		{
			VertexBuffer.SetData(PointParticleVertex.SizeInBytes * StartIndex, Vertices, StartIndex, (CurrentParticleIndex - StartIndex), PointParticleVertex.SizeInBytes, SetDataOptions.NoOverwrite);

			// Reset
			StartIndex = -1;
			SendBatch = false;
		}

		protected override void SetPass()
		{
			Pass = Effect.CurrentTechnique.Passes["Point"];
		}

		// Disposing
		private bool _disposedValue;
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (!_disposedValue)
			{
				if (disposing)
				{
					GPUParticleManager.RemoveSystem(this);
				}

				_disposedValue = true;
			}
		}
	}
}
