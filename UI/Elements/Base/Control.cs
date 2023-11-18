using ParticleLibrary.UI.Interfaces;
using ParticleLibrary.UI.Primitives.Complex;
using Terraria.UI;

namespace ParticleLibrary.UI.Elements.Base
{
	public class Control : UIElement, IDebuggable
	{
		// Locking
		public bool Screenlocked { get; set; }
		public bool HorizontalLocked { get; set; }
		public bool VerticalLocked { get; set; }

		// Dimensions
		//public new StyleDimension Top { get; set; }
		//public new StyleDimension Left { get; set; }
		//public new StyleDimension Width { get; set; }
		//public new StyleDimension Height { get; set; }

		//public new StyleDimension MaxWidth { get; set; } = StyleDimension.Fill;
		//public new StyleDimension MaxHeight { get; set; } = StyleDimension.Fill;
		//public new StyleDimension MinWidth { get; set; } = StyleDimension.Empty;
		//public new StyleDimension MinHeight { get; set; } = StyleDimension.Empty;

		//public new float PaddingTop { get; set; }
		//public new float PaddingLeft { get; set; }
		//public new float PaddingRight { get; set; }
		//public new float PaddingBottom { get; set; }

		//public new float MarginTop { get; set; }
		//public new float MarginLeft { get; set; }
		//public new float MarginRight { get; set; }
		//public new float MarginBottom { get; set; }

		//public new float HAlign { get; set; }
		//public new float VAlign { get; set; }

		public virtual void DebugRender(Box box, ref float colorIntensity, float alpha)
		{
		}
	}
}
