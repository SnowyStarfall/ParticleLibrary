using Microsoft.Xna.Framework;
using ParticleLibrary.EmitterSystem;
using Terraria;

namespace ParticleLibrary.Examples
{
	public class ExampleEmitter : Emitter
	{
		private int _counter;
		public override void AI()
		{
			if (_counter == 10)
			{
				_counter = 0;
				ParticleManager.NewParticle<ExampleParticle>(position, Main.rand.NextVector2Unit() * 4f, new Color(1f, 1f, 1f, 1f), 1f);
			}

			_counter++;
		}
	}
}
