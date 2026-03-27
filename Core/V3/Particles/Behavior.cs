namespace ParticleLibrary.Core.V3.Particles
{
    public abstract class Behavior<TInfo>
        where TInfo : struct
    {
        /// <summary>
        /// The texture for this behavior.
        /// </summary>
        public abstract string Texture { get; }

        /// <summary>
        /// Called once upon creation.
        /// </summary>
        /// <param name="info">The <see cref="TInfo"/>'s data.</param>
        public virtual void Initialize(ref TInfo info)
        {
            // This is only virtual because:
            // a) not everyone needs it
            // b) it would explode all dependent mods if it was abstract
            // c) b
        }

        /// <summary>
        /// Called every frame.
        /// </summary>
        /// <param name="info">The <see cref="TInfo"/>'s data.</param>
        public abstract void Update(ref TInfo info);
    }
}
