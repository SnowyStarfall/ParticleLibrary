
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ParticleLibrary.Examples
{
	public class ExampleParticle : Particle
	{
		private float _startScale;

		public override void SetDefaults()
		{
			width = 16;
			height = 16;
			timeLeft = 120;
			tileCollide = false;
			SpawnAction = Spawn;
		}

		public override void AI()
		{
			Scale = _startScale * (timeLeft / 120f);

			velocity *= 0.96f;
		}

		public void Spawn()
		{
			Scale *= 0.5f;
			_startScale = Scale;
		}
	}
}
