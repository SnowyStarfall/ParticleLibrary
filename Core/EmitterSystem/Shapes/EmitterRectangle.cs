using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace ParticleLibrary.Core.Shapes
{
	public class EmitterRectangle : EmitterShape
	{
		public override Vector2 Solve(Vector2 center, EmitterOrigin origin, float width, float height)
		{
			if (origin is EmitterOrigin.Spread)
			{
				return center += new Vector2(width * Main.rand.NextFloat(0f, 1f + float.Epsilon),
							 				 height * Main.rand.NextFloat(0f, 1f + float.Epsilon));
			}
			else // if (origin is EmitterOrigin.Rim)
			{
				int edge = Main.rand.Next(0, 4);

				switch (edge)
				{
					case 0: // Top edge
						center.X += Main.rand.NextFloat(0f, width + float.Epsilon);
						break;

					case 1: // Right edge
						center.X += width;
						center.Y += Main.rand.NextFloat(0f, height + float.Epsilon);
						break;

					case 2: // Bottom edge
						center.X += Main.rand.NextFloat(0f, width + float.Epsilon);
						center.Y += height;
						break;

					case 3: // Left edge
						center.Y += Main.rand.NextFloat(0f, height + float.Epsilon);
						break;

					default:
						throw new InvalidOperationException("Unexpected edge value.");
				}
			}

			return center;
		}
	}
}
