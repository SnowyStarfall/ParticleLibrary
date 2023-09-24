using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.Core.Systems.EmitterSystem;
using ParticleLibrary.Core.Systems.Test;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ParticleLibrary.Content.Items
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
		public float Progress;

		public Emitter Emitter;

		public GParticleSystem ParticleSystem;

		public Devtool()
		{
			ParticleSystem = new(ModContent.Request<Texture2D>(Resources.Debug.Plus, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		}

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

				//EmitterManager.NewEmitter<ExampleEmitter>(Main.MouseWorld);
				return true;
			}

			if (xKey)
			{
				if (rightClick)
				{
					return true;
				}

				ParticleSystem.AddParticle(Main.MouseWorld, Main.rand.NextVector2Unit() * 2f);
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
				return Alt(player, rightClick);
			else
				return Normal(player, rightClick);
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

				Main.NewText("Coordinates cleared.", new Color(218, 70, 70));
				return true;
			}

			X = (int)(Main.MouseWorld.X / 16);
			Y = (int)(Main.MouseWorld.Y / 16);

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

		private int _counter;
		public void UpdateParticles()
		{
			if (!Main.hasFocus)
				return;
			if (Main.gamePaused)
				return;

			if (_counter >= 1)
			{
				_counter = 0;
				for (int i = 0; i < 3; i++)
				{
					//ParticleManager.NewParticle<EmberParticle>(Main.MouseWorld, Main.rand.NextVector2Unit() * 2f, Color.Green, 1f, AI3: 1f, Layer: Particle.Layer.BeforeTiles);
				}
			}
			_counter++;
		}
	}
}
