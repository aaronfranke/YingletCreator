#ifndef COLORIZE_UTILS_INCLUDED
#define COLORIZE_UTILS_INCLUDED

////////////////////////////////////////////////////////////////////////////////////////////
//  Borrowed from unity
////////////////////////////////////////////////////////////////////////////////////////////
void Unity_Hue_Degrees_float(float3 In, float Offset, out float3 Out)
{
	// RGB to HSV
	float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
	float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
	float D = Q.x - min(Q.w, Q.y);
	float E = 1e-10;
	float V = (D == 0) ? Q.x : (Q.x + E);
	float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), V);

	float hue = hsv.x + Offset / 360;
	hsv.x = (hue < 0)
			? hue + 1
			: (hue > 1)
				? hue - 1
				: hue;

	// HSV to RGB
	float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
	Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
}

// Unity's hue implementation is frankly kind of shit
void Modified_Hue_Degrees_float(float3 In, float Offset, out float3 Out)
{
	Offset = -Offset / 180;
    float3x3 RGBtoYIQ = float3x3(
        0.299,  0.587,  0.114,
        0.596, -0.274, -0.322,
        0.211, -0.523,  0.311);
    
    float3x3 YIQtoRGB = float3x3(
        1.0,  0.956,  0.621,
        1.0, -0.272, -0.647,
        1.0, -1.106,  1.703);
    
    float3 yiq = mul(RGBtoYIQ, In);
    float angle = Offset * 3.14159265;
    float cosA = cos(angle);
    float sinA = sin(angle);
    
    float3x3 hueRotation = float3x3(
        1,      0,       0,
        0,  cosA,  -sinA,
        0,  sinA,   cosA);
    
    yiq = mul(hueRotation, yiq);
    Out = mul(YIQtoRGB, yiq);
}

void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
{
	Out = A * B;
}

void Unity_Contrast_float(float3 In, float Contrast, out float3 Out)
{
	// float midpoint = pow(0.5, 2.2);
	float midpoint = 0.217638f;
	Out =  (In - midpoint) * Contrast + midpoint;
}

void Modified_Contrast_float(float3 In, float Contrast, float3 Midpoint, out float3 Out)
{
	// Unity tries to find a nice generic midpoint to move towards
	// But it often times ends up being pretty far off from what we want
	// Let's pass our own one in
	Out =  (In - Midpoint) * Contrast + Midpoint;
}


void Unity_Saturation_float(float3 In, float Saturation, out float3 Out)
{
	float luma = dot(In, float3(0.2126729, 0.7151522, 0.0721750));
	Out =  luma.xxx + Saturation.xxx * (In - luma.xxx);
}

#endif // COLORIZE_UTILS_INCLUDED