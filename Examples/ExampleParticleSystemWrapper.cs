﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;

namespace ParticleLibrary.Examples
{
	/// <summary>
	/// This class serves as a wrapper for any GPU particle system, allowing custom functionality to be implemented.
	/// An example of such functionality would be to embed the system into a <see cref="RenderTarget2D"/>, which requires manually calling <see cref="IGPUParticleSystem{T}.Draw(Layer)"/>
	/// </summary>
	/// <typeparam name="T"><see cref="QuadParticle"/> and <see cref="PointParticle"/> both inherit from <see cref="GPUParticle"/></typeparam>
	public class ExampleParticleSystemWrapper<T>
		where T : GPUParticle
	{
		public IGPUParticleSystem<T> System { get; }
		public GPUSystemSettings Settings { get; }

		public ExampleParticleSystemWrapper(IGPUParticleSystem<T> system, GPUSystemSettings settings)
		{
			System = system;
			Settings = settings;
		}

		/// <summary>
		/// A shorthand for accessing the <see cref="IGPUParticleSystem{T}.AddParticle(Vector2, Vector2, T)"/> method.
		/// </summary>
		/// <param name="position">The location of the particle.</param>
		/// <param name="velocity">The velocity of the particle.</param>
		/// <param name="particle">The particle settings.</param>
		public void AddParticle(Vector2 position, Vector2 velocity, T particle)
		{
			System.NewParticle(position, velocity, particle);
		}
	}
}
