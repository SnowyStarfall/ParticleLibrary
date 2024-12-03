using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ParticleLibrary.Core
{
	/// <summary>
	/// Base class for all particles. Inherit this class to create your own particle.
	/// </summary>
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Particles instead")]
	public abstract class Particle
	{
		/// <summary>
		/// Runs before <see cref="Spawn"/>.
		/// </summary>
		protected Particle()
		{
			Initialize();
		}

		/// <summary>
		/// Texture property for a particle. Override this to directly specify a texture.
		/// </summary>
		public virtual Texture2D Sprite { get; set; }

		/// <summary>
		/// Texture path for a particle. Override this to specify a custom path.
		/// </summary>
		public virtual string Texture => GetType().Namespace.Replace(".", "/") + "/" + GetType().Name;

		/// <summary>
		/// The expected visual bounds for this particle. Used for visual culling.
		/// X and Y are used for position offset. Width and Height are the size of the bounds.
		/// Defaults to null. The particle will never be culled.
		/// </summary>
		public virtual Rectangle? Bounds { get; set; } = null;

		/// <summary>
		/// The visual position taking into account Main.screenPosition.
		/// </summary>
		public Vector2 VisualPosition => AnchorPosition + Position - Main.screenPosition;

		/// <summary>
		/// The location of the particle.
		/// </summary>
		public Vector2 Position;
		/// <summary>
		/// How fast <see cref="Position"/> moves in world coordinates per frame.
		/// </summary>
		public Vector2 Velocity;
		/// <summary>
		/// How fast <see cref="Velocity"/> changes each frame.
		/// </summary>
		public Vector2 VelocityDeviation;
		/// <summary>
		/// How much <see cref="Velocity"/> is multiplied each frame. Defaults to (1, 1).
		/// </summary>
		public Vector2 VelocityAcceleration = new(1f);
		/// <summary>
		/// The reference position used for this particle when calculating its position. Defaults to (0, 0).
		/// </summary>
		public Vector2 AnchorPosition;

		/// <summary>
		/// Float representation of scale.
		/// </summary>
		public float Scale { get => Scale2D.X; set => Scale2D = new Vector2(value, value); }
		/// <summary>
		/// The scale of this particle.
		/// </summary>
		public Vector2 Scale2D;
		/// <summary>
		/// How much <see cref="Scale2D"/> increases each frame.
		/// </summary>
		public Vector2 ScaleVelocity;
		/// <summary>
		/// How much <see cref="ScaleVelocity"/> increases each frame.
		/// </summary>
		public Vector2 ScaleAcceleration = Vector2.One;

		/// <summary>
		/// The rotation of this particle.
		/// </summary>
		public float Rotation;
		/// <summary>
		/// How much <see cref="Rotation"/> changes each frame.
		/// </summary>
		public float RotationVelocity;
		/// <summary>
		/// How much <see cref="RotationVelocity"/> changes each frame.
		/// </summary>
		public float RotationAcceleration = 1f;

		/// <summary>
		/// Where the particle should be drawn.
		/// </summary>
		public Layer Layer = Layer.BeforeDust;
		/// <summary>
		/// The color of this particle.
		/// </summary>
		public Color Color;
		/// <summary>
		/// The opacity of this particle.
		/// </summary>
		public float Opacity = 1f;
		/// <summary>
		/// The frame of this particle.
		/// </summary>
		public Rectangle Frame;
		/// <summary>
		/// Whether this particle should collide with tiles.
		/// </summary>
		public bool TileCollide;
		/// <summary>
		/// The amount of frames this particle has left in its lifetime.
		/// </summary>
		public int TimeLeft;

		private void Initialize()
		{
			if (Main.netMode is not NetmodeID.Server && Sprite is null)
			{
				try
				{
					Sprite = ModContent.Request<Texture2D>(Texture).Value;
				}
				catch
				{
					Sprite = ParticleLibrary.EmptyPixel;
				}
			}
		}

		/// <summary>
		/// Runs when the particle spawns, AFTER constructors and AFTER <see cref="Position"/>, <see cref="Velocity"/>, <see cref="Color"/>, <see cref="Scale"/>, and <see cref="Layer"/> are set.
		/// </summary>
		public virtual void Spawn()
		{
		}

		/// <summary>
		/// Runs when the particle is updated.
		/// </summary>
		public virtual void Update()
		{
		}

		/// <summary>
		/// Renders the particle.
		/// </summary>
		/// <param name="spriteBatch">Provided SpriteBatch.</param>
		/// <param name="location">Draw position of the particle. This factors in Main.screenPosition.</param>
		/// <returns>bool</returns>
		public virtual void Draw(SpriteBatch spriteBatch, Vector2 location)
		{
			spriteBatch.Draw(Sprite, location, Frame, Color * Opacity, Rotation, Sprite.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
		}

		/// <summary>
		/// Runs when the particle dies.
		/// </summary>
		public virtual void Death()
		{
		}

		/// <summary>
		/// Runs when the particle collides with a tile.
		/// </summary>
		/// <param name="oldVelocity">The old velocity of the particle.</param>
		public virtual void TileCollision(Vector2 oldVelocity)
		{
		}

		/// <summary>
		/// Kills a particle.
		/// </summary>
		public void Kill()
		{
			Death();
			ParticleSystem._particlesToRemove?.Add(this);
			ParticleSystem.ParticleCount--;
		}
	}
}
