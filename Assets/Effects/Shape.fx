#define PI 3.14159265

matrix UIScaleMatrix;

float4 OutlineColor = float4(1, 0, 0, 1);
float OutlineThickness;
float Radius;
float2 Size;

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

float RoundedRectangle(float2 center, float2 size, float radius)
{
	// Debug calculations to see what the code is returning
	
	// length(max(abs([64, 64]) - ([128, 128] / 2) + 0, 0)) - 0;
	// length(max([64, 64] - [64, 64] + 0, 0)) - 0;
	// length(0) - 0;
	// 0
	
	// length(max(abs([64, 64]) - ([128, 128] / 2) + 32, 0)) - 32;
	// length(max([64, 64] - [64, 64] + 32, 0)) - 32;
	// length(32) - 32;
	// 45.3 - 32; 
	// 13.3
	
	// length(max(abs([64, 64]) - ([128, 128] / 2) + 2, 0)) - 2;
	// length(max([64, 64] - [64, 64] + 2, 0)) - 2;
	// length(34) - 32;
	// 48.1 - 32; 
	// 16.1
	
	return length(max(abs(center) - (size / 2) + radius, 0)) - radius;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Convert our UV to pixel coordinates
	float2 position = float2(input.TexCoords.x * Size.x, input.TexCoords.y * Size.y);

    // Calculate distance to edge
	float distance = RoundedRectangle(position - (Size / 2), Size, Radius);

    // Clip pixels that are outside of our shape
	clip(-distance);
	
	// Return our colors
	if (Radius > 0)
	{
		return distance <= -OutlineThickness ? input.Color : OutlineColor;
	}
	else
	{
		return position.x <= OutlineThickness || position.x >= Size.x - OutlineThickness ||
			   position.y <= OutlineThickness || position.y >= Size.y - OutlineThickness ?
			   OutlineColor :
			   input.Color;
	}
}

technique DefaultTechnique
{
	pass CirclePass
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
};