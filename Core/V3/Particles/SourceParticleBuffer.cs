using System;
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
	public class SourceParticleBuffer<TBehavior> : GeometryBuffer<VertexPositionTexture, SourceParticleInstance>, IUpdatable, ICreatable<SourceParticleInfo>
		where TBehavior : Behavior<SourceParticleInfo>, new()
	{
		/// <summary>
		/// The <see cref="BlendState"/> of this <see cref="ParticleBuffer{TBehavior}"/>.
		/// </summary>
		public BlendState BlendState { get; private set; }

		/// <summary>
		/// The <see cref="SamplerState"/> of this <see cref="ParticleBuffer{TBehavior}"/>.
		/// </summary>
		public SamplerState SamplerState { get; private set; }

		/// <summary>
		/// The <typeparamref name="TBehavior"/> associated with this <see cref="ParticleBuffer{TBehavior}"/>.
		/// </summary>
		public TBehavior Behavior { get; private set; }

		// Geometry
		private readonly int _vertexCount;
		private readonly int _primitiveCount;

		// Data
		private readonly int _maxInstances;
		private SourceParticleInfo[] _infos;
		private SourceParticleInstance[] _instances;
		private Stack<int> _inactiveInstances;
		private Vector2 _textureSize;

		private int Count => _maxInstances - _inactiveInstances.Count;

		public SourceParticleBuffer(int maxInstances = 256) : base(maxInstances)
		{
			VertexPositionTexture[] vertices =
			[
				new(new(-0.5f, -0.5f, 0f), Vector2.Zero),
				new(new(-0.5f, 0.5f, 0f), Vector2.UnitY),
				new(new(0.5f, -0.5f, 0f), Vector2.UnitX),
				new(new(0.5f, 0.5f, 0f), Vector2.One)
			];

			short[] indices = [0, 2, 3, 0, 3, 1];

			_vertexCount = vertices.Length;
			_primitiveCount = indices.Length / 3;

			BlendState = BlendState.AlphaBlend;
			SamplerState = SamplerState.PointClamp;

			Behavior = new TBehavior();
			_maxInstances = GetMaxInstances();
			_infos = new SourceParticleInfo[_maxInstances];
			_instances = new SourceParticleInstance[_maxInstances];
			_inactiveInstances = new Stack<int>(_maxInstances);

			for (int i = 0; i < _maxInstances; i++)
			{
				_infos[i].Free = true;
			}

			for (int i = _maxInstances - 1; i >= 0; i--)
			{
				_inactiveInstances.Push(i);
			}

			var instanceElements = new VertexElement[4];
			instanceElements[0] = new VertexElement(sizeof(float) * 0, VertexElementFormat.Vector4, VertexElementUsage.Normal, 0);
			instanceElements[1] = new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2, VertexElementUsage.Normal, 1);
			instanceElements[2] = new VertexElement(sizeof(float) * 6, VertexElementFormat.Color, VertexElementUsage.Color, 0);
			instanceElements[3] = new VertexElement(sizeof(float) * 7, VertexElementFormat.Vector4, VertexElementUsage.Normal, 2);

			Initialize(vertices, indices, instanceElements);
		}

		/// <summary>
		/// Updates the particles.
		/// </summary>
		public void Update()
		{
			if (Count == 0)
			{
				return;
			}

			for (int i = 0; i < _infos.Length; i++)
			{
				if (_infos[i].Time <= 0)
				{
					if (!_infos[i].Free)
					{
						_infos[i].Free = true;
						_inactiveInstances.Push(i);
						_instances[i].Color = Color.Transparent;
					}

					continue;
				}

				Behavior.Update(ref _infos[i]);

				_instances[i].Position_Scale = new Vector4(_infos[i].Position.X, _infos[i].Position.Y, _infos[i].Scale.X, _infos[i].Scale.Y);
				_instances[i].Rotation_Depth = new Vector2(_infos[i].Rotation, _infos[i].Depth);
				_instances[i].Color = _infos[i].Color;
				_instances[i].Source = _infos[i].Source;
			}

			SetData(_instances);
		}

		/// <summary>
		/// Renders the particles.
		/// </summary>
		public override void Render()
		{
			// No active instances
			if (Count == 0 || !ParticleManagerV3.SourceParticleeffect.IsLoaded)
			{
				return;
			}

			// Retrieve buffers
			GetBuffers(out VertexBufferBinding[] vertexBuffers, out IndexBuffer indexBuffer);
			Texture2D texture = LibUtilities.GetTexture(Behavior.Texture);
			_textureSize = texture.Size();

			// Set our variables
			Main.graphics.GraphicsDevice.RasterizerState = Main.LocalPlayer.gravDir == -1 ? RasterizerState.CullClockwise : RasterizerState.CullCounterClockwise;
			Main.graphics.GraphicsDevice.BlendState = BlendState;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState;
			Main.graphics.GraphicsDevice.SetVertexBuffers(vertexBuffers);
			Main.graphics.GraphicsDevice.Indices = indexBuffer;

			// Apply effect and draw
			Main.graphics.GraphicsDevice.Textures[0] = texture;
			ParticleManagerV3.SourceParticleeffect.Apply(texture.Size());
			Main.graphics.GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertexCount, 0, _primitiveCount, _maxInstances);
		}

		/// <summary>
		/// Creates a new particle from the given info.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="useDefaultSource">Whether to use the full texture as the source.</param>
		public void Create(SourceParticleInfo info)
		{
			// Don't create particles on the server.
			if (Main.netMode is NetmodeID.Server)
			{
				return;
			}

			// Active instances.
			if (Count == _maxInstances)
			{
				return;
			}

			// Create instance.
			int index = _inactiveInstances.Pop();
			SourceParticleInstance instance = new()
			{
				Position_Scale = new Vector4(info.Position.X, info.Position.Y, info.Scale.X, info.Scale.Y),
				Rotation_Depth = new Vector2(info.Rotation, info.Depth),
				Color = info.InitialColor,
			};
			
			// Check source for data.
			if (info.Source.IsDefault())
			{
				Rectangle source = new(
					x: 0, 
					y: 0, 
					width: _textureSize.X,
					height: _textureSize.Y
				);

				info.Source = source;
				instance.Source = source;
			}

			// Initialize info.
			Behavior.Initialize(ref info);

			// Add to buffer.
			_infos[index] = info;
			_instances[index] = instance;
		}

		public SourceParticleBuffer<TBehavior> SetBlendState(BlendState blendState)
		{
			if (blendState is null)
			{
				return this;
			}

			BlendState = blendState;
			return this;
		}

		public SourceParticleBuffer<TBehavior> SetSamplerState(SamplerState samplerState)
		{
			if (samplerState is null)
			{
				return this;
			}

			SamplerState = samplerState;
			return this;
		}

		public override void Dispose()
		{
			base.Dispose();

			_infos = null;
			_instances = null;
			_inactiveInstances = null;

			GC.SuppressFinalize(this);
		}
	}
}