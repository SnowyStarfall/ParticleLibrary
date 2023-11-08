using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.UI.Interfaces;
using ParticleLibrary.UI.Primitives.Shapes;
using ParticleLibrary.Utilities;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace ParticleLibrary.UI.Elements.Base
{
    public class Button : Panel, IDebuggable
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

            spriteBatch.Begin(LibUtilities.CustomUISettings);

            CalculatedStyle inner = GetInnerDimensions();
            Vector2 size = FontAssets.MouseText.Value.MeasureString(Content ?? "");

            spriteBatch.DrawText(Content, 1, inner.Position(), Color.White, Color.Black, scale: Utils.Clamp(inner.Height / size.Y, 0f, 2f));

            spriteBatch.End();

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

        public override void DebugRender(PrimRectangle rectangle, float alpha)
        {
            base.DebugRender(rectangle, alpha);

            CalculatedStyle inner = GetInnerDimensions();
            Vector2 size = FontAssets.MouseText.Value.MeasureString(Content ?? "");

            rectangle.SetSize(inner.ToRectangle());
            rectangle.Draw();

            rectangle.Position = inner.Position();
            rectangle.SetSize(new Rectangle((int)inner.X, (int)inner.Y, (int)size.X, (int)size.Y));
            rectangle.Draw();
        }
    }
}
