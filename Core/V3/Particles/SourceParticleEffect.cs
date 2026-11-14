using Microsoft.Xna.Framework;
using Terraria;

namespace ParticleLibrary.Core.V3.Particles;

public class SourceParticleEffect
{
	/// <summary>
	/// Whether to offset by <see cref="Main.offScreenRange"/>
	/// </summary>
	public bool UseOffScreenRange { get; set; }

	public bool IsLoaded => Assets.Effects.InstancedParticle.Asset.IsLoaded;
		
	public void Apply(Vector2 size)
	{
		Vector2 offset = -Main.screenPosition;
		if (UseOffScreenRange)
		{
			// TODO: Breaks positioning
			// offset += new Vector2(Main.offScreenRange);
			UseOffScreenRange = false;
		}

		var shaderPass = Assets.Effects.SourceParticle.CreateShaderPass();
		shaderPass.Parameters.Transform = Main.GameViewMatrix.NormalizedTransformationMatrix;
		shaderPass.Parameters.Offset = offset;
		shaderPass.Parameters.Size = size;
		shaderPass.Apply();
	}
}