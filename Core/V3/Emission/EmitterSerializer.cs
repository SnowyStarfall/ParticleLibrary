using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core.V3.Emission
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
			TagCompound tag = [];
			if (Emitter is null)
			{
				return tag;
			}

			tag.Set("Assembly", Emitter.Assembly, true);
			tag.Set("Type", Emitter.Type, true);
			tag.Set("Position", Emitter.Position, true);
			tag.Set("Size", Emitter.Size, true);

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

			Vector2 position = tag.Get<Vector2>("Position");
			Point size = tag.Get<Point>("Size");

			Type t = result.Code.GetType(type) ?? typeof(Emitter);
			Emitter e = Activator.CreateInstance(t, position, size) as Emitter;
			return new(e);
		}
	}
}
