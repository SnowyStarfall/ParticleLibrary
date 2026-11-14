using ParticleLibrary.Core.V3.Particles;
using ParticleLibrary.Utilities;

namespace ParticleLibrary.Core.V3
{
	public struct Rectangle
	{
		public float X;
		public float Y;
		public float Width;
		public float Height;

		public Rectangle(float x, float y, float width, float height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}
	}

	public static class RectangleExtensions
	{
		extension(Rectangle rectangle)
		{
			public bool IsDefault()
			{
				return rectangle is { X: float.PositiveInfinity, Y: float.PositiveInfinity, Width: float.PositiveInfinity, Height: float.PositiveInfinity };
			}
		}
	}
}