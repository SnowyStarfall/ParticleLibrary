﻿using Microsoft.Xna.Framework;
using System;

namespace ParticleLibrary.Core
{
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Particles instead")]
	public class RenderQuad
	{
		public Color TopLeft { get; set; }
		public Color TopRight { get; set; }
		public Color BottomLeft { get; set; }
		public Color BottomRight { get; set; }

		public RenderQuad(Color topLeft, Color topRight, Color bottomLeft, Color bottomRight)
		{
			TopLeft = topLeft;
			TopRight = topRight;
			BottomLeft = bottomLeft;
			BottomRight = bottomRight;
		}

		public RenderQuad(Color color)
		{
			TopLeft = color;
			TopRight = color;
			BottomLeft = color;
			BottomRight = color;
		}
	}
}
