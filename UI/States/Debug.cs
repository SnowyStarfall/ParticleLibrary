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

		public Dictionary<string, Core.Particle> Particles { get; private set; }
		public Dictionary<string, PointParticle> PointParticles { get; private set; }
		public Dictionary<string, QuadParticle> QuadParticles { get; private set; }
		public Dictionary<string, Emitter> Emitters { get; private set; }
		public Dictionary<string, QuadParticleSystem> QuadParticleSystems { get; private set; }
		public Dictionary<string, PointParticleSystem> PointParticleSystems { get; private set; }

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

			Particles = new();
			QuadParticles = new();
			PointParticles = new();
			Emitters = new();
			QuadParticleSystems = new();
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

			// TODO: Fix
			//GetTypes();
			//AddTypes();

			//// Append all panels to base
			//Base.Append(SearchPanel);
			//Append(Base);

			SearchPanel.Append(SearchBar);
			SearchPanel.Append(SearchList);
			Append(SearchPanel);
		}

		public override void OnActivate()
		{
		}

		public override void OnDeactivate()
		{

		}

		public void Unload()
		{
			// TODO: Implement unloading logic

			Particles = null;
			QuadParticles = null;
			PointParticles = null;
			Emitters = null;
			QuadParticleSystems = null;
			PointParticleSystems = null;

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

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Core.Particle)) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) is not null))
				{
					string key = p.FullName;
					if (!Particles.ContainsKey(key))
					{
						Particles.Add(key, Activator.CreateInstance(p) as Core.Particle);
					}
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(QuadParticle)) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) is not null))
				{
					string key = p.FullName;
					if (!QuadParticles.ContainsKey(key))
					{
						QuadParticles.Add(key, Activator.CreateInstance(p) as QuadParticle);
					}
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Emitter)) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) is not null))
				{
					string key = p.FullName;
					if (!Emitters.ContainsKey(key))
					{
						Emitters.Add(key, Activator.CreateInstance(p) as Emitter);
					}
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(QuadParticleSystem)) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) is not null))
				{
					string key = p.FullName;
					if (!QuadParticleSystems.ContainsKey(key))
					{
						QuadParticleSystems.Add(key, Activator.CreateInstance(p) as QuadParticleSystem);
					}
				}

				foreach (var p in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(PointParticleSystem)) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) is not null))
				{
					string key = p.FullName;
					if (!PointParticleSystems.ContainsKey(key))
					{
						PointParticleSystems.Add(key, Activator.CreateInstance(p) as PointParticleSystem);
					}
				}
			}
		}

		private void AddTypes()
		{
			int m = 0;
			float c = 1.3f;

			foreach (var item in Particles)
			{
				SearchList.Add(new Button(m % 2 == 0 ? Theme.Low : Theme.Low * c, Theme.Mid, Theme.LowAccent, Theme.HighAccent)
				{
					Content = $"{item.Key.GetType().Name} {item.Value.GetType().Name}",
					HideOverflow = true
				});

				m++;
			}

			foreach (var item in QuadParticles)
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

			foreach (var item in QuadParticleSystems)
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
