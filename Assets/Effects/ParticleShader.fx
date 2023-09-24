// View parameters.
matrix TransformMatrix;

// Current time in frames.
texture Texture;
sampler TextureSampler = sampler_state
{
	Texture = (Texture);
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 TexCoords : TEXCOORD0;
	float4 Color : COLOR0;
	
	float2 Velocity : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
	float4 Color : COLOR0;
};

// Custom vertex shader animates particles entirely on the GPU.
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	
	output.Position = mul(input.Position, TransformMatrix) + float4(input.Velocity.x, input.Velocity.y, 0, 1);
	output.TexCoords = input.TexCoords;
	output.Color = input.Color;
	
	return output;
}

// Pixel shader for drawing particles.
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 v = tex2D(TextureSampler, input.TexCoords);

	return input.Color;
}


technique DefaultTechnique
{
	pass Update
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
	}
	pass Render
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}