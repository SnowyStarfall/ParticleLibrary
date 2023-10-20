using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System.Drawing;
using Terraria.ModLoader.IO;

namespace ParticleLibrary.Core
{
	public class EmitterSettings
	{
		/// <summary>
		/// The shape of the emitter.
		/// </summary>
		public EmitterShape Shape { get; private set; }

		/// <summary>
		/// How the emitter distributes particles.
		/// </summary>
		public EmitterOrigin Origin { get; private set; }

		/// <summary>
		/// The position of the emitter, originating from the center.
		/// </summary>
		public Vector2 Position
		{
			get => _position;
			set
			{
				_position = value;
				RecalculateBounds();
			}
		}
		private Vector2 _position;

		public float X
		{
			get => _position.X;
			set 
			{ 
				_position.X = value;
				RecalculateBounds();
			}
		}

		public float Y
		{
			get => _position.Y;
			set
			{
				_position.Y = value;
				RecalculateBounds();
			}
		}

		/// <summary>
		/// The width of the <see cref="Shape"/>. 
		/// No effect when used with <see cref="EmitterShape.Point"/>. 
		/// Used as diameter for <see cref="EmitterShape.Circle"/>
		/// Used as side length for <see cref="EmitterShape.Rectangle"/>
		/// </summary>
		public float Width
		{
			get => _width;
			set
			{
				if (value < 1)
					value = 1;
				_width = value;
				RecalculateBounds();
			}
		}
		private float _width = 32f;

		/// <summary>
		/// The height of the <see cref="Shape"/>. 
		/// No effect when used with <see cref="EmitterShape.Point"/>. 
		/// Used as diameter for <see cref="EmitterShape.Circle"/>
		/// Used as side length for <see cref="EmitterShape.Rectangle"/>
		/// </summary>
		public float Height
		{
			get => _height;
			set
			{
				if (value < 1)
					value = 1;
				_height = value;
				RecalculateBounds();
			}
		}
		private float _height = 32f;

		/// <summary>
		/// How far offscreen the emitter's bounds can be before its update method is no longer called.
		/// </summary>
		public float Padding
		{
			get => _padding;
			set
			{
				if (value < 0)
					value = 0;
				_padding = value;
				RecalculateBounds();
			}
		}
		private float _padding = 64f;

		/// <summary>
		/// The bounds of the emitter. Used for culling.
		/// </summary>
		public RectangleF Bounds { get => _bounds; }
		private RectangleF _bounds;

		/// <summary>
		/// The minimum interval between particle spawns.
		/// </summary>
		public int MinimumInterval { get; set; } = 10;
		/// <summary>
		/// The maximum interval between particle spawns.
		/// </summary>
		public int MaximumInterval { get; set; } = 10;

		/// <summary>
		/// The minimum amount of particles to spawn at once.
		/// </summary>
		public int MinimumSpawns { get; set; } = 1;
		/// <summary>
		/// The maximum amount of particles to spawn at once.
		/// </summary>
		public int MaximumSpawns { get; set; } = 1;

		/// <summary>
		/// Custom string Data for this emitter.
		/// </summary>
		public string Data { get; set; }

		/// <summary>
		/// Whether this emitter should save when the world is exited.
		/// </summary>
		public bool Save { get; set; } = false;

		public EmitterSettings()
		{
			_bounds = new(_position.X, _position.Y, _width, _height);
		}

		public void RecalculateBounds()
		{
			_bounds.X = _position.X - _width / 2f;
			_bounds.Y = _position.Y - _height / 2f;
			_bounds.Width = _width;
			_bounds.Height = _height;
		}

		public void SetPosition(float x, float y)
		{
			_position.X = x;
			_position.Y = y;

			RecalculateBounds();
		}

		internal void SaveData(TagCompound tag)
		{
			tag.Set("Shape", Shape);
			tag.Set("Origin", Origin);
			tag.Set("Position", Position);
			tag.Set("Size", new Vector3(Width, Height, Padding));
			tag.Set("Interval", new Vector2(MinimumInterval, MaximumInterval));
			tag.Set("Spawns", new Vector2(MinimumSpawns, MaximumSpawns));
			tag.Set("Data", Data);
			tag.Set("Save", Save);
		}

		internal void LoadData(TagCompound tag)
		{
			Shape = tag.Get<EmitterShape>("Shape");
			Origin = tag.Get<EmitterOrigin>("Origin");
			Position = tag.Get<Vector2>("Position");

			Vector3 size = tag.Get<Vector3>("Size");
			Width = size.X;
			Height = size.Y;
			Padding = size.Z;

			Vector2 interval = tag.Get<Vector2>("Interval");
			MinimumInterval = (int)interval.X;
			MaximumInterval = (int)interval.Y;

			Vector2 spawns = tag.Get<Vector2>("Spawns");
			MinimumSpawns = (int)spawns.X;
			MaximumSpawns = (int)spawns.Y;

			Data = tag.GetString("Data");
			Save = tag.GetBool("Save");
		}
	}
}
