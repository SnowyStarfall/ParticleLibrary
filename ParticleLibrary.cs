using log4net;
using ParticleLibrary.Core;
using Terraria.ModLoader;

namespace ParticleLibrary
{
	public class ParticleLibrary : Mod
	{
		public static ParticleLibrary Instance { get; private set; }

		public static bool Debug
		{
			get
			{
#if DEBUG
				return true;
#else
				return false;
#endif
			}
		}

		internal static ILog Log => Instance.Logger;

		public override void Load()
		{
			Instance = this;

			EmitterSettings.Load();
		}

		public override void Unload()
		{
			Instance = null;

			EmitterSettings.Unload();
		}
	}
}