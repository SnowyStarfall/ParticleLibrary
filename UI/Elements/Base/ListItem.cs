using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.UI.Interfaces;
using ParticleLibrary.Utilities;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace ParticleLibrary.UI.Elements.Base
{
	public class ListItem : UIElement, IConsistentUpdateable
	{
		public object Content { get; set; }

		private readonly TextShifter _textShifter;
		private bool _overflows;
		private float _hiddenLength;

		public ListItem(object content)
		{
			Content = content;
			_textShifter = new();
			_overflows = Overflows();
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

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			_overflows = Overflows();

			CalculatedStyle inner = GetInnerDimensions();
			Vector2 size = FontAssets.MouseText.Value.MeasureString(Content.ToString());
			float scale = Utils.Clamp(inner.Height / size.Y, 0f, 2f);
			Vector2 position = inner.Position() - new Vector2(_textShifter.Progress * _hiddenLength, 0f);

			spriteBatch.DrawText(Content.ToString(), 1, position, Color.White, Color.Black, scale: scale);
		}

		private bool Overflows()
		{
			Vector2 position = GetInnerDimensions().Position();
			Vector2 size = FontAssets.MouseText.Value.MeasureString(Content.ToString());

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
	}
}
