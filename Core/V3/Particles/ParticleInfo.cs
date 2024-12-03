using Microsoft.Xna.Framework;
using SystemVector2 = System.Numerics.Vector2;

namespace ParticleLibrary.Core.V3.Particles
{
	public struct ParticleInfo
	{
		// World
		public SystemVector2 Position;
		public SystemVector2 Velocity;
		public float Rotation;
		public SystemVector2 Scale;

		// Other
		public Color Color;
		public int Time;

		// Readonly
		public readonly SystemVector2 InitialScale;
		public readonly Color InitialColor;
		public readonly int Duration;
		public readonly float[] Data;

		internal bool Free = false;

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
	}
}
