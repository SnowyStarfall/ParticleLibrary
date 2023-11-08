using Microsoft.Xna.Framework;
using ParticleLibrary.UI.Elements.Base;

namespace ParticleLibrary.UI.Elements.Custom
{
	public class ParticleList : ListBox
	{
		public ParticleList(Color fill, Color outline, float outlineThickness = 1, float cornerRadius = 0, bool soft = false) : base(fill, outline, outlineThickness, cornerRadius, soft)
		{
		}


	}
}
