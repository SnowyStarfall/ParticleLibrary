using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleLibrary.UI.Primitives.Shapes
{
    public class PrimRectangle
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

        public Vector2 Size
        {
            get => _size;
            set
            {
                _size = value;
                CalculateVertices();
            }
        }
        private Vector2 _size;

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

        public PrimRectangle(Vector2 positiom, Vector2 size, MatrixType matrix) : this(positiom, size, Color.White, matrix)
        {
        }

        public PrimRectangle(Vector2 position, Vector2 size, Color color, MatrixType matrix)
        {
            _position = position;
            _size = size;
            _color = color;
            _matrix = matrix;

            CalculateVertices();
        }

        public void Draw()
        {
            if (_vertices.Length == 0)
                return;

            if (_matrix is MatrixType.World)
                Primitive.WorldEffect.CurrentTechnique.Passes[0].Apply();
            else
                Primitive.InterfaceEffect.CurrentTechnique.Passes[0].Apply();

            Primitive.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertices, 0, _vertices.Length - 1);
        }

        public void SetSize(Rectangle size)
        {
			_position = new Vector2(size.X, size.Y);
			_size = new Vector2(size.Width, size.Height);

            CalculateVertices();
		}

        private void CalculateVertices()
        {
            _vertices = new VertexPositionColor[5];

            _vertices[0] = new VertexPositionColor(new Vector3(_position, 0f), _color);
            _vertices[1] = new VertexPositionColor(new Vector3(_position + new Vector2(_size.X, 0f), 0f), _color);
            _vertices[2] = new VertexPositionColor(new Vector3(_position + _size, 0f), _color);
            _vertices[3] = new VertexPositionColor(new Vector3(_position + new Vector2(0f, _size.Y), 0f), _color);
            _vertices[4] = _vertices[0];
        }
    }
}
