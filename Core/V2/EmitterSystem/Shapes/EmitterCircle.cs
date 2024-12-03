using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace ParticleLibrary.Core.Shapes
{
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Emission instead")]
	public class EmitterCircle : EmitterShape
	{
		public override Vector2 Solve(Vector2 center, EmitterOrigin origin, float width, float height)
		{
			if (origin is EmitterOrigin.Spread)
			{
				center += new Vector2(width, height);

				float scalar = Main.rand.NextFloat(0f, 1f + float.Epsilon);
				float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi + float.Epsilon);
				center.X += MathF.Cos(angle) * width * scalar;
				center.Y += MathF.Sin(angle) * height * scalar;

				return center;
			}
			else // if (origin is EmitterOrigin.Rim)
			{
				center += new Vector2(width, height);

				float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi + float.Epsilon);
				center.X += MathF.Cos(angle) * width;
				center.Y += MathF.Sin(angle) * height;

				return center;
			}
		}
	}
}
