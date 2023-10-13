namespace ParticleLibrary.Core.Systems.GEmitterSystem
{
	public class GEmitterSettings
	{
		/// <summary>
		/// The shape of the emitter.
		/// </summary>
		public GEmitterShape Shape { get; private set; }
		/// <summary>
		/// How the emitter distributes particles.
		/// </summary>
		public GEmitterOrigin Origin { get; private set; }

		/// <summary>
		/// The width of the <see cref="Shape"/>. 
		/// No effect when used with <see cref="GEmitterShape.Point"/>. 
		/// Used as diameter for <see cref="GEmitterShape.Circle"/>
		/// Used as side length for <see cref="GEmitterShape.Rectangle"/>
		/// </summary>
		public float Width { get; set; } = 32f;
		/// <summary>
		/// The height of the <see cref="Shape"/>. 
		/// No effect when used with <see cref="GEmitterShape.Point"/>. 
		/// Used as diameter for <see cref="GEmitterShape.Circle"/>
		/// Used as side length for <see cref="GEmitterShape.Rectangle"/>
		/// </summary>
		public float Height { get; set; } = 32f;

		/// <summary>
		/// The minimum interval between particle spawns.
		/// </summary>
		public int MinimumInterval { get; set; } = 10;
		/// <summary>
		/// The maximum interval between particle spawns.
		/// </summary>
		public int MaximumInterval { get; set; } = 10;

		/// <summary>
		/// The minimum amount of particles to spawn at once.
		/// </summary>
		public int MinimumSpawns { get; set; } = 1;
		/// <summary>
		/// The maximum amount of particles to spawn at once.
		/// </summary>
		public int MaximumSpawns { get; set; } = 1;
	}
}
