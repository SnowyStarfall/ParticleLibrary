﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;
using ParticleLibrary.Core.Data;
using ParticleLibrary.Utilities;
using Terraria;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Examples
{
	/// <summary>
	/// This class demonstrates how to create an emitter.
	/// It's required that you create a custom emitter class, since <see cref="Emitter.SpawnParticle(Vector2, SpatialParameters, VisualParameters)"/> won't spawn particles for you.
	/// Emitters provide the calculation functionality, expecting it to be used to create any kind of particle.
	/// </summary>
	public class ExampleEmitter : Emitter
	{
		public ExampleEmitter(EmitterSettings emitterSettings = null, EmitterParticleSettings particleSettings = null, EmitterColorSettings colorSettings = null) : base(emitterSettings, particleSettings, colorSettings)
		{
			// It's recommended to have a look through EmitterSettings, EmitterParticleSettings, and EmitterColorSettings
			// I won't go over them in detail since they're completely documented, but an Emitter can be completely customized
			// Alternatively, you can completely ignore the settings.
			// Here is an example of how you would spawn this emitter in.
			// Please don't uncomment this code...I promise a stack overflow exception!!! c:

			//Core.EmitterSystem.NewEmitter<ExampleEmitter>(new EmitterSettings
			//{
			//	Position = Main.MouseWorld
			//});
		}

		/// <summary>
		/// This method runs after instantiation (constructor finishes). You can do whatever you want in here.
		/// </summary>
		public override void Initialize()
		{
		}

		/// <summary>
		/// You can completely customize how this Emitter updates if you want to.
		/// If you want to avoid calculation overhead (if you're really worried about it), just don't call base.Update();
		/// </summary>
		public override void Update()
		{
			base.Update();
		}

		/// <summary>
		/// You must override this method for the emitter to work.
		/// The method provides information on what settings to spawn the particle with.
		/// These parameters are generated by <see cref="Emitter.EmitterSettings"/>, <see cref="Emitter.ParticleSettings"/>, and <see cref="Emitter.ColorSettings"/>
		/// You aren't required to use these parameters, so I'll showcase both.
		/// We'll be using the particle systems we set up in our <see cref="ExampleParticleSystemManager"/>.
		/// </summary>
		/// <param name="position">The position to spawn at.</param>
		/// <param name="spatial">The spatial parameters.</param>
		/// <param name="visual">The visual parameters.</param>
		public override void SpawnParticle(Vector2 position, SpatialParameters spatial, VisualParameters visual)
		{
			// You can spawn your particles like this, but if this is too much code to look at, then use the method below.
			ExampleParticleSystemManager.ExampleQuadSystem.AddParticle(position, spatial.Velocity, new QuadParticle()
			{
				StartColor = visual.StartColor,
				EndColor = visual.EndColor,
				VelocityAcceleration = spatial.VelocityAcceleration,
				Scale = spatial.Scale,
				ScaleVelocity = spatial.ScaleVelocity,
				Rotation = spatial.Rotation,
				RotationVelocity = spatial.RotationVelocity,
				Depth = spatial.Depth,
				DepthVelocity = spatial.DepthVelocity,
			});

			// This showcases a smaller way of creating the QuadParticle settings
			ExampleParticleSystemManager.ExampleQuadSystem.AddParticle(position, spatial.Velocity, new QuadParticle().FromEmitter(spatial, visual));

			// Alternatively, we can choose not to use the parameters given to us
			ExampleParticleSystemManager.ExampleQuadSystem.AddParticle(position, Main.rand.NextVector2Unit() * Main.rand.NextFloat(-8f, 8f + float.Epsilon), ExampleParticleSystemManager.ExampleQuadParticle);

			// For the sake of being thorough, here is the same code but for our Point particles.
			// Point particles need much fewer parameters than QuadParticles
			ExampleParticleSystemManager.ExamplePointSystem.AddParticle(position, spatial.Velocity, new PointParticle()
			{
				StartColor = visual.StartColor,
				EndColor = visual.EndColor,
				VelocityAcceleration = spatial.VelocityAcceleration,
				Depth = spatial.Depth,
				DepthVelocity = spatial.DepthVelocity,
			});

			ExampleParticleSystemManager.ExamplePointSystem.AddParticle(position, spatial.Velocity, new PointParticle().FromEmitter(spatial, visual));

			ExampleParticleSystemManager.ExamplePointSystem.AddParticle(position, Main.rand.NextVector2Unit() * Main.rand.NextFloat(-8f, 8f + float.Epsilon), ExampleParticleSystemManager.ExamplePointParticle);
		}

		/// <summary>
		/// You can use this method to draw things around the Emitter. You can get pretty creative with this, but you don't have to use it.
		/// This method draws behind dust. Currently there is no way to change the layer like with particles, but if it's something that people ask for enough, it can be added.
		/// SpriteBatch has already begun, there is no need to call Begin() or End().
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="location"></param>
		public override void Draw(SpriteBatch spriteBatch, Vector2 location)
		{
		}

		/// <summary>
		/// You can use this method to save custom data for custom Emitters, if you have it. Just make sure to load the data correctly.
		/// </summary>
		/// <param name="tag"></param>
		public override void SaveData(TagCompound tag)
		{
		}

		/// <summary>
		/// You can use this method to load custom data for custom Emitters, if you have it. Just make sure you've saved the data correctly.
		/// </summary>
		/// <param name="tag"></param>
		public override void LoadData(TagCompound tag)
		{
		}
	}
}