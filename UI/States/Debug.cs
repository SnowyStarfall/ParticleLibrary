using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleLibrary.UI.Elements.Base;
using ParticleLibrary.UI.Themes;
using System.Collections.Generic;
using System.Reflection;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ParticleLibrary.Core;
using System.Linq;

namespace ParticleLibrary.UI.States
{
	internal class Debug : UIState
	{
		public bool Visible => ParticleLibraryConfig.Instance.DebugUI;
		public Theme Theme => ParticleLibraryConfig.CurrentTheme;

		public Dictionary<Mod, CParticle> CParticles { get; private set; }
		public Dictionary<Mod, GParticle> GParticles { get; private set; }
		public Dictionary<Mod, Emitter> Emitters { get; private set; }
		public Dictionary<Mod, GParticleSystem> GParticleSystems { get; private set; }
		public Dictionary<Mod, PointParticleSystem> PointParticleSystems { get; private set; }

		public UIElement Base { get; set; }
		public Panel SearchPanel { get; set; }
		public SearchBar SearchBar { get; set; }
		public Button SearchButton { get; set; }
		public List SearchList { get; set; }

		private Point _lastScreenSize = Main.ScreenSize;

		public override void OnInitialize()
		{
			// Visibility
			if (!Visible)
				return;

			CParticles = new();
			GParticles = new();
			Emitters = new();
			GParticleSystems = new();
			PointParticleSystems = new();

			Base = new();

			SearchPanel = new(Theme.Low, Theme.LowAccent, 1f, 4f)
			{
				Resizable = true,
				Draggable = true,
				Screenlocked = true
			};
			SearchPanel.Width.Set(384f, 0f);
			SearchPanel.Height.Set(512f, 0f);
			SearchPanel.MinWidth.Set(0f, 0.125f);
			SearchPanel.MinHeight.Set(0f, 0.125f);

			SearchBar = new(Theme.Mid, Theme.LowAccent, Theme.HighAccent, 1f, 4f)
			{
			};
			SearchBar.Left.Set(0f, 0.05f);
			SearchBar.Top.Set(0, 0.05f);
			SearchBar.Width.Set(0f, 0.9f);
			SearchBar.Height.Set(0f, 0.1f);
			SearchBar.MaxHeight.Set(24f, 0f);

			SearchButton = new(Theme.Mid, Theme.High, Theme.LowAccent, Theme.HighAccent)
			{
				Content = "Test Content",
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			SearchButton.Width.Set(64f, 0f);
			SearchButton.Height.Set(32f, 0f);

			SearchList = new(Theme.Low, Theme.LowAccent, 1f, 4f)
			{
				HAlign = 0.5f
			};
			SearchList.Top.Set(0f, 0.15f);
			SearchList.Width.Set(0f, 0.9f);
			SearchList.Height.Set(0f, 0.8f);

			GetTypes();
			AddTypes();

			//// Append all panels to base
			//Base.Append(SearchPanel);
			//Append(Base);

			SearchPanel.Append(SearchBar);
			SearchPanel.Append(SearchButton);
			SearchPanel.Append(SearchList);
			Append(SearchPanel);
		}

		public void Unload()
		{
			// TODO: Implement unloading logic

			RemoveAllChildren();
			Base = null;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Ensure that our state always covers the entire screen
			if (_lastScreenSize != Main.ScreenSize)
			{
				_lastScreenSize = Main.ScreenSize;
				Recalculate();
			}
		}

		private void GetTypes()
		{
			for (int i = 0; i < ModLoader.Mods.Length; i++)
			{
				Mod mod = ModLoader.Mods[i];
				Assembly assembly = mod.Code;

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CParticle))))
				{
					CParticles.Add(mod, Activator.CreateInstance(p) as CParticle);
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(GParticle))))
				{
					GParticles.Add(mod, Activator.CreateInstance(p) as GParticle);
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Emitter))))
				{
					Emitters.Add(mod, Activator.CreateInstance(p) as Emitter);
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(GParticleSystem))))
				{
					GParticleSystems.Add(mod, Activator.CreateInstance(p) as GParticleSystem);
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(PointParticleSystem))))
				{
					PointParticleSystems.Add(mod, Activator.CreateInstance(p) as PointParticleSystem);
				}
			}
		}

		private void AddTypes()
		{
			int m = 0;

			foreach (var item in CParticles)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * 1.1f, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}"
				});

				m++;
			}

			foreach (var item in GParticles)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * 1.1f, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}"
				});

				m++;
			}

			foreach (var item in Emitters)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * 1.1f, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}"
				});

				m++;
			}

			foreach (var item in GParticleSystems)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * 1.1f, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}"
				});

				m++;
			}

			foreach (var item in PointParticleSystems)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * 1.1f, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}"
				});

				m++;
			}

			// TODO: Remove
			SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * 1.1f, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
			{
				Content = $"ParticleLibrary Test 1"
			});
			m++;
			SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * 1.1f, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
			{
				Content = $"ParticleLibrary Teeeeest 2"
			});
			m++;
			SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * 1.1f, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
			{
				Content = $"ParticleLibrary Teeeeeeeeeeeeest 3"
			});
			m++;
			SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * 1.1f, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
			{
				Content = $"ParticleLibrary Teeeeeeeeeeeeeeeeeeeeeeeeest 4"
			});
			m++;
			SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * 1.1f, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
			{
				Content = $"ParticleLibrary AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
			});
			m++;
		}
	}
}
