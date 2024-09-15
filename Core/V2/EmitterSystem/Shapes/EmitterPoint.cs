using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core.Shapes
{
	public class EmitterPoint : EmitterShape
	{
		public override Vector2 Solve(Vector2 center, EmitterOrigin origin, float width, float height)
		{
			return center;
		}
	}
}
