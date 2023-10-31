using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ReLogic.OS;
using System;
using Terraria;
using Terraria.ID;
using Terraria.UI.Chat;

namespace ParticleLibrary.Interface
{
	public class TextWriter
	{
		public string Text { get; private set; } = string.Empty;
		public bool AllowNewline { get; set; }

		private int _caretCounter;
		private int _caretIndex;

		private bool _arrowHeld;
		private int _arrowCounter;

		private bool _backspaceHeld;
		private int _backspaceCounter;

		private KeyboardState _keyState;
		private KeyboardState _oldKeyState;

		public void Update()
		{
			_caretCounter++;
			if (_caretCounter > 50)
				_caretCounter = 0;

			if (_arrowCounter > 0)
				_arrowCounter--;

			if (_backspaceCounter > 0)
			{
				_backspaceCounter--;
			}
		}

		public string Write()
		{
			// Don't operate on server
			if (Main.netMode is NetmodeID.Server)
				return Text;

			// Don't operate unless Terraria is focsued
			if (!Main.hasFocus)
				return Text;

			// Control is held
			if (Main.keyState.PressingControl())
			{
				// Cut
				if (Main.keyState.IsKeyDown(Keys.X) && !_oldKeyState.IsKeyDown(Keys.X))
				{
					Platform.Get<IClipboard>().Value = Text;
					Text = string.Empty;
				}
				// Copy
				else if (Main.keyState.IsKeyDown(Keys.C) && !_oldKeyState.IsKeyDown(Keys.C))
				{
					Platform.Get<IClipboard>().Value = Text;
				}
				// Paste
				else if (Main.keyState.IsKeyDown(Keys.V) && !_oldKeyState.IsKeyDown(Keys.V))
				{
					string copied = Platform.Get<IClipboard>().Value;
					if (!AllowNewline)
						copied = copied.Replace("\n", string.Empty);
					Text += copied;
				}
			}
			// Shift is held
			else if (Main.keyState.PressingShift())
			{
				// Delete line
				if (Main.keyState.IsKeyDown(Keys.Delete) && !_oldKeyState.IsKeyDown(Keys.Delete))
				{
					Text = string.Empty;
				}
				// Paste
				else if (Main.keyState.IsKeyDown(Keys.Insert) && !_oldKeyState.IsKeyDown(Keys.Insert))
				{
					string copied = Platform.Get<IClipboard>().Value;
					if (!AllowNewline)
						copied = copied.Replace("\n", string.Empty);
					Text += copied;
				}
			}
			else if (Main.keyState.IsKeyDown(Keys.Left))
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
				// Some magical stuff for getting the value of recently pressed keys
				string tempText = string.Empty;
				for (int i = 0; i < Main.keyCount; i++)
				{
					Keys key = (Keys)Main.keyInt[i];
					string value = Main.keyString[i];

					// Enter
					if (key == Keys.Enter && AllowNewline)
					{
						tempText += "\n";
					}

					// Backspace
					if (key == Keys.Back && _backspaceCounter <= 0) // Backspace
					{
						if (!_backspaceHeld)
						{
							_backspaceHeld = true;
							_backspaceCounter = 10;
						}


						if (Text.Length > 0)
							Text = Text.Remove(_caretIndex - 1, 1);

						if (_caretIndex > 0)
							_caretIndex--;
					}
					else
					{
						_backspaceHeld = false;
						_backspaceCounter = 0;
					}

					// Letters, numbers, symbols
					if (key >= Keys.Space && key != Keys.F16)
					{
						tempText += value;
					}
				}

				Text = Text.Insert(_caretIndex, tempText);
				_caretIndex += tempText.Length;

				// Reset caret manipulation
				_arrowCounter = 0;
				_arrowHeld = false;
			}

			// Update our old key state and clear input
			_oldKeyState = Keyboard.GetState();
			Main.clrInput();

			return Text;
		}
	}
}
