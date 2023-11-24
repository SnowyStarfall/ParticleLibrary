#define PI 3.14159265
#define TAU 6.28318531

matrix TransformMatrix;

float Time;

float2 ScreenPosition;
float2 Offset;

bool Fade;

//float Gravity;
//float TerminalGravity;

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

// Quad Particle structs
struct GVertexShaderInput
{
	float4 Position : POSITION0;
	float4 TexCoords : TEXCOORD0;
		
	float4 StartColor : COLOR0;
	float4 EndColor : COLOR1;
	
	float4 Velocity : NORMAL0; // XY Velocity | ZW Acceleration
	float2 Acceleration : NORMAL1;
	float2 Size : NORMAL2;
	float4 Scale : NORMAL3; // XY Scale | ZW Velocity
	float4 Rotation : NORMAL4; // XY Corner | Z Rotation | W Velocity

	float4 DepthTime : NORMAL5; // X Depth | Y Velocity | Z Time
};

struct GVertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
	float4 Color : COLOR0;
};

// Point Particle structs
struct PVertexShaderInput
{
	float4 Position : POSITION0;
	
	float4 StartColor : COLOR0;
	float4 EndColor : COLOR1;
	
	float4 Velocity : NORMAL0; // XY Velocity | ZW Acceleration
	float2 Acceleration : NORMAL1;

	float4 DepthTime : NORMAL2; // X Depth | Y Velocity | Z Time
};

struct PVertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

// Vertex functions
float4 ComputePosition(float4 position, float2 velocity, float2 deviation, float2 acceleration, float time)
{
	// Displacement
	float2 d = (velocity * time);
	// Deviation
	float2 e = deviation * time;
	//// Gravity
	//float g = Gravity * time;
	// Acceleration
	float a = (1.0 - pow(abs(acceleration), time)) / (1.0 - abs(acceleration));
	
	return position + float4((velocity + e) * a, 0, 0);
}

float2 ComputeSize(float2 size, float2 scale, float2 velocity, float time)
{
	//float2 s = size * (scale + (velocity * time));
	float2 s = size * scale; // TODO: Fix. Bouncing issue returns when changing scale over time

	return s;
}

float4 ComputeColor(float4 start, float4 end, float age)
{
	float4 color = lerp(start, end, age);
	
	if (Fade)
		color *= 1.0 - age;
	
	return color;
}

float2x2 ComputeRotation(float2 rotation, float time)
{
	float r = rotation.x + (rotation.y * time);
	
	float c = cos(r);
	float s = sin(r);

	return float2x2(c, -s, s, c);
}

float ComputeDepth(float depth, float velocity, float time)
{
	float d = depth + (velocity * time);
	
	return d;
}

// Pixel functions
float4 PointParticle(GVertexShaderOutput input)
{
	float m = (0.5 - distance(input.TexCoords, float2(0.5, 0.5))) * 2;
	float n = pow(m, 15);
	
	return float4(n, n, n, 0) * input.Color;
}

float4 QuadDebug(GVertexShaderOutput input)
{
		// Quad debugging
	if (input.TexCoords.x < 0.1 && input.TexCoords.y < 0.1)
		return float4(1, 0, 0, 1);
	if (input.TexCoords.x > 0.9 && input.TexCoords.y < 0.1)
		return float4(1, 1, 0, 1);
	if (input.TexCoords.x < 0.1 && input.TexCoords.y > 0.9)
		return float4(0, 1, 0, 1);
	if (input.TexCoords.x > 0.9 && input.TexCoords.y > 0.9)
		return float4(0, 0, 1, 1);
	
	return float4(0, 0, 0, 0);
}

// Quad Particles
GVertexShaderOutput GVertexShaderFunction(GVertexShaderInput input)
{
	GVertexShaderOutput output;
	
	// Set up our reference variables
	
	// The total time elapsed since this particle spawned
	float time = Time - input.DepthTime.z;
	
	// Branching here is worth the performance gain
	if (time >= input.DepthTime.w && input.DepthTime.w != -1)
	{
		output.Position = 0;
		output.TexCoords = 0;
		output.Color = 0;
		
		return output;
	}

	// The normalized age of this particle
	float age = clamp(time / input.DepthTime.w, 0.0, 1.0);
	// The size at this point in the particle's life, multiplied by the corner to be an offset for the rotation
	float2 size = ComputeSize(input.Size, input.Scale.xy, input.Scale.zw, time) * input.Rotation.xy;
	// The rotation matrix. We only calculate when rotation is relevant
	float2x2 rotation = /*TODO: input.Rotation.z == 0 && input.Rotation.w == 0 ? float2x2(1, 0, 0, 1) :*/ComputeRotation(input.Rotation.zw, time);
	
	// Calculate the position over time
	output.Position = ComputePosition(input.Position, input.Velocity.xy, input.Velocity.zw, input.Acceleration, time);
	// Apply rotation over time. We only mul when rotation is relevant
	output.Position.xy += -size + ( /*TODO: input.Rotation.z == 0 && input.Rotation.w == 0 ? 0 : */mul(size, rotation));
	// Apply offset for when drawing on the same layer as water
	output.Position.xy += Offset;
	// Apply matrix and offset for screen view
	output.Position = mul(output.Position - float4(ScreenPosition, 0, 0), TransformMatrix);
	// Apply depth over time (automatically handled after this function returns)
	output.Position.w = ComputeDepth(input.DepthTime.x, input.DepthTime.y, time);

	// Assign the rest of the values
	output.TexCoords = input.TexCoords;
	output.Color = ComputeColor(input.StartColor, input.EndColor, age);
	
	return output;
}

float4 GPixelShaderFunction(GVertexShaderOutput input) : COLOR0
{
	float4 v = tex2D(TextureSampler, input.TexCoords);

	return v * input.Color;
	
	//return PointParticle(input);
	//return QuadDebug(input);
}

// Point Particles
PVertexShaderOutput PVertexShaderFunction(PVertexShaderInput input)
{
	PVertexShaderOutput output;
	
	// Set up our reference variables
	
	// The total time elapsed since this particle spawned
	float time = Time - input.DepthTime.z;
	
	// Branching here is worth the performance gain
	if (time >= input.DepthTime.w && input.DepthTime.w != -1)
	{
		output.Position = 0;
		output.Color = 0;
		
		return output;
	}

	// The normalized age of this particle
	float age = clamp(time / input.DepthTime.w, 0.0, 1.0);
	
	// Calculate the position over time
	output.Position = ComputePosition(input.Position, input.Velocity.xy, input.Velocity.zw, input.Acceleration, time);
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

float4 PPixelShaderFunction(PVertexShaderOutput input) : COLOR0
{
	return input.Color;
}


technique DefaultTechnique
{
	pass Quad
	{
		VertexShader = compile vs_2_0 GVertexShaderFunction();
		PixelShader = compile ps_2_0 GPixelShaderFunction();
	}
	pass Point
	{
		VertexShader = compile vs_2_0 PVertexShaderFunction();
		PixelShader = compile ps_2_0 PPixelShaderFunction();
	}
}