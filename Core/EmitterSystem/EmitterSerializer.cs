﻿using System;
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

			Emitter e = result.Code.CreateInstance(type) as Emitter;
			if (e is not null)
			{
				e.EmitterSettings.LoadData(tag);
				e.ParticleSettings.LoadData(tag);
				e.ColorSettings.LoadData(tag);
			}

			return new(e);
		}
	}
}
