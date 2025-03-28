Shader "CharacterCompositor/ColorizeWithMask"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" { }
        _MixTex ("Mix Texture", 2D) = "white" { }
        _MaskTex ("Mask Texture", 2D) = "white" { }

        _HueShift ("Hue Shift", Range(0, 360)) = 0
        _Multiplication ("Multiplication", Range(0, 2)) = 1
        _Contrast ("Contrast", Range(0, 2)) = 1
        _Saturation ("Saturation", Range(0, 2)) = 1
        _ContrastMidpoint ("Contrast Midpoint", Color) = (0.5, 0.5, 0.5, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            Name "Universal Forward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "ColorizeUtils.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            // Properties
            // Textures and Samplers
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_MixTex); SAMPLER(sampler_MixTex);
            TEXTURE2D(_MaskTex); SAMPLER(sampler_MaskTex);
            float _HueShift;
            float _Multiplication;
            float _Contrast;
            float _Saturation;
            float4 _ContrastMidpoint;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 mainTexColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float4 mixTexColor = SAMPLE_TEXTURE2D(_MixTex, sampler_MixTex, i.uv);
                float4 maskTexColor = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
                
                // Contrast
                Modified_Contrast_float(mixTexColor.rgb, _Contrast, _ContrastMidpoint.rgb, mixTexColor.rgb);
                // Hue
                Unity_Hue_Degrees_float(mixTexColor.rgb, _HueShift, mixTexColor.rgb);
                // Multiplication
                Unity_Multiply_float3_float3(mixTexColor.rgb, _Multiplication, mixTexColor.rgb);
                // Saturation
                Unity_Saturation_float(mixTexColor.rgb, _Saturation, mixTexColor.rgb);


                float4 outColor = mainTexColor;
                mixTexColor.a = mixTexColor.a * maskTexColor.a;
                outColor.rgb = lerp(mainTexColor.rgb, mixTexColor.rgb, mixTexColor.a);
                outColor.a = max(mainTexColor.a, mixTexColor.a);
                return outColor;
            }

            ENDHLSL
        }
    }
}