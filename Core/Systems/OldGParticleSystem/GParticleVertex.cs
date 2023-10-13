using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core.Systems
{
	internal struct ParticleVertexInput : IVertexType
	{
		public Vector4 Position;
		public Color Color;
		public Vector2 TexCoord;

		/// <summary>
		/// Life is stored in the X value. Duration is stored in the Y value. Random is stored in the ZW values.
		/// </summary>
		public Vector4 LifeDurationRandom;

		/// <summary>
		/// Velocity is stored in the XY values. Acceleration is stored in the ZW values.
		/// </summary>
		public Vector4 Velocity;

		public Vector2 Scale;
		public Vector2 ScaleVelocity;
		public Vector2 ScaleAcceleration;

		/// <summary>
		/// Rotation is stored in the X value. Velocity is stored in the Y value. Acceleration is stored in the Z value.
		/// </summary>
		public Vector3 Rotation;

		public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
		(
			new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0),
			new VertexElement(sizeof(float) * 4, VertexElementFormat.Color, VertexElementUsage.Color, 0),
			new VertexElement(sizeof(float) * 5, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),

			new VertexElement(sizeof(float) * 7, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1), // Life Duration Random

			new VertexElement(sizeof(float) * 10, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 2), // Velocity

			new VertexElement(sizeof(float) * 14, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 3), // Scale
			new VertexElement(sizeof(float) * 16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 4), // ScaleVelocity
			new VertexElement(sizeof(float) * 18, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 5), // ScaleAcceleration

			new VertexElement(sizeof(float) * 20, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 6) // Rotation
		);

		readonly VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
	}
}
