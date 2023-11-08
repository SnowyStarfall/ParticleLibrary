using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.UI.Elements.Base;
using ParticleLibrary.UI.Themes;
using Terraria;
using Terraria.UI;

namespace ParticleLibrary.UI.States
{
    internal class Debug : UIState
	{
		public bool Visible => ParticleLibraryConfig.Instance.DebugUI;
		public Theme Theme => ParticleLibraryConfig.CurrentTheme;

		public UIElement Base { get; set; }
		public Panel SearchPanel { get; set; }
		public SearchBar SearchBar { get; set; }
		public Button SearchButton { get; set; }

		private Point _lastScreenSize = Main.ScreenSize;

		public override void OnInitialize()
		{
			// Visibility
			if (!Visible)
				return;

			Base = new();

			SearchPanel = new(Theme.Low, Theme.LowAccent, 1f, 1f)
			{
				Resizable = true,
				Draggable = true
			};
			SearchPanel.Width.Set(384f, 0f);
			SearchPanel.Height.Set(512f, 0f);
			SearchPanel.MinWidth.Set(0f, 0.125f);
			SearchPanel.MinHeight.Set(0f, 0.125f);

			SearchBar = new(Theme.Mid, Theme.LowAccent, Theme.HighAccent, 1f, 12f)
			{
			};
			SearchBar.Left.Set(0f, 0.05f);
			SearchBar.Top.Set(32f, 0f);
			SearchBar.Width.Set(0f, 0.9f);
			SearchBar.Height.Set(24f, 0f);

			SearchButton = new(Theme.Mid, Theme.High, Theme.LowAccent, Theme.HighAccent)
			{
				Content = "Test Content"
			};
			SearchButton.HAlign = 0.5f;
			SearchButton.VAlign = 0.5f;
			SearchButton.Width.Set(64f, 0f);
			SearchButton.Height.Set(32f, 0f);

			//// Append all panels to base
			//Base.Append(SearchPanel);
			//Append(Base);

			SearchPanel.Append(SearchBar);
			SearchPanel.Append(SearchButton);
			Append(SearchPanel);
		}

		public void Unload()
		{
			// TODO: Implement unloading logic

			RemoveAllChildren();
			Base = null;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

#if DEBUG
			if (Main.keyState.IsKeyDown(Keys.OemCloseBrackets))
			{
				Unload();
				OnInitialize();
			}
#endif

			// Ensure that our state always covers the entire screen
			if (_lastScreenSize != Main.ScreenSize)
			{
				_lastScreenSize = Main.ScreenSize;
				Recalculate();
			}
		}
	}
}
