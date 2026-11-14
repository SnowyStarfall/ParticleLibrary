using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace ParticleLibrary.Core.V3.Particles
{
    public class InstancedParticleEffect
    {
        /// <summary>
        /// Whether to offset by <see cref="Main.offScreenRange"/>
        /// </summary>
        public bool UseOffScreenRange { get; set; }

        public bool IsLoaded => Assets.Effects.InstancedParticle.Asset.IsLoaded;
		
        public void Apply()
        {
            Vector2 offset = -Main.screenPosition;
            if (UseOffScreenRange)
            {
	            // TODO: Breaks positioning
	            // offset += new Vector2(Main.offScreenRange);
	            UseOffScreenRange = false;
            }

			var shaderPass = Assets.Effects.InstancedParticle.CreateShaderPass();
			shaderPass.Parameters.Transform = Main.GameViewMatrix.NormalizedTransformationMatrix;
			shaderPass.Parameters.Offset = offset;
			shaderPass.Apply();
        }
    }
}