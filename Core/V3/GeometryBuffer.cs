using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core.V3.Interfaces;
using System;
using Terraria;

namespace ParticleLibrary.Core.V3
{
	/// <summary>
	/// An instanced representation of geometry.
	/// </summary>
	/// <typeparam name="TVertex">The geometry vertex.</typeparam>
	/// <typeparam name="TInstance">The instance struct.</typeparam>
	public abstract class GeometryBuffer<TVertex, TInstance> : IInstancedBuffer<TVertex, TInstance>
		where TVertex : struct
		where TInstance : struct
	{
		// Buffers
		private VertexBuffer _geometryBuffer;
		private IndexBuffer _indexBuffer;
		private DynamicVertexBuffer _instanceBuffer;
		private VertexBufferBinding[] _bufferBindings;

		// Instance
		private VertexDeclaration _instanceDeclaration;
		private readonly int _maxInstances;

		// Geometry
		private TVertex[] _vertices;
		private short[] _indices;

		public GeometryBuffer(int maxInstances)
		{
			ParticleLibrary.Log.Info("Registering new buffer...");

			_maxInstances = ParticleManagerV3.RegisterBuffer(this, maxInstances);

			ParticleLibrary.Log.Info("...Registered geometry buffer successfully");
		}

		/// <summary>
		/// Initializes the vertex and index buffers. This must be called.
		/// </summary>
		/// <param name="vertices">The geometry.</param>
		/// <param name="indices">The indices for the geometry.</param>
		/// <param name="instanceElements">The elements describing the instance struct.</param>
		public void Initialize(TVertex[] vertices, short[] indices, VertexElement[] instanceElements)
		{
			Main.QueueMainThreadAction(() =>
			{
				_vertices = vertices;
				_indices = indices;

				// Create our static buffers.
				_geometryBuffer = new(Main.graphics.GraphicsDevice, typeof(TVertex), _vertices.Length, BufferUsage.WriteOnly);
				_indexBuffer = new(Main.graphics.GraphicsDevice, IndexElementSize.SixteenBits, _indices.Length, BufferUsage.WriteOnly);
				_geometryBuffer.SetData(_vertices);
				_indexBuffer.SetData(_indices);

				// Initialize our instance buffer - which holds our instance data - and our buffer bindings.
				_instanceDeclaration = new(instanceElements);
				_instanceBuffer = new(Main.graphics.GraphicsDevice, _instanceDeclaration, _maxInstances, BufferUsage.WriteOnly);

				// Initialize our buffer bindings, allowing us to assign multiple vertex buffers.
				_bufferBindings = new VertexBufferBinding[2];
				_bufferBindings[0] = new VertexBufferBinding(_geometryBuffer);
				_bufferBindings[1] = new VertexBufferBinding(_instanceBuffer, 0, 1);
			});
		}

		/// <summary>
		/// Overwrites the instance buffer data with new data.
		/// </summary>
		/// <param name="data"></param>
		public void SetData(TInstance[] data) => _instanceBuffer.SetData(data);

		/// <summary>
		/// Overwrites a section of the instance buffer data with new data, starting at index <paramref name="startIndex"/> with a length of <paramref name="data"/>.Length.
		/// </summary>
		/// <param name="data">The data</param>
		/// <param name="startIndex">The index to begin replacement.</param>
		public void SetData(TInstance[] data, int startIndex) => _instanceBuffer.SetData(data, startIndex, data.Length, SetDataOptions.None);

		/// <summary>
		/// Provides direct access to the buffers and their values.
		/// </summary>
		/// <param name="bufferBindings"></param>
		/// <param name="indexBuffer"></param>
		public void GetBuffers(out VertexBufferBinding[] bufferBindings, out IndexBuffer indexBuffer)
		{
			bufferBindings = _bufferBindings;
			indexBuffer = _indexBuffer;
		}

		/// <summary>
		/// Gets the maximum amount of instances.
		/// </summary>
		/// <returns>The maximum amount of instances</returns>
		public int GetMaxInstances()
		{
			return _maxInstances;
		}

		public abstract void Render();

		public void Dispose()
		{
			_geometryBuffer.Dispose();
			_geometryBuffer = null;
			_indexBuffer.Dispose();
			_indexBuffer = null;
			_instanceBuffer.Dispose();
			_instanceBuffer = null;
			_instanceDeclaration.Dispose();
			_instanceDeclaration = null;
			_bufferBindings = null;

			GC.SuppressFinalize(this);
		}
	}
}
