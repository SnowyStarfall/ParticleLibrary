using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.UI.Interfaces;
using ParticleLibrary.UI.Primitives.Complex;
using ParticleLibrary.UI.Primitives.Shapes;
using ParticleLibrary.Utilities;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace ParticleLibrary.UI.Elements.Base
{
    public class Button : Panel
    {
        public Color HoverFill { get; private set; }
        public Color HoverOutline { get; private set; }
        public string Content { get; set; }

        public delegate void Click(Button button);
        public event Click OnClick;

        protected bool _hovered;

        public Button(Color fill, Color hoverFill, Color outline, Color hoverOutline, float outlineThickness = 1f, float cornerRadius = 0f, bool soft = false) : base(fill, outline, outlineThickness, cornerRadius, soft)
        {
            HoverFill = hoverFill;
            HoverOutline = hoverOutline;
        }

        // Override required as ModContent.Request<Effect>() returns the same Effect instance
        // therefore requiring us to override so we can set the outline color every frame
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculateResize();

            if (_hovered)
                Primitive.SetOutline(HoverOutline);
            else
                Primitive.SetOutline(Outline);

            spriteBatch.End();

            Primitive.Draw();

            spriteBatch.Begin(LibUtilities.ClarityUISettings);

            CalculatedStyle inner = GetInnerDimensions();
            Vector2 size = FontAssets.MouseText.Value.MeasureString(Content ?? "");

            spriteBatch.DrawText(Content, 1, inner.Position(), Color.White, Color.Black, scale: Utils.Clamp(inner.Height / size.Y, 0f, 2f));

            spriteBatch.End();

            spriteBatch.Begin(LibUtilities.DefaultUISettings);
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

        public override void DebugRender(Box box, float alpha)
        {
            base.DebugRender(box, alpha);

            CalculatedStyle inner = GetInnerDimensions();
            Vector2 size = FontAssets.MouseText.Value.MeasureString(Content ?? "");

			box.SetSize(inner.ToRectangle());
			box.Draw();

			box.SetPosition(inner.Position());
			box.SetSize(new Rectangle((int)inner.X, (int)inner.Y, (int)size.X, (int)size.Y));
			box.Draw();
        }
    }
}
