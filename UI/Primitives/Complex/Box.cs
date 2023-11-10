using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ModLoader;
using static ParticleLibrary.Resources.Assets;

namespace ParticleLibrary.UI.Primitives.Complex
{
	public class Box : IDisposable
	{
		public Vector2 Position { get; private set; }
		public Vector2 Size { get; private set; }
		public Color Fill { get; private set; }
		public Color Outline { get; private set; }
		public float CornerRadius { get; private set; }
		public float OutlineThickness { get; private set; }
		public bool Soft { get; set; }

		private bool _verticesDirty = false;
		private readonly float _originalRadius;

		private readonly List<VertexPositionColorTexture> _vertices;
		private readonly List<short> _indices;
		private DynamicVertexBuffer _vertexBuffer;
		private DynamicIndexBuffer _indexBuffer;

		private Effect _effect;
		private EffectParameter _matrix;
		private EffectParameter _outline;
		private EffectParameter _texel;

		private bool Rounded => CornerRadius != 0f;

		public Box(Vector2 position, Vector2 size, Color? fill = null, Color? outline = null, float cornerRadius = 16f, float outlineThickness = 1f)
		{
			Position = position;
			Size = size;
			Fill = fill ?? new Color(0.1f, 0.1f, 0.1f, 0.75f);
			Outline = outline ?? Color.Red;

			CornerRadius = MathF.Abs(cornerRadius);
			_originalRadius = MathF.Abs(cornerRadius);

			OutlineThickness = outlineThickness == 0f ? 0f : outlineThickness + 0.0001f;

			_vertices = new();
			_indices = new();

			Main.QueueMainThreadAction(() =>
			{
				_vertexBuffer = new(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), 16, BufferUsage.WriteOnly);
				_indexBuffer = new(Main.graphics.GraphicsDevice, IndexElementSize.SixteenBits, 54, BufferUsage.WriteOnly);

				_effect = ModContent.Request<Effect>(Effects.Shape, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value.Clone();
				_matrix = _effect.Parameters["UIScaleMatrix"];
				_outline = _effect.Parameters["Outline"];
				_texel = _effect.Parameters["Texel"];

				_outline.SetValue(Outline.ToVector4());
			});

			_verticesDirty = true;
		}

		public void Draw()
		{
			// Preventative measures to keep rounding from overflowing.
			float min = MathF.Min(Size.X, Size.Y);
			if (Rounded && (CornerRadius > min / 2f || CornerRadius < min / 2f && CornerRadius < _originalRadius))
			{
				CornerRadius = min / 2f;
				_verticesDirty = true;
			}

			// We should recalculate only if the vertices have changed
			if (_verticesDirty)
				Recalculate();

			// We can't operate on less than four vertices.
			if (_vertices.Count < 4)
				return;

			// Set parameters
			_matrix.SetValue(Main.UIScaleMatrix);

			Vector2 texel;

			// Initialize value
			if (Rounded)
				texel = new(1f / CornerRadius / 2f * OutlineThickness);
			else
				texel = new(1f / Size.X / 2f * OutlineThickness, 1f / Size.Y / 2f * OutlineThickness);

			// Prevent clipping
			if (texel.X == 0.5f)
				texel.X -= 0.0000001f;
			if (texel.Y == 0.5f)
				texel.Y -= 0.0000001f;

			_texel.SetValue(texel);

			int triCount = CornerRadius == 0f ? 2 : 18;

			// Draw box
			if (Rounded)
			{
				if (Soft)
					_effect.CurrentTechnique.Passes["SoftCirclePass"].Apply();
				else
					_effect.CurrentTechnique.Passes["CirclePass"].Apply();
			}
			else
				_effect.CurrentTechnique.Passes["BoxPass"].Apply();

			Main.graphics.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
			Main.graphics.GraphicsDevice.Indices = _indexBuffer;

			Main.graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Count, 0, triCount);
		}

		private void Recalculate()
		{
			_verticesDirty = false;
			_vertices.Clear();
			_indices.Clear();

			if (CornerRadius == 0f)
			{
				SimpleSquare();
				return;
			}

			NineSlice();

			_vertexBuffer.SetData(_vertices.ToArray(), SetDataOptions.Discard);
			_indexBuffer.SetData(_indices.ToArray(), 0, _indices.Count, SetDataOptions.Discard);
		}

		private void SimpleSquare()
		{
			// TL -- A
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f), Fill, new Vector2(0f)));
			// BL -- D
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(0f, Size.Y, 0f), Fill, new Vector2(0f, 1f)));
			// TR -- B
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(Size.X, 0f, 0f), Fill, new Vector2(1f, 0f)));
			// BR -- C
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(Size, 0f), Fill, new Vector2(1f)));

			// A - B
			// | \ |
			// D - C

			// A > B > C
			_indices.Add(0);
			_indices.Add(2);
			_indices.Add(3);

			// A > C > D
			_indices.Add(0);
			_indices.Add(3);
			_indices.Add(1);

			_vertexBuffer.SetData(_vertices.ToArray(), SetDataOptions.Discard);
			_indexBuffer.SetData(_indices.ToArray(), 0, _indices.Count, SetDataOptions.Discard);
		}

		private void NineSlice()
		{
			// 4 x 4 square of vertices

			float widthWithoutCorner = Size.X - CornerRadius * 2f;
			float heightWithoutCorner = Size.Y - CornerRadius * 2f;

			// VERTICES

			// COLUMN 1
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f), Fill, new Vector2(0f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(0f, CornerRadius, 0f), Fill, new Vector2(0f, 0.5f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(0f, CornerRadius + heightWithoutCorner, 0f), Fill, new Vector2(0f, 0.5f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(0f, Size.Y, 0f), Fill, new Vector2(0f, 1f)));

			// COLUMN 2
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(CornerRadius, 0f, 0f), Fill, new Vector2(0.5f, 0f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(CornerRadius, CornerRadius, 0f), Fill, new Vector2(0.5f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(CornerRadius, CornerRadius + heightWithoutCorner, 0f), Fill, new Vector2(0.5f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(CornerRadius, Size.Y, 0f), Fill, new Vector2(0.5f, 1f)));

			// COLUMN 3
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(CornerRadius + widthWithoutCorner, 0f, 0f), Fill, new Vector2(0.5f, 0f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(CornerRadius + widthWithoutCorner, CornerRadius, 0f), Fill, new Vector2(0.5f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(CornerRadius + widthWithoutCorner, CornerRadius + heightWithoutCorner, 0f), Fill, new Vector2(0.5f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(CornerRadius + widthWithoutCorner, Size.Y, 0f), Fill, new Vector2(0.5f, 1f)));

			// COLUMN 4
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(Size.X, 0f, 0f), Fill, new Vector2(1f, 0f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(Size.X, CornerRadius, 0f), Fill, new Vector2(1f, 0.5f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(Size.X, CornerRadius + heightWithoutCorner, 0f), Fill, new Vector2(1f, 0.5f)));
			_vertices.Add(new VertexPositionColorTexture(new Vector3(Position, 0f) + new Vector3(Size.X, Size.Y, 0f), Fill, new Vector2(1f)));

			// INDICES

			for (int y = 0; y < 3; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					// A
					short tlIndex = (short)(y + x * 4);
					// B
					short blIndex = (short)(y + 1 + x * 4);
					// C
					short trIndex = (short)(y + (x + 1) * 4);
					// D
					short brIndex = (short)(y + 1 + (x + 1) * 4);

					// A - B
					// | \ |
					// D - C

					// A > B > C
					_indices.Add(tlIndex);
					_indices.Add(trIndex);
					_indices.Add(brIndex);

					// A > C > D
					_indices.Add(tlIndex);
					_indices.Add(brIndex);
					_indices.Add(blIndex);
				}
			}
		}

		public void SetPosition(Vector2 position)
		{
			Position = position;
			_verticesDirty = true;
		}

		public void SetSize(Vector2 size)
		{
			Size = new Vector2(MathF.Abs(size.X), MathF.Abs(size.Y));
			_verticesDirty = true;
		}

		public void SetSize(Rectangle size)
		{
			Position = new Vector2(size.X, size.Y);
			Size = new Vector2(MathF.Abs(size.Width), MathF.Abs(size.Height));
			_verticesDirty = true;
		}

		public void SetFill(Color color)
		{
			Fill = color;
			_verticesDirty = true;
		}

		public void SetOutline(Color color)
		{
			Outline = color;
			_outline.SetValue(color.ToVector4());
		}

		public void SetCornerRadius(float cornerRadius)
		{
			CornerRadius = MathF.Abs(cornerRadius);
			_verticesDirty = true;
		}

		public void SetOutlineThickness(float thickness)
		{
			OutlineThickness = thickness == 0f ? 0f : thickness + 0.0001f;
		}

		public void Dispose()
		{
			_vertexBuffer.Dispose();
			_indexBuffer.Dispose();
			_effect.Dispose();

			GC.SuppressFinalize(this);
		}
	}
}
