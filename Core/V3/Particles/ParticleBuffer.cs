using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core.V3.Interfaces;
using ParticleLibrary.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ParticleLibrary.Core.V3.Particles
{
	/// <summary>
	/// Creates a new buffer of particles.
	/// </summary>
	/// <typeparam name="TBehavior"></typeparam>
	public class ParticleBuffer<TBehavior> : GeometryBuffer<VertexPositionTexture, ParticleInstance>, IUpdatable, ICreatable<ParticleInfo>
		where TBehavior : Behavior<ParticleInfo>, new()
	{
		// Geometry
		private static readonly VertexPositionTexture[] _vertices;
		private static readonly short[] _indices;
		private BlendState _blendState;
		private SamplerState _samplerState;

		// Data
		private readonly TBehavior _behavior;
		private readonly int _maxInstances;
		private readonly ParticleInfo[] _infos;
		private readonly ParticleInstance[] _instances;
		private readonly Stack<int> _inactiveInstances;

		private int Count => _maxInstances - _inactiveInstances.Count;

		static ParticleBuffer()
		{
			_vertices = [
				new(new(-0.5f, -0.5f, 0f), Vector2.Zero),
				new(new(-0.5f, 0.5f, 0f), Vector2.UnitY),
				new(new(0.5f, -0.5f, 0f), Vector2.UnitX),
				new(new(0.5f, 0.5f, 0f), Vector2.One)
			];

			_indices = [0, 2, 3, 0, 3, 1];
		}

		public ParticleBuffer(int maxInstances = 256) : base(maxInstances)
		{
			_blendState = BlendState.AlphaBlend;
			_samplerState = SamplerState.PointClamp;

			_behavior = new TBehavior();
			_maxInstances = GetMaxInstances();
			_infos = new ParticleInfo[_maxInstances];
			_instances = new ParticleInstance[_maxInstances];
			_inactiveInstances = new Stack<int>(_maxInstances);

			for (int i = 0; i < _maxInstances; i++)
			{
				ref var info = ref _infos[i];
				info.Free = true;
			}

			for (int i = _maxInstances - 1; i >= 0; i--)
			{
				_inactiveInstances.Push(i);
			}

			var instanceElements = new VertexElement[3];
			instanceElements[0] = new VertexElement(sizeof(float) * 0, VertexElementFormat.Vector4, VertexElementUsage.Normal, 0);
			instanceElements[1] = new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2, VertexElementUsage.Normal, 1);
			instanceElements[2] = new VertexElement(sizeof(float) * 6, VertexElementFormat.Color, VertexElementUsage.Color, 0);

			Initialize(_vertices, _indices, instanceElements);
		}

		/// <summary>
		/// Updates the particles.
		/// </summary>
		public void Update()
		{
			for (int i = 0; i < _infos.Length; i++)
			{
				var particle = _infos[i];
				if (particle.Time <= 0)
				{
					if (!particle.Free)
					{
						particle.Free = true;
						_inactiveInstances.Push(i);
						_instances[i].Color = Color.Transparent;
					}

					continue;
				}

				_behavior.Update(ref particle);
				_infos[i] = particle;

				_instances[i].Position_Scale = new Vector4(particle.Position.X, particle.Position.Y, particle.Scale.X, particle.Scale.Y);
				_instances[i].Rotation_Depth = new Vector2(particle.Rotation, particle.Depth);
				_instances[i].Color = particle.Color;
			}

			// Active instances
			if (Count > 0)
			{
				SetData(_instances);
			}
		}

		/// <summary>
		/// Renders the particles.
		/// </summary>
		public override void Render()
		{
			// No active instances
			if (Count == 0)
			{
				return;
			}

			// Retrieve buffers
			GetBuffers(out VertexBufferBinding[] vertexBuffers, out IndexBuffer indexBuffer);
			var texture = LibUtilities.GetTexture(_behavior.Texture);

			// Set our variables
			Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
			Main.graphics.GraphicsDevice.BlendState = _blendState;
			Main.graphics.GraphicsDevice.SamplerStates[0] = _samplerState;
			Main.graphics.GraphicsDevice.SetVertexBuffers(vertexBuffers);
			Main.graphics.GraphicsDevice.Indices = indexBuffer;

			// Apply effect and draw
			Main.graphics.GraphicsDevice.Textures[0] = texture;
			ParticleManagerV3.InstancedParticleEffect.Apply();
			Main.graphics.GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Length, 0, _vertices.Length / 2, _maxInstances);
		}

		/// <summary>
		/// Creates a new particle from the given info.
		/// </summary>
		/// <param name="info">The info.</param>
		public void Create(ParticleInfo info)
		{
			if (Main.netMode is NetmodeID.Server)
			{
				return;
			}

			// Active instances
			if (Count == _maxInstances)
			{
				return;
			}

			int index = _inactiveInstances.Pop();
			var instance = new ParticleInstance
			{
				Position_Scale = new Vector4(info.Position.X, info.Position.Y, info.Scale.X, info.Scale.Y),
				Rotation_Depth = new Vector2(info.Rotation, info.Depth),
				Color = info.InitialColor
			};

			_infos[index] = info;
			_instances[index] = instance;
		}

		public ParticleBuffer<TBehavior> SetBlendState(BlendState blendState)
		{
			if (blendState is null)
			{
				return this;
			}

			_blendState = blendState;
			return this;
		}

		public ParticleBuffer<TBehavior> SetSamplerState(SamplerState samplerState)
		{
			if (samplerState is null)
			{
				return this;
			}

			_samplerState = samplerState;
			return this;
		}
	}
}
