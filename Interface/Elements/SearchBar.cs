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

namespace ParticleLibrary.Interface.Elements
{
	public class SearchBar : Panel, IConsistentUpdateable
	{
		public Color ActiveOutline { get; private set; }
		public string Text { get; private set; } = string.Empty;

		public delegate void TextChanged(string oldValue, string newValue);
		public event TextChanged OnTextChanged;

		protected bool _active;

		private int _caretCounter;
		private int _caretIndex;

		private bool _arrowHeld;
		private int _arrowCounter;

		private TextWriter _textWriter;

		public SearchBar(Color fill, Color outline, Color activeOutline, float outlineThickness = 1f, float cornerRadius = 0f, bool soft = false) : base(fill, outline, outlineThickness, cornerRadius, soft)
		{
			ActiveOutline = activeOutline;

			_textWriter = new();
		}

		// For things that need to be consistently updated.
		public void Update()
		{
			_caretCounter++;
			if (_caretCounter > 50)
				_caretCounter = 0;

			if (_arrowCounter > 0)
				_arrowCounter--;

			_textWriter.Update();
		}

		// For things that don't need to be consistently updated.
		public override void Update(GameTime gameTime)
		{
			// We're not typing, so this code shouldn't run.
			if (!_active)
				return;

			Text = _textWriter.Write();

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

			// Caret manipulation
			if (Main.keyState.IsKeyDown(Keys.Left))
			{
				if (_arrowCounter <= 0)
				{
					if (!_arrowHeld)
					{
						_arrowCounter = 30;
						_arrowHeld = true;
						if (_caretIndex > 0)
							_caretIndex--;
						Main.NewText(_caretIndex);
					}
					else
					{
						_arrowCounter = 2;
						if (_caretIndex > 0)
							_caretIndex--;
						Main.NewText(_caretIndex);
					}
				}
			}
			else if (Main.keyState.IsKeyDown(Keys.Right))
			{
				if (_arrowCounter <= 0)
				{
					if (!_arrowHeld)
					{
						_arrowCounter = 30;
						_arrowHeld = true;
						if (_caretIndex < Text.Length)
							_caretIndex++;
						Main.NewText(_caretIndex);
					}
					else
					{
						_arrowCounter = 2;
						if (_caretIndex < Text.Length)
							_caretIndex++;
						Main.NewText(_caretIndex);
					}
				}
			}
			else
			{
				_arrowCounter = 0;
				_arrowHeld = false;
			}


			Main.NewText(_textWriter.Text ?? "NULL");

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
				//Main.instance.HandleIME();

				// Change the stored value.
				//string newText = _textWriter.Write();

				//if (newText != Text)
				//{
				//	//string newText = inputText;

				//	OnTextChanged?.Invoke(Text, newText);
				//	Text = newText;

				//	//_caretIndex += inputText.Length;
				//}

			}

			// Draw text with clipping
			CalculatedStyle inner = GetInnerDimensions();

			Main.graphics.GraphicsDevice.ScissorRectangle = inner.ToRectangle();
			Main.graphics.GraphicsDevice.RasterizerState.ScissorTestEnable = true;

			spriteBatch.Begin(LibUtilities.DefaultUISettings);

			Vector2 position = inner.Center() - new Vector2(inner.Width / 2f - 8f, 10f);

			spriteBatch.DrawText(Text, position, Color.White);

			if (_caretCounter < 25)
			{
				//Vector2 size = FontAssets.MouseText.Value.MeasureString(_caretIndex == 0 ? string.Empty : Text[0.._caretIndex]);
				//spriteBatch.DrawLine(position + new Vector2(size.X + 4f, -inner.Height / 2f + 4f + 10f), position + new Vector2(size.X + 4f, inner.Height / 2f - 4f + 10f), ParticleLibraryConfig.CurrentTheme.HighAccent, 2f);
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
