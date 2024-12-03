using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleLibrary.Core
{
	[Obsolete("This type is obsolete, use ParticleLibrary.Core.V3.Particles instead")]
	public interface IGPUParticleSystem<T> where T : class
	{
		void Draw(Layer layer = Layer.None);

		void NewParticle(Vector2 position, Vector2 velocity, T particle, int? lifespan = null);

		// Setters
		/// <summary>
		/// Sets the texture to use for the particles.
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException"></exception>
		void SetTexture(Texture2D value);

		/// <summary>
		/// Sets the lifespan of the particles.
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		void SetLifespan(int value);

		/// <summary>
		/// Sets the position in the draw order to draw the particles on.
		/// </summary>
		/// <param name="value"></param>
		void SetLayer(Layer value);

		/// <summary>
		/// Sets the BlendState to use when drawing particles.
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException"></exception>
		void SetBlendState(BlendState value);

		/// <summary>
		/// Sets whether the particles should fade over their lifespan
		/// </summary>
		/// <param name="value"></param>
		void SetFade(bool value);
	}
}
