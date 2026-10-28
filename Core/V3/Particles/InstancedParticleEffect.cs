using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary.Core.V3.Particles
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

        /// <summary>
        /// Whether to offset by <see cref="Main.offScreenRange"/>
        /// </summary>
        public bool UseOffScreenRange { get; set; }

        private Effect _effect;
        private EffectParameter _textureParameter;
        private EffectParameter _transformParameter;
        private EffectParameter _offsetParameter;
        
        public InstancedParticleEffect()
        {
            _effect = ModContent.Request<Effect>(Resources.Assets.Effects.InstancedParticle, AssetRequestMode.ImmediateLoad).Value;
            _transformParameter = _effect.Parameters["Transform"];
            _offsetParameter = _effect.Parameters["Offset"];
        }
        
        public void Apply()
        {
            Vector2 offset = -Main.screenPosition;
            if (UseOffScreenRange)
            {
	            // TODO: Breaks positioning
	            // offset += new Vector2(Main.offScreenRange);
	            UseOffScreenRange = false;
            }
            
            _transformParameter.SetValue(Main.GameViewMatrix.NormalizedTransformationMatrix);
            _offsetParameter.SetValue(offset);
            _effect.CurrentTechnique.Passes[0].Apply();
        }
    }
}