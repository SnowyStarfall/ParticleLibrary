﻿using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ParticleLibrary.Interface
{
	internal class UISystem : ModSystem
	{
		internal static UISystem Instance;
		internal UserInterface DebugUILayer;
		internal DebugUI DebugUIElement;

		public override void Load()
		{
			Instance = this;

			if (!Main.dedServ)
			{
				DebugUILayer = new UserInterface();
				DebugUIElement = new DebugUI();
				DebugUILayer.SetState(DebugUIElement);
			}
		}

		public override void Unload()
		{
			Instance = null;
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1)
			{
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer("ParticleLibrary:DebugUI", () =>
				{
					DebugUILayer.Update(Main._drawInterfaceGameTime);
					DebugUIElement.Draw(Main.spriteBatch);
					return true;
				}));
			}
		}
	}
}