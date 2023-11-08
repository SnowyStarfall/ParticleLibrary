using Microsoft.Xna.Framework.Input;
using ReLogic.OS;
using System;
using Terraria;
using Terraria.ID;

namespace ParticleLibrary.UI
{
	public class TextWriter
	{
		public string Text { get; private set; } = string.Empty;
		public bool AllowNewline { get; set; }

		public bool CaretVisible => CaretCounter < 25;
		public int CaretCounter { get; private set; }
		public int CaretIndex { get; private set; }
		public int SelectionIndex { get; private set; } = -1;

		private bool _arrowHeld;
		private int _arrowCounter;

		private bool _backspaceHeld;
		private int _backspaceCounter;

		private KeyboardState _oldKeyState;

		public void Update()
		{
			// We just update counters here - This method only runs once per frame
			CaretCounter++;
			if (CaretCounter > 50)
				CaretCounter = 0;

			if (_arrowCounter > 0)
				_arrowCounter--;

			if (_backspaceCounter > 0)
				_backspaceCounter--;
		}

		public string Write()
		{
			// Don't operate on server
			if (Main.netMode is NetmodeID.Server)
				return Text;

			// Don't operate unless Terraria is focsued
			if (!Main.hasFocus)
				return Text;

			bool control = Main.keyState.PressingControl();
			bool shift = Main.keyState.PressingShift();
			bool left = Main.keyState.IsKeyDown(Keys.Left);
			bool right = Main.keyState.IsKeyDown(Keys.Right);

			if (control)
			{
				// Cut
				if (Main.keyState.IsKeyDown(Keys.X) && !_oldKeyState.IsKeyDown(Keys.X))
				{
					if (SelectionIndex != -1)
					{
						int min = Math.Min(CaretIndex, SelectionIndex);
						int max = Math.Max(CaretIndex, SelectionIndex);

						Platform.Get<IClipboard>().Value = Text[min..max];
						Text = Text.Remove(min, max - min);
						SelectionIndex = -1;
						if(CaretIndex != min)
							CaretIndex = min;
					}
					else
					{
						Platform.Get<IClipboard>().Value = Text;
						Text = string.Empty;
						CaretIndex = 0;
					}


					EndWriting();
					return Text;
				}
				// Copy
				else if (Main.keyState.IsKeyDown(Keys.C) && !_oldKeyState.IsKeyDown(Keys.C))
				{
					if (SelectionIndex != -1)
					{
						int min = Math.Min(CaretIndex, SelectionIndex);
						int max = Math.Max(CaretIndex, SelectionIndex);

						Platform.Get<IClipboard>().Value = Text[min..max];
						SelectionIndex = -1;
					}
					else
					{
						Platform.Get<IClipboard>().Value = Text;
					}

					EndWriting();
					return Text;
				}
				// Paste
				else if (Main.keyState.IsKeyDown(Keys.V) && !_oldKeyState.IsKeyDown(Keys.V))
				{
					string copied = Platform.Get<IClipboard>().Value;
					if (!AllowNewline)
						copied = copied.Replace("\n", string.Empty);

					if (SelectionIndex != -1)
					{
						int min = Math.Min(CaretIndex, SelectionIndex);
						int max = Math.Max(CaretIndex, SelectionIndex);

						Text = Text.Remove(min, max - min);
						Text = Text.Insert(min, copied);
						SelectionIndex = -1;
					}
					else
					{
						Text = Text.Insert(CaretIndex, copied);
					}

					CaretIndex += copied.Length;

					EndWriting();
					return Text;
				}
				// Select all
				else if (Main.keyState.IsKeyDown(Keys.A) && !_oldKeyState.IsKeyDown(Keys.A))
				{
					SelectionIndex = 0;
					CaretIndex = Text.Length;

					EndWriting();
					return Text;
				}
			}

			if (shift)
			{
				// Delete line
				if (Main.keyState.IsKeyDown(Keys.Delete) && !_oldKeyState.IsKeyDown(Keys.Delete))
				{
					Text = string.Empty;
					SelectionIndex = -1;

					EndWriting();
					return Text;
				}
				// Paste
				else if (Main.keyState.IsKeyDown(Keys.Insert) && !_oldKeyState.IsKeyDown(Keys.Insert))
				{
					string copied = Platform.Get<IClipboard>().Value;
					if (!AllowNewline)
						copied = copied.Replace("\n", string.Empty);

					if (SelectionIndex != -1)
					{
						int min = Math.Min(CaretIndex, SelectionIndex);
						int max = Math.Max(CaretIndex, SelectionIndex);

						Text = Text.Remove(min, max - min);
						Text = Text.Insert(min, copied);
						SelectionIndex = -1;
					}
					else
					{
						Text = Text.Insert(CaretIndex, copied);
					}

					CaretIndex += copied.Length;

					EndWriting();
					return Text;
				}
			}

			// Caret manipulation
			if (left)
			{
				if (_arrowCounter <= 0)
				{
					if (!_arrowHeld)
					{
						_arrowCounter = 30;
						_arrowHeld = true;
					}
					else
					{
						_arrowCounter = 2;
					}

					if (shift)
					{
						if (SelectionIndex == -1)
						{
							SelectionIndex = CaretIndex;
						}
					}
					else
					{
						SelectionIndex = -1;
					}

					if (CaretIndex > 0)
					{
						CaretIndex--;
					}
				}

				EndWriting();
				return Text;
			}
			else if (right)
			{
				if (_arrowCounter <= 0)
				{
					if (!_arrowHeld)
					{
						_arrowCounter = 30;
						_arrowHeld = true;
					}
					else
					{
						_arrowCounter = 2;
					}

					if (shift)
					{
						if (SelectionIndex == -1)
						{
							SelectionIndex = CaretIndex;
						}
					}
					else
					{
						SelectionIndex = -1;
					}

					if (CaretIndex < Text.Length)
					{
						CaretIndex++;
					}
				}

				EndWriting();
				return Text;
			}
			else
			{
				_arrowCounter = 0;
				_arrowHeld = false;
			}

			// Writing
			string tempText = string.Empty;
			for (int i = 0; i < Main.keyCount; i++)
			{
				Keys key = (Keys)Main.keyInt[i];
				string value = Main.keyString[i];

				// Enter
				if (key == Keys.Enter && AllowNewline)
				{
					tempText += "\n";

					if (SelectionIndex != -1)
					{
						int min = Math.Min(CaretIndex, SelectionIndex);
						int max = Math.Max(CaretIndex, SelectionIndex);

						Text = Text.Remove(min, max - min);
						SelectionIndex = -1;
						if (CaretIndex != min)
							CaretIndex = min;
					}
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
					{
						if (SelectionIndex != -1 && SelectionIndex != CaretIndex)
						{
							int min = Math.Min(CaretIndex, SelectionIndex);
							int max = Math.Max(CaretIndex, SelectionIndex);

							Text = Text.Remove(min, max - min);
							SelectionIndex = -1;
							CaretIndex = min;
						}
						else
						{
							Text = Text.Remove(CaretIndex - 1, 1);
							if (CaretIndex > 0)
								CaretIndex--;
						}
					}

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

			// Finally, insert text and adjust caret
			if (SelectionIndex != -1 && tempText.Length > 0)
			{
				int min = Math.Min(CaretIndex, SelectionIndex);
				int max = Math.Max(CaretIndex, SelectionIndex);

				Text = Text.Remove(min, max - min);
				Text = Text.Insert(min, tempText);
				SelectionIndex = -1;
				if (CaretIndex != min)
					CaretIndex = min;
			}
			else
			{
				Text = Text.Insert(CaretIndex, tempText);
			}

			CaretIndex += tempText.Length;

			// End and return
			EndWriting();
			return Text;
		}

		private void EndWriting()
		{
			// Update our old key state and clear input
			_oldKeyState = Keyboard.GetState();
			Main.clrInput();
		}
	}
}
