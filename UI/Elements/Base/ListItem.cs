using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.UI.Interfaces;
using ParticleLibrary.Utilities;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace ParticleLibrary.UI.Elements.Base
{
	public class ListItem : UIElement, IConsistentUpdateable
	{
		public object Content { get; set; }

		private bool _overflows;
		private float _hiddenLength;

		private int _startCounter;
		private int _moveCounter;
		private int _endCounter;

		public ListItem(object content)
		{
			Content = content;
			_overflows = Overflows();
		}

		public void Update()
		{
			if (_overflows)
			{
				if (_endCounter >= 60)
				{
					_startCounter = 0;
					_moveCounter = 0;
					_endCounter = 0;
				}

				if (_moveCounter >= 100 && _endCounter < 60)
				{
					_endCounter++;
				}

				if (_startCounter >= 60 && _moveCounter < 100)
				{
					_moveCounter++;
				}

				if (_startCounter < 60)
				{
					_startCounter++;
				}

				return;
			}

			_startCounter = 0;
			_moveCounter = 0;
			_endCounter = 0;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			_overflows = Overflows();

			CalculatedStyle dim = GetInnerDimensions();
			Vector2 size = FontAssets.MouseText.Value.MeasureString(Content.ToString());
			float scale = Utils.Clamp(dim.Height / size.Y, 0f, 2f);

			spriteBatch.DrawText(Content.ToString(), 1, dim.Position() - new Vector2((_moveCounter / 100f) * _hiddenLength, 0f), Color.White, Color.Black, scale: scale);
		}

		private bool Overflows()
		{
			Vector2 position = GetInnerDimensions().Position();
			Vector2 size = FontAssets.MouseText.Value.MeasureString(Content.ToString());

			Rectangle dim = GetInnerDimensions().ToRectangle();
			Rectangle textDim = new((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

			if ((textDim.X + textDim.Width) > (dim.X + dim.Width))
			{
				_hiddenLength = (position.X + size.X) - (dim.X + dim.Width);
				return true;
			}

			_hiddenLength = 0f;

			return false;
		}
	}
}
