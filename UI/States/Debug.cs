using Microsoft.Xna.Framework;
using ParticleLibrary.Core;
using ParticleLibrary.UI.Elements.Base;
using ParticleLibrary.UI.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ParticleLibrary.UI.States
{
	internal class Debug : UIState
	{
		public bool Visible => ParticleLibraryConfig.Instance.DebugUI;
		public Theme Theme => ParticleLibraryConfig.CurrentTheme;

		public Dictionary<Mod, Core.Particle> CParticles { get; private set; }
		public Dictionary<Mod, QuadParticle> GParticles { get; private set; }
		public Dictionary<Mod, Emitter> Emitters { get; private set; }
		public Dictionary<Mod, QuadParticleSystem> GParticleSystems { get; private set; }
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

			SearchList = new(Theme.Low, Theme.LowAccent, 1f, 4f)
			{
				HAlign = 0.5f
			};
			SearchList.Top.Set(0f, 0.15f);
			SearchList.Width.Set(0f, 0.9f);
			SearchList.Height.Set(0f, 0.8f);
			SearchList.ItemHeight = new(28f, 0f);

			GetTypes();
			AddTypes();

			//// Append all panels to base
			//Base.Append(SearchPanel);
			//Append(Base);

			SearchPanel.Append(SearchBar);
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

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Core.Particle))))
				{
					CParticles.Add(mod, Activator.CreateInstance(p) as Core.Particle);
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(QuadParticle))))
				{
					GParticles.Add(mod, Activator.CreateInstance(p) as QuadParticle);
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Emitter))))
				{
					Emitters.Add(mod, Activator.CreateInstance(p) as Emitter);
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(QuadParticleSystem))))
				{
					GParticleSystems.Add(mod, Activator.CreateInstance(p) as QuadParticleSystem);
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
			float c = 1.3f;

			foreach (var item in CParticles)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}",
					HideOverflow = true
				});

				m++;
			}

			foreach (var item in GParticles)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}",
					HideOverflow = true
				});

				m++;
			}

			foreach (var item in Emitters)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}",
					HideOverflow = true
				});

				m++;
			}

			foreach (var item in GParticleSystems)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}",
					HideOverflow = true
				});

				m++;
			}

			foreach (var item in PointParticleSystems)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}",
					HideOverflow = true
				});

				m++;
			}

			// TODO: Remove
			SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
			{
				Content = $"ParticleLibrary Test 1",
				HideOverflow = true
			});
			m++;
			SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
			{
				Content = $"ParticleLibrary Teeeeest 2",
				HideOverflow = true
			});
			m++;
			SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
			{
				Content = $"ParticleLibrary Teeeeeeeeeeeeest 3",
				HideOverflow = true
			});
			m++;
			SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
			{
				Content = $"ParticleLibrary Teeeeeeeeeeeeeeeeeeeeeeeeest 4",
				HideOverflow = true
			});
			m++;
			SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
			{
				Content = $"ParticleLibrary AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
				HideOverflow = true
			});
		}
	}
}
