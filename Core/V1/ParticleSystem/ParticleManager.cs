using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ParticleLibrary.Particle;

namespace ParticleLibrary
{
	/// <summary>
	/// This class manages the particle system. New instances of this class can be created.
	/// </summary>
	[Obsolete("This type is obsolete and WILL be removed in a future version")]
	public class ParticleManager : ModSystem
	{
		public delegate Particle NewParticleCreated(Vector2 Position, Vector2 Velocity, Particle Particle, Color Color, Vector2 Scale, float AI0 = 0, float AI1 = 0, float AI2 = 0, float AI3 = 0, float AI4 = 0, float AI5 = 0, float AI6 = 0, float AI7 = 0, Layer Layer = Layer.BeforeDust, bool Important = false);
		public static event NewParticleCreated OnNewParticle;

		/// <summary>
		/// A list that contains all active particles.
		/// </summary>
		public static List<Particle> particles;
		/// <summary>
		/// A list that contains all active important particles.
		/// Important particles are exempt fron the client-side particle limitation.
		/// Use only when absolutely necessary.
		/// </summary>
		public static List<Particle> importantParticles;

		public override void OnModLoad()
		{
			particles = new(ParticleLibraryConfig.Instance.MaxParticles);
			importantParticles = new();
			On_Dust.UpdateDust += UpdateParticles;
			Main.QueueMainThreadAction(Load_MainThread);
		}

		private void Load_MainThread()
		{
			//On.Terraria.Main.DrawSurfaceBG += DrawParticlesBeforeSurfaceBackground;
			On_Main.DoDraw_WallsAndBlacks += DrawParticlesBeforeWalls;
			On_Main.DoDraw_Tiles_NonSolid += DrawParticlesBeforeNonSolidTiles;
			On_Main.DrawPlayers_BehindNPCs += DrawParticlesBeforePlayersBehindNPCs;
			On_Main.DrawNPCs += DrawParticlesBeforeNPCs;
			On_Main.DoDraw_Tiles_Solid += DrawParticlesBeforeSolidTiles;
			On_Main.DrawProjectiles += DrawParticlesBeforeProjectiles;
			On_Main.DrawPlayers_AfterProjectiles += DrawParticlesBeforePlayers;
			On_Main.DrawItems += DrawParticlesBeforeItems;
			On_Main.DrawRain += DrawParticlesBeforeRain;
			On_Main.DrawGore += DrawParticlesBeforeGore;
			On_Main.DrawDust += DrawParticlesBeforeDust;
			On_Main.DrawWaters += DrawParticlesBeforeWater;
			On_Main.DrawInterface += DrawParticlesBeforeInterface;
			On_Main.DrawMenu += DrawParticlesBeforeAndAfterMainMenu;
		}

		public override void Unload()
		{
			particles.Clear();
			particles = null;
			importantParticles.Clear();
			importantParticles = null;
			OnNewParticle = null;
		}

		public override void OnWorldLoad()
		{
			Dispose();
		}

		public override void OnWorldUnload()
		{
			Dispose();
		}

		/// <summary>
		/// Disposes the current list of particles.
		/// </summary>
		public static void Dispose()
		{
			particles.Clear();
			importantParticles.Clear();
		}

		#region Updating
		private void UpdateParticles(Terraria.On_Dust.orig_UpdateDust orig)
		{
			UpdateParticles();
			orig();
		}
		private void UpdateParticles()
		{
			if (!Main.gamePaused)
			{
				PreUpdate();
				Update();
				PostUpdate();
			}
		}

		internal static void PreUpdate()
		{
			for (int i = 0; i < particles?.Count; i++)
			{
				//if (!UISystem.Instance.DebugUIElement.Instance.FreezeAI && !ParticleLibraryConfig.Instance.DebugUI)
				particles[i].PreAI();
				//if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
				//particles[i].velocity = Vector2.Zero;
			}
			for (int i = 0; i < importantParticles?.Count; i++)
			{
				//if (!UISystem.Instance.DebugUIElement.Instance.FreezeAI && !ParticleLibraryConfig.Instance.DebugUI)
				importantParticles[i].PreAI();
				//if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
				//importantParticles[i].velocity = Vector2.Zero;
			}
		}

		internal static void Update()
		{
			for (int i = 0; i < particles?.Count; i++)
			{
				Particle particle = particles[i];
				particle.oldDirection = particle.direction;

				particle.rotationVelocity += particle.rotationAcceleration;
				particle.rotation += particle.rotationVelocity;

				particle.scaleVelocity += particle.scaleAcceleration;
				particle.scale += particle.scaleVelocity;

				if (particle.tileCollide)
				{
					particle.velocity += particle.velocityAcceleration;
					particle.velocity.Y += particle.gravity;
					Vector2 oldVelocity = particle.velocity;
					if (Collision.SolidCollision(particle.position, particle.width, particle.height, true))
					{
						particle.velocity = Collision.TileCollision(particle.position, particle.velocity, particle.width, particle.height);
						particle.TileCollision(oldVelocity);
					}
					//if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
					//particles[i].velocity = Vector2.Zero;
					particle.position += particle.velocity;
					UpdateArrays(particle);
				}
				else
				{
					particle.velocity += particle.velocityAcceleration;
					particle.velocity.Y += particle.gravity;
					Vector2 oldVelocity = particle.velocity;
					if (Collision.SolidCollision(particle.position, particle.width, particle.height))
						particle.TileCollision(oldVelocity);
					//if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
					//particles[i].velocity = Vector2.Zero;

					particle.position += particle.velocity;
					UpdateArrays(particle);
				}

				particle.direction = particle.velocity.X >= 0f ? 1 : -1;
				particle.lavaWet = Collision.LavaCollision(particle.position, particle.width, particle.height);
				particle.wet = Collision.WetCollision(particle.position, particle.width, particle.height);

				//if ((!UISystem.Instance.DebugUIElement.Instance.FreezeAI && ParticleLibraryConfig.Instance.DebugUI) || !ParticleLibraryConfig.Instance.DebugUI)
				//{
				particle.AI();

				if (particle.timeLeft-- == 0 || !particles[i].active)
				{
					particle.DeathAction?.Invoke();
					particles.RemoveAt(i);
					i--;
				}
				//}
			}
			for (int i = 0; i < importantParticles?.Count; i++)
			{
				Particle particle = importantParticles[i];
				particle.oldDirection = particle.direction;

				particle.rotationVelocity += particle.rotationAcceleration;
				particle.rotation += particle.rotationVelocity;

				particle.scaleVelocity += particle.scaleAcceleration;
				particle.scale += particle.scaleVelocity;

				if (particle.tileCollide)
				{
					particle.velocity += particle.velocityAcceleration;
					particle.velocity.Y += particle.gravity;
					Vector2 oldVelocity = particle.velocity;
					if (Collision.SolidCollision(particle.position, particle.width, particle.height, true))
					{
						particle.velocity = Collision.TileCollision(particle.position, particle.velocity, particle.width, particle.height);
						particle.TileCollision(oldVelocity);
					}
					//if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
					//importantParticles[i].velocity = Vector2.Zero;
					particle.position += particle.velocity;
					UpdateArrays(particle);
				}
				else
				{
					particle.velocity += particle.velocityAcceleration;
					particle.velocity.Y += particle.gravity;
					Vector2 oldVelocity = particle.velocity;
					if (Collision.SolidCollision(particle.position, particle.width, particle.height))
						particle.TileCollision(oldVelocity);
					//if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
					//importantParticles[i].velocity = Vector2.Zero;
					particle.position += particle.velocity;
					UpdateArrays(particle);
				}

				particle.direction = particle.velocity.X >= 0f ? 1 : -1;
				particle.lavaWet = Collision.LavaCollision(particle.position, particle.width, particle.height);
				particle.wet = Collision.WetCollision(particle.position, particle.width, particle.height);

				//if ((!UISystem.Instance.DebugUIElement.Instance.FreezeAI && ParticleLibraryConfig.Instance.DebugUI) || !ParticleLibraryConfig.Instance.DebugUI)
				//{
				particle.AI();

				if (particle.timeLeft-- == 0 || !importantParticles[i].active)
				{
					particle.DeathAction?.Invoke();
					importantParticles.RemoveAt(i);
					i--;
				}
				//}
			}
		}

		internal static void PostUpdate()
		{
			for (int i = 0; i < particles?.Count; i++)
			{
				//if (!UISystem.Instance.DebugUIElement.Instance.FreezeAI && !ParticleLibraryConfig.Instance.DebugUI)
				particles[i].PostAI();
				//if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
				//particles[i].velocity = Vector2.Zero;
			}
			for (int i = 0; i < importantParticles?.Count; i++)
			{
				//if (!UISystem.Instance.DebugUIElement.Instance.FreezeAI && !ParticleLibraryConfig.Instance.DebugUI)
				importantParticles[i].PostAI();
				//if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
				//importantParticles[i].velocity = Vector2.Zero;
			}
		}
		#endregion

		#region Drawing
		//private void DrawParticlesBeforeSurfaceBackground(On.Terraria.Main.orig_DrawSurfaceBG orig, Main self)
		//{
		//	Main.spriteBatch.End();
		//	Draw(x => x.layer == Layer.BeforeSurfaceBackground);
		//	Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null);
		//	orig(self);
		//}
		private void DrawParticlesBeforeWalls(On_Main.orig_DoDraw_WallsAndBlacks orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeWalls);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			orig(self);
		}

		private void DrawParticlesBeforeNonSolidTiles(On_Main.orig_DoDraw_Tiles_NonSolid orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeNonSolidTiles);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void DrawParticlesBeforeSolidTiles(On_Main.orig_DoDraw_Tiles_Solid orig, Main self)
		{
			Draw(Layer.BeforeTiles);

			orig(self);
		}

		private void DrawParticlesBeforePlayersBehindNPCs(On_Main.orig_DrawPlayers_BehindNPCs orig, Main self)
		{
			Draw(Layer.BeforePlayersBehindNPCs);
			orig(self);
		}

		private void DrawParticlesBeforeNPCs(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
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

		private void DrawParticlesBeforeProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
		{
			Draw(Layer.BeforeProjectiles);
			orig(self);
		}

		private void DrawParticlesBeforePlayers(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
		{
			Draw(Layer.BeforePlayers);
			orig(self);
		}

		private void DrawParticlesBeforeItems(On_Main.orig_DrawItems orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeItems);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			orig(self);
		}

		private void DrawParticlesBeforeRain(On_Main.orig_DrawRain orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeRain);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			orig(self);
		}

		private void DrawParticlesBeforeGore(On_Main.orig_DrawGore orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeGore);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			orig(self);
		}

		private void DrawParticlesBeforeDust(On_Main.orig_DrawDust orig, Main self)
		{
			Draw(Layer.BeforeDust);
			orig(self);
		}

		private void DrawParticlesBeforeWater(On_Main.orig_DrawWaters orig, Main self, bool isBackground)
		{
			Main.spriteBatch.End();
			Draw(Layer.BeforeWater);
			Main.spriteBatch.Begin();
			orig(self, isBackground);
		}

		private void DrawParticlesBeforeInterface(On_Main.orig_DrawInterface orig, Main self, GameTime gameTime)
		{
			Draw(Layer.BeforeUI);
			orig(self, gameTime);
			Draw(Layer.AfterUI);
		}

		private void DrawParticlesBeforeAndAfterMainMenu(On_Main.orig_DrawMenu orig, Main self, GameTime gameTime)
		{
			if (Main.gameMenu && Main.hasFocus)
				UpdateParticles();

			Main.spriteBatch.End();
			Draw(Layer.BeforeMainMenu);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

			orig(self, gameTime);
			Draw(Layer.AfterMainMenu);
		}

		private void Draw(Layer layer)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < particles?.Count; i++)
				{
					Particle particle = particles[i];
					if (particle != null && particle.layer == layer)
					{
						Vector2 offset = particle.layer == Layer.BeforeWater ? new Vector2(Main.offScreenRange, Main.offScreenRange) : Vector2.Zero;
						Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
						if (particle.PreDraw(Main.spriteBatch, particle.VisualPosition + offset, Lighting.GetColor((int)(particle.position.X / 16), (int)(particle.position.Y / 16))))
							particle.Draw(Main.spriteBatch, particle.VisualPosition + offset, Lighting.GetColor((int)(particle.position.X / 16), (int)(particle.position.Y / 16)));
						particle.PostDraw(Main.spriteBatch, particle.VisualPosition + offset, Lighting.GetColor((int)(particle.position.X / 16), (int)(particle.position.Y / 16)));
						Main.spriteBatch.End();
					}
				}
				for (int i = 0; i < importantParticles?.Count; i++)
				{
					Particle particle = importantParticles[i];
					if (particle != null && particle.layer == layer)
					{
						Vector2 offset = particle.layer == Layer.BeforeWater ? new Vector2(Main.offScreenRange, Main.offScreenRange) : Vector2.Zero;
						Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
						if (particle.PreDraw(Main.spriteBatch, particle.VisualPosition + offset, Lighting.GetColor((int)(particle.position.X / 16), (int)(particle.position.Y / 16))))
							particle.Draw(Main.spriteBatch, particle.VisualPosition + offset, Lighting.GetColor((int)(particle.position.X / 16), (int)(particle.position.Y / 16)));
						particle.PostDraw(Main.spriteBatch, particle.VisualPosition + offset, Lighting.GetColor((int)(particle.position.X / 16), (int)(particle.position.Y / 16)));
						Main.spriteBatch.End();
					}
				}
			}
		}
		#endregion

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
		/// <param name="Layer">When the particle is drawn.</param>
		/// <param name="Important">Whether the particle should ignore the particle limit.</param>
		/// <exception cref="NullReferenceException"></exception>
		public static Particle NewParticle<T>(Vector2 Position, Vector2 Velocity, Color Color, float Scale, float AI0 = 0, float AI1 = 0, float AI2 = 0, float AI3 = 0, float AI4 = 0, float AI5 = 0, float AI6 = 0, float AI7 = 0, Layer Layer = Layer.BeforeDust, bool Important = false) where T : Particle
		{
			Particle Particle = Activator.CreateInstance<T>();
			return NewParticle(Position, Velocity, Particle, Color, new Vector2(Scale), AI0, AI1, AI2, AI3, AI4, AI5, AI6, AI7, Layer, Important);
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
		/// <param name="Layer">When the particle is drawn.</param>
		/// <param name="Important">Whether the particle should ignore the particle limit.</param>
		/// <exception cref="NullReferenceException"></exception>
		public static Particle NewParticle<T>(Vector2 Position, Vector2 Velocity, Color Color, Vector2 Scale, float AI0 = 0, float AI1 = 0, float AI2 = 0, float AI3 = 0, float AI4 = 0, float AI5 = 0, float AI6 = 0, float AI7 = 0, Layer Layer = Layer.BeforeDust, bool Important = false) where T : Particle
		{
			Particle Particle = Activator.CreateInstance<T>();
			return NewParticle(Position, Velocity, Particle, Color, Scale, AI0, AI1, AI2, AI3, AI4, AI5, AI6, AI7, Layer, Important);
		}

		/// <summary>
		/// Spawns a new particle at the desired position.
		/// </summary>
		/// <param name="Position">The particle's position.</param>
		/// <param name="Velocity">The particle's velocity.</param>
		/// <param name="Particle">The particle type.</param>
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
		/// <param name="Layer">When the particle is drawn.</param>
		/// <param name="Important">Whether the particle should ignore the particle limit.</param>
		/// <exception cref="NullReferenceException"></exception>
		public static Particle NewParticle(Vector2 Position, Vector2 Velocity, Particle Particle, Color Color, float Scale, float AI0 = 0, float AI1 = 0, float AI2 = 0, float AI3 = 0, float AI4 = 0, float AI5 = 0, float AI6 = 0, float AI7 = 0, Layer Layer = Layer.BeforeDust, bool Important = false)
		{
			return NewParticle(Position, Velocity, Particle, Color, new Vector2(Scale), AI0, AI1, AI2, AI3, AI4, AI5, AI6, AI7, Layer, Important);
		}

		/// <summary>
		/// Spawns a new particle at the desired position.
		/// </summary>
		/// <param name="Position">The particle's position.</param>
		/// <param name="Velocity">The particle's velocity.</param>
		/// <param name="Particle">The particle type.</param>
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
		/// <param name="Layer">When the particle is drawn.</param>
		/// <param name="Important">Whether the particle should ignore the particle limit.</param>
		/// <exception cref="NullReferenceException"></exception>
		public static Particle NewParticle(Vector2 Position, Vector2 Velocity, Particle Particle, Color Color, Vector2 Scale, float AI0 = 0, float AI1 = 0, float AI2 = 0, float AI3 = 0, float AI4 = 0, float AI5 = 0, float AI6 = 0, float AI7 = 0, Layer Layer = Layer.BeforeDust, bool Important = false)
		{
			Particle ??= (Particle)Activator.CreateInstance(Particle.GetType());

			if (!Important && particles?.Count > ParticleLibraryConfig.Instance.MaxParticles)
				particles.TrimExcess();
			if (!Important && particles?.Count == ParticleLibraryConfig.Instance.MaxParticles)
				return null;

			Particle.position = Position;
			Particle.velocity = Velocity;
			Particle.color = Color;
			Particle.scale = Scale;
			Particle.active = true;
			Particle.ai = new float[] { AI0, AI1, AI2, AI3, AI4, AI5, AI6, AI7 };
			Particle.layer = Layer;
			Particle.important = Important;
			Particle.SpawnAction?.Invoke();

			if (Important)
				importantParticles?.Add(Particle);
			else
				particles?.Add(Particle);

			OnNewParticle?.Invoke(Position, Velocity, Particle, Color, Scale, AI0, AI1, AI2, AI3, AI4, AI5, AI6, AI7, Layer, Important);
			return Particle;
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
