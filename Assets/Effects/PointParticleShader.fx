
matrix TransformMatrix;
float Time;
float2 ScreenPosition;
float Lifespan;
bool Fade;
float Gravity;
float TerminalGravity;
float2 Offset;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	
	float4 StartColor : COLOR0;
	float4 EndColor : COLOR1;
	
	float4 Velocity : NORMAL0; // XY Velocity | ZW Acceleration
	float Size : PSIZE0; // X Size

	float3 DepthTime : NORMAL1; // X Depth | Y Velocity | Z Time
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

float4 ComputePosition(float4 position, float2 velocity, float2 acceleration, float time)
{
	// Displacement
	float2 d = (velocity * time);
	// Acceleration
	float2 a = (0.5 * acceleration * pow(time, 2));
	// Gravity
	float g = (0.5 * Gravity * pow(time, 2));

	return position + float4(d + a + float2(0, g), 0, 0);
}

float4 ComputeColor(float4 start, float4 end, float age)
{
	float4 color = lerp(start, end, age);
	
	if (Fade)
		color *= 1.0 - age;
	
	return color;
}

float ComputeDepth(float depth, float velocity, float time)
{
	float d = depth + (velocity * time);
	
	return d;
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	
	// Set up our reference variables
	
	// The total time elapsed since this particle spawned
	float time = Time - input.DepthTime.z;
	
	// Branching here is worth the performance gain
	if (time >= Lifespan)
	{
		output.Position = 0;
		output.Color = 0;
		
		return output;
	}

	// The normalized age of this particle
	float age = clamp(time / Lifespan, 0.0, 1.0);
	
	// Calculate the position over time
	output.Position = ComputePosition(input.Position, input.Velocity.xy, input.Velocity.zw, time);
	// Apply offset for when drawing on the same layer as water
	output.Position.xy += Offset;
	// Apply matrix and offset for screen view
	output.Position = mul(output.Position - float4(ScreenPosition, 0, 0), TransformMatrix);
	// Apply depth over time (automatically handled after this function returns)
	output.Position.w = ComputeDepth(input.DepthTime.x, input.DepthTime.y, time);

	// Assign the rest of the values
	output.Color = ComputeColor(input.StartColor, input.EndColor, age);
	
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return input.Color;
}

technique DefaultTechnique
{
	pass Particles
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}