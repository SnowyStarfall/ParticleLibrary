﻿using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace ParticleLibrary.Core.EmitterSystem.Shapes
{
	public class EmitterRectangle : EmitterShape
	{
		public override Vector2 Solve(Vector2 center, EmitterOrigin origin, Emitter emitter)
		{
			if (origin is EmitterOrigin.Spread)
			{
				return center += new Vector2(emitter.EmitterSettings.Width * Main.rand.NextFloat(0f, 1f + float.Epsilon),
							 				 emitter.EmitterSettings.Height * Main.rand.NextFloat(0f, 1f + float.Epsilon));
			}
			else // if (origin is EmitterOrigin.Rim)
			{
				int edge = Main.rand.Next(0, 4);

				switch (edge)
				{
					case 0: // Top edge
						center.X += Main.rand.NextFloat(0f, emitter.EmitterSettings.Width + float.Epsilon);
						break;

					case 1: // Right edge
						center.X += emitter.EmitterSettings.Width;
						center.Y += Main.rand.NextFloat(0f, emitter.EmitterSettings.Height + float.Epsilon);
						break;

					case 2: // Bottom edge
						center.X += Main.rand.NextFloat(0f, emitter.EmitterSettings.Width + float.Epsilon);
						center.Y += emitter.EmitterSettings.Height;
						break;

					case 3: // Left edge
						center.Y += Main.rand.NextFloat(0f, emitter.EmitterSettings.Height + float.Epsilon);
						break;

					default:
						throw new InvalidOperationException("Unexpected edge value.");
				}
			}

			return center;
		}
	}
}
