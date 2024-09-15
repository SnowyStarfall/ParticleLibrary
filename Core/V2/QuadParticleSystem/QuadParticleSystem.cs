using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using Terraria;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// Represents a Quad particle system. Do not forget to call <see cref="Dispose(bool)"/> when no longer using it
	/// </summary>
	public class QuadParticleSystem : GPUParticleSystem<QuadParticleSystemSettings, QuadParticle, QuadParticleVertex>, IDisposable
	{
		private static readonly VertexBuffer _geometryBuffer;
		private static readonly IndexBuffer _indexBuffer;

		private static readonly VertexElement[] _instanceElements;
		private static readonly VertexDeclaration _instanceDeclaration;

		static QuadParticleSystem()
		{
			// Create our static buffers. These represent our geometry (vertex quad) that will be used for all particle systems in the same way.
			_geometryBuffer = new(Main.graphics.GraphicsDevice, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly);
			_indexBuffer = new(Main.graphics.GraphicsDevice, IndexElementSize.SixteenBits, 6, BufferUsage.WriteOnly);

			_geometryBuffer.SetData(new VertexPositionTexture[4]
{
				new(new(-0.5f, -0.5f, 0f), Vector2.Zero),
				new(new(0.5f, -0.5f, 0f), Vector2.UnitX),
				new(new(-0.5f, 0.5f, 0f), Vector2.UnitY),
				new(new(0.5f, -0.5f, 0f), Vector2.One)
			});

			_indexBuffer.SetData(new short[6]
			{
				0, 1, 3, 0, 3, 2
			});

			// Initialize our instancing vertex info
			_instanceElements = new VertexElement[9];
			_instanceElements[0] = new VertexElement(sizeof(float) * 0, VertexElementFormat.Vector4, VertexElementUsage.Normal, 0);
			_instanceElements[1] = new VertexElement(sizeof(float) * 4, VertexElementFormat.Color, VertexElementUsage.Color, 0);
			_instanceElements[2] = new VertexElement(sizeof(float) * 5, VertexElementFormat.Color, VertexElementUsage.Color, 0);
			_instanceElements[3] = new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector4, VertexElementUsage.Normal, 1);
			_instanceElements[4] = new VertexElement(sizeof(float) * 10, VertexElementFormat.Vector2, VertexElementUsage.Normal, 2);
			_instanceElements[5] = new VertexElement(sizeof(float) * 12, VertexElementFormat.Vector2, VertexElementUsage.Normal, 3);
			_instanceElements[6] = new VertexElement(sizeof(float) * 14, VertexElementFormat.Vector4, VertexElementUsage.Normal, 4);
			_instanceElements[7] = new VertexElement(sizeof(float) * 18, VertexElementFormat.Vector4, VertexElementUsage.Normal, 5);
			_instanceElements[8] = new VertexElement(sizeof(float) * 22, VertexElementFormat.Vector4, VertexElementUsage.Normal, 6);

			_instanceDeclaration = new(_instanceElements);
		}


		// Buffers
		protected override DynamicVertexBuffer VertexBuffer { get; set; }
		protected override DynamicIndexBuffer IndexBuffer { get; set; }

		protected override QuadParticleVertex[] Vertices { get; set; }
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

		public QuadParticleSystem(QuadParticleSystemSettings settings) : base(settings)
		{
			GPUParticleManager.AddQuadSystem(this);
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
				Device.Indices = IndexBuffer;

				// Do particle pass
				Pass.Apply();
				Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, MaxParticles * 4, 0, MaxParticles * 2);

				// Reset blend state
				Device.BlendState = previousBlendState;
			}
		}

		/// <summary>
		/// Creates a particle at the given position with the given velocity and base settings
		/// </summary>
		/// <param name="position"></param>
		/// <param name="velocity"></param>
		/// <param name="particle"></param>
		/// <param name="lifespan"></param>
		public override void NewParticle(Vector2 position, Vector2 velocity, QuadParticle particle, int? lifespan = null)
		{
			Vector2 size = new(Texture.Width * particle.Scale.X, Texture.Height * particle.Scale.Y);
			int lifeSpan = lifespan ?? Lifespan;

			Vertices[CurrentParticleIndex * 4] = new QuadParticleVertex()
			{
				Position = new Vector4(position.X - size.X / 2f, position.Y - size.Y / 2f, 0f, 1f),
				TexCoord = new Vector2(),

				StartColor = particle.StartQuad?.TopLeft ?? particle.StartColor,
				EndColor = particle.EndQuad?.TopLeft ?? particle.EndColor,

				Velocity = LibUtilities.Vec4From2Vec2(velocity, particle.VelocityDeviation),
				Acceleration = particle.VelocityAcceleration,
				Size = new Vector2(Texture.Width, Texture.Height),
				Scale = LibUtilities.Vec4From2Vec2(particle.Scale, particle.ScaleVelocity),
				Rotation = new Vector4(-0.5f, -0.5f, particle.Rotation, particle.RotationVelocity),

				DepthTime = new Vector4(particle.Depth, particle.DepthVelocity, CurrentTime, lifeSpan)
			};
			Vertices[CurrentParticleIndex * 4 + 1] = new QuadParticleVertex()
			{
				Position = new Vector4(position.X - size.X / 2f, position.Y + size.Y / 2f, 0f, 1f),
				TexCoord = new Vector2(0f, 1f),

				StartColor = particle.StartQuad?.BottomLeft ?? particle.StartColor,
				EndColor = particle.EndQuad?.BottomLeft ?? particle.EndColor,

				Velocity = LibUtilities.Vec4From2Vec2(velocity, particle.VelocityDeviation),
				Acceleration = particle.VelocityAcceleration,
				Size = new Vector2(Texture.Width, Texture.Height),
				Scale = LibUtilities.Vec4From2Vec2(particle.Scale, particle.ScaleVelocity),
				Rotation = new Vector4(-0.5f, 0.5f, particle.Rotation, particle.RotationVelocity),

				DepthTime = new Vector4(particle.Depth, particle.DepthVelocity, CurrentTime, lifeSpan)
			};
			Vertices[CurrentParticleIndex * 4 + 2] = new QuadParticleVertex()
			{
				Position = new Vector4(position.X + size.X / 2f, position.Y - size.Y / 2f, 0f, 1f),
				TexCoord = new Vector2(1f, 0f),

				StartColor = particle.StartQuad?.TopRight ?? particle.StartColor,
				EndColor = particle.EndQuad?.TopRight ?? particle.EndColor,

				Velocity = LibUtilities.Vec4From2Vec2(velocity, particle.VelocityDeviation),
				Acceleration = particle.VelocityAcceleration,
				Size = new Vector2(Texture.Width, Texture.Height),
				Scale = LibUtilities.Vec4From2Vec2(particle.Scale, particle.ScaleVelocity),
				Rotation = new Vector4(0.5f, -0.5f, particle.Rotation, particle.RotationVelocity),

				DepthTime = new Vector4(particle.Depth, particle.DepthVelocity, CurrentTime, lifeSpan)
			};
			Vertices[CurrentParticleIndex * 4 + 3] = new QuadParticleVertex()
			{
				Position = new Vector4(position.X + size.X / 2f, position.Y + size.Y / 2f, 0f, 1f),
				TexCoord = new Vector2(1f),

				StartColor = particle.StartQuad?.BottomRight ?? particle.StartColor,
				EndColor = particle.EndQuad?.BottomRight ?? particle.EndColor,

				Velocity = LibUtilities.Vec4From2Vec2(velocity, particle.VelocityDeviation),
				Acceleration = particle.VelocityAcceleration,
				Size = new Vector2(Texture.Width, Texture.Height),
				Scale = LibUtilities.Vec4From2Vec2(particle.Scale, particle.ScaleVelocity),
				Rotation = new Vector4(0.5f, 0.5f, particle.Rotation, particle.RotationVelocity),

				DepthTime = new Vector4(particle.Depth, particle.DepthVelocity, CurrentTime, lifeSpan)
			};

			int vertexIndex = CurrentParticleIndex * 4;

			// _currentParticleIndex is 0
			// vertexIndex would be 0
			// vertices would be 0, 1, 2, 3
			// indices would be 0, 2, 3, 0, 3, 1

			// _currentParticleIndex is 1
			// vertexIndex would be 4
			// vertices would be 4, 5, 6, 7
			// indices would be 4, 6, 7, 4, 7, 5

			Indices[CurrentParticleIndex * 6] = vertexIndex;
			Indices[CurrentParticleIndex * 6 + 1] = vertexIndex + 2;
			Indices[CurrentParticleIndex * 6 + 2] = vertexIndex + 3;
			Indices[CurrentParticleIndex * 6 + 3] = vertexIndex;
			Indices[CurrentParticleIndex * 6 + 4] = vertexIndex + 3;
			Indices[CurrentParticleIndex * 6 + 5] = vertexIndex + 1;

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
										  //_currentBufferIndex = 0;
				return;
			}

			// Increment our buffer index and send our batch of new particles without waiting, but only if we didn't just batch
			//if (++_currentBufferIndex >= BufferSize)
			//{
			//	_currentBufferIndex = 0;
			//	_startIndex = _currentParticleIndex - BufferSize;

			//	if (_currentParticleIndex != 0)
			//	{
			//		SetBuffers();
			//		return;
			//	}
			//}

			SendBatch = true;
		}

		public override void Clear()
		{
			Main.QueueMainThreadAction(() =>
			{
				VertexBuffer.SetData(Array.Empty<QuadParticleVertex>(), SetDataOptions.Discard);
				IndexBuffer.SetData(Array.Empty<int>());
			});
		}

		// Effect
		protected override void CreateBuffers()
		{
			VertexBuffer = new(Device, typeof(QuadParticleVertex), MaxParticles * 4, BufferUsage.WriteOnly);
			IndexBuffer = new(Device, IndexElementSize.ThirtyTwoBits, MaxParticles * 6, BufferUsage.WriteOnly);

			Vertices = new QuadParticleVertex[MaxParticles * 4];
			Indices = new int[MaxParticles * 6];
		}

		protected override void SetBuffers()
		{
			VertexBuffer.SetData(QuadParticleVertex.SizeInBytes * StartIndex * 4, Vertices, StartIndex, (CurrentParticleIndex - StartIndex) * 4, QuadParticleVertex.SizeInBytes, SetDataOptions.NoOverwrite);
			IndexBuffer.SetData(sizeof(int) * StartIndex * 6, Indices, StartIndex, (CurrentParticleIndex - StartIndex) * 6, SetDataOptions.NoOverwrite);

			// Reset
			StartIndex = -1;
			SendBatch = false;
		}

		protected override void SetPass()
		{
			Pass = Effect.CurrentTechnique.Passes["Quad"];
		}

		// Dispose
		private bool _disposedValue;

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (!_disposedValue)
			{
				if (disposing)
				{
					GPUParticleManager.RemoveQuadSystem(this);
				}

				_disposedValue = true;
			}
		}
	}
}
