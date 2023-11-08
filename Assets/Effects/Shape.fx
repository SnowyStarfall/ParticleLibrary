#define VS_SHADERMODEL vs_2_0
#define PS_SHADERMODEL ps_2_0

#define PI 3.14159265

matrix UIScaleMatrix;

float4 Outline = float4(1, 0, 0, 1);
float2 Texel;

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
	float d = 0.5 - distance(input.TexCoords, float2(0.5, 0.5));
	
	return (d > 0 && d <= Texel.x) || (d >= 1 - Texel.x) ? Outline : input.Color * ceil(d);
}

float4 SoftCircleFunction(VertexShaderOutput input) : COLOR0
{
	float d = 0.5 - distance(input.TexCoords, float2(0.5, 0.5));
	
	return (d > 0 && d <= Texel.x) || (d >= 1 - Texel.x) ? Outline : input.Color * (Texel.x * 2);
}

float4 BoxFunction(VertexShaderOutput input) : COLOR0
{	
	float4 color = input.TexCoords.x <= Texel.x || input.TexCoords.x >= 1 - Texel.x || input.TexCoords.y <= Texel.y || input.TexCoords.y >= 1 - Texel.y ? Outline : input.Color;
	
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