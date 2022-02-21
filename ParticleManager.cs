
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
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
			particles = new List<Particle>(6000);
			On.Terraria.Main.DrawDust += ParticleManager.DrawParticles;
		}
		/// <summary>
		/// </summary>
		public static void Unload()
		{
			particles.Clear();
			particles = null;
			On.Terraria.Main.DrawDust -= DrawParticles;
		}
		/// <summary>
		/// Disposes the current list of particles.
		/// </summary>
		public static void Dispose()
		{
			particles.Clear();
		}
		private static void DrawParticles(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			PreUpdate();
			Update(Main.spriteBatch);
			PostUpdate(Main.spriteBatch);
			Main.spriteBatch.End();
			orig(self);
		}
		internal static void PreUpdate()
		{
			if (Main.hasFocus)
				for (int i = 0; i < particles?.Count; i++)
					particles[i].PreAI();
		}
		internal static void Update(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < particles?.Count; i++)
			{
				Particle particle = particles[i];
				if (Main.hasFocus)
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
				}
				bool draw = particle.PreDraw(spriteBatch, particle.VisualPosition, Lighting.GetColor((int)(particles[i].position.X / 16), (int)(particles[i].position.Y / 16)));
				if (draw)
					particle.Draw(spriteBatch, particle.VisualPosition, Lighting.GetColor((int)(particles[i].position.X / 16), (int)(particles[i].position.Y / 16)));
				if (Main.hasFocus)
				{
					if (particle.timeLeft-- == 0 || !particles[i].active)
					{
						particle.DeathAction?.Invoke();
						particles.RemoveAt(i);
					}
				}
			}
		}
		internal static void PostUpdate(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < particles?.Count; i++)
			{
				if (Main.hasFocus)
					particles[i].PostAI();
				particles[i].PostDraw(spriteBatch, particles[i].VisualPosition, Lighting.GetColor((int)(particles[i].position.X / 16), (int)(particles[i].position.Y / 16)));
			}
		}
		/// <summary>
		/// Spawns a new particle at the desired position.
		/// </summary>
		/// <param name="Position">The position to create a particle at.</param>
		/// <param name="Velocity">The velocity to pass to the particle.</param>
		/// <param name="Type">The type of particle. Use new MyParticle() to pass a type.</param>
		/// <param name="Color">The color to use when drawing the particle.</param>
		/// <param name="Scale">The scale to use when drawing the particle.</param>
		/// <param name="AI0">Value to pass to the particle's AI array.</param>
		/// <param name="AI1"></param>
		/// <param name="AI2"></param>
		/// <param name="AI3"></param>
		/// <param name="AI4"></param>
		/// <param name="AI5"></param>
		/// <param name="AI6"></param>
		/// <param name="AI7"></param>
		/// <exception cref="NullReferenceException"></exception>
		public static void NewParticle(Vector2 Position, Vector2 Velocity, Particle Type, Color Color, float Scale, float AI0 = 0, float AI1 = 0, float AI2 = 0, float AI3 = 0, float AI4 = 0, float AI5 = 0, float AI6 = 0, float AI7 = 0)
		{
			if (Type.texture == null)
				throw new NullReferenceException($"Texture was null for {Type.GetType().Name}.");
			Type.position = Position;
			Type.velocity = Velocity;
			Type.color = Color;
			Type.scale = Scale;
			Type.active = true;
			Type.ai = new float[] { AI0, AI1, AI2, AI3, AI4, AI5, AI6, AI7 };
			if (particles?.Count > 6000)
				particles.TrimExcess();
			if (particles?.Count < 6000)
			{
				Type.SpawnAction?.Invoke();
				particles?.Add(Type);
			}
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
