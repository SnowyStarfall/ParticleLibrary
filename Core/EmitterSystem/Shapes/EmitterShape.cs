using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core.Shapes
{
	public abstract class EmitterShape
	{
		/// <summary>
		/// Originating mod.
		/// </summary>
		internal string Assembly { get; init; }
		/// <summary>
		/// Originating type.
		/// </summary>
		internal string Type { get; init; }

		public EmitterShape()
		{
			Assembly = GetType().Assembly.GetName().Name;
			Type = GetType().FullName;
		}

		/// <summary>
		/// Solve the position with the provided parameters
		/// </summary>
		/// <param name="center">The emitter's center, accounting for width and height</param>
		/// <param name="origin">The emitter's origin, determining how to distrubute particles</param>
		/// <param name="emitter">The emitter</param>
		/// <returns></returns>
		public abstract Vector2 Solve(Vector2 center, EmitterOrigin origin, Emitter emitter);
	}
}
