using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core.Systems.ParticleSystem
{
	internal struct GParticleVertex : IVertexType
	{
		public Vector4 Position;
		public Vector2 TexCoord;
		public Color StartColor;
		public Color EndColor;

		public Vector4 Velocity;
		public float TimeOfAdd;


		public static readonly VertexDeclaration VertexDeclaration = new		(
			new VertexElement(sizeof(float) * 0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0),
			new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),

			new VertexElement(sizeof(float) * 6, VertexElementFormat.Color, VertexElementUsage.Color, 0), // Start Color
			new VertexElement(sizeof(float) * 7, VertexElementFormat.Color, VertexElementUsage.Color, 1), // End Color

			new VertexElement(sizeof(float) * 8, VertexElementFormat.Vector4, VertexElementUsage.Normal, 0), // Velocity
			new VertexElement(sizeof(float) * 12, VertexElementFormat.Single, VertexElementUsage.Normal, 1) // TimeOfAdd
		);

		readonly VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
	}
}
