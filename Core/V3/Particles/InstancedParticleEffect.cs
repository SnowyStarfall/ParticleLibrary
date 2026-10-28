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
            set => _textureParameter.SetValue(value);
        }
        
        /// <summary>
        /// Whether to offset by <see cref="Main.offScreenRange"/>
        /// </summary>
        public bool UseOffScreenRange { get; set; }
        
        private Asset<Effect> _effect;
        private EffectParameter _textureParameter;
        private EffectParameter _transformParameter;
        private EffectParameter _offsetParameter;
        
        public bool IsLoaded => _effect.IsLoaded;
        
        public InstancedParticleEffect()
        {
            _effect = ModContent.Request<Effect>(Resources.Assets.Effects.InstancedParticle);
        }
        
        internal void Setup()
        {
            _transformParameter = _effect.Value.Parameters["Transform"];
            _offsetParameter = _effect.Value.Parameters["Offset"];
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
            _effect.Value.CurrentTechnique.Passes[0].Apply();
        }
    }
}