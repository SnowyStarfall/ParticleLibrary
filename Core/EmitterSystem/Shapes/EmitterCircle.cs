using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace ParticleLibrary.Core.EmitterSystem.Shapes
{
	public class EmitterCircle : EmitterShape
	{
		public override Vector2 Solve(Vector2 center, EmitterOrigin origin, Emitter emitter)
		{
			if (origin is EmitterOrigin.Spread)
			{
				center += new Vector2(emitter.EmitterSettings.Width, emitter.EmitterSettings.Height);

				float scalar = Main.rand.NextFloat(0f, 1f + float.Epsilon);
				float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi + float.Epsilon);
				center.X += MathF.Cos(angle) * emitter.EmitterSettings.Width * scalar;
				center.Y += MathF.Sin(angle) * emitter.EmitterSettings.Height * scalar;

				return center;
			}
			else // if (origin is EmitterOrigin.Rim)
			{
				center += new Vector2(emitter.EmitterSettings.Width, emitter.EmitterSettings.Height);

				float angle = Main.rand.NextFloat(0f, MathHelper.TwoPi + float.Epsilon);
				center.X += MathF.Cos(angle) * emitter.EmitterSettings.Width;
				center.Y += MathF.Sin(angle) * emitter.EmitterSettings.Height;

				return center;
			}
		}
	}
}
