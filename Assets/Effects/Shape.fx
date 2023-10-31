#define VS_SHADERMODEL vs_2_0
#define PS_SHADERMODEL ps_2_0

#define PI 3.14159265

matrix UIScaleMatrix;

float4 Outline = float4(1, 0, 0, 1);
float Texel;

// Simple structs to make referencing data easier, as it's passed into our functions
struct VertexShaderInput
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;
	float4 Color : COLOR0;
};

float4 CircleFunction(VertexShaderOutput input) : COLOR0
{
	float m = 0.5 - distance(input.TexCoords, float2(0.5, 0.5));
	
	return (m > 0 && m <= Texel) || (m >= 1 - Texel) ? Outline : input.Color * ceil(m);
}

float4 SoftCircleFunction(VertexShaderOutput input) : COLOR0
{
	float m = 0.5 - distance(input.TexCoords, float2(0.5, 0.5));
	
	return (m > 0 && m <= Texel) || (m >= 1 - Texel) ? Outline : input.Color * (m * 2);
}

float4 BoxFunction(VertexShaderOutput input) : COLOR0
{	
	float4 color = input.TexCoords.x <= Texel || input.TexCoords.x >= 1 - Texel || input.TexCoords.y <= Texel || input.TexCoords.y >= 1 - Texel ? Outline : input.Color;
	
	return color;
}

technique DefaultTechnique
{
	pass CirclePass
	{
		PixelShader = compile PS_SHADERMODEL CircleFunction();
	}
	pass SoftCirclePass
	{
		PixelShader = compile PS_SHADERMODEL SoftCircleFunction();
	}
	pass BoxPass
	{
		PixelShader = compile PS_SHADERMODEL BoxFunction();
	}
};