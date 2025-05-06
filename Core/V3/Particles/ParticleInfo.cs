using Microsoft.Xna.Framework;
using System;
using SystemVector2 = System.Numerics.Vector2;

namespace ParticleLibrary.Core.V3.Particles
{
	/// <summary>
	/// A struct representing a particle.
	/// </summary>
	public struct ParticleInfo
	{
		/// <summary>
		/// The particle's position.
		/// </summary>
		public SystemVector2 Position;

		/// <summary>
		/// The particle's velocity.
		/// </summary>
		public SystemVector2 Velocity;

		/// <summary>
		/// The particle's rotation.
		/// </summary>
		public float Rotation;

		/// <summary>
		/// The particle's scale.
		/// </summary>
		public SystemVector2 Scale;

		/// <summary>
		/// The particle's depth. Defaults to 1f. Depth is nonlinear and is achieved via perspective divide.
		/// </summary>
		public float Depth = 1f;

		/// <summary>
		/// The particle's color.
		/// </summary>
		public Color Color;

		/// <summary>
		/// The time left before this particle disappears. Particles die at Time <= 0
		/// </summary>
		public int Time;

		/// <summary>
		/// The particle's initial scale.
		/// </summary>
		public readonly SystemVector2 InitialScale;

		/// <summary>
		/// The particle's initial color.
		/// </summary>
		public readonly Color InitialColor;

		/// <summary>
		/// The particle's duration.
		/// </summary>
		public readonly int Duration;

		/// <summary>
		/// An optional array of values. Can be <see langword="null"/> if no values are provided to the constructor.
		/// </summary>
		public readonly float[] Data;

		internal bool Free = false;

		/// <summary>
		/// Creates a new instance of the <see cref="ParticleInfo"/> struct.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="velocity">The velocity.</param>
		/// <param name="rotation">The rotation.</param>
		/// <param name="scale">The scale.</param>
		/// <param name="color">The color.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="data">Optional values.</param>
		public ParticleInfo(SystemVector2 position, SystemVector2 velocity, float rotation, SystemVector2 scale, Color color, int duration, params float[] data)
		{
			Position = position;
			Velocity = velocity;
			Rotation = rotation;
			Scale = scale;
			InitialScale = scale;

			Color = color;
			InitialColor = color;

			Time = duration;
			Duration = duration;
			Data = data;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="ParticleInfo"/> struct.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="velocity">The velocity.</param>
		/// <param name="rotation">The rotation.</param>
		/// <param name="scale">The scale.</param>
		/// <param name="depth">The depth. Scales nonlinearly. Does not represent world-space units.</param>
		/// <param name="color">The color.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="data">Optional values.</param>
		public ParticleInfo(SystemVector2 position, SystemVector2 velocity, float rotation, SystemVector2 scale, float? depth, Color color, int duration, params float[] data)
		{
			Position = position;
			Velocity = velocity;
			Rotation = rotation;

			Scale = scale;
			InitialScale = scale;

			Depth = depth ?? 1f;

			Color = color;
			InitialColor = color;

			Time = duration;
			Duration = duration;
			Data = data;
		}
	}
}
