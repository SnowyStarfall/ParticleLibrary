using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.UI.Interfaces;
using ParticleLibrary.UI.Primitives.Complex;
using ParticleLibrary.Utilities;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace ParticleLibrary.UI.Elements.Base
{
	public class Button : Panel, IConsistentUpdateable
	{
		public Color HoverFill { get; private set; }
		public Color HoverOutline { get; private set; }
		public string Content { get; set; }

		public delegate void Click(Button button);
		public event Click OnClick;

		protected bool _hovered;

		private readonly TextShifter _textShifter;
		private bool _overflows;
		private float _hiddenLength;

		public Button(Color fill, Color hoverFill, Color outline, Color hoverOutline, float outlineThickness = 1f, float cornerRadius = 0f, bool soft = false) : base(fill, outline, outlineThickness, cornerRadius, soft)
		{
			HoverFill = hoverFill;
			HoverOutline = hoverOutline;

			_textShifter = new();
		}

		public void Update()
		{
			if (_overflows)
			{
				_textShifter.Update();

				return;
			}

			_textShifter.Reset();
		}

		// Override required as ModContent.Request<Effect>() returns the same Effect instance
		// therefore requiring us to override so we can set the outline color every frame
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculateResize();

			_overflows = Overflows();

			if (_hovered)
				Primitive.SetOutlineColor(HoverOutline);
			else
				Primitive.SetOutlineColor(Outline);

			Rectangle scissorRectangle = spriteBatch.GetClippingRectangle(this);

			spriteBatch.End();

			Primitive.Draw();

			spriteBatch.BeginScissorIf(HideOverflow, LibUtilities.ClarityUISettings, scissorRectangle, true, out Rectangle oldScissorRectangle);

			CalculatedStyle inner = GetInnerDimensions();
			Vector2 size = FontAssets.MouseText.Value.MeasureString(Content ?? "");
			float scale = Utils.Clamp(inner.Height / size.Y, 0f, 2f);
			Vector2 position = inner.Position() - new Vector2(_textShifter.Progress * _hiddenLength, 0f);

			spriteBatch.DrawText(Content, 1, position, Color.White, Color.Black, scale: scale);

			spriteBatch.EndScissorIf(HideOverflow, oldScissorRectangle);

			spriteBatch.BeginScissorIf(HideOverflow, LibUtilities.DefaultUISettings, oldScissorRectangle, true, out _);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);

			if (evt.Target != this)
			{
				return;
			}

			Primitive.SetFill(HoverFill);
			_hovered = true;
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);

			if (evt.Target != this)
			{
				return;
			}

			Primitive.SetFill(Fill);
			_hovered = false;
		}

		public override void LeftMouseDown(UIMouseEvent evt)
		{
			base.LeftMouseDown(evt);

			if (evt.Target != this)
			{
				return;
			}

			Primitive.SetFill(Fill);
		}

		public override void LeftMouseUp(UIMouseEvent evt)
		{
			base.LeftMouseUp(evt);

			if (evt.Target != this)
			{
				return;
			}

			Primitive.SetFill(HoverFill);
		}

		public override void LeftClick(UIMouseEvent evt)
		{
			base.LeftClick(evt);

			if (evt.Target != this)
			{
				return;
			}

			OnClick?.Invoke(this);
		}

		public override void SetOutline(Color outline)
		{
			Outline = outline;
		}

		public void SetHoverOutline(Color outline)
		{
			HoverOutline = outline;
		}

		private bool Overflows()
		{
			Vector2 position = GetInnerDimensions().Position();
			Vector2 size = FontAssets.MouseText.Value.MeasureString(Content);

			Rectangle inner = GetInnerDimensions().ToRectangle();
			float scale = Utils.Clamp(inner.Height / size.Y, 0f, 2f);

			if ((position.X + (size.X * scale)) > (inner.X + inner.Width))
			{
				_hiddenLength = ((position.X + size.X * scale) - (inner.X + inner.Width));
				return true;
			}

			_hiddenLength = 0f;
			return false;
		}

		public override void DebugRender(Box box, ref float colorIntensity, float alpha)
		{
			base.DebugRender(box, ref colorIntensity, alpha);

			CalculatedStyle inner = GetInnerDimensions();
			Vector2 position = GetInnerDimensions().Position();
			Vector2 size = FontAssets.MouseText.Value.MeasureString(Content ?? "");
			float scale = Utils.Clamp(inner.Height / size.Y, 0f, 2f);

			colorIntensity += 0.025f;
			box.SetOutlineColor(Main.hslToRgb(colorIntensity, colorIntensity, 0.5f) * alpha);
			box.SetSize(inner.ToRectangle());
			box.Draw();

			colorIntensity += 0.025f;
			box.SetOutlineColor(Main.hslToRgb(colorIntensity, colorIntensity, 0.5f) * alpha);
			box.SetPosition(position);
			box.SetSize(size * scale);
			box.Draw();
		}
	}
}
