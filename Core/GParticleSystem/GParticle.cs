using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core
{
    public class GParticle
    {
        public Color StartColor { get; set; } = Color.White;
        public Color EndColor { get; set; } = Color.White;

        public Vector2 VelocityAcceleration { get; set; } = Vector2.Zero;

        public Vector2 Scale { get; set; } = Vector2.One;
        public Vector2 ScaleVelocity { get; set; } = Vector2.Zero;

        public float Rotation { get; set; } = 0f;
        public float RotationVelocity { get; set; } = 0f;

        public float Depth { get; set; } = 1f;
        public float DepthVelocity { get; set; } = 0f;
    }
}
