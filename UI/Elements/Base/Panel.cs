using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.UI.Interfaces;
using ParticleLibrary.UI.Primitives.Complex;
using ParticleLibrary.Utilities;
using Terraria;
using Terraria.UI;

namespace ParticleLibrary.UI.Elements.Base
{
	public class Panel : UIElement, IDebuggable
	{
		// Visual
		public Box Primitive { get; protected set; }
		public Color Fill { get; protected set; }
		public Color Outline { get; protected set; }

		public bool Visible { get; set; } = true;
		public bool Draggable { get; set; }
		public bool Resizable { get; set; }
		public bool Screenlocked { get; set; }
		public bool HideOverflow { get; set; }

		protected CalculatedStyle _oldDimensions;

		private Rectangle _draggableArea;
		private bool _dragging;
		private Vector2 _dragStart;

		private Rectangle _resizableArea;
		private bool _resizing;
		private Vector2 _resizeStart;
		private Vector2 _originalSize;

		public Panel(Color fill, Color outline, float outlineThickness = 1f, float cornerRadius = 0f, bool soft = false)
		{
			Fill = fill;
			Outline = outline;

			CalculatedStyle newDimensions = GetDimensions();

			Primitive = new(newDimensions.Position(), new Vector2(_oldDimensions.Width, _oldDimensions.Height), fill, outline, cornerRadius, outlineThickness)
			{
				Soft = soft
			};

			_draggableArea = new();
			_resizableArea = new();

			CalculateAreas(newDimensions);
		}

		// Function
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
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

			spriteBatch.Begin(LibUtilities.DefaultUISettings);
		}

		// Setters
		public virtual void SetFill(Color fill)
		{
			Fill = fill;
			Primitive.SetFill(fill);
		}

		public virtual void SetOutline(Color outline)
		{
			Outline = outline;
			Primitive.SetOutlineColor(outline);
		}

		// Events
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			base.LeftMouseDown(evt);

			if (evt.Target != this)
			{
				return;
			}

			if (Draggable && _draggableArea.Contains(Main.MouseScreen.ToPoint()))
			{
				_dragging = true;
				_dragStart = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);

				return;
			}

			if (Resizable && _resizableArea.Contains(Main.MouseScreen.ToPoint()))
			{
				CalculatedStyle style = GetDimensions();

				_resizing = true;
				_resizeStart = new Vector2(evt.MousePosition.X, evt.MousePosition.Y);
				_originalSize = new Vector2(style.Width, style.Height);

				return;
			}
		}

		public override void LeftMouseUp(UIMouseEvent evt)
		{
			base.LeftMouseUp(evt);

			if (evt.Target != this)
			{
				return;
			}

			if (Draggable && _dragging)
			{
				_dragging = false;

				Left.Set(evt.MousePosition.X - _dragStart.X, 0f);
				Top.Set(evt.MousePosition.Y - _dragStart.Y, 0f);
			}

			if (Resizable && _resizing)
			{
				_resizing = false;
			}

			Recalculate();
		}

		// Debug
		public virtual void DebugRender(Box box, ref float colorIntensity, float alpha)
		{
			if (Draggable)
			{
				if (_dragging)
				{
					box.SetOutlineColor(new Color(0f, 1f, 0f, 1f) * alpha);
				}
				else
				{
					box.SetOutlineColor(Color.Red * alpha);
				}
				box.SetSize(_draggableArea);
				box.Draw();
			}

			if (Resizable)
			{
				if (_resizing)
				{
					box.SetOutlineColor(new Color(0f, 1f, 0f, 1f) * alpha);
				}
				else
				{
					box.SetOutlineColor(Color.Red * alpha);
				}
				box.SetSize(_resizableArea);
				box.Draw();
			}
		}

		// Calculation
		protected void CalculateResize()
		{
			// Establish area
			Rectangle parentDim = Parent.GetDimensions().ToRectangle();
			Rectangle panelDim = GetDimensions().ToRectangle();

			// Dragging logic
			if (Draggable && _dragging)
			{
				Left.Set(Main.MouseScreen.X - _dragStart.X, 0f);
				Top.Set(Main.MouseScreen.Y - _dragStart.Y, 0f);

				Recalculate();

				// Reestablish area after recalculation
				parentDim = Parent.GetDimensions().ToRectangle();
				panelDim = GetDimensions().ToRectangle();
			}

			// Resizing logic
			if (Resizable && _resizing)
			{
				Width.Set(_originalSize.X + (Main.MouseScreen.X - _resizeStart.X), 0f);
				Height.Set(_originalSize.Y + (Main.MouseScreen.Y - _resizeStart.Y), 0f);

				Recalculate();

				// Reestablish area after recalculation
				parentDim = Parent.GetDimensions().ToRectangle();
				panelDim = GetDimensions().ToRectangle();
			}

			// Clamping logic
			if (!parentDim.Contains(panelDim))
			{
				Recalculate();

				// Reestablish area after recalculation
				//parentDim = Parent.GetDimensions().ToRectangle();
				panelDim = GetDimensions().ToRectangle();
			}

			Rectangle screen = new(0, 0, Main.screenWidth, Main.screenHeight);

			if (Screenlocked && !screen.Contains(panelDim))
			{
				if (panelDim.X < screen.X)
				{
					Left.Pixels = screen.X;
				}
				else if (panelDim.X + panelDim.Width > screen.X + screen.Width)
				{
					Left.Pixels = screen.X + screen.Width - panelDim.Width;
				}
				if (panelDim.Y < screen.Y)
				{
					Top.Pixels = screen.Y;
				}
				else if (panelDim.Y + panelDim.Height > screen.Y + screen.Height)
				{
					Top.Pixels = screen.Y + screen.Height - panelDim.Height;
				}

				Recalculate();
			}

			// Update things if we resized
			CalculatedStyle newDimensions = GetDimensions();
			if (!_oldDimensions.Equals(newDimensions))
			{
				_oldDimensions = newDimensions;

				Primitive.SetPosition(_oldDimensions.Position());
				Primitive.SetSize(new Vector2(_oldDimensions.Width, _oldDimensions.Height));

				CalculateAreas(newDimensions);
			}
		}

		private void CalculateAreas(CalculatedStyle style)
		{
			_draggableArea.X = (int)style.X;
			_draggableArea.Y = (int)style.Y;
			_draggableArea.Width = (int)style.Width;
			_draggableArea.Height = 32;

			_resizableArea.X = (int)(style.X + style.Width - 32);
			_resizableArea.Y = (int)(style.Y + style.Height - 32);
			_resizableArea.Width = 32;
			_resizableArea.Height = 32;
		}
	}
}
