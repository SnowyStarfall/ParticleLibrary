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
		public float TimeOfAdd;

		public GParticleVertex(Vector2 position, Vector2 texCoord, Color color, Vector2 velocity, float timeOfAdd)
		{
			Position = new Vector4(position.X, position.Y, 0f, 1f);
			TexCoord = texCoord;
			Color = color;
			Velocity = velocity;
			TimeOfAdd = timeOfAdd;
		}

		public static readonly VertexDeclaration VertexDeclaration = new		(
			new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0),
			new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
			new VertexElement(sizeof(float) * 6, VertexElementFormat.Color, VertexElementUsage.Color, 0),

			new VertexElement(sizeof(float) * 7, VertexElementFormat.Vector2, VertexElementUsage.Normal, 0), // Velocity
			new VertexElement(sizeof(float) * 9, VertexElementFormat.Single, VertexElementUsage.Normal, 1) // TimeOfAdd
		);

		readonly VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
	}
}
