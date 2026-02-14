using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary.Core
{
	// TODO: Add sample code showcasing how to add parallax
	/// <summary>
	/// This class can be used by any mod to draw at any position in the draw order.
	/// Some events have special requirements to draw correctly such as <see cref="OnDraw_BeforeBackground"/> and <see cref="OnDraw_BeforeWater"/>.
	/// 
	/// <para>
	/// For <see cref="OnDraw_BeforeBackground"/>, initialize your SpriteBatch with this Matrix.
	/// <code>
	/// Matrix matrix = Main.BackgroundViewMatrix.TransformationMatrix;
	///	matrix.Translation -= Main.BackgroundViewMatrix.ZoomMatrix.Translation * new Vector3(1f, Main.BackgroundViewMatrix.Effects.HasFlag(SpriteEffects.FlipVertically)? (-1f) : 1f, 1f);
	/// </code>
	/// Your drawing will not manually parallax. You will need to do this yourself for now.
	/// </para>
	/// 
	/// <para>
	/// For <see cref="OnDraw_BeforeWater"/>, ADD <see cref="Main.offScreenRange"/> to your draw position.
	/// </para>
	/// </summary>
	public class DrawHooks : ModSystem
	{
		public delegate void Update();
		public static event Update OnUpdateDust;
		public static event Update OnUpdateMenu;

		public delegate void Draw(Layer layer);
		/// <summary>
		/// Before background.
		/// </summary>
		public static event Draw OnDraw_BeforeBackground;
		/// <summary>
		/// After backgroumd
		/// </summary>
		public static event Draw OnDraw_BeforeWalls;
		/// <summary>
		/// After walls
		/// </summary>
		public static event Draw OnDraw_BeforeNonSolidTiles;
		/// <summary>
		/// After non-solid tiles
		/// </summary>
		public static event Draw OnDraw_BeforeNPCsBehindTiles;
		/// <summary>
		/// After NPCs behind tiles, like worms
		/// </summary>
		public static event Draw OnDraw_BeforeSolidTiles;
		/// <summary>
		/// After solid tiles
		/// </summary>
		public static event Draw OnDraw_BeforePlayersBehindNPCs;
		/// <summary>
		/// After player details drawn behind NPCs
		/// </summary>
		public static event Draw OnDraw_BeforeNPCs;
		/// <summary>
		/// After NPCs
		/// </summary>
		public static event Draw OnDraw_BeforeProjectiles;
		/// <summary>
		/// After projectiles
		/// </summary>
		public static event Draw OnDraw_BeforePlayers;
		/// <summary>
		/// After players
		/// </summary>
		public static event Draw OnDraw_BeforeItems;
		/// <summary>
		/// After items in the world
		/// </summary>
		public static event Draw OnDraw_BeforeRain;
		/// <summary>
		/// After rain
		/// </summary>
		public static event Draw OnDraw_BeforeGore;
		/// <summary>
		/// After gore
		/// </summary>
		public static event Draw OnDraw_BeforeDust;
		/// <summary>
		/// After dust
		/// </summary>
		public static event Draw OnDraw_BeforeWater;
		/// <summary>
		/// After water
		/// </summary>
		public static event Draw OnDraw_BeforeInterface;
		public static event Draw OnDraw_AfterInterface;
		public static event Draw OnDraw_BeforeMainMenu;
		public static event Draw OnDraw_AfterMainMenu;

		public override void Load()
		{
			if (Main.dedServ)
			{
				return;
			}

			On_Dust.UpdateDust += Update_BeforeDust;
			On_Main.UpdateMenu += Update_BeforeMenu;

			Main.QueueMainThreadAction(() =>
			{
				On_Main.DrawSurfaceBG += Draw_BeforeBackground;
				On_Main.DoDraw_WallsAndBlacks += Draw_BeforeWalls;
				On_Main.DoDraw_Tiles_NonSolid += Draw_BeforeNonSolidTiles;
				On_Main.DrawPlayers_BehindNPCs += Draw_BeforePlayersBehindNPCs;
				On_Main.DrawNPCs += Draw_BeforeNPCs;
				On_Main.DoDraw_Tiles_Solid += Draw_BeforeSolidTiles;
				On_Main.DrawProjectiles += Draw_BeforeProjectiles;
				On_Main.DrawPlayers_AfterProjectiles += Draw_BeforePlayers;
				On_Main.DrawItems += Draw_BeforeItems;
				On_Main.DrawRain += Draw_BeforeRain;
				On_Main.DrawGore += Draw_BeforeGore;
				On_Main.DrawDust += Draw_BeforeDust;
				On_Main.DrawWaters += Draw_BeforeWater;
				On_Main.DrawInterface += Draw_OnInterface;
				On_Main.DrawMenu += Draw_OnMainMenu;
			});
		}

		public override void Unload()
		{
			OnUpdateDust = null;
			OnUpdateMenu = null;
			OnDraw_BeforeBackground = null;
			OnDraw_BeforeWalls = null;
			OnDraw_BeforeNonSolidTiles = null;
			OnDraw_BeforeSolidTiles = null;
			OnDraw_BeforePlayersBehindNPCs = null;
			OnDraw_BeforeNPCsBehindTiles = null;
			OnDraw_BeforeNPCs = null;
			OnDraw_BeforeProjectiles = null;
			OnDraw_BeforePlayers = null;
			OnDraw_BeforeItems = null;
			OnDraw_BeforeRain = null;
			OnDraw_BeforeGore = null;
			OnDraw_BeforeDust = null;
			OnDraw_BeforeWater = null;
			OnDraw_BeforeInterface = null;
			OnDraw_AfterInterface = null;
			OnDraw_BeforeMainMenu = null;
			OnDraw_AfterMainMenu = null;
		}

		private void Update_BeforeDust(On_Dust.orig_UpdateDust orig)
		{
			OnUpdateDust?.Invoke();

			orig();
		}

		private void Update_BeforeMenu(On_Main.orig_UpdateMenu orig)
		{
			OnUpdateMenu?.Invoke();

			orig();
		}

		private void Draw_BeforeBackground(On_Main.orig_DrawSurfaceBG orig, Main self)
		{
			// Usually SpriteBatch has began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}


			Matrix matrix = Main.BackgroundViewMatrix.TransformationMatrix;
			matrix.Translation -= Main.BackgroundViewMatrix.ZoomMatrix.Translation * new Vector3(1f, Main.BackgroundViewMatrix.Effects.HasFlag(SpriteEffects.FlipVertically) ? (-1f) : 1f, 1f);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, matrix);
			OnDraw_BeforeBackground?.Invoke(Layer.BeforeBackground);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforeWalls(On_Main.orig_DoDraw_WallsAndBlacks orig, Main self)
		{
			// Usually SpriteBatch has began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeWalls?.Invoke(Layer.BeforeWalls);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforeNonSolidTiles(On_Main.orig_DoDraw_Tiles_NonSolid orig, Main self)
		{
			// Usually SpriteBatch has began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeNonSolidTiles?.Invoke(Layer.BeforeNonSolidTiles);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforeSolidTiles(On_Main.orig_DoDraw_Tiles_Solid orig, Main self)
		{
			// Usually SpriteBatch has NOT began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeSolidTiles?.Invoke(Layer.BeforeSolidTiles);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforePlayersBehindNPCs(On_Main.orig_DrawPlayers_BehindNPCs orig, Main self)
		{
			// Usually SpriteBatch has NOT began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforePlayersBehindNPCs?.Invoke(Layer.BeforePlayersBehindNPCs);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforeNPCs(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
		{
			if (behindTiles)
			{
				// Usually SpriteBatch has began here, but if another mod changes this, then we won't break anything by being safe here
				SpriteBatcState spriteBatchState = new(Main.spriteBatch);
				if (spriteBatchState.BeginCalled)
				{
					Main.spriteBatch.End();
				}

				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
				OnDraw_BeforeNPCsBehindTiles?.Invoke(Layer.BeforeNPCsBehindTiles);
				Main.spriteBatch.End();

				if (spriteBatchState.BeginCalled)
				{
					SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
				}
			}
			else
			{
				// Usually SpriteBatch has began here, but if another mod changes this, then we won't break anything by being safe here
				SpriteBatcState spriteBatchState = new(Main.spriteBatch);
				if (spriteBatchState.BeginCalled)
				{
					Main.spriteBatch.End();
				}

				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
				OnDraw_BeforeNPCs?.Invoke(Layer.BeforeNPCs);
				Main.spriteBatch.End();

				if (spriteBatchState.BeginCalled)
				{
					SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
				}
			}

			orig(self, behindTiles);
		}

		private void Draw_BeforeProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
		{
			// Usually SpriteBatch has NOT began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeProjectiles?.Invoke(Layer.BeforeProjectiles);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforePlayers(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
		{
			// Usually SpriteBatch has NOT began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforePlayers?.Invoke(Layer.BeforePlayers);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforeItems(On_Main.orig_DrawItems orig, Main self)
		{
			// Usually SpriteBatch has began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeItems?.Invoke(Layer.BeforeItems);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforeRain(On_Main.orig_DrawRain orig, Main self)
		{
			// Usually SpriteBatch has began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeRain?.Invoke(Layer.BeforeRain);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforeGore(On_Main.orig_DrawGore orig, Main self)
		{
			// Usually SpriteBatch has began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeGore?.Invoke(Layer.BeforeGore);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforeDust(On_Main.orig_DrawDust orig, Main self)
		{
			// Usually SpriteBatch has NOT began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeDust?.Invoke(Layer.BeforeDust);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self);
		}

		private void Draw_BeforeWater(On_Main.orig_DrawWaters orig, Main self, bool isBackground)
		{
			// Usually SpriteBatch has began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeWater?.Invoke(Layer.BeforeWater);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self, isBackground);
		}

		private void Draw_OnInterface(On_Main.orig_DrawInterface orig, Main self, GameTime gameTime)
		{
			// Usually SpriteBatch has NOT began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeInterface?.Invoke(Layer.BeforeInterface);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self, gameTime);

			// Usually SpriteBatch has NOT began here, but if another mod changes this, then we won't break anything by being safe here
			spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_AfterInterface?.Invoke(Layer.AfterInterface);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}
		}

		private void Draw_OnMainMenu(On_Main.orig_DrawMenu orig, Main self, GameTime gameTime)
		{
			// Usually SpriteBatch has began here, but if another mod changes this, then we won't break anything by being safe here
			SpriteBatcState spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_BeforeMainMenu?.Invoke(Layer.BeforeMainMenu);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}

			orig(self, gameTime);

			// Usually SpriteBatch has NOT began here, but if another mod changes this, then we won't break anything by being safe here
			spriteBatchState = new(Main.spriteBatch);
			if (spriteBatchState.BeginCalled)
			{
				Main.spriteBatch.End();
			}

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			OnDraw_AfterMainMenu?.Invoke(Layer.AfterMainMenu);
			Main.spriteBatch.End();

			if (spriteBatchState.BeginCalled)
			{
				SpriteBatcState.Apply(Main.spriteBatch, spriteBatchState);
			}
		}

		public static void Hook(Layer layer, Draw method)
		{
			ArgumentNullException.ThrowIfNull(method);

			switch (layer)
			{
				case Layer.BeforeBackground:
					OnDraw_BeforeBackground += method;
					break;
				case Layer.BeforeWalls:
					OnDraw_BeforeWalls += method;
					break;
				case Layer.BeforeNonSolidTiles:
					OnDraw_BeforeNonSolidTiles += method;
					break;
				case Layer.BeforeNPCsBehindTiles:
					OnDraw_BeforeNPCsBehindTiles += method;
					break;
				case Layer.BeforeSolidTiles:
					OnDraw_BeforeSolidTiles += method;
					break;
				case Layer.BeforePlayersBehindNPCs:
					OnDraw_BeforePlayersBehindNPCs += method;
					break;
				case Layer.BeforeNPCs:
					OnDraw_BeforeNPCs += method;
					break;
				case Layer.BeforeProjectiles:
					OnDraw_BeforeProjectiles += method;
					break;
				case Layer.BeforePlayers:
					OnDraw_BeforePlayers += method;
					break;
				case Layer.BeforeItems:
					OnDraw_BeforeItems += method;
					break;
				case Layer.BeforeRain:
					OnDraw_BeforeRain += method;
					break;
				case Layer.BeforeGore:
					OnDraw_BeforeGore += method;
					break;
				case Layer.BeforeDust:
					OnDraw_BeforeDust += method;
					break;
				case Layer.BeforeWater:
					OnDraw_BeforeWater += method;
					break;
				case Layer.BeforeInterface:
					OnDraw_BeforeInterface += method;
					break;
				case Layer.AfterInterface:
					OnDraw_AfterInterface += method;
					break;
				case Layer.BeforeMainMenu:
					OnDraw_BeforeMainMenu += method;
					break;
				case Layer.AfterMainMenu:
					OnDraw_AfterMainMenu += method;
					break;
			}
		}

		public static void UnHook(Layer layer, Draw method)
		{
			ArgumentNullException.ThrowIfNull(method);

			switch (layer)
			{
				case Layer.BeforeBackground:
					OnDraw_BeforeBackground -= method;
					break;
				case Layer.BeforeWalls:
					OnDraw_BeforeWalls -= method;
					break;
				case Layer.BeforeNonSolidTiles:
					OnDraw_BeforeNonSolidTiles -= method;
					break;
				case Layer.BeforeNPCsBehindTiles:
					OnDraw_BeforeNPCsBehindTiles -= method;
					break;
				case Layer.BeforeSolidTiles:
					OnDraw_BeforeSolidTiles -= method;
					break;
				case Layer.BeforePlayersBehindNPCs:
					OnDraw_BeforePlayersBehindNPCs -= method;
					break;
				case Layer.BeforeNPCs:
					OnDraw_BeforeNPCs -= method;
					break;
				case Layer.BeforeProjectiles:
					OnDraw_BeforeProjectiles -= method;
					break;
				case Layer.BeforePlayers:
					OnDraw_BeforePlayers -= method;
					break;
				case Layer.BeforeItems:
					OnDraw_BeforeItems -= method;
					break;
				case Layer.BeforeRain:
					OnDraw_BeforeRain -= method;
					break;
				case Layer.BeforeGore:
					OnDraw_BeforeGore -= method;
					break;
				case Layer.BeforeDust:
					OnDraw_BeforeDust -= method;
					break;
				case Layer.BeforeWater:
					OnDraw_BeforeWater -= method;
					break;
				case Layer.BeforeInterface:
					OnDraw_BeforeInterface -= method;
					break;
				case Layer.AfterInterface:
					OnDraw_AfterInterface -= method;
					break;
				case Layer.BeforeMainMenu:
					OnDraw_BeforeMainMenu -= method;
					break;
				case Layer.AfterMainMenu:
					OnDraw_AfterMainMenu -= method;
					break;
			}
		}

		public static void GetClip(out Rectangle rectangle, out RasterizerState rasterizer)
		{
			rectangle = Main.graphics.GraphicsDevice.ScissorRectangle;
			rasterizer = Main.graphics.GraphicsDevice.RasterizerState;
		}

		public static void SetClip(Rectangle rectangle, RasterizerState rasterizer)
		{
			Main.graphics.GraphicsDevice.ScissorRectangle = rectangle;
			Main.graphics.GraphicsDevice.RasterizerState = rasterizer;
		}
	}
}
