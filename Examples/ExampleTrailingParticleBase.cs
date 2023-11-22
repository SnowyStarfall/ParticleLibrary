using Microsoft.Xna.Framework;
using System;

namespace ParticleLibrary.Examples
{
	/// <summary>
	/// This class demonstrates how you can centralize certain functionalities instead of copy-pasting the same code everywhere.
	/// In the previous major version of this library, there were arrays to handle things like old positions, but they have since been removed.
	/// This shows how you can reimplement that feature in a way that is maintainable.
	/// </summary>
	public abstract class ExampleTrailingParticleBase : Core.Particle
	{
		public Vector2[] OldPositions { get; private set; }

		/// <summary>
		/// See <see cref="ExampleParticle"/> for the reason why two constructors is necessary.
		/// </summary>
		/// <param name="length"></param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public ExampleTrailingParticleBase(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length));
			}

			OldPositions = new Vector2[length];
		}
		public ExampleTrailingParticleBase() { }

		public override void Update()
		{
			for(int i = OldPositions.Length - 1; i >= 0; i--)
			{
				if (i != 0)
				{
					OldPositions[i] = OldPositions[i - 1];
				}
				else
				{
					OldPositions[i] = Position;
				}
			}
		}
	}
}
