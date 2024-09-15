using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core.V3
{
    public struct ParticleInfo
    {
        // World
        public Vector2 Position;
        public Vector2 Velocity;
        public float Rotation;
        public Vector2 Scale;

        // Other
        public Color Color;
        public int Time;

        // Readonly
        public readonly Vector2 InitialScale;
        public readonly Color InitialColor;
        public readonly int Duration;
        public readonly float[] Data;

        public ParticleInfo(Vector2 position, Vector2 velocity, float rotation, Vector2 scale, Color color, int duration, params float[] data)
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
