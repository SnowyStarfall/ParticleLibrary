using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using Terraria.UI;

namespace ParticleLibrary.Interface.Elements
{
	public class Button : Panel
	{
		public Color HoverFill { get; private set; }
		public Color HoverOutline { get; private set; }
		public Action Click { get; private set; }

		protected bool _hovered;

		public Button(Color fill, Color hoverFill, Color outline, Color hoverOutline, Action click, float outlineThickness = 1f, float cornerRadius = 0f, bool soft = false) : base(fill, outline, outlineThickness, cornerRadius, soft)
		{
			HoverFill = hoverFill;
			HoverOutline = hoverOutline;
			Click = click;
		}

		// Override required as ModContent.Request<Effect>() returns the same Effect instance
		// therefore requiring us to override so we can set the outline color every frame
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle newDimensions = GetDimensions();
			if (!_oldDimensions.Equals(newDimensions))
			{
				_oldDimensions = newDimensions;
				Primitive.SetPosition(_oldDimensions.Position());
				Primitive.SetSize(new Vector2(_oldDimensions.Width, _oldDimensions.Height));
			}

			if (_hovered)
				Primitive.SetOutline(HoverOutline);
			else
				Primitive.SetOutline(Outline);

			spriteBatch.End();

			Primitive.Draw();

			spriteBatch.Begin(LibUtilities.DefaultUISettings);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			Primitive.SetFill(HoverFill);
			_hovered = true;
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			Primitive.SetFill(Fill);
			_hovered = false;
		}

		public override void LeftMouseDown(UIMouseEvent evt)
		{
			Primitive.SetFill(Fill);
		}

		public override void LeftMouseUp(UIMouseEvent evt)
		{
			Primitive.SetFill(HoverFill);
		}

		public override void LeftClick(UIMouseEvent evt)
		{
			Click?.Invoke();
		}

		public override void SetOutline(Color outline)
		{
			Outline = outline;
		}

		public void SetHoverOutline(Color outline)
		{
			HoverOutline = outline;
		}
	}
}
