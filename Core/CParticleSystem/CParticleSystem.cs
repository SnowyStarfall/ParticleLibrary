using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// This class manages the particle system.
	/// </summary>
	public class CParticleSystem : ModSystem
	{
		/// <summary>
		/// A list that contains all active particles.
		/// </summary>
		public static IReadOnlyCollection<CParticle> Particles => Array.AsReadOnly(_particles.Buffer);
		internal static FastList<CParticle> _particles;
		internal static FastList<CParticle> _particlesToAdd;
		internal static FastList<CParticle> _particlesToRemove;

		public static int ParticleCount { get; internal set; }
		internal static double UpdateTime_InMilliseconds { get; private set; }

		public Rectangle ScreenLocation => new((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

		public override void OnModLoad()
		{
			_particles = new(ParticleLibraryConfig.Instance.MaxCPUParticles);
			_particlesToAdd = new();
			_particlesToRemove = new();

			On_Dust.UpdateDust += UpdateWorldParticles;
			On_Main.UpdateMenu += UpdateMenuParticles;
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

		private void UpdateWorldParticles(On_Dust.orig_UpdateDust orig)
		{
			Update();

			orig();
		}

		private void UpdateMenuParticles(On_Main.orig_UpdateMenu orig)
		{
			if (Main.gameMenu && Main.hasFocus)
				Update();

			orig();
		}

		private void Update()
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

					p.Rotation += p.RotationVelocity;
					p.RotationVelocity += p.RotationAcceleration;

					p.Scale2D.X += p.ScaleVelocity.X;
					p.Scale2D.Y += p.ScaleVelocity.Y;
					p.ScaleVelocity.X += p.ScaleAcceleration.X;
					p.ScaleVelocity.Y += p.ScaleAcceleration.Y;

					// TODO: Uncomment this
					//if (!UISystem.Instance.DebugUIElement.Instance.FreezeVelocity)
					{
						p.Position.X += p.Velocity.X;
						p.Position.Y += p.Velocity.Y;
					}

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

					// TODO: Uncomment this
					//if (!UISystem.Instance.DebugUIElement.Instance.FreezeAI)
					{
						p.Update();

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

				Rectangle r = (Rectangle)p.Bounds;
				ScreenLocation.Intersects(ref r, out bool intersects);
				if (p.Layer == layer && (p.Bounds is null || intersects))
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
		public static CParticle NewParticle<T>(Vector2 position, Vector2 velocity, Color color, float scale, Layer layer = Layer.BeforeDust) where T : CParticle
		{
			CParticle particle = Activator.CreateInstance<T>();
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
		public static CParticle NewParticle<T>(Vector2 position, Vector2 velocity, Color color, Vector2 scale, Layer layer = Layer.BeforeDust) where T : CParticle
		{
			CParticle particle = Activator.CreateInstance<T>();
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
		public static CParticle NewParticle(Vector2 position, Vector2 velocity, CParticle particle, Color color, float scale, Layer layer = Layer.BeforeDust)
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
		public static CParticle NewParticle(Vector2 position, Vector2 velocity, CParticle particle, Color color, Vector2 scale, Layer layer = Layer.BeforeDust)
		{
			if (particle is null)
				throw new ArgumentNullException(nameof(particle));

			if (ParticleCount >= ParticleLibraryConfig.Instance.MaxCPUParticles)
				return null;

			particle.Position = position;
			particle.Velocity = velocity;
			particle.Color = color;
			particle.Scale2D = scale;
			particle.Layer = layer;

			_particlesToAdd.Add(particle);
			ParticleCount++;

			return particle;
		}
	}
}
