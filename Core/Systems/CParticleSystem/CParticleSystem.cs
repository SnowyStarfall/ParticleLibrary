using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core.Systems;
using ParticleLibrary.Debug;
using ParticleLibrary.Utilities;
using ReLogic.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ParticleLibrary.Core.Systems.Particle;

namespace ParticleLibrary.Core.Systems
{
	/// <summary>
	/// This class manages the particle system. New instances of this class can be created.
	/// </summary>
	public class CParticleSystem : ModSystem
	{
		/// <summary>
		/// A list that contains all active particles.
		/// </summary>
		public static IReadOnlyCollection<Particle> Particles => Array.AsReadOnly(_particles.Buffer);
		internal static FastList<Particle> _particles;
		internal static FastList<Particle> _particlesToAdd;
		internal static FastList<Particle> _particlesToRemove;

		public static int ParticleCount { get; internal set; }
		internal static double UpdateTime_InMilliseconds { get; private set; }

		public override void OnModLoad()
		{
			_particles = new(ParticleLibraryConfig.Instance.MaxParticles);
			_particlesToAdd = new();
			_particlesToRemove = new();

			On_Dust.UpdateDust += UpdateParticles;
			Main.QueueMainThreadAction(() =>
			{
				//On.Terraria.Main.DrawSurfaceBG += DrawParticles_BeforeSurfaceBackground;
				On_Main.DoDraw_WallsAndBlacks += DrawParticles_BeforeWalls;
				On_Main.DoDraw_Tiles_NonSolid += DrawParticles_BeforeNonSolidTiles;
				On_Main.DrawPlayers_BehindNPCs += DrawParticles_BeforePlayersBehindNPCs;
				On_Main.DrawNPCs += DrawParticles_BeforeNPCs;
				On_Main.DoDraw_Tiles_Solid += DrawParticles_BeforeSolidTiles;
				On_Main.DrawProjectiles += DrawParticles_BeforeProjectiles;
				On_Main.DrawPlayers_AfterProjectiles += DrawParticles_BeforePlayers;
				On_Main.DrawItems += DrawParticles_BeforeItems;
				On_Main.DrawRain += DrawParticles_BeforeRain;
				On_Main.DrawGore += DrawParticles_BeforeGore;
				On_Main.DrawDust += DrawParticles_BeforeDust;
				On_Main.DrawWaters += DrawParticles_BeforeWater;
				On_Main.DrawInterface += DrawParticles_BeforeInterface;
				On_Main.DrawMenu += DrawParticles_BeforeAndAfterMainMenu;
			});
		}

		public override void Unload()
		{
			_particles = null;
			_particlesToAdd = null;
			_particlesToRemove = null;
		}

		public override void OnWorldLoad()
		{
			Dispose();
		}

		public override void OnWorldUnload()
		{
			Dispose();
		}

		internal void Dispose()
		{
			_particles.Clear();
			_particlesToAdd.Clear();
			_particlesToRemove.Clear();
		}

		private void UpdateParticles(On_Dust.orig_UpdateDust orig)
		{
			if (!Main.gamePaused)
			{
				// Add particles in batch
				foreach (var p in _particlesToRemove.Buffer)
				{
					if (p is null)
						continue;

					_particles.Remove(p);
				}
				_particlesToRemove.Clear();

				// Remove particles in batch
				foreach (var p in _particlesToAdd.Buffer)
				{
					if (p is null)
						continue;

					_particles.Add(p);
				}
				_particlesToAdd.Clear();


				// Update particles in batch
				Stopwatch s = Stopwatch.StartNew();

				foreach (var p in _particles.Buffer)
				{
					if (p is null)
						continue;

					p.RotationVelocity += p.RotationAcceleration;
					p.Rotation += p.RotationVelocity;

					p.ScaleVelocity.X += p.ScaleAcceleration.X;
					p.ScaleVelocity.Y += p.ScaleAcceleration.Y;
					p.Scale2D.X += p.ScaleVelocity.X;
					p.Scale2D.Y += p.ScaleVelocity.Y;

					if (p.TileCollide)
					{
						p.Velocity.X += p.VelocityAcceleration.X;
						p.Velocity.Y += p.VelocityAcceleration.Y;

						Vector2 oldVelocity = p.Velocity;
						p.Velocity = Collision.TileCollision(p.Position, p.Velocity, 1, 1);
						if (p.Velocity != oldVelocity)
							p.TileCollision(oldVelocity);

						p.Velocity = Vector2.Zero;
					}

					//if (!UISystem.Instance.DebugUIElement.Instance.FreezeVelocity)
					{
						p.Position.X += p.Velocity.X;
						p.Position.Y += p.Velocity.Y;
					}

					//if (!UISystem.Instance.DebugUIElement.Instance.FreezeAI)
					{
						p.AI();

						if (--p.TimeLeft == 0)
						{
							p.Death();
							_particlesToRemove.Add(p);
							ParticleCount--;
						}
					}
				}

				s.Stop();

				UpdateTime_InMilliseconds = s.Elapsed.TotalMilliseconds;
			}

			orig();
		}

		private void DrawParticles_BeforeWalls(On_Main.orig_DoDraw_WallsAndBlacks orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeWalls);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticles_BeforeNonSolidTiles(On_Main.orig_DoDraw_Tiles_NonSolid orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeNonSolidTiles);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticles_BeforeSolidTiles(On_Main.orig_DoDraw_Tiles_Solid orig, Main self)
		{
			Draw(Layer.BeforeTiles);

			orig(self);
		}

		private void DrawParticles_BeforePlayersBehindNPCs(On_Main.orig_DrawPlayers_BehindNPCs orig, Main self)
		{
			Draw(Layer.BeforePlayersBehindNPCs);

			orig(self);
		}

		private void DrawParticles_BeforeNPCs(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
		{
			if (behindTiles)
			{
				Main.spriteBatch.End();
				Draw(Layer.BeforeNPCsBehindTiles);
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			}
			else
			{
				Main.spriteBatch.End();
				Draw(Layer.BeforeNPCs);
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			}

			orig(self, behindTiles);
		}

		private void DrawParticles_BeforeProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
		{
			Draw(Layer.BeforeProjectiles);

			orig(self);
		}

		private void DrawParticles_BeforePlayers(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
		{
			Draw(Layer.BeforePlayers);

			orig(self);
		}

		private void DrawParticles_BeforeItems(On_Main.orig_DrawItems orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeItems);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticles_BeforeRain(On_Main.orig_DrawRain orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeRain);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticles_BeforeGore(On_Main.orig_DrawGore orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeGore);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticles_BeforeDust(On_Main.orig_DrawDust orig, Main self)
		{
			Draw(Layer.BeforeDust);

			orig(self);
		}

		private void DrawParticles_BeforeWater(On_Main.orig_DrawWaters orig, Main self, bool isBackground)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeWater);
			Main.spriteBatch.Begin();

			orig(self, isBackground);
		}

		private void DrawParticles_BeforeInterface(On_Main.orig_DrawInterface orig, Main self, GameTime gameTime)
		{
			Draw(Layer.BeforeUI);

			orig(self, gameTime);

			Draw(Layer.AfterUI);
		}

		private void DrawParticles_BeforeAndAfterMainMenu(On_Main.orig_DrawMenu orig, Main self, GameTime gameTime)
		{
			// TODO: Move this
			//if (Main.gameMenu && Main.hasFocus)
			//	UpdateParticles();

			Main.spriteBatch.End();
			Draw(Layer.BeforeMainMenu);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

			orig(self, gameTime);
			Draw(Layer.AfterMainMenu);
		}

		private void Draw(Layer layer)
		{
			if (Main.netMode == NetmodeID.Server)
				return;

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			foreach (var p in _particles.Buffer)
			{
				if (p is null)
					continue;

				if (p.Layer == layer)
				{
					Vector2 offset = p.Layer == Layer.BeforeWater ? new Vector2(Main.offScreenRange, Main.offScreenRange) : Vector2.Zero;
					p.Draw(Main.spriteBatch, p.VisualPosition + offset);
				}
			}

			Main.spriteBatch.End();
		}

		/// <summary>
		/// Spawns a new particle at the desired position.
		/// </summary>
		/// <param name="position">The particle's position.</param>
		/// <param name="velocity">The particle's velocity.</param>
		/// <param name="color">The particle's color.</param>
		/// <param name="scale">The particle's size.</param>
		/// <param name="layer">When the particle is drawn.</param>
		/// <exception cref="NullReferenceException"></exception>
		public static Particle NewParticle<T>(Vector2 position, Vector2 velocity, Color color, float scale, Layer layer = Layer.BeforeDust) where T : Particle
		{
			Particle particle = Activator.CreateInstance<T>();
			return NewParticle(position, velocity, particle, color, new Vector2(scale), layer);
		}

		/// <summary>
		/// Spawns a new particle at the desired position.
		/// </summary>
		/// <param name="position">The particle's position.</param>
		/// <param name="velocity">The particle's velocity.</param>
		/// <param name="color">The particle's color.</param>
		/// <param name="scale">The particle's size.</param>
		/// <param name="layer">When the particle is drawn.</param>
		/// <exception cref="NullReferenceException"></exception>
		public static Particle NewParticle<T>(Vector2 position, Vector2 velocity, Color color, Vector2 scale, Layer layer = Layer.BeforeDust) where T : Particle
		{
			Particle particle = Activator.CreateInstance<T>();
			return NewParticle(position, velocity, particle, color, scale, layer);
		}

		/// <summary>
		/// Spawns a new particle at the desired position.
		/// </summary>
		/// <param name="position">The particle's position.</param>
		/// <param name="velocity">The particle's velocity.</param>
		/// <param name="particle">The particle type.</param>
		/// <param name="color">The particle's color.</param>
		/// <param name="scale">The particle's size.</param>
		/// <param name="layer">When the particle is drawn.</param>
		/// <exception cref="NullReferenceException"></exception>
		public static Particle NewParticle(Vector2 position, Vector2 velocity, Particle particle, Color color, float scale, Layer layer = Layer.BeforeDust)
		{
			return NewParticle(position, velocity, particle, color, new Vector2(scale), layer);
		}

		/// <summary>
		/// Spawns a new particle at the desired position.
		/// </summary>
		/// <param name="position">The particle's position.</param>
		/// <param name="velocity">The particle's velocity.</param>
		/// <param name="particle">The particle type.</param>
		/// <param name="color">The particle's color.</param>
		/// <param name="scale">The particle's size.</param>
		/// <param name="layer">When the particle is drawn.</param>
		/// <exception cref="NullReferenceException"></exception>
		public static Particle NewParticle(Vector2 position, Vector2 velocity, Particle particle, Color color, Vector2 scale, Layer layer = Layer.BeforeDust)
		{
			if (particle is null)
				throw new ArgumentNullException(nameof(particle));

			if (ParticleCount >= ParticleLibraryConfig.Instance.MaxParticles)
				return null;

			particle.Position = position;
			particle.Velocity = velocity;
			particle.Color = color;
			particle.Scale2D = scale;
			particle.Layer = layer;

			_particlesToAdd.Add(particle);
			ParticleCount++;
			particle.Spawn();

			return particle;
		}
	}
}
