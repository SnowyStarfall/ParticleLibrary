namespace ParticleLibrary.Core.V3.Particles
{
    public abstract class Behavior<TInfo>
        where TInfo : struct
    {
        public abstract string Texture { get; }

        public abstract void Update(ref TInfo info);
    }
}
