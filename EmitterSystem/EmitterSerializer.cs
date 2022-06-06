using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.EmitterSystem
{
	internal class EmitterSerializer : TagSerializable
	{
		public static Func<TagCompound, EmitterSerializer> DESERIALIZER = DeserializeData;
		public Emitter emitter;
		public EmitterSerializer(Emitter emitter)
		{
			this.emitter = emitter;
		}
		public TagCompound SerializeData()
		{
			TagCompound tag = new();
			if (emitter != null)
			{
				tag.Set("Assembly", emitter.Assembly, true);
				tag.Set("Type", emitter.Type, true);
				tag.Set("Data", emitter.Data, true);
				tag.Set("Save", emitter.Save, true);
				tag.Set("CullDistance", emitter.CullDistance, true);
				tag.Set("whoAmI", emitter.whoAmI, true);
				tag.Set("active", emitter.active, true);
				tag.Set("positionX", emitter.position.X, true);
				tag.Set("positionY", emitter.position.Y, true);
				tag.Set("velocityX", emitter.velocity.X, true);
				tag.Set("velocityY", emitter.velocity.Y, true);
				tag.Set("oldPositionX", emitter.oldPosition.X, true);
				tag.Set("oldPositionY", emitter.oldPosition.Y, true);
				tag.Set("oldVelocityX", emitter.oldVelocity.X, true);
				tag.Set("oldVelocityY", emitter.oldVelocity.Y, true);
				tag.Set("oldDirection", emitter.oldDirection, true);
				tag.Set("direction", emitter.direction, true);
				tag.Set("width", emitter.width, true);
				tag.Set("height", emitter.height, true);
				tag.Set("wet", emitter.wet, true);
				tag.Set("honeyWet", emitter.honeyWet, true);
				tag.Set("wetCount", emitter.wetCount, true);
				tag.Set("lavaWet", emitter.lavaWet, true);
			}
			return tag;
		}
		public static EmitterSerializer DeserializeData(TagCompound tag)
		{
			string assembly = tag.GetString("Assembly");
			string type = tag.GetString("Type");
			bool exists = ModLoader.TryGetMod(assembly, out Mod result);
			if (!exists)
				return null;
			Emitter e = result.Code.CreateInstance(type) as Emitter;
			e.Assembly = assembly;
			e.Type = type;
			e.Data = tag.GetString("Data");
			e.Save = tag.GetBool("Save");
			e.CullDistance = tag.GetFloat("CullDistance");
			e.whoAmI = tag.GetInt("whoAmI");
			e.active = tag.GetBool("active");
			e.position.X = tag.GetFloat("positionX");
			e.position.Y = tag.GetFloat("positionY");
			e.velocity.X = tag.GetFloat("velocityX");
			e.velocity.Y = tag.GetFloat("velocityY");
			e.oldPosition.X = tag.GetFloat("oldPositionX");
			e.oldPosition.Y = tag.GetFloat("oldPositionY");
			e.oldVelocity.X = tag.GetFloat("oldVelocityX");
			e.oldVelocity.Y = tag.GetFloat("oldVelocityY");
			e.oldDirection = tag.GetInt("oldDirection");
			e.direction = tag.GetInt("direction");
			e.width = tag.GetInt("width");
			e.height = tag.GetInt("height");
			e.wet = tag.GetBool("wet");
			e.honeyWet = tag.GetBool("honeyWet");
			e.wetCount = tag.GetByte("wetCount");
			e.lavaWet = tag.GetBool("lavaWet");
			return new(e);
		}
	}
}
