using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria;

namespace ParticleLibrary.Debug
{
	public class Circle
	{
		public Circle(Vector2 positiom, Vector2 radius) : this(positiom, radius, Color.White)
		{
		}

		public Circle(Vector2 position, Vector2 radius, Color color)
		{
			this.position = position;
			this.radius = radius;
			this.color = color;
			graphics = Main.graphics;

			Initialize();
		}

		public void Draw()
		{
			effect.CurrentTechnique.Passes[0].Apply();
			if (vertices.Length == 0)
				return;
			graphics.GraphicsDevice.DrawUserPrimitives
				(PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1);
		}

		private void Initialize()
		{
			InitializeBasicEffect();
			InitializeVertices();
		}

		private void InitializeBasicEffect()
		{
			Main.QueueMainThreadAction(() =>
			{
				effect = new BasicEffect(graphics.GraphicsDevice)
				{
					VertexColorEnabled = true,
					Projection = Matrix.CreateOrthographicOffCenter(0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height, 0, 0, 1)
				};
			});
		}

		private void InitializeVertices()
		{
			vertices = new VertexPositionColor[CalculatePointCount()];
			if (vertices.Length == 0)
				return;
			var pointTheta = ((float)Math.PI * 2) / (vertices.Length - 1);
			for (int i = 0; i < vertices.Length; i++)
			{
				var theta = pointTheta * i;
				var x = position.X + ((float)Math.Sin(theta) * (int)(Radius.X));
				var y = position.Y + ((float)Math.Cos(theta) * (int)(Radius.Y));
				vertices[i].Position = new Vector3(x, y, 0);
				vertices[i].Color = Color;
			}
			vertices[^1] = vertices[0];
		}

		private int CalculatePointCount()
		{
			return (int)Math.Ceiling(Radius.X > Radius.Y ? Radius.X : radius.Y * Math.PI);
		}

		private GraphicsDeviceManager graphics;
		private VertexPositionColor[] vertices;
		private BasicEffect effect;

		private Vector2 position;
		public Vector2 Position { get => position; set { position = value; InitializeVertices(); } }

		private Vector2 radius;
		public Vector2 Radius
		{
			get => radius; set { radius = new Vector2(value.X < 1 ? 1 : value.X, value.Y < 1 ? 1 : value.Y); InitializeVertices(); }
		}
		private Color color;
		public Color Color
		{
			get { return color; }
			set { color = value; InitializeVertices(); }
		}
		public int Points
		{
			get { return CalculatePointCount(); }
		}
	}
}
