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
	/// This class manages the CPU particle system.
	/// </summary>
	public class NewParticleManager : ModSystem
	{
		/// <summary>
		/// A list that contains all active particles.
		/// </summary>
		public static IReadOnlyCollection<Particle> Particles => Array.AsReadOnly(_particles.Buffer);
		internal static FastList<Particle> _particles;
		internal static FastList<Particle> _particlesToAdd;
		internal static FastList<Particle> _particlesToRemove;

		/// <summary>
		/// The amount of particles currently maintained by the system.
		/// </summary>
		public static int ParticleCount { get; internal set; }
		internal static double UpdateTime_InMilliseconds { get; private set; }

		/// <summary>
		/// Shorthand for screen location rectangle.
		/// </summary>
		public Rectangle ScreenLocation => new((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

		public override void OnModLoad()
		{
			_particles = new(ParticleLibraryConfig.Instance.MaxParticles);
			_particlesToAdd = new();
			_particlesToRemove = new();

			DrawHooks.OnUpdateDust += UpdateWorldParticles;
			DrawHooks.OnUpdateMenu += UpdateMenuParticles;
			Main.QueueMainThreadAction(() =>
			{
				DrawHooks.OnDraw_BeforeBackground += Draw;
				DrawHooks.OnDraw_BeforeWalls += Draw;
				DrawHooks.OnDraw_BeforeNonSolidTiles += Draw;
				DrawHooks.OnDraw_BeforePlayersBehindNPCs += Draw;
				DrawHooks.OnDraw_BeforeNPCs += Draw;
				DrawHooks.OnDraw_BeforeSolidTiles += Draw;
				DrawHooks.OnDraw_BeforeProjectiles += Draw;
				DrawHooks.OnDraw_BeforePlayers += Draw;
				DrawHooks.OnDraw_BeforeItems += Draw;
				DrawHooks.OnDraw_BeforeRain += Draw;
				DrawHooks.OnDraw_BeforeGore += Draw;
				DrawHooks.OnDraw_BeforeDust += Draw;
				DrawHooks.OnDraw_BeforeWater += Draw;
				DrawHooks.OnDraw_BeforeInterface += Draw;
				DrawHooks.OnDraw_AfterInterface += Draw;
				DrawHooks.OnDraw_BeforeMainMenu += Draw;
				DrawHooks.OnDraw_AfterMainMenu += Draw;
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

		private void UpdateWorldParticles()
		{
			Update();
		}

		private void UpdateMenuParticles()
		{
			if (Main.gameMenu)
				Update();
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
				//Stopwatch s = Stopwatch.StartNew();

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
					//{
					p.Position.X += p.Velocity.X;
					p.Position.Y += p.Velocity.Y;
					//}

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
					//{
					p.Update();

					if (--p.TimeLeft == 0)
					{
						p.Death();
						_particlesToRemove.Add(p);
						ParticleCount--;
					}
					//}
				}

				//s.Stop();

				//UpdateTime_InMilliseconds = s.Elapsed.TotalMilliseconds;
			}
		}

		private void Draw(Layer layer)
		{
			if (Main.netMode == NetmodeID.Server)
				return;

			Rectangle previousScissor = Main.graphics.GraphicsDevice.ScissorRectangle;
			RasterizerState previousRasterizer = Main.graphics.GraphicsDevice.RasterizerState;

			Main.graphics.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
			Main.graphics.GraphicsDevice.RasterizerState = LibUtilities.OverflowHiddenRasterizerState;

			Matrix matrix;

			// Compensate for matrix on background layer
			if (layer is Layer.BeforeBackground)
			{
				matrix = Main.BackgroundViewMatrix.TransformationMatrix;
				matrix.Translation -= Main.BackgroundViewMatrix.ZoomMatrix.Translation * new Vector3(1f, Main.BackgroundViewMatrix.Effects.HasFlag(SpriteEffects.FlipVertically) ? (-1f) : 1f, 1f);
			}
			else
			{
				matrix = Main.GameViewMatrix.TransformationMatrix;
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, matrix);

			foreach (var p in _particles.Buffer)
			{
				if (p is null || p.Layer != layer)
				{
					continue;
				}

				Rectangle r = (Rectangle)p.Bounds;
				ScreenLocation.Intersects(ref r, out bool intersects);
				if (!(p.Bounds is null || intersects))
				{
					continue;
				}

				// Compensate for offset on water layer
				Vector2 position = p.Layer == Layer.BeforeWater ? p.VisualPosition + new Vector2(Main.offScreenRange) : p.VisualPosition;
				p.Draw(Main.spriteBatch, position);
			}

			Main.spriteBatch.End();

			Main.graphics.GraphicsDevice.ScissorRectangle = previousScissor;
			Main.graphics.GraphicsDevice.RasterizerState = previousRasterizer;
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

			return particle;
		}
	}
}
