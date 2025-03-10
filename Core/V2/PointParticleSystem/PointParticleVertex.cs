﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleLibrary.Core
{
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Particles instead")]
	public struct PointParticleVertex : IVertexType
	{
		/// <summary>
		/// Position of particle
		/// </summary>
		public Vector4 Position;

		/// <summary>
		/// Starting color
		/// </summary>
		public Color StartColor;

		/// <summary>
		/// Ending color
		/// </summary>
		public Color EndColor;

		/// <summary>
		/// Packed Velocity (XY), and Velocity Acceleration (ZW)
		/// </summary>
		public Vector4 Velocity;

		/// <summary>
		/// Acceleration
		/// </summary>
		public Vector2 Acceleration;

		/// <summary>
		/// Depth (X), Depth Velocity (Y), and Time (Z)
		/// </summary>
		public Vector4 DepthTime;

		public static readonly VertexDeclaration VertexDeclaration = new
		(
			new VertexElement(sizeof(float) * 0, VertexElementFormat.Vector4, VertexElementUsage.Position, 0),

			new VertexElement(sizeof(float) * 4, VertexElementFormat.Color, VertexElementUsage.Color, 0), // Start Color

			new VertexElement(sizeof(float) * 5, VertexElementFormat.Color, VertexElementUsage.Color, 1), // End Color

			new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector4, VertexElementUsage.Normal, 0), // Velocity

			new VertexElement(sizeof(float) * 10, VertexElementFormat.Vector2, VertexElementUsage.Normal, 1), // Acceleration

			new VertexElement(sizeof(float) * 12, VertexElementFormat.Vector4, VertexElementUsage.Normal, 1) // Depth Time
		);

		readonly VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

		public const int SizeInBytes = sizeof(float) * 16; // 64
	}
}
