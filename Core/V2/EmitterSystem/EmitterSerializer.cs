using System;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core
{
	internal class EmitterSerializer : TagSerializable
	{
		public static Func<TagCompound, EmitterSerializer> DESERIALIZER = DeserializeData;
		public readonly Emitter Emitter;

		public EmitterSerializer(Emitter emitter)
		{
			Emitter = emitter;
		}

		public TagCompound SerializeData()
		{
			TagCompound tag = new();
			if (Emitter is null)
			{
				return tag;
			}

			tag.Set("Assembly", Emitter.Assembly, true);
			tag.Set("Type", Emitter.Type, true);
			Emitter.EmitterSettings.SaveData(tag);
			Emitter.ParticleSettings.SaveData(tag);
			Emitter.ColorSettings.SaveData(tag);

			return tag;
		}

		public static EmitterSerializer DeserializeData(TagCompound tag)
		{
			string assembly = tag.GetString("Assembly");
			string type = tag.GetString("Type");

			bool exists = ModLoader.TryGetMod(assembly, out Mod result);
			if (!exists)
			{
				return null;
			}

			EmitterSettings settings = new();
			EmitterParticleSettings particleSettings = new();
			EmitterColorSettings colorSettings = new();

			settings.LoadData(tag);
			particleSettings.LoadData(tag);
			colorSettings.LoadData(tag);

			Type t = result.Code.GetType(type) ?? typeof(Emitter);
			Emitter e = Activator.CreateInstance(t, settings, particleSettings, colorSettings) as Emitter;
			return new(e);
		}

		internal static Emitter CreateInstance<T>(EmitterSettings settings, EmitterParticleSettings particleSettings, EmitterColorSettings colorSettings) where T : Emitter, new()
		{
			var e = new T
			{
				EmitterSettings = settings,
				ParticleSettings = particleSettings,
				ColorSettings = colorSettings
			};

			return e;
		}
	}
}
