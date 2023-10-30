using System.Collections.Generic;

namespace ParticleLibrary.Core.EmitterSystem.SpatialHash
{
	/// <summary>
	/// Hashes an [int, int] coordinate into a UInt32, and wraps it with an Emitter to create fast lookup times.
	/// Sourced from Nez <see href="https://github.com/prime31/Nez/blob/master/Nez.Portable/Utils/Collections/FastList.cs"/>
	/// </summary>
	internal class IntIntDictionary
	{
		private Dictionary<long, List<Emitter>> _store = new();

		public void Add(int x, int y, List<Emitter> list)
		{
			_store.Add(GetKey(x, y), list);
		}

		public void Remove(Emitter obj)
		{
			//foreach (var list in _store.Values)
			//{
			//	if (list.Contains(obj))
			//		list.Remove(obj);
			//}

			long key = GetKey((int)(obj.EmitterSettings.X / 128), (int)(obj.EmitterSettings.Y / 128));
			if(_store.ContainsKey(key))
			{
				_store[key].Remove(obj);
			}
		}

		public void Clear()
		{
			_store.Clear();
		}

		public bool TryGetValue(int x, int y, out List<Emitter> list)
		{
			return _store.TryGetValue(GetKey(x, y), out list);
		}

		public HashSet<Emitter> GetAllObjects()
		{
			var set = new HashSet<Emitter>();

			foreach (var list in _store.Values)
				set.UnionWith(list);

			return set;
		}

		private long GetKey(int x, int y)
		{
			return unchecked((long)x << 32 | (uint)y);
		}
	}
}
