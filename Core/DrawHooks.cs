using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
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
	///	matrix.Translation -= Main.BackgroundViewMatrix.ZoomMatrix.Translation* new Vector3(1f, Main.BackgroundViewMatrix.Effects.HasFlag(SpriteEffects.FlipVertically)? (-1f) : 1f, 1f);
	/// </code>
	/// Your drawing will not manually parallax. You will need to do this yourself for now.
	/// </para>
	/// 
	/// <para>
	/// For <see cref="OnDraw_BeforeWater"/>, ADD <see cref="Main.offScreenRange"/> to your draw position.
	/// </para>
	/// 
	/// SpriteBatch ends BEFORE the Draw events are invoked.
	/// This means that you MUST begin SpriteBatch before you can use it. SpriteBatch is Main.spriteBatch.
	/// </summary>
	public class DrawHooks : ModSystem
	{
		public delegate void Update();
		public static event Update OnUpdateDust;
		public static event Update OnUpdateMenu;

		public delegate void Draw(Layer layer);
		public static event Draw OnDraw_BeforeBackground;
		public static event Draw OnDraw_BeforeWalls;
		public static event Draw OnDraw_BeforeNonSolidTiles;
		public static event Draw OnDraw_BeforeSolidTiles;
		public static event Draw OnDraw_BeforePlayersBehindNPCs;
		public static event Draw OnDraw_BeforeNPCsBehindTiles;
		public static event Draw OnDraw_BeforeNPCs;
		public static event Draw OnDraw_BeforeProjectiles;
		public static event Draw OnDraw_BeforePlayers;
		public static event Draw OnDraw_BeforeItems;
		public static event Draw OnDraw_BeforeRain;
		public static event Draw OnDraw_BeforeGore;
		public static event Draw OnDraw_BeforeDust;
		public static event Draw OnDraw_BeforeWater;
		public static event Draw OnDraw_BeforeInterface;
		public static event Draw OnDraw_AfterInterface;
		public static event Draw OnDraw_BeforeMainMenu;
		public static event Draw OnDraw_AfterMainMenu;

		public override void Load()
		{
			if (Main.netMode is NetmodeID.Server)
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

		public static void Hook(Layer layer, Draw method)
		{
			if (method is null)
			{
				throw new ArgumentNullException(nameof(method));
			}

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
			if (method is null)
			{
				throw new ArgumentNullException(nameof(method));
			}

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
			Main.spriteBatch.End();

			Matrix matrix = Main.BackgroundViewMatrix.TransformationMatrix;
			matrix.Translation -= Main.BackgroundViewMatrix.ZoomMatrix.Translation * new Vector3(1f, Main.BackgroundViewMatrix.Effects.HasFlag(SpriteEffects.FlipVertically) ? (-1f) : 1f, 1f);

			OnDraw_BeforeBackground?.Invoke(Layer.BeforeBackground);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, matrix);

			orig(self);
		}

		private void Draw_BeforeWalls(On_Main.orig_DoDraw_WallsAndBlacks orig, Main self)
		{
			Main.spriteBatch.End();

			OnDraw_BeforeWalls?.Invoke(Layer.BeforeWalls);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void Draw_BeforeNonSolidTiles(On_Main.orig_DoDraw_Tiles_NonSolid orig, Main self)
		{
			Main.spriteBatch.End();

			OnDraw_BeforeNonSolidTiles?.Invoke(Layer.BeforeNonSolidTiles);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void Draw_BeforeSolidTiles(On_Main.orig_DoDraw_Tiles_Solid orig, Main self)
		{
			OnDraw_BeforeSolidTiles?.Invoke(Layer.BeforeSolidTiles);

			orig(self);
		}

		private void Draw_BeforePlayersBehindNPCs(On_Main.orig_DrawPlayers_BehindNPCs orig, Main self)
		{
			OnDraw_BeforePlayersBehindNPCs?.Invoke(Layer.BeforePlayersBehindNPCs);

			orig(self);
		}

		private void Draw_BeforeNPCs(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
		{
			if (behindTiles)
			{
				Main.spriteBatch.End();

				OnDraw_BeforeNPCsBehindTiles?.Invoke(Layer.BeforeNPCsBehindTiles);

				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			}
			else
			{
				Main.spriteBatch.End();

				OnDraw_BeforeNPCs?.Invoke(Layer.BeforeNPCs);

				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			}

			orig(self, behindTiles);
		}

		private void Draw_BeforeProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
		{
			OnDraw_BeforeProjectiles?.Invoke(Layer.BeforeProjectiles);

			orig(self);
		}

		private void Draw_BeforePlayers(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
		{
			OnDraw_BeforePlayers?.Invoke(Layer.BeforePlayers);

			orig(self);
		}

		private void Draw_BeforeItems(On_Main.orig_DrawItems orig, Main self)
		{
			Main.spriteBatch.End();

			OnDraw_BeforeItems?.Invoke(Layer.BeforeItems);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void Draw_BeforeRain(On_Main.orig_DrawRain orig, Main self)
		{
			Main.spriteBatch.End();

			OnDraw_BeforeRain?.Invoke(Layer.BeforeRain);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void Draw_BeforeGore(On_Main.orig_DrawGore orig, Main self)
		{
			Main.spriteBatch.End();

			OnDraw_BeforeGore?.Invoke(Layer.BeforeGore);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			orig(self);
		}

		private void Draw_BeforeDust(On_Main.orig_DrawDust orig, Main self)
		{
			OnDraw_BeforeDust?.Invoke(Layer.BeforeDust);

			orig(self);
		}

		private void Draw_BeforeWater(On_Main.orig_DrawWaters orig, Main self, bool isBackground)
		{
			Main.spriteBatch.End();

			OnDraw_BeforeWater?.Invoke(Layer.BeforeWater);

			Main.spriteBatch.Begin();

			orig(self, isBackground);
		}

		private void Draw_OnInterface(On_Main.orig_DrawInterface orig, Main self, GameTime gameTime)
		{
			OnDraw_BeforeInterface?.Invoke(Layer.BeforeInterface);

			orig(self, gameTime);

			OnDraw_AfterInterface?.Invoke(Layer.AfterInterface);
		}

		private void Draw_OnMainMenu(On_Main.orig_DrawMenu orig, Main self, GameTime gameTime)
		{
			Main.spriteBatch.End();

			OnDraw_BeforeMainMenu?.Invoke(Layer.BeforeMainMenu);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

			orig(self, gameTime);

			OnDraw_AfterMainMenu?.Invoke(Layer.AfterMainMenu);
		}
	}
}
