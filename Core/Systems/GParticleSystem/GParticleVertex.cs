using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core.Systems
{
	internal struct GParticleVertex : IVertexType
	{
		public Vector4 Position;
		public Vector2 TexCoord;

		public Color StartColor;
		public Color EndColor;

		public Vector4 Velocity;
		public Vector4 Scale;
		public Vector4 Rotation;

		public Vector3 DepthTime;
		public Color Random;

		public static readonly VertexDeclaration VertexDeclaration = new
		(
			new VertexElement(sizeof(float) * 0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0),
			new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),

			new VertexElement(sizeof(float) * 6, VertexElementFormat.Color, VertexElementUsage.Color, 0), // Start Color
			new VertexElement(sizeof(float) * 7, VertexElementFormat.Color, VertexElementUsage.Color, 1), // End Color

			new VertexElement(sizeof(float) * 8, VertexElementFormat.Vector4, VertexElementUsage.Normal, 0), // Velocity
			new VertexElement(sizeof(float) * 12, VertexElementFormat.Vector4, VertexElementUsage.Normal, 1), // Scale
			new VertexElement(sizeof(float) * 16, VertexElementFormat.Vector4, VertexElementUsage.Normal, 2), // Rotation

			new VertexElement(sizeof(float) * 20, VertexElementFormat.Vector3, VertexElementUsage.Normal, 3), // Depth Time
			new VertexElement(sizeof(float) * 23, VertexElementFormat.Color, VertexElementUsage.Color, 2) // Random
		);

		readonly VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
	}
}
