
using log4net.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Debug;
using ParticleLibrary.ParticleSystem;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ParticleLibrary.Particle;
using static System.Formats.Asn1.AsnWriter;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace ParticleLibrary
{
	/// <summary>
	/// This class manages the particle system. New instances of this class can be created.
	/// </summary>
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
			On.Terraria.Dust.UpdateDust += UpdateParticles;
			//On.Terraria.Main.DrawSurfaceBG += DrawParticlesBeforeSurfaceBackground;
			On.Terraria.Main.DoDraw_WallsAndBlacks += DrawParticlesBeforeWalls;
			On.Terraria.Main.DoDraw_Tiles_NonSolid += DrawParticlesBeforeNonSolidTiles;
			On.Terraria.Main.DrawPlayers_BehindNPCs += DrawParticlesBeforePlayersBehindNPCs;
			On.Terraria.Main.DrawNPCs += DrawParticlesBeforeNPCs;
			On.Terraria.Main.DoDraw_Tiles_Solid += DrawParticlesBeforeSolidTiles;
			On.Terraria.Main.DrawProjectiles += DrawParticlesBeforeProjectiles;
			On.Terraria.Main.DrawPlayers_AfterProjectiles += DrawParticlesBeforePlayers;
			On.Terraria.Main.DrawItems += DrawParticlesBeforeItems;
			On.Terraria.Main.DrawRain += DrawParticlesBeforeRain;
			On.Terraria.Main.DrawGore += DrawParticlesBeforeGore;
			On.Terraria.Main.DrawDust += DrawParticlesBeforeDust;
			On.Terraria.Main.DrawWater += DrawParticlesBeforeWater;
			On.Terraria.Main.DrawInterface += DrawParticlesBeforeInterface;
			On.Terraria.Main.DrawMenu += DrawParticlesBeforeAndAfterMainMenu;
		}

		public override void Unload()
		{
			particles = null;
			importantParticles = null;
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
		private void UpdateParticles(On.Terraria.Dust.orig_UpdateDust orig)
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
				if (!UISystem.Instance.DebugUIElement.Instance.FreezeAI && !ParticleLibraryConfig.Instance.DebugUI)
					particles[i].PreAI();
				if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
					particles[i].velocity = Vector2.Zero;
			}
			for (int i = 0; i < importantParticles?.Count; i++)
			{
				if (!UISystem.Instance.DebugUIElement.Instance.FreezeAI && !ParticleLibraryConfig.Instance.DebugUI)
					importantParticles[i].PreAI();
				if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
					importantParticles[i].velocity = Vector2.Zero;
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
					if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
						particles[i].velocity = Vector2.Zero;
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
					if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
						particles[i].velocity = Vector2.Zero;
					particle.position += particle.velocity;
					UpdateArrays(particle);
				}

				particle.direction = particle.velocity.X >= 0f ? 1 : -1;
				particle.lavaWet = Collision.LavaCollision(particle.position, particle.width, particle.height);
				particle.wet = Collision.WetCollision(particle.position, particle.width, particle.height);

				if ((!UISystem.Instance.DebugUIElement.Instance.FreezeAI && ParticleLibraryConfig.Instance.DebugUI) || !ParticleLibraryConfig.Instance.DebugUI)
				{
					particle.AI();

					if (particle.timeLeft-- == 0 || !particles[i].active)
					{
						particle.DeathAction?.Invoke();
						particles.RemoveAt(i);
						i--;
					}
				}
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
					if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
						importantParticles[i].velocity = Vector2.Zero;
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
					if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
						importantParticles[i].velocity = Vector2.Zero;
					particle.position += particle.velocity;
					UpdateArrays(particle);
				}

				particle.direction = particle.velocity.X >= 0f ? 1 : -1;
				particle.lavaWet = Collision.LavaCollision(particle.position, particle.width, particle.height);
				particle.wet = Collision.WetCollision(particle.position, particle.width, particle.height);

				if ((!UISystem.Instance.DebugUIElement.Instance.FreezeAI && ParticleLibraryConfig.Instance.DebugUI) || !ParticleLibraryConfig.Instance.DebugUI)
				{
					particle.AI();

					if (particle.timeLeft-- == 0 || !importantParticles[i].active)
					{
						particle.DeathAction?.Invoke();
						importantParticles.RemoveAt(i);
						i--;
					}
				}
			}
		}

		internal static void PostUpdate()
		{
			for (int i = 0; i < particles?.Count; i++)
			{
				if (!UISystem.Instance.DebugUIElement.Instance.FreezeAI && !ParticleLibraryConfig.Instance.DebugUI)
					particles[i].PostAI();
				if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
					particles[i].velocity = Vector2.Zero;
			}
			for (int i = 0; i < importantParticles?.Count; i++)
			{
				if (!UISystem.Instance.DebugUIElement.Instance.FreezeAI && !ParticleLibraryConfig.Instance.DebugUI)
					importantParticles[i].PostAI();
				if (UISystem.Instance.DebugUIElement.Instance.FreezeVelocity && ParticleLibraryConfig.Instance.DebugUI)
					importantParticles[i].velocity = Vector2.Zero;
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
		private void DrawParticlesBeforeWalls(On.Terraria.Main.orig_DoDraw_WallsAndBlacks orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(x => x.layer == Layer.BeforeWalls);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			orig(self);
		}
		private void DrawParticlesBeforeNonSolidTiles(On.Terraria.Main.orig_DoDraw_Tiles_NonSolid orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(x => x.layer == Layer.BeforeNonSolidTiles);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}
		private void DrawParticlesBeforeSolidTiles(On.Terraria.Main.orig_DoDraw_Tiles_Solid orig, Main self)
		{
			Draw(x => x.layer == Layer.BeforeTiles);

			orig(self);
		}
		private void DrawParticlesBeforePlayersBehindNPCs(On.Terraria.Main.orig_DrawPlayers_BehindNPCs orig, Main self)
		{
			Draw(x => x.layer == Layer.BeforePlayersBehindNPCs);
			orig(self);
		}
		private void DrawParticlesBeforeNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
		{
			if (behindTiles)
			{
				Main.spriteBatch.End();
				Draw(x => x.layer == Layer.BeforeNPCsBehindTiles);
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			}
			else
			{
				Main.spriteBatch.End();
				Draw(x => x.layer == Layer.BeforeNPCs);
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			}
			orig(self, behindTiles);
		}
		private void DrawParticlesBeforeProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			Draw(x => x.layer == Layer.BeforeProjectiles);
			orig(self);
		}
		private void DrawParticlesBeforePlayers(On.Terraria.Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
		{
			Draw(x => x.layer == Layer.BeforePlayers);
			orig(self);
		}
		private void DrawParticlesBeforeItems(On.Terraria.Main.orig_DrawItems orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(x => x.layer == Layer.BeforeItems);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			orig(self);
		}
		private void DrawParticlesBeforeRain(On.Terraria.Main.orig_DrawRain orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(x => x.layer == Layer.BeforeRain);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			orig(self);
		}
		private void DrawParticlesBeforeGore(On.Terraria.Main.orig_DrawGore orig, Main self)
		{
			Main.spriteBatch.End();
			Draw(x => x.layer == Layer.BeforeGore);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			orig(self);
		}
		private void DrawParticlesBeforeDust(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			Draw(x => x.layer == Layer.BeforeDust);
			orig(self);
		}
		private void DrawParticlesBeforeWater(On.Terraria.Main.orig_DrawWater orig, Main self, bool bg, int Style, float Alpha)
		{
			Main.spriteBatch.End();
			Draw(x => x.layer == Layer.BeforeWater);
			Main.spriteBatch.Begin();
			orig(self, bg, Style, Alpha);
		}
		private void DrawParticlesBeforeInterface(On.Terraria.Main.orig_DrawInterface orig, Main self, GameTime gameTime)
		{
			Draw(x => x.layer == Layer.BeforeUI);
			orig(self, gameTime);
			Draw(x => x.layer == Layer.AfterUI);
		}
		private void DrawParticlesBeforeAndAfterMainMenu(On.Terraria.Main.orig_DrawMenu orig, Main self, GameTime gameTime)
		{
			if (Main.gameMenu && Main.hasFocus)
				UpdateParticles();

			Main.spriteBatch.End();
			Draw(x => x.layer == Layer.BeforeMainMenu);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

			orig(self, gameTime);
			Draw(x => x.layer == Layer.AfterMainMenu);
		}
		private void Draw(Predicate<Particle> predicate)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < particles?.Count; i++)
				{
					Particle particle = particles[i];
					if (particle != null && predicate.Invoke(particle))
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
					if (particle != null && predicate.Invoke(particle))
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
			Particle type = (Particle)Activator.CreateInstance(Particle.GetType());

			if (!Important && particles?.Count > ParticleLibraryConfig.Instance.MaxParticles)
				particles.TrimExcess();
			if (!Important && particles?.Count == ParticleLibraryConfig.Instance.MaxParticles)
				return null;

			type.position = Position;
			type.velocity = Velocity;
			type.color = Color;
			type.scale = Scale;
			type.active = true;
			type.ai = new float[] { AI0, AI1, AI2, AI3, AI4, AI5, AI6, AI7 };
			type.layer = Layer;
			type.important = Important;
			type.SpawnAction?.Invoke();

			if (Important)
				importantParticles?.Add(type);
			else
				particles?.Add(type);

			OnNewParticle?.Invoke(Position, Velocity, type, Color, Scale, AI0, AI1, AI2, AI3, AI4, AI5, AI6, AI7, Layer, Important);
			return type;
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
