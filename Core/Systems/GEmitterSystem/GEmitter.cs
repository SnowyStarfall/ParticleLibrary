using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ParticleLibrary.Core.Systems.GEmitterSystem
{
	/// <summary>
	/// A basic emitter. Can be inheritied to provide custom functionality.
	/// </summary>
	public class GEmitter
	{
		public GParticleSystem System { get; private set; }

		public GEmitterSettings EmitterSettings { get; private set; }
		public GEmitterParticleSettings ParticleSettings { get; private set; }
		public GEmitterColorSettings ColorSettings { get; private set; }

		private RectangleF _bounds;

		public GEmitter(GParticleSystem system, GEmitterSettings emitterSettings, GEmitterParticleSettings particleSettings, GEmitterColorSettings colorSettings)
		{
			if (system is null)
				throw new ArgumentNullException(nameof(system));

			System = system;

			EmitterSettings = emitterSettings ?? new();
			ParticleSettings = particleSettings ?? new();
			ColorSettings = colorSettings ?? new();
		}

		public GEmitter(GParticleSystem system)
		{
			if (system is null)
				throw new ArgumentNullException(nameof(system));

			System = system;

			EmitterSettings = new();
			ParticleSettings = new();
			ColorSettings = new();
		}

		public void Update()
		{
			// TODO: Update functionality
			// TODO: Needs management by overarching system
			// TODO: Needs culling
			// TODO: Needs serializing
		}

		public void SetSystem(GParticleSystem system)
		{
			if (system is null)
				throw new ArgumentNullException(nameof(system));

			System = system;
		}

		public void SetShape(GEmitterShape shape)
		{
			Shape = shape;
		}

		public void SetOrigin(GEmitterOrigin origin)
		{
			Origin = origin;
		}

		public void SetFrequency(int min, int max)
		{

		}

		public void SetVelocity(Vector2 min, Vector2 max)
		{

		}

		public void SetVelocity(float min, float max, float minRadians, float maxRadians)
		{

		}
	}
}
