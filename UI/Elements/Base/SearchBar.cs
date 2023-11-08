using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;
using Terraria;
using Terraria.GameContent;
using ParticleLibrary.Utilities;
using System.Linq;
using System;
using ParticleLibrary.UI.Interfaces;

namespace ParticleLibrary.UI.Elements.Base
{
    public class SearchBar : Panel, IConsistentUpdateable
    {
        public Color ActiveOutline { get; private set; }
        public string Text { get; private set; } = string.Empty;

        public delegate void TextChanged(string oldValue, string newValue);
        public event TextChanged OnTextChanged;

        protected bool _active;

        private TextWriter _textWriter;

        public SearchBar(Color fill, Color outline, Color activeOutline, float outlineThickness = 1f, float cornerRadius = 0f, bool soft = false) : base(fill, outline, outlineThickness, cornerRadius, soft)
        {
            ActiveOutline = activeOutline;

            _textWriter = new();
        }

        // For things that need to be consistently updated.
        public void Update()
        {
            _textWriter.Update();
        }

        // For things that don't need to be consistently updated.
        public override void Update(GameTime gameTime)
        {
            // We're not typing, so this code shouldn't run.
            if (!_active)
                return;

            // Cancellation conditionals.
            if (Main.keyState.IsKeyDown(Keys.Escape))
            {
                ToggleActive();
                return;
            }

            if (Main.keyState.IsKeyDown(Keys.Enter))
            {
                ToggleActive();
                return;
            }

            if (PlayerInput.Triggers.JustPressed.MouseLeft && !ContainsPoint(Main.MouseScreen))
            {
                ToggleActive();
                return;
            }

            // Lets Terraria know that we're writing text so that it can handle it.
            PlayerInput.WritingText = true;
            Main.CurrentInputTextTakerOverride = this;
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

            if (_active)
                Primitive.SetOutline(ActiveOutline);
            else
                Primitive.SetOutline(Outline);

            spriteBatch.End();

            Primitive.Draw();

            if (_active)
            {
                // Lets Terraria know that we're writing text so that it can handle it.
                // For whatever reason, this part has to be here. How yucky.
                PlayerInput.WritingText = true;

                // Change the stored value.
                string newText = _textWriter.Write();

                if (newText != Text)
                {
                    //string newText = inputText;
                    OnTextChanged?.Invoke(Text, newText);
                    Text = newText;
                }
            }

            // Draw text with clipping
            CalculatedStyle inner = GetInnerDimensions();

            Main.graphics.GraphicsDevice.ScissorRectangle = inner.ToRectangle();
            Main.graphics.GraphicsDevice.RasterizerState.ScissorTestEnable = true;

            spriteBatch.Begin(LibUtilities.CustomUISettings);

            Vector2 position = inner.Center() - new Vector2(inner.Width / 2f - 8f, 10f);

            if (_textWriter.SelectionIndex != -1)
            {
                Vector2 unselectedSize = _textWriter.CaretIndex == 0 || _textWriter.SelectionIndex == 0 ? Vector2.Zero : FontAssets.MouseText.Value.MeasureString(Text[0..Math.Min(_textWriter.CaretIndex, _textWriter.SelectionIndex)]);
                Vector2 selectedSize = _textWriter.CaretIndex == 0 && _textWriter.SelectionIndex == 0 ? Vector2.Zero : FontAssets.MouseText.Value.MeasureString(Text[Math.Min(_textWriter.CaretIndex, _textWriter.SelectionIndex)..Math.Max(_textWriter.CaretIndex, _textWriter.SelectionIndex)]);
                spriteBatch.Draw(ParticleLibrary.WhitePixel, position + new Vector2(unselectedSize.X, 0f), new Rectangle(0, 0, (int)selectedSize.X, (int)selectedSize.Y), ParticleLibraryConfig.CurrentTheme.HighAccent.WithAlpha(0.5f));
            }

            spriteBatch.DrawText(Text, position, Color.White);

            if (_textWriter.CaretVisible)
            {
                Vector2 size = _textWriter.CaretIndex == 0 ? Vector2.Zero : FontAssets.MouseText.Value.MeasureString(Text[0.._textWriter.CaretIndex]);
                spriteBatch.DrawLine(position + new Vector2(size.X, -inner.Height / 2f + 4f + 10f), position + new Vector2(size.X, inner.Height / 2f - 4f + 10f), ParticleLibraryConfig.CurrentTheme.HighAccent, 2f);
            }

            spriteBatch.End();

            //Main.graphics.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            Main.graphics.GraphicsDevice.RasterizerState.ScissorTestEnable = false;

            spriteBatch.Begin(LibUtilities.DefaultUISettings);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            _active = true;
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        public override void SetOutline(Color outline)
        {
            Outline = outline;
        }

        public void SetHoverOutline(Color outline)
        {
            ActiveOutline = outline;
        }

        public void ToggleActive()
        {
            _active = false;
            Main.clrInput();
        }
    }
}
