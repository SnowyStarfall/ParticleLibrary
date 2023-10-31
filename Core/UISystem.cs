using Microsoft.Xna.Framework;
using ParticleLibrary.Core.Primitives.Shapes;
using ParticleLibrary.Interface;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ParticleLibrary.Core
{
    internal class UISystem : ModSystem
    {
        internal static UserInterface DebugUILayer { get; private set; }
        internal static Debug DebugUIElement { get; private set; }

        internal static PrimRectangle Rectangle { get; private set; }

        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            DebugUILayer = new UserInterface();
            DebugUIElement = new Debug();
            DebugUILayer.SetState(DebugUIElement);

            Rectangle = new(Vector2.Zero, Vector2.Zero);
        }

        private GameTime _lastUpdateUIGameTIme;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUIGameTIme = gameTime;

            if (DebugUILayer?.CurrentState is not null)
                DebugUILayer.Update(gameTime);
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
                    //DrawDebugHitbox(EntityHealthElement, 1f);
                    //PrintHoveredElement(EntityHealthElement, Color.Red);
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

            Color color = Main.hslToRgb(colorIntensity, colorIntensity, 0.5f);
            CalculatedStyle innerDimensions = element.GetInnerDimensions();

            Rectangle.Position = innerDimensions.Position();
            Rectangle.Size = new Vector2(innerDimensions.Width, innerDimensions.Height);
            Rectangle.Color = color;

            Rectangle.Draw();

            foreach (UIElement e in element.Children)
            {
                DrawDebugHitbox(e, 0.5f);
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
