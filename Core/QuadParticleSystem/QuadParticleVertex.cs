using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleLibrary.Core
{
	public struct QuadParticleVertex : IVertexType
	{
		/// <summary>
		/// Position of particle
		/// </summary>
		public Vector4 Position;

		/// <summary>
		/// Texture coordinate of this vertex
		/// </summary>
		public Vector2 TexCoord;

		/// <summary>
		/// Starting color
		/// </summary>
		public Color StartColor;

		/// <summary>
		/// Ending color
		/// </summary>
		public Color EndColor;

		/// <summary>
		/// Packed Velocity (XY), and Velocity Deviation (ZW)
		/// </summary>
		public Vector4 Velocity;

		/// <summary>
		/// Acceleration
		/// </summary>
		public Vector2 Acceleration;

		/// <summary>
		/// Texture width and height
		/// </summary>
		public Vector2 Size;

		/// <summary>
		/// Scale (XY), and Scale Velocity (ZW)
		/// </summary>
		public Vector4 Scale;

		/// <summary>
		/// Corner (XY), Rotation (Z), and Rotation Velocity (W)
		/// </summary>
		public Vector4 Rotation;

		/// <summary>
		/// Depth (X), Depth Velocity (Y), and Time (Z)
		/// </summary>
		public Vector4 DepthTime;

		public static readonly VertexDeclaration VertexDeclaration = new
		(
			new VertexElement(sizeof(float) * 0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0),

			new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),

			new VertexElement(sizeof(float) * 6, VertexElementFormat.Color, VertexElementUsage.Color, 0), // Start Color

			new VertexElement(sizeof(float) * 7, VertexElementFormat.Color, VertexElementUsage.Color, 1), // End Color

			new VertexElement(sizeof(float) * 8, VertexElementFormat.Vector4, VertexElementUsage.Normal, 0), // Velocity

			new VertexElement(sizeof(float) * 12, VertexElementFormat.Vector2, VertexElementUsage.Normal, 1), // Acceleration

			new VertexElement(sizeof(float) * 14, VertexElementFormat.Vector2, VertexElementUsage.Normal, 2), // Size

			new VertexElement(sizeof(float) * 16, VertexElementFormat.Vector4, VertexElementUsage.Normal, 3), // Scale

			new VertexElement(sizeof(float) * 20, VertexElementFormat.Vector4, VertexElementUsage.Normal, 4), // Rotation

			new VertexElement(sizeof(float) * 24, VertexElementFormat.Vector4, VertexElementUsage.Normal, 5) // Depth Time
		);

		readonly VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

		public const int SizeInBytes = sizeof(float) * 28; // 112
	}
}
