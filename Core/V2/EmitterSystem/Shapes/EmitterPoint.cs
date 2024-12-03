using Microsoft.Xna.Framework;
using System;

namespace ParticleLibrary.Core.Shapes
{
    public class EmitterPoint : EmitterShape
	{
		[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Emission instead")]
		public override Vector2 Solve(Vector2 center, EmitterOrigin origin, float width, float height)
		{
			return center;
		}
	}
}
