using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.UI.Interfaces;
using ParticleLibrary.UI.Primitives;
using ParticleLibrary.UI.Primitives.Complex;
using ParticleLibrary.UI.Primitives.Shapes;
using ParticleLibrary.UI.States;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ParticleLibrary.UI
{
	internal class UISystem : ModSystem
	{
		internal static UserInterface DebugUILayer { get; private set; }
		internal static Debug DebugUIElement { get; private set; }

		internal static Box Box { get; private set; }
		//internal static PrimRectangle Rectangle { get; private set; }
		//internal static PrimCircle Circle { get; private set; }

		public override void Load()
		{
			if (Main.netMode == NetmodeID.Server)
				return;

			DebugUILayer = new UserInterface();
			DebugUIElement = new Debug();
			DebugUILayer.SetState(DebugUIElement);

			Box = new(Vector2.Zero, Vector2.Zero, Color.Transparent, outlineThickness: 1);
			//Rectangle = new(Vector2.Zero, Vector2.Zero, MatrixType.Interface);
			//Circle = new(Vector2.Zero, Vector2.Zero, MatrixType.Interface);
		}

		public override void PostUpdateEverything()
		{
			if (DebugUIElement.Visible)
				DebugUIElement.ExecuteRecursively(RecursiveUpdate);
		}

		public override void PostUpdateInput()
		{
#if DEBUG
			if (Main.keyState.IsKeyDown(Keys.OemCloseBrackets))
			{
				DebugUIElement.Unload();
				DebugUIElement.OnInitialize();
			}
#endif
		}

		private GameTime _lastUpdateUIGameTIme;
		public override void UpdateUI(GameTime gameTime)
		{
			_lastUpdateUIGameTIme = gameTime;

			if (DebugUILayer?.CurrentState is not null)
				DebugUILayer.Update(gameTime);
		}

		public void RecursiveUpdate(UIElement element)
		{
			if (element is null)
				return;

			if (element is IConsistentUpdateable updateable)
				updateable.Update();
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1)
			{
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer("ParticleLibrary: DebugUI", () =>
				{
					if (_lastUpdateUIGameTIme != null && DebugUILayer?.CurrentState != null)
						DebugUIElement.Draw(Main.spriteBatch);
					//DrawDebugHitbox(DebugUIElement, 1f);
					//PrintHoveredElement(DebugUIElement, Color.Red);
					return true;
				}, InterfaceScaleType.UI));
			}
		}

		public static void DrawDebugHitbox(UIElement element, float colorIntensity = 0.5f)
		{
			if (element.IsMouseHovering)
			{
				colorIntensity += 0.1f;
			}

			float alpha = 0.2f;
			Color color;

			Rectangle outerDimensions = element.GetInnerDimensions().ToRectangle();
			Rectangle dimensions = element.GetInnerDimensions().ToRectangle();
			Rectangle innerDimensions = element.GetInnerDimensions().ToRectangle();

			color = Main.hslToRgb(colorIntensity, colorIntensity, 0.5f) * (alpha / 2f);
			Box.SetSize(outerDimensions);
			Box.SetOutline(color);
			Box.Draw();

			color = Main.hslToRgb(colorIntensity, colorIntensity, 0.5f) * alpha;
			Box.SetSize(dimensions);
			Box.SetOutline(color);
			Box.Draw();

			color = Main.hslToRgb(colorIntensity, colorIntensity, 0.5f) * (alpha / 2f);
			Box.SetSize(innerDimensions);
			Box.SetOutline(color);
			Box.Draw();

			//Rectangle.SetSize(innerDimensions);
			//Rectangle.Color = color;
			//Rectangle.Draw();

			//Circle.Position = innerDimensions.Center();
			//Circle.Radius = new Vector2(innerDimensions.Width / 2f, innerDimensions.Height / 2f);
			//Circle.Draw();

			if (element is IDebuggable debuggable)
			{
				debuggable.DebugRender(Box, alpha);
			}

			foreach (UIElement e in element.Children)
			{
				DrawDebugHitbox(e, colorIntensity);
			}
		}

		public static void PrintHoveredElement(UIElement element, Color color)
		{
			string s = element.GetElementAt(Main.MouseScreen)?.ToString();
			if (s is not null)
				Main.NewText(s, color);
		}
	}
}
