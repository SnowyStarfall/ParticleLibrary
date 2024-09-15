namespace ParticleLibrary.Core.V3
{
	public abstract class Behavior<TInfo>
		where TInfo : struct
	{
		public abstract string Texture { get; }

		public abstract void Update(ref TInfo info);
	}
}
