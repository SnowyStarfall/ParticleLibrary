using System;

namespace ParticleLibrary.Core.V3.Interfaces
{
	public interface IInstancedBuffer<TVertex, TInstance> : IBuffer, IRenderable
		where TVertex : struct
		where TInstance : struct
	{
	}
}
