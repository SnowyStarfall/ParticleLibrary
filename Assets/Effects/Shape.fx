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

float RoundedRectangle(float2 centerPosition, float2 size, float radius)
{
	return length(max(abs(centerPosition) - (size / 2) + radius, 0)) - radius;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Convert our UV to pixel coordinates
	float2 position = float2(input.TexCoords.x * Size.x, input.TexCoords.y * Size.y);

    // Calculate distance to edge
	float distance = RoundedRectangle(position - (Size / 2), Size, Radius);

    // Clip pixels that are outside of our shape
	clip(-distance);
	
	// Return our outline color
	// IF
	// The distance is within the outline thickness range
	// ELSE
	// Return our base panel color
	return distance >= -OutlineThickness ? OutlineColor : input.Color;
}

technique DefaultTechnique
{
	pass CirclePass
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
};