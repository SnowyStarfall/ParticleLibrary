using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleLibrary.Utilities
{
	public static class ParticleUtils
	{
		public static Vector4 Vec4From2Vec2(Vector2 xy, Vector2 zw) => new(xy, zw.X, zw.Y);
	}
}
