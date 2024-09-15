using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.UI.Primitives.Complex;
using ParticleLibrary.Utilities;
using Terraria;
using Terraria.UI;

namespace ParticleLibrary.UI.Elements.Base
{
	public class ScrollBar : Panel
	{
		public Box Handle { get; protected set; }

		public float ViewPosition
		{
			get
			{
				return _viewPosition;
			}
			set
			{
				_viewPosition = MathHelper.Clamp(value, 0f, _maxViewSize - _viewSize);
			}
		}

		public bool CanScroll => _maxViewSize != _viewSize;

		private float _viewPosition;
		private float _viewSize = 1f;
		private float _maxViewSize = 20f;
		private bool _isHoveringOverHandle;
		private bool _dragging;
		private float _dragOffset;

		public ScrollBar(Color fill, Color outline, float outlineThickness = 1, float cornerRadius = 0, bool soft = false) : base(fill, outline, outlineThickness, cornerRadius, soft)
		{
			Width.Set(20f, 0f);
			MaxWidth.Set(20f, 0f);
			PaddingTop = 5f;
			PaddingBottom = 5f;

			CalculatedStyle newDimensions = GetDimensions();

			Handle = new(newDimensions.Position(), new Vector2(_oldDimensions.Width, _oldDimensions.Height), ParticleLibraryConfig.CurrentTheme.Mid, ParticleLibraryConfig.CurrentTheme.MidAccent, cornerRadius, outlineThickness)
			{
				Soft = soft
			};
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculateResize();

			if (!Visible)
			{
				return;
			}

			Primitive.SetOutlineColor(Outline);

			spriteBatch.End();

			Primitive.Draw();
			Handle.Draw();

			spriteBatch.Begin(LibUtilities.DefaultUISettings);
		}

		public void SetView(float viewSize, float maxViewSize)
		{
			viewSize = MathHelper.Clamp(viewSize, 0f, maxViewSize);
			_viewPosition = MathHelper.Clamp(_viewPosition, 0f, maxViewSize - viewSize);
			_viewSize = viewSize;
			_maxViewSize = maxViewSize;
		}

		protected override void CalculateResize()
		{
			// Update things if we resized
			CalculatedStyle newDimensions = GetDimensions();
			if (!_oldDimensions.Equals(newDimensions))
			{
				_oldDimensions = newDimensions;

				Primitive.SetPosition(_oldDimensions.Position());
				Primitive.SetSize(new Vector2(_oldDimensions.Width, _oldDimensions.Height));

				Rectangle handleRectangle = GetHandleRectangle();

				Handle.SetSize(handleRectangle);

				CalculateAreas(newDimensions);
			}
		}

		public override void LeftMouseDown(UIMouseEvent evt)
		{
			base.LeftMouseDown(evt);

			if (evt.Target != this)
			{
				return;
			}

			Rectangle handleRectangle = GetHandleRectangle();
			if (handleRectangle.Contains(new Point((int)evt.MousePosition.X, (int)evt.MousePosition.Y)))
			{
				_dragging = true;
				_dragOffset = evt.MousePosition.Y - handleRectangle.Y;
			}
			else
			{
				CalculatedStyle innerDimensions = GetInnerDimensions();
				float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - (handleRectangle.Height >> 1);
				_viewPosition = MathHelper.Clamp(num / innerDimensions.Height * _maxViewSize, 0f, _maxViewSize - _viewSize);
			}
		}

		public override void LeftMouseUp(UIMouseEvent evt)
		{
			base.LeftMouseUp(evt);

			_dragging = false;
		}

		private Rectangle GetHandleRectangle()
		{
			CalculatedStyle innerDimensions = GetInnerDimensions();
			if (_maxViewSize == 0f && _viewSize == 0f)
			{
				_viewSize = 1f;
				_maxViewSize = 1f;
			}

			return new Rectangle((int)innerDimensions.X, (int)(innerDimensions.Y + innerDimensions.Height * (_viewPosition / _maxViewSize)) - 3, 20, (int)(innerDimensions.Height * (_viewSize / _maxViewSize)) + 7);
		}

		public override void DebugRender(Box box, ref float colorIntensity, float alpha)
		{
			base.DebugRender(box, ref colorIntensity, alpha);


			colorIntensity += 0.025f;

			Rectangle dimensions = GetDimensions().ToRectangle();
			box.SetOutlineColor(Main.hslToRgb(colorIntensity, colorIntensity, 0.5f) * alpha);
			box.SetSize(dimensions);
			box.Draw();

			Rectangle handle = GetHandleRectangle();
			if (_dragging)
			{
				box.SetOutlineColor(new Color(0f, 1f, 0f, 1f) * alpha);
			}
			else
			{	
				box.SetOutlineColor(Color.Red * alpha);
			}
			box.SetSize(handle);
			box.Draw();
		}
	}
}
