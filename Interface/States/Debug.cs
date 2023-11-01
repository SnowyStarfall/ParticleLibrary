using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.Interface.Elements;
using Terraria;
using Terraria.UI;

namespace ParticleLibrary.Interface.States
{
	internal class Debug : UIState
	{
		public bool Visible => ParticleLibraryConfig.Instance.DebugUI;

		public Panel Base { get; set; }
		public SearchBar SearchBar { get; set; }

		public override void OnInitialize()
		{
			// Visibility
			if (!Visible)
				return;

			Base = new(ParticleLibraryConfig.CurrentTheme.Low, ParticleLibraryConfig.CurrentTheme.LowAccent, 1f)
			{
				Resizable = true,
				Draggable = true
			};
			Base.Width.Set(384f, 0f);
			Base.Height.Set(512f, 0f);
			Base.MinWidth.Set(0f, 0.125f);
			Base.MinHeight.Set(0f, 0.125f);

			SearchBar = new(ParticleLibraryConfig.CurrentTheme.Mid, ParticleLibraryConfig.CurrentTheme.LowAccent, ParticleLibraryConfig.CurrentTheme.HighAccent, 1f, 0f)
			{
			};
			SearchBar.Left.Set(0f, 0.05f);
			SearchBar.Top.Set(32f, 0f);
			SearchBar.Width.Set(0f, 0.9f);
			SearchBar.Height.Set(24f, 0f);

			Base.Append(SearchBar);

			Append(Base);
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
			if (Width.Pixels != Main.screenWidth)
			{
				Width.Pixels = Main.screenWidth;
				Recalculate();
			}

			if(Height.Pixels != Main.screenHeight)
			{
				Height.Pixels = Main.screenHeight;
				Recalculate();
			}
		}
	}
}
