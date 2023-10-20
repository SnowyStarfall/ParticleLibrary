using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core
{
    public class GRenderVertex
    {
        public Vector2 TexCoord;
        public Color Color;

        public GRenderVertex(Vector2 texCoord, Color color)
        {
            TexCoord = texCoord;
            Color = color;
        }
    }
}
