uniform extern float4x4 World;
uniform extern float4x4 View;
uniform extern float4x4 Projection;
uniform extern float4x4 WorldTransform;

uniform extern texture SpriteTexture;

struct VS_INPUT
{
	float4 Position : POSITION;
	float Size : PSIZE;
	float Rotation : TEXCOORD;
	float4 Color : COLOR0;
};

struct VS_OUTPUT
{
	float4 Position : POSITION;
	float Size : PSIZE;
	float Rotation : TEXCOORD;
	float4 Color : COLOR0;
};

struct PS_INPUT
{
	float4 Color : COLOR;
	float Rotation : TEXCOORD;

    #ifdef XBOX
        float2 TexCoord : SPRITETEXCOORD;
    #else
        float2 TexCoord : TEXCOORD1;
    #endif       
};

sampler Sampler = sampler_state
{
    Texture = <SpriteTexture>;
    
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    MipFilter = LINEAR;
    
    AddressU = CLAMP;
    AddressV = CLAMP;
};                        

VS_OUTPUT VertexShader(VS_INPUT input)
{
	VS_OUTPUT output = (VS_OUTPUT)0;
	
	float4x4 worldTransformed = mul(World, WorldTransform);
	float4x4 worldView = mul(worldTransformed, View);
	float4x4 worldViewProjection = mul(worldView, Projection);
	
	output.Position = mul(input.Position, worldViewProjection);
	output.Color = input.Color;
	output.Size = input.Size;
	output.Rotation = input.Rotation;

	return output;
}

float4 PixelShader_1_1(PS_INPUT input) : COLOR0
{
	return tex2D(Sampler, input.TexCoord) * input.Color;
}

technique PointSprites_1_1
{
	pass P0
	{
		vertexShader = compile vs_1_1 VertexShader();
		pixelShader = compile ps_1_1 PixelShader_1_1();
	}
}