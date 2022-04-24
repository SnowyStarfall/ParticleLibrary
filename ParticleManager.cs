
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ParticleLibrary
{
	/// <summary>
	/// This class manages the particle system.
	/// </summary>
	public class ParticleManager
	{
		/// <summary>
		/// A list that contains all active particles.
		/// </summary>
		public static List<Particle> particles;
		/// <summary>
		/// </summary>
		public static void Load()
		{
			particles = new List<Particle>(ParticleLibraryConfig.Instance.MaxParticles);
			On.Terraria.Dust.UpdateDust += UpdateParticles;
			On.Terraria.Main.DrawDust += DrawParticles;
		}
		/// <summary>
		/// </summary>
		public static void Unload()
		{
			particles = null;
			On.Terraria.Dust.UpdateDust -= UpdateParticles;
			On.Terraria.Main.DrawDust -= DrawParticles;
		}
		/// <summary>
		/// Disposes the current list of particles.
		/// </summary>
		public static void Dispose()
		{
			particles?.Clear();
		}
		private static void UpdateParticles(On.Terraria.Dust.orig_UpdateDust orig)
		{
			PreUpdate();
			Update();
			PostUpdate();
			orig();
		}
		private static void DrawParticles(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
				for (int i = 0; i < particles?.Count; i++)
				{
					Particle particle = particles[i];
					if (particle != null)
					{
						bool draw = particle.PreDraw(Main.spriteBatch, particle.VisualPosition, Lighting.GetColor((int)(particle.position.X / 16), (int)(particle.position.Y / 16)));
						if (draw)
							particle.Draw(Main.spriteBatch, particle.VisualPosition, Lighting.GetColor((int)(particle.position.X / 16), (int)(particle.position.Y / 16)));
						particle.PostDraw(Main.spriteBatch, particle.VisualPosition, Lighting.GetColor((int)(particle.position.X / 16), (int)(particle.position.Y / 16)));
					}
				}
				Main.spriteBatch.End();
			}
			orig(self);
		}
		internal static void PreUpdate()
		{
			if (Main.hasFocus && !Main.gamePaused)
				for (int i = 0; i < particles?.Count; i++)
					particles[i].PreAI();
		}
		internal static void Update()
		{
			for (int i = 0; i < particles?.Count; i++)
			{
				Particle particle = particles[i];
				if (Main.hasFocus && !Main.gamePaused)
				{
					particle.oldDirection = particle.direction;
					if (particle.tileCollide && !Collision.SolidCollision(particles[i].position + new Vector2(particles[i].width / 2f, particles[i].height / 2f) * particles[i].scale, 1, 1) || !particle.tileCollide)
					{
						particle.velocity.Y += particles[i].gravity;
						particle.position += particles[i].velocity;
						UpdateArrays(particle);
					}

					particle.direction = particle.velocity.X >= 0f ? 1 : -1;
					particle.lavaWet = Collision.LavaCollision(particle.position, particle.width, particle.height);
					particle.wet = Collision.WetCollision(particle.position, particle.width, particle.height);

					particle.AI();

					if (particle.timeLeft-- == 0 || !particles[i].active)
					{
						particle.DeathAction?.Invoke();
						particles.RemoveAt(i);
					}
				}
			}
		}
		internal static void PostUpdate()
		{
			for (int i = 0; i < particles?.Count; i++)
			{
				if (Main.hasFocus && !Main.gamePaused)
					particles[i].PostAI();
			}
		}
		/// <summary>
		/// Creates a new instance of a particle Type.
		/// </summary>
		/// <typeparam name="T">The particle.</typeparam>
		/// <returns>A new instance of this particle</returns>
		public static Particle NewInstance<T>() where T : Particle
		{
			return Activator.CreateInstance<T>();
		}
		/// <summary>
		/// Spawns a new particle at the desired position.
		/// </summary>
		/// <typeparam name="T">The particle.</typeparam>
		/// <param name="Position">The particle's position.</param>
		/// <param name="Velocity">The particle's velocity.</param>
		/// <param name="Color">The particle's color.</param>
		/// <param name="Scale">The particle's size.</param>
		/// <param name="AI0">Value to pass to the particle's AI array.</param>
		/// <param name="AI1"></param>
		/// <param name="AI2"></param>
		/// <param name="AI3"></param>
		/// <param name="AI4"></param>
		/// <param name="AI5"></param>
		/// <param name="AI6"></param>
		/// <param name="AI7"></param>
		public static void NewParticle<T>(Vector2 Position, Vector2 Velocity, Color Color, float Scale, float AI0 = 0, float AI1 = 0, float AI2 = 0, float AI3 = 0, float AI4 = 0, float AI5 = 0, float AI6 = 0, float AI7 = 0) where T : Particle
		{
			Particle Particle = Activator.CreateInstance<T>();
			NewParticle(Position, Velocity, Particle, Color, Scale, AI0, AI1, AI2, AI3, AI4, AI5, AI6, AI7);
		}
		/// <summary>
		/// Spawns a new particle at the desired position.
		/// </summary>
		/// <param name="Position">The particle's position.</param>
		/// <param name="Velocity">The particle's velocity.</param>
		/// <param name="Color">The particle's color.</param>
		/// <param name="Scale">The particle's size.</param>
		/// <param name="AI0">Value to pass to the particle's AI array.</param>
		/// <param name="AI1"></param>
		/// <param name="AI2"></param>
		/// <param name="AI3"></param>
		/// <param name="AI4"></param>
		/// <param name="AI5"></param>
		/// <param name="AI6"></param>
		/// <param name="AI7"></param>
		/// <exception cref="NullReferenceException"></exception>
		public static void NewParticle(Vector2 Position, Vector2 Velocity, Particle particle, Color Color, float Scale, float AI0 = 0, float AI1 = 0, float AI2 = 0, float AI3 = 0, float AI4 = 0, float AI5 = 0, float AI6 = 0, float AI7 = 0)
		{
			Particle type = (Particle)Activator.CreateInstance(particle.GetType());

			if (particles?.Count > ParticleLibraryConfig.Instance.MaxParticles)
				particles.TrimExcess();
			if (particles?.Count == ParticleLibraryConfig.Instance.MaxParticles)
				return;
			if (type.texture == null)
				throw new NullReferenceException($"Texture was null for {type.GetType().Name}.");

			type.position = Position;
			type.velocity = Velocity;
			type.color = Color;
			type.scale = Scale;
			type.active = true;
			type.ai = new float[] { AI0, AI1, AI2, AI3, AI4, AI5, AI6, AI7 };
			type.SpawnAction?.Invoke();
			particles?.Add(type);
		}
		internal static void UpdateArrays(Particle particle)
		{
			if (particle.oldPos != null)
			{
				for (int i = particle.oldPos.Length - 1; i >= 0; i--)
				{
					particle.oldPos[i] = i == 0 ? particle.position : particle.oldPos[i - 1];
					if (i == 0)
						break;
				}
			}
			if (particle.oldCen != null)
			{
				for (int i = particle.oldCen.Length - 1; i >= 0; i--)
				{
					particle.oldCen[i] = i == 0 ? particle.Center : particle.oldCen[i - 1];
					if (i == 0)
						break;
				}
			}
			if (particle.oldRot != null)
			{
				for (int i = particle.oldRot.Length - 1; i >= 0; i--)
				{
					particle.oldRot[i] = i == 0 ? particle.rotation : particle.oldRot[i - 1];
					if (i == 0)
						break;
				}
			}
			if (particle.oldVel != null)
			{
				for (int i = particle.oldVel.Length - 1; i >= 0; i--)
				{
					particle.oldVel[i] = i == 0 ? particle.velocity : particle.oldVel[i - 1];
					if (i == 0)
						break;
				}
			}
		}
	}
}
