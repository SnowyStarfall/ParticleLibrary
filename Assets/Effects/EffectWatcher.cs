using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using System.Threading;
using Terraria;

namespace ParticleLibrary.Assets.Effects
{
	public sealed class EffectWatcher : IDisposable
	{
		public event Action<Effect> OnUpdate;

		private static readonly SemaphoreSlim _semaphore = new(1, 1);

		private readonly string _additionalPath;
		private readonly string _fileName;
		private readonly string _documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		private readonly FileSystemWatcher _watcher;

		public EffectWatcher(string additionalPath, string fileName)
		{
			_additionalPath = additionalPath;
			_fileName = fileName;

			_watcher = new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $@"\My Games\Terraria\tModLoader\ModSources\ParticleLibrary\Assets\Effects\{_additionalPath}")
			{
				NotifyFilter = NotifyFilters.LastWrite
			};

			_watcher.Changed += OnChanged;

			_watcher.Filter = _fileName + ".xnb";
			_watcher.EnableRaisingEvents = true;
		}

		private async void OnChanged(object sender, FileSystemEventArgs e)
		{
			try
			{
				await _semaphore.WaitAsync().ConfigureAwait(false);

				FileStream stream = new(_documents + $@"\My Games\Terraria\tModLoader\ModSources\ParticleLibrary\Assets\Effects\{_additionalPath}{_fileName}.xnb", FileMode.Open, FileAccess.Read);
				var effect = Main.Assets.CreateUntracked<Effect>(stream, $"{_fileName}.xnb", AssetRequestMode.ImmediateLoad).Value;

				OnUpdate?.Invoke(effect);

				Main.NewText(_fileName + ": reloaded");
			}
			catch (Exception ex)
			{
			}
			finally
			{
				_semaphore.Release();
			}
		}

		public void Dispose()
		{
			_watcher.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
