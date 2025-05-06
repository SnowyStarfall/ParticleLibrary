using ParticleLibrary.Core;
using ParticleLibrary.Core.V3;
using ParticleLibrary.Core.V3.Interfaces;
using ParticleLibrary.Core.V3.Particles;
using System;
using System.Collections.Generic;

namespace ParticleLibrary.Examples.V3
{
	/// <summary>
	/// A container providing an easy way to store multiple buffers under the same collection.
	/// </summary>
	/// <typeparam name="TKey">They key to use for the dictionary.</typeparam>
	public class ParticleDictionary<TKey>
	{
		/// <summary>
		/// A readonly dictionary of the contained buffers.
		/// </summary>
		public IReadOnlyDictionary<TKey, ICreatable<ParticleInfo>> ParticleBuffers
		{
			get => _particleBuffers.AsReadOnly();
		}

		private readonly Dictionary<TKey, ICreatable<ParticleInfo>> _particleBuffers;

		/// <summary>
		/// Creates a new instance of <see cref="ParticleDictionary{TKey}"/>.
		/// </summary>
		public ParticleDictionary()
		{
			_particleBuffers = [];
		}

		/// <summary>
		/// Adds a buffer of type <typeparamref name="TBuffer"/> to the collection and registers it to <paramref name="layer"/>.
		/// </summary>
		/// <param name="value">The key to add the buffer to.</param>
		/// <param name="buffer">The buffer to add to the collection.</param>
		/// <param name="layer">The layer to register to.</param>
		/// <returns>This collection.</returns>
		public ParticleDictionary<TKey> Add<TBuffer>(TKey value, TBuffer buffer, Layer layer)
			where TBuffer : IUpdatable, IRenderable, ICreatable<ParticleInfo>
		{
			_particleBuffers.Add(value, buffer);

			ParticleManagerV3.RegisterUpdatable(buffer);
			ParticleManagerV3.RegisterRenderable(layer, buffer);

			return this;
		}

		/// <summary>
		/// Creates a new particle with all contained buffers in <see cref="ParticleBuffers"/> 
		/// with the given <see cref="ParticleInfo"/>.
		/// </summary>
		/// <param name="info">The info.</param>
		public void Create(ParticleInfo info)
		{
			foreach((var _, var buffer) in _particleBuffers)
			{
				buffer.Create(info);
			}
		}
	}
}
