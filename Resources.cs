

namespace ParticleLibrary
{
    /// <remarks>
    /// This file originates from Nez, a Monogame library, which includes a T4 template that will auto-generate the content of this file.
    /// Modified by SnowyStarfall to be more thorough.
	/// To use, right click the Resources.tt file in Visual Studio and click "Run Custom Tool". It will generate Resources.cs nested below itself.
    /// See: <see href="https://github.com/prime31/Nez/blob/master/FAQs/ContentManagement.md#auto-generating-content-paths"/>
    /// </remarks>
    internal class Resources
    {
		public static class Assets
		{
			public static class Effects
			{
				public const string InstancedParticle = @"ParticleLibrary/Assets/Effects/InstancedParticle";
				public const string Particle = @"ParticleLibrary/Assets/Effects/Particle";
				public const string Shape = @"ParticleLibrary/Assets/Effects/Shape";
			}

			public static class Textures
			{
				public static class Interface
				{
					public const string BlackPanel = @"ParticleLibrary/Assets/Textures/Interface/BlackPanel";
					public const string BlackPanelBackground = @"ParticleLibrary/Assets/Textures/Interface/BlackPanelBackground";
					public const string Box = @"ParticleLibrary/Assets/Textures/Interface/Box";
					public const string BoxBackground = @"ParticleLibrary/Assets/Textures/Interface/BoxBackground";
					public const string BoxHighlight = @"ParticleLibrary/Assets/Textures/Interface/BoxHighlight";
					public const string Circle = @"ParticleLibrary/Assets/Textures/Interface/Circle";
					public const string CircleTarget = @"ParticleLibrary/Assets/Textures/Interface/CircleTarget";
					public const string Minus = @"ParticleLibrary/Assets/Textures/Interface/Minus";
					public const string PanelBackground = @"ParticleLibrary/Assets/Textures/Interface/PanelBackground";
					public const string PanelFrame = @"ParticleLibrary/Assets/Textures/Interface/PanelFrame";
					public const string Plus = @"ParticleLibrary/Assets/Textures/Interface/Plus";
					public const string PointTarget = @"ParticleLibrary/Assets/Textures/Interface/PointTarget";
					public const string Scrollbar = @"ParticleLibrary/Assets/Textures/Interface/Scrollbar";
					public const string Scrollbutton = @"ParticleLibrary/Assets/Textures/Interface/Scrollbutton";
					public const string Square = @"ParticleLibrary/Assets/Textures/Interface/Square";
					public const string SquareTarget = @"ParticleLibrary/Assets/Textures/Interface/SquareTarget";
					public const string WhitePixel = @"ParticleLibrary/Assets/Textures/Interface/WhitePixel";
					public const string X = @"ParticleLibrary/Assets/Textures/Interface/X";
				}

				public const string EmptyPixel = @"ParticleLibrary/Assets/Textures/EmptyPixel";
				public const string ExponentCurve = @"ParticleLibrary/Assets/Textures/ExponentCurve";
				public const string Star = @"ParticleLibrary/Assets/Textures/Star";
				public const string WhitePixel = @"ParticleLibrary/Assets/Textures/WhitePixel";
			}

		}

		public static class Core
		{
			public static class V1
			{
				public static class ParticleSystem
				{
					public const string GlowParticle = @"ParticleLibrary/Core/V1/ParticleSystem/GlowParticle";
				}

			}

		}

		public static class Examples
		{
			public static class V2
			{
				public const string ExampleParticle = @"ParticleLibrary/Examples/V2/ExampleParticle";
			}

		}

		public const string Icon = @"ParticleLibrary/icon";
		public const string Icon_workshop = @"ParticleLibrary/icon_workshop";
    }
}

