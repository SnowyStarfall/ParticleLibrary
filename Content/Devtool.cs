﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ParticleLibrary.ExampleParticles;
using Terraria.GameContent;
using ReLogic.Graphics;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Core;
using ParticleLibrary.Core.PointParticleSystem;

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

                //EmitterManager.NewEmitter<ExampleEmitter>(Main.MouseWorld);
                return true;
            }

            if (xKey)
            {
                if (rightClick)
                {
                    for (int i = 0; i < 10000; i++)
                    {

                        Vector2 shift = new Vector2(256f, 0f).RotatedBy(MathHelper.ToRadians(i * 36f));

                        for (int k = 0; k < 10; k++)
                        {
                            Vector2 position = Main.MouseWorld + shift;
                            Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 4f);

                            GParticle settings = new()
                            {
                                StartColor = new(1f, 0f, 0f, 0f),
                                EndColor = new(0f, 1f, 0f, 0f),

                                Scale = new Vector2(0.5f),
                                ScaleVelocity = new Vector2(-0.5f / 120f),
                                Rotation = 0f,
                                RotationVelocity = -0.1f,

                                Depth = Main.rand.NextFloat(0.5f, 1.5f + float.Epsilon),
                                DepthVelocity = Main.rand.NextFloat(-0.003f, 0.003f + float.Epsilon),
                            };

                            GParticleManager.ParticleSystem.AddParticle(position, velocity, settings);
                        }
                    }
                    return true;
                }


                for (int i = 0; i < 10; i++)
                {
                    Vector2 shift = new Vector2(256f, 0f).RotatedBy(MathHelper.ToRadians(i * 36f));

                    for (int k = 0; k < 1; k++)
                    {
                        Vector2 position = Main.MouseWorld + shift;
                        Vector2 velocity = Vector2.Zero;

                        GParticle settings = new()
                        {
                            StartColor = new(1f, 0f, 0f, 0f),
                            EndColor = new(0f, 1f, 0f, 0f),

                            Scale = new Vector2(0.5f),
                            //ScaleVelocity = new Vector2(-0.5f / 120f),
                            Rotation = 0f,
                            RotationVelocity = -0.1f,

                            Depth = Main.rand.NextFloat(0.5f, 1.5f + float.Epsilon),
                            //DepthVelocity = Main.rand.NextFloat(-0.003f, 0.003f + float.Epsilon),
                        };

                        GParticleManager.ParticleSystem.AddParticle(position, velocity, settings);
                    }
                }
                return true;
            }

            if (cKey)
            {
                if (rightClick)
                {
                    return true;
                }

                Vector2 position = Main.MouseWorld;
                Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 4f);

                GParticle settings = new()
                {
                    StartColor = new(1f, 0f, 0f, 0f),
                    EndColor = new(0f, 1f, 0f, 0f),

                    Scale = new Vector2(0.125f),
                    Rotation = 0f,
                    RotationVelocity = -0.1f
                };

                GParticleManager.ParticleSystem.AddParticle(position, velocity, settings);

                return true;
            }

            if (vKey)
            {
                if (rightClick)
                {
                    return true;
                }

                float offset = 0f;
                for (int i = 0; i < 1000; i++)
                {
                    Vector2 shift = new Vector2(256f + (offset += 0.001f), 0f).RotatedBy(MathHelper.ToRadians(i * 36f));

                    for (int k = 0; k < 1; k++)
                    {
                        Vector2 position = Main.MouseWorld + shift;
                        Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 4f);

                        PointParticle settings = new()
                        {
                            StartColor = new(1f, 0f, 0f, 0f),
                            EndColor = new(0f, 1f, 0f, 0f),

                            Size = 10f,

                            Depth = Main.rand.NextFloat(0.5f, 1.5f + float.Epsilon),
                            //DepthVelocity = Main.rand.NextFloat(-0.003f, 0.003f + float.Epsilon),
                        };

                        PointParticleManager.ParticleSystem.AddParticle(position, velocity, settings);
                    }
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
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer);
            Main.spriteBatch.DrawString(FontAssets.MouseText.Value, CParticleSystem.ParticleCount.ToString(), Main.ScreenSize.ToVector2() * 0.5f + new Vector2(100f), Color.White);
            Main.spriteBatch.DrawString(FontAssets.MouseText.Value, CParticleSystem.UpdateTime_InMilliseconds.ToString(), Main.ScreenSize.ToVector2() * 0.5f + new Vector2(100f, 116f), Color.White);
            Main.spriteBatch.End();
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
                for (int i = 0; i < 300; i++)
                {
                    CParticleSystem.NewParticle<GlowParticle>(Main.MouseWorld, Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 10f), Color.Green, 1f, layer: Layer.BeforeTiles);
                }
            }
            _counter++;
        }
    }
}