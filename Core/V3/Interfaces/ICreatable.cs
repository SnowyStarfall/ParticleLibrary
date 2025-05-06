namespace ParticleLibrary.Core.V3.Interfaces
{
	public interface ICreatable<TInfo>
		where TInfo : struct
	{
		void Create(TInfo info);
	}
}
