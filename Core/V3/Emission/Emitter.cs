using Microsoft.Xna.Framework;

namespace ParticleLibrary.Core.V3.Emission
{
    /// <summary>
    /// Base class for all emitters. Inherit this class to create your own emitter.
    /// </summary>
    public abstract class Emitter
    {
		/// <summary>
		/// Originating mod.
		/// </summary>
		public string Assembly { get; }

		/// <summary>
		/// Originating type.
		/// </summary>
		public string Type { get; }

        /// <summary>
        /// Position of the emitter.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The size of the emitter. Used for culling.
        /// </summary>
        public Point Size;

        public Emitter(Vector2 position, Point size)
        {
            Assembly = GetType().Assembly.GetName().Name;
            Type = GetType().FullName;
            Position = position;
            Size = size;

            Initialize();
		}

        /// <summary>
        /// Runs on instantiation.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Runs on PreUpdateWorld.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Kills this emitter.
        /// </summary>
        public void Kill()
        {
            EmitterManagerV3.Remove(this);
        }
    }
}
