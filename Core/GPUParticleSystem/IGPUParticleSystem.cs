using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Renderers;

namespace ParticleLibrary.Core
{
	public interface IGPUParticleSystem<T> where T : class
	{
		public void Draw(Layer layer = Layer.None);

		public void NewParticle(Vector2 position, Vector2 velocity, T particle, int? lifespan = null);

		// Setters
		/// <summary>
		/// Sets the texture to use for the particles.
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public void SetTexture(Texture2D value);

		/// <summary>
		/// Sets the lifespan of the particles.
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public void SetLifespan(int value);

		/// <summary>
		/// Sets the position in the draw order to draw the particles on.
		/// </summary>
		/// <param name="value"></param>
		public void SetLayer(Layer value);

		/// <summary>
		/// Sets the BlendState to use when drawing particles.
		/// </summary>
		/// <param name="value"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public void SetBlendState(BlendState value);

		/// <summary>
		/// Sets whether the particles should fade over their lifespan
		/// </summary>
		/// <param name="value"></param>
		public void SetFade(bool value);

		/// <summary>
		/// Sets the gravity to apply to the particles
		/// </summary>
		/// <param name="value"></param>
		public void SetGravity(float value);

		/// <summary>
		/// Sets the maximum amount of velocity a particle should recieve from gravity. Currently unused for now
		/// </summary>
		/// <param name="value"></param>
		public void SetTerminalGravity(float value);
	}
}
