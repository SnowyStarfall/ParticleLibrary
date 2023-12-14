using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleLibrary.UI.Primitives.Shapes
{
	public class PrimCircle
	{
		public MatrixType Matrix
		{
			get => _matrix;
			set
			{
				_matrix = value;
				CalculateVertices();
			}
		}
		private MatrixType _matrix;

		public Vector2 Position
		{
			get => _position;
			set
			{
				_position = value;
				CalculateVertices();
			}
		}
		private Vector2 _position;

		public Vector2 Radius
		{
			get => _radius;
			set
			{
				_radius = new Vector2(value.X < 1 ? 1 : value.X, value.Y < 1 ? 1 : value.Y);
				CalculateVertices();
			}
		}
		private Vector2 _radius;

		public Color Color
		{
			get => _color;
			set
			{
				_color = value;
				CalculateVertices();
			}
		}
		private Color _color;

		private VertexPositionColor[] _vertices;

		public PrimCircle(Vector2 positiom, Vector2 radius, MatrixType matrix) : this(positiom, radius, Color.White, matrix)
		{
		}

		public PrimCircle(Vector2 position, Vector2 radius, Color color, MatrixType matrix)
		{
			_position = position;
			_radius = radius;
			_color = color;
			_matrix = matrix;

			CalculateVertices();
		}

		public void Draw()
		{
			if (_vertices.Length == 0)
				return;

			if (_matrix is MatrixType.World)
				PrimitiveSystem.WorldEffect.CurrentTechnique.Passes[0].Apply();
			else
				PrimitiveSystem.InterfaceEffect.CurrentTechnique.Passes[0].Apply();

			PrimitiveSystem.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertices, 0, _vertices.Length - 1);
		}

		private void CalculateVertices()
		{
			int length = CalculatePointCount();
			if (length == 0)
				return;

			_vertices = new VertexPositionColor[length];

			var pointTheta = (float)Math.PI * 2 / (_vertices.Length - 1);
			for (int i = 0; i < _vertices.Length; i++)
			{
				var theta = pointTheta * i;
				var x = _position.X + (float)Math.Sin(theta) * (int)_radius.X;
				var y = _position.Y + (float)Math.Cos(theta) * (int)_radius.Y;
				_vertices[i].Position = new Vector3(x, y, 0);
				_vertices[i].Color = _color;
			}

			_vertices[^1] = _vertices[0];
		}

		private int CalculatePointCount()
		{
			return (int)Math.Ceiling(_radius.X > _radius.Y ? _radius.X : _radius.Y * Math.PI);
		}
	}
}
