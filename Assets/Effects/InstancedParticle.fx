sampler Texture : register( s0 );

matrix Transform;
float2 Offset;

struct VertexShaderInput
{
	float2 Position : POSITION0;
	float2 Texture : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 Texture : TEXCOORD0;
};

struct Particle
{
	// Position: XY, Scale: ZW
	float4 Position_Scale : NORMAL0;
	// Rotation: X, Depth: Y
	float2 Rotation_Depth : NORMAL1;
	float4 Color : COLOR0;
};

float4x4 TranslationMatrix( float2 position )
{
	return float4x4(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        position.x, position.y, 0, 1
    );
}

float4x4 RotationMatrix( float rotation )
{
	float cosRoll = cos( rotation );
	float sinRoll = sin( rotation );
	return float4x4(
        cosRoll, sinRoll, 0, 0,
        -sinRoll, cosRoll, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );
}

float4x4 ScaleMatrix( float2 scale )
{
	return float4x4(
        scale.x, 0, 0, 0,
        0, scale.y, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );
}

VertexShaderOutput Vertex( VertexShaderInput input, Particle instance )
{
	VertexShaderOutput output;
	
	if ( !any( instance.Color ) )
	{
		output.Position = 0;
		output.Color = 0;
		output.Texture = 0;
		
		return output;
	}
	
	float4x4 rotation = RotationMatrix( instance.Rotation_Depth.x );
	float4x4 scale = ScaleMatrix( instance.Position_Scale.zw );
	float2 position = mul( mul( float4( input.Position, 0, 1 ), scale ), rotation );
	
	output.Position = mul( float4( position + instance.Position_Scale.xy + Offset, 0, 1 ), Transform );
	output.Position.w = instance.Rotation_Depth.y;
	output.Color = instance.Color;
	output.Texture = input.Texture;

	return output;
}

float4 Pixel( VertexShaderOutput input ) : COLOR0
{
	return tex2D( Texture, input.Texture ) * input.Color;
}

technique ShaderTechnique
{
	pass ShaderPass
	{
		VertexShader = compile vs_3_0 Vertex();
		PixelShader = compile ps_3_0 Pixel();
	}
};