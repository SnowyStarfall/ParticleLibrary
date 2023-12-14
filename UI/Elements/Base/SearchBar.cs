using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.UI.Interfaces;
using ParticleLibrary.UI.Primitives;
using ParticleLibrary.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;

namespace ParticleLibrary.UI.Elements.Base
{
	public class SearchBar : Panel, IConsistentUpdateable
	{
		public Color ActiveOutline { get; private set; }
		public string Text { get; private set; } = string.Empty;

		public delegate void TextChanged(string oldValue, string newValue);
		public event TextChanged OnTextChanged;

		protected bool _active;

		private readonly TextWriter _textWriter;

		public SearchBar(Color fill, Color outline, Color activeOutline, float outlineThickness = 1f, float cornerRadius = 0f, bool soft = false) : base(fill, outline, outlineThickness, cornerRadius, soft)
		{
			ActiveOutline = activeOutline;

			OverflowHidden = true;
			_textWriter = new();
			HideOverflow = true;
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
				Primitive.SetOutlineColor(ActiveOutline);
			else
				Primitive.SetOutlineColor(Outline);

			Rectangle scissorRectangle = spriteBatch.GetClippingRectangle(this);

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

			// Draw text
			CalculatedStyle inner = GetInnerDimensions();
			Vector2 position = inner.Center() - new Vector2(inner.Width / 2f - 8f, 10f);
			Vector2 size = _textWriter.CaretIndex == 0 ? Vector2.Zero : FontAssets.MouseText.Value.MeasureString(Text[0.._textWriter.CaretIndex]);

			spriteBatch.BeginScissorIf(HideOverflow, LibUtilities.ClarityUISettings, scissorRectangle, true, out Rectangle oldScissorRectangle);

			if (_textWriter.SelectionIndex != -1)
			{
				Vector2 unselectedSize = _textWriter.CaretIndex == 0 || _textWriter.SelectionIndex == 0 ? Vector2.Zero : FontAssets.MouseText.Value.MeasureString(Text[0..Math.Min(_textWriter.CaretIndex, _textWriter.SelectionIndex)]);
				Vector2 selectedSize = _textWriter.CaretIndex == 0 && _textWriter.SelectionIndex == 0 ? Vector2.Zero : FontAssets.MouseText.Value.MeasureString(Text[Math.Min(_textWriter.CaretIndex, _textWriter.SelectionIndex)..Math.Max(_textWriter.CaretIndex, _textWriter.SelectionIndex)]);
				spriteBatch.Draw(ParticleLibrary.WhitePixel, position + new Vector2(unselectedSize.X, -inner.Height / 2f + 12f), new Rectangle(0, 0, (int)selectedSize.X, (int)(inner.Height - 4f)), ParticleLibraryConfig.CurrentTheme.HighAccent.WithAlpha(0.5f));
			}

			float offset = size.X > inner.Width ? size.X - inner.Width : 0f;
			spriteBatch.DrawText(Text, position - new Vector2(offset, 0f), Color.White);

			if (_textWriter.CaretVisible)
			{
				spriteBatch.DrawLine(position + new Vector2(size.X, -inner.Height / 2f + 4f + 10f), position + new Vector2(size.X, inner.Height / 2f - 4f + 10f), ParticleLibraryConfig.CurrentTheme.HighAccent, 2f);
			}

			spriteBatch.EndScissorIf(HideOverflow, oldScissorRectangle);

			spriteBatch.BeginScissorIf(HideOverflow, LibUtilities.DefaultUISettings, oldScissorRectangle, true, out _);
		}

		public override void LeftClick(UIMouseEvent evt)
		{
			base.LeftClick(evt);

			if (evt.Target != this)
			{
				return;
			}

			_active = true;
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);

			if (evt.Target != this)
			{
				return;
			}

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
