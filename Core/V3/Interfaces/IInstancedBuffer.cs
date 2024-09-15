using System;

namespace ParticleLibrary.Core.V3.Interfaces
{
	public interface IInstancedBuffer<TVertex, TInstance> : IBuffer, IRenderable, IDisposable
		where TVertex : struct
		where TInstance : struct
	{
	}
}
