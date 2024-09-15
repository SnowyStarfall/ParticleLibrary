using Microsoft.Xna.Framework;
using ParticleLibrary.Core.V3.Interfaces;
using ParticleLibrary.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary.Core.V3
{
	public class ParticleManagerV3 : ModSystem
	{
		public static InstancedParticleEffect ParticleEffect { get; private set; }

		private static List<IBuffer> _buffers;
		private static List<IUpdatable> _updateables;
		private static Dictionary<Layer, List<IRenderable>> _renderables;

		public override void Load()
		{
			ParticleEffect = new();

			_buffers = [];
			_updateables = [];
			_renderables = [];

			DrawHooks.Hook(Layer.BeforeBackground, Render);
			DrawHooks.Hook(Layer.BeforeWalls, Render);
			DrawHooks.Hook(Layer.BeforeNonSolidTiles, Render);
			DrawHooks.Hook(Layer.BeforeNPCsBehindTiles, Render);
			DrawHooks.Hook(Layer.BeforeSolidTiles, Render);
			DrawHooks.Hook(Layer.BeforePlayersBehindNPCs, Render);
			DrawHooks.Hook(Layer.BeforeNPCs, Render);
			DrawHooks.Hook(Layer.BeforeProjectiles, Render);
			DrawHooks.Hook(Layer.BeforePlayers, Render);
			DrawHooks.Hook(Layer.BeforeItems, Render);
			DrawHooks.Hook(Layer.BeforeRain, Render);
			DrawHooks.Hook(Layer.BeforeGore, Render);
			DrawHooks.Hook(Layer.BeforeDust, Render);
			DrawHooks.Hook(Layer.BeforeWater, Render);
			DrawHooks.Hook(Layer.BeforeInterface, Render);
			DrawHooks.Hook(Layer.AfterInterface, Render);
			DrawHooks.Hook(Layer.BeforeMainMenu, Render);
			DrawHooks.Hook(Layer.AfterMainMenu, Render);
		}

		public override void Unload()
		{
			Main.QueueMainThreadAction(() =>
			{
				foreach (var buffer in _buffers)
				{
					if (buffer is IDisposable disposable)
					{
						disposable.Dispose();
					}
				}

				_buffers.Clear();
				_buffers = null;

				_updateables.Clear();
				_updateables = null;

				_renderables.Clear();
				_renderables = null;
			});

			DrawHooks.UnHook(Layer.BeforeBackground, Render);
			DrawHooks.UnHook(Layer.BeforeWalls, Render);
			DrawHooks.UnHook(Layer.BeforeNonSolidTiles, Render);
			DrawHooks.UnHook(Layer.BeforeNPCsBehindTiles, Render);
			DrawHooks.UnHook(Layer.BeforeSolidTiles, Render);
			DrawHooks.UnHook(Layer.BeforePlayersBehindNPCs, Render);
			DrawHooks.UnHook(Layer.BeforeNPCs, Render);
			DrawHooks.UnHook(Layer.BeforeProjectiles, Render);
			DrawHooks.UnHook(Layer.BeforePlayers, Render);
			DrawHooks.UnHook(Layer.BeforeItems, Render);
			DrawHooks.UnHook(Layer.BeforeRain, Render);
			DrawHooks.UnHook(Layer.BeforeGore, Render);
			DrawHooks.UnHook(Layer.BeforeDust, Render);
			DrawHooks.UnHook(Layer.BeforeWater, Render);
			DrawHooks.UnHook(Layer.BeforeInterface, Render);
			DrawHooks.UnHook(Layer.AfterInterface, Render);
			DrawHooks.UnHook(Layer.BeforeMainMenu, Render);
			DrawHooks.UnHook(Layer.AfterMainMenu, Render);
		}

		public override void PostUpdateDusts()
		{
			foreach (var updatable in _updateables)
			{
				updatable.Update();
			}
		}

		private void Render(Layer layer)
		{
			Main.spriteBatch.End();

			if (!_renderables.TryGetValue(layer, out List<IRenderable> value))
			{
				Main.spriteBatch.Begin();

				return;
			}

			foreach (var renderable in value)
			{
				renderable.Render();
			}

			Main.spriteBatch.Begin();
		}

		/// <summary>
		/// Registers the updatable to update after dust.
		/// </summary>
		/// <param name="updatable">The updatable.</param>
		public static void RegisterUpdatable(IUpdatable updatable)
		{
			if (_updateables.Contains(updatable))
			{
				return;
			}

			_updateables.Add(updatable);
		}

		/// <summary>
		/// Unregisters the updatable from updating after dust.
		/// </summary>
		/// <param name="updatable">The updatable.</param>
		public static void UnregisterUpdatable(IUpdatable updatable)
		{
			if (!_updateables.Contains(updatable))
			{
				return;
			}

			_updateables.Remove(updatable);
		}

		/// <summary>
		/// Registers the renderable to render on the given layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="renderable">The renderable.</param>
		public static void RegisterRenderable(Layer layer, IRenderable renderable)
		{
			if (!_renderables.TryGetValue(layer, out List<IRenderable> value))
			{
				_renderables.Add(layer, [renderable]);
				return;
			}

			value.Add(renderable);
		}

		/// <summary>
		/// Unregisters the renderable from the layers.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="renderable">The renderable.</param>
		public static void UnregisterRenderable(Layer layer, IRenderable renderable)
		{
			if (!_renderables.TryGetValue(layer, out List<IRenderable> value))
			{
				return;
			}

			value.Remove(renderable);
			if (value.Count == 0)
			{
				_renderables.Remove(layer);
			}
		}

		/// <summary>
		/// Registers an instanced buffer taking into account <see cref="ParticleLibraryConfig.ParticleLimit"/>
		/// where the requested number of instances is divided by the corresponding setting's value.
		/// <list>
		/// <item><see cref="ParticleLimit.None"/> = 0</item>
		/// <item><see cref="ParticleLimit.Low"/> = 1/8</item>
		/// <item><see cref="ParticleLimit.Medium"/> = 1/4</item>
		/// <item><see cref="ParticleLimit.High"/> = 1/2</item>
		/// <item><see cref="ParticleLimit.Unlimited"/> = 1/1</item>
		/// </list>
		/// </summary>
		/// <typeparam name="TVertex">The vertex struct.</typeparam>
		/// <typeparam name="TInstance">The instance struct.</typeparam>
		/// <param name="buffer">The buffer.</param>
		/// <param name="maxInstances">The maxiumum expected instances.</param>
		/// <returns>The actual value of instances to use, given the initial value and calculated depending on <see cref="ParticleLibraryConfig.ParticleLimit"/>.</returns>
		internal static int RegisterBuffer<TVertex, TInstance>(IInstancedBuffer<TVertex, TInstance> buffer, int maxInstances)
			where TVertex : struct
			where TInstance : struct
		{
			if (_buffers.Contains(buffer))
			{
				return 0;
			}

			_buffers.Add(buffer);

			return ParticleLibraryConfig.Instance.ParticleLimit switch
			{
				ParticleLimit.None => 0,
				ParticleLimit.Low => maxInstances / 8,
				ParticleLimit.Medium => maxInstances / 4,
				ParticleLimit.High => maxInstances / 2,
				ParticleLimit.Unlimited => maxInstances,
				_ => maxInstances / 4
			};
		}
	}
}