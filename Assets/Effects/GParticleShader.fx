matrix TransformMatrix;
float Time;
float2 ScreenPosition;
float Lifespan;

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
	
	float4 StartColor : COLOR0;
	float4 EndColor : COLOR1;
	
	float4 Velocity : NORMAL0;
	float TimeOfAdd : NORMAL1;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
	float4 Color : COLOR0;
};

float4 ComputePosition(float4 position, float2 velocity, float2 acceleration, float time)
{
	float2 displacement = (velocity * time) + (0.5 * acceleration * pow(time, 2));
	displacement -= ScreenPosition;

	return position + float4(displacement, 0, 0);
}

float4 ComputeColor(float4 start, float4 end, float age)
{
	return lerp(start, end, age);
}

	// Custom vertex shader animates particles entirely on the GPU.
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	
	float time = Time - input.TimeOfAdd;
	float age = clamp(time / Lifespan, 0.0, 1.0);
	
	output.Position = mul(ComputePosition(input.Position, input.Velocity.xy, input.Velocity.zw, time), TransformMatrix);
	output.TexCoords = input.TexCoords;
	output.Color = ComputeColor(input.StartColor, input.EndColor, age);
	
	return output;
}

	// Pixel shader for drawing particles.
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 v = tex2D(TextureSampler, input.TexCoords);

	return v * input.Color;
}


technique DefaultTechnique
{
	pass Particles
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
	pass Update
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
	}
	pass Render
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}