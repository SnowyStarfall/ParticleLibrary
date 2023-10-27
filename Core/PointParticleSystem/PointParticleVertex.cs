using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleLibrary.Core.PointParticleSystem
{
	internal struct PointParticleVertex : IVertexType
	{
		public Vector4 Position;

		public Color StartColor;
		public Color EndColor;

		public Vector4 Velocity;
		public float Size;

		public Vector3 DepthTime;

		public static readonly VertexDeclaration VertexDeclaration = new
		(
			new VertexElement(sizeof(float) * 0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0),

			new VertexElement(sizeof(float) * 4, VertexElementFormat.Color, VertexElementUsage.Color, 0), // Start Color
			new VertexElement(sizeof(float) * 5, VertexElementFormat.Color, VertexElementUsage.Color, 1), // End Color

			new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector4, VertexElementUsage.Normal, 0), // Velocity
			new VertexElement(sizeof(float) * 10, VertexElementFormat.Single, VertexElementUsage.PointSize, 0), // Size

			new VertexElement(sizeof(float) * 11, VertexElementFormat.Vector3, VertexElementUsage.Normal, 1) // Depth Time
		);

		readonly VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

		public const int SizeInBytes = sizeof(float) * 14;
	}
}
