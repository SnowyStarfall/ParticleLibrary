﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.Core.V3;
using ParticleLibrary.Core.V3.Emission;
using ParticleLibrary.Examples;
using ParticleLibrary.Utilities;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ParticleLibrary.Content
{
    public class Devtool : ModItem
	{
		public int Width => Main.screenWidth;
		public int Height => Main.screenHeight;
		public Vector2 Position => new(X * 16f, Y * 16f);

		public int X;
		public int Y;
		public int Divisions;

		public bool UpdateHooked;
		public bool DrawHooked;
		public float Progress;

		public Emitter Emitter;

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
		}

		public override bool AltFunctionUse(Player player) => true;

		// Uses
		public override bool? UseItem(Player player)
		{
			bool rightClick = player.altFunctionUse == 2;
			bool alt = Main.keyState.IsKeyDown(Keys.LeftAlt);

			bool zKey = Main.keyState.IsKeyDown(Keys.Z);
			bool xKey = Main.keyState.IsKeyDown(Keys.X);
			bool cKey = Main.keyState.IsKeyDown(Keys.C);
			bool vKey = Main.keyState.IsKeyDown(Keys.V);

			if (zKey)
			{
				if (rightClick)
				{
					return true;
				}

				return true;
			}

			if (xKey)
			{
				if (rightClick)
				{
					return true;
				}

				return true;
			}

			if (cKey)
			{
				if (rightClick)
				{
					return true;
				}

				return true;
			}

			if (vKey)
			{
				if (rightClick)
				{
					return true;
				}

				return true;
			}

			if (alt)
			{
				return Alt(player, rightClick);
			}
			else
			{
				return Normal(player, rightClick);
			}
		}

		public bool Alt(Player player, bool rightClick)
		{
			if (rightClick)
			{
				if (UpdateHooked)
					On_Main.UpdateParticleSystems -= Update;
				UpdateHooked = false;
				Main.NewText("Update disabled.", new Color(218, 70, 70));
				return true;
			}
			if (!UpdateHooked)
				On_Main.UpdateParticleSystems += Update;
			UpdateHooked = true;
			Main.NewText("Update enabled.", new Color(218, 70, 70));
			return true;
		}

		public bool Normal(Player player, bool rightClick)
		{
			// Right click
			if (rightClick)
			{
				X = 0;
				Y = 0;
				if (DrawHooked)
				{
					On_Main.DrawProjectiles -= Draw;
					DrawHooked = false;
				}

				Main.NewText("Coordinates cleared.", new Color(218, 70, 70));
				return true;
			}

			X = (int)(Main.MouseWorld.X / 16);
			Y = (int)(Main.MouseWorld.Y / 16);
			if (!DrawHooked)
			{
				On_Main.DrawProjectiles += Draw;
				DrawHooked = true;
			}

			Dust.QuickBox(new Vector2(X, Y) * 16, new Vector2(X + 1, Y + 1) * 16, 2, new Color(218, 70, 70), null);
			Main.NewText($"Drawing sprites at [{X}, {Y}]. Right-click to discard.", new Color(218, 70, 70));
			return true;
		}

		// Updating
		private void Update(On_Main.orig_UpdateParticleSystems orig, Main self)
		{
			orig(self);

			UpdateParticles();
		}

		private void Draw(On_Main.orig_DrawProjectiles orig, Main self)
		{
			orig(self);

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer);
			Main.spriteBatch.DrawString(FontAssets.MouseText.Value, Core.ParticleSystem.ParticleCount.ToString(), Main.ScreenSize.ToVector2() * 0.5f + new Vector2(100f), Color.White);
			Main.spriteBatch.DrawString(FontAssets.MouseText.Value, Core.ParticleSystem.UpdateTime_InMilliseconds.ToString(), Main.ScreenSize.ToVector2() * 0.5f + new Vector2(100f, 116f), Color.White);
			Main.spriteBatch.End();
		}


		private int _counter;
		public void UpdateParticles()
		{
			if (!Main.hasFocus)
				return;
			if (Main.gamePaused)
				return;

			Vector2 position = new(X * 16, Y * 16);

			if (_counter % 1 == 0)
			{
				//_counter = 0;
				//for (int i = 0; i < 300; i++)
				//{
				//	NewParticleManager.NewParticle<ExampleParticle>(Main.MouseWorld, Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 10f), Color.Green, 1f);
				//}

				Point mouseTile = new((int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f));
				float randDegrees = Main.rand.NextFloat(0f, 1f + float.Epsilon) * MathHelper.TwoPi;

				//if (Main.tile[mouseTile].HasTile)
				//{
				//	Vector2 pos = EmitterSettings.Rectangle.Solve(new Vector2(mouseTile.X * 16 + 8f, mouseTile.Y * 16 + 8f), EmitterOrigin.Rim, 16f, 16f);
				//	ExampleParticleSystemManager.ExampleQuadSystem.NewParticle(pos, new Vector2(0f, 0f).RotatedBy(randDegrees), quad, 120);

				//}

				//for (int i = 0; i < 100; i++)
				//{
				//	Core.ParticleSystem.NewParticle<ExampleParticle>(Main.MouseWorld, Main.rand.NextVector2Unit() * 10f, Color.White.WithAlpha(0f), 1f);
				//}

				//for (int i = 0; i < 100; i++)
				//{
				//}



				//for (int i = 0; i < 10; i++)
				//{
				//	Vector2 velocity = new Vector2(4f, 0f).RotatedBy(MathHelper.ToRadians(i * 36f + _counter));
				//	quad.VelocityAcceleration = new Vector2(velocity.X / (i * 36f) * -1, velocity.Y / (i * 36f) * -1);

				//	//ExampleParticleSystemManager.ExampleQuadSystem.NewParticle(position, velocity, quad, i * 36);
				//	ExampleParticleSystemManager.ExamplePointSystem.NewParticle(position, velocity, point, i * 36);
				//}

				//PointParticle settings = new()
				//{
				//	StartColor = new(1f, 0f, 0f, 0f),
				//	EndColor = new(0f, 1f, 0f, 0f),

				//	//Depth = Main.rand.NextFloat(0.5f, 1.5f + float.Epsilon),
				//	//DepthVelocity = Main.rand.NextFloat(-0.003f, 0.003f + float.Epsilon),
				//};

				//for (int i = 0; i < 360f; i++)
				//{
				//	float rad = MathHelper.ToRadians(i);

				//	Vector2 position = Main.MouseWorld + new Vector2(0f, 0f).RotatedBy(rad + Main.rand.NextFloat(-0.02f, 0.02f));
				//	Vector2 velocity = new Vector2(0f, -4f + Main.rand.NextFloat(-1f, 1f)).RotatedBy(rad + Main.rand.NextFloat(-1f, 1f));
				//	settings.VelocityAcceleration = new Vector2(0f, 0.05f);

				//	PointParticleManager.ParticleSystem.AddParticle(position, velocity, settings);
				//}

				//for (int i = 0; i < 360f; i++)
				//{
				//	float rad = MathHelper.ToRadians(i);

				//	Vector2 position = Main.MouseWorld + new Vector2(0f, 0f).RotatedBy(rad);
				//	Vector2 velocity = new Vector2(0f, -1f + Main.rand.NextFloat(-0.2f, 0.2f)).RotatedBy(rad + Main.rand.NextFloat(-0.5f, 0.5f));
				//	settings.VelocityAcceleration = Vector2.Zero;

				//	PointParticleManager.ParticleSystem.AddParticle(position, velocity, settings);
				//}
			}

			_counter++;
		}
	}
}
