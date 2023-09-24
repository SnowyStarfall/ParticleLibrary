using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core.Systems.ParticleSystem
{
	internal struct GParticleVertex : IVertexType
	{
		public Vector4 Position;
		public Vector2 TexCoord;
		public Color Color;

		public Vector2 Velocity;


		public static readonly VertexDeclaration VertexDeclaration = new		(
			new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0),
			new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
			new VertexElement(sizeof(float) * 6, VertexElementFormat.Color, VertexElementUsage.Color, 0),

			new VertexElement(sizeof(float) * 7, VertexElementFormat.Vector2, VertexElementUsage.Normal, 0) // Velocity
		);

		readonly VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
	}
}
