using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary.Assets.Effects;
using ParticleLibrary.UI.Primitives;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary.Core.V3
{
	public class InstancedParticleEffect
	{
		public Texture2D Texture
		{
			set
			{
				if (value is not null)
				{
					_textureParameter.SetValue(value);
				}
			}
		}

		private Effect _effect;
		private EffectParameter _textureParameter;
		private EffectParameter _transformParameter;
		private EffectParameter _offsetParameter;

		private readonly EffectWatcher _watcher;

		public InstancedParticleEffect()
		{
			_effect = ModContent.Request<Effect>(Resources.Assets.Effects.InstancedParticle, AssetRequestMode.ImmediateLoad).Value;
			_transformParameter = _effect.Parameters["Transform"];
			_offsetParameter = _effect.Parameters["Offset"];

			if (ParticleLibrary.Debug)
			{
				_watcher = new("", "InstancedParticle");
				_watcher.OnUpdate += Update;
			}
		}

		public void Apply()
		{
			_transformParameter.SetValue(PrimitiveSystem.WorldViewProjection);
			_offsetParameter.SetValue(-Main.screenPosition);
			_effect.CurrentTechnique.Passes[0].Apply();
		}

		private void Update(Effect obj)
		{
			_effect = obj;
			_transformParameter = obj.Parameters["Transform"];
			_offsetParameter = obj.Parameters["Offset"];
		}
	}
}