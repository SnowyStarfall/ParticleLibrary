using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace ParticleLibrary.Core.Systems
{
	public class GParticleSystem : ModSystem
	{
		internal Dictionary<ParticleSettings, Mod> ParticleTypes;


		public override void Load()
		{
			ParticleTypes = new();
		}

		//public override void Unload()
		//{
		//}

		//public static Particle NewParticle<T>(Vector2 Position, Vector2 Velocity, Color Color, float Scale, Layer Layer = Layer.BeforeDust, bool Important = false) where T : Particle
		//{
		//    Particle Particle = Activator.CreateInstance<T>();
		//    return NewParticle(Position, Velocity, Particle, Color, new Vector2(Scale), Layer, Important);
		//}

		//public static Particle NewParticle<T>(Vector2 Position, Vector2 Velocity, Color Color, Vector2 Scale, Layer Layer = Layer.BeforeDust, bool Important = false) where T : Particle
		//{
		//    Particle Particle = Activator.CreateInstance<T>();
		//    return NewParticle(Position, Velocity, Particle, Color, Scale, Layer, Important);
		//}

		//public static Particle NewParticle(Vector2 Position, Vector2 Velocity, Particle Particle, Color Color, float Scale, Layer Layer = Layer.BeforeDust, bool Important = false)
		//{
		//    return NewParticle(Position, Velocity, Particle, Color, new Vector2(Scale), Layer, Important);
		//}

		//public static Particle NewParticle(Vector2 Position, Vector2 Velocity, Particle Particle, Color Color, Vector2 Scale, Layer Layer = Layer.BeforeDust, bool Important = false)
		//{
		//    Particle type = (Particle)Activator.CreateInstance(Particle.GetType());

		//    if (!Important && particles?.Count > ParticleLibraryConfig.Instance.MaxParticles)
		//        particles.TrimExcess();
		//    if (!Important && particles?.Count == ParticleLibraryConfig.Instance.MaxParticles)
		//        return null;

		//    type.position = Position;
		//    type.velocity = Velocity;
		//    type.color = Color;
		//    type.scale = Scale;
		//    type.active = true;
		//    type.layer = Layer;
		//    type.important = Important;
		//    type.SpawnAction?.Invoke();

		//    if (Important)
		//        importantParticles?.Add(type);
		//    else
		//        particles?.Add(type);

		//    OnNewParticle?.Invoke(Position, Velocity, type, Color, Scale, Layer, Important);
		//    return type;
		//}
	}
}