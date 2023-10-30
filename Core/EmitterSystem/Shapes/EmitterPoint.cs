using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleLibrary.Core.EmitterSystem.Shapes
{
	public class EmitterPoint : EmitterShape
	{
		public override Vector2 Solve(Vector2 center, EmitterOrigin origin, Emitter emitter)
		{
			return center;
		}
	}
}
