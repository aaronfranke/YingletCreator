Shader "CharacterCompositor/Colorize"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" { }
        _GrayscaleTex ("Mix Texture", 2D) = "white" { }

        _Color("Main Color", Color) = (0,.75,.75,1)
        _DarkColor("Dark Color", Color) = (0,.25,.25,1)

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
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_GrayscaleTex); SAMPLER(sampler_GrayscaleTex);
            float4 _Color;
            float4 _DarkColor;

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
                float4 grayscaleColor = SAMPLE_TEXTURE2D(_GrayscaleTex, sampler_GrayscaleTex, i.uv);

                return GrayscaleToColored(mainTexColor, grayscaleColor, _Color, _DarkColor);


            }

            ENDHLSL
        }
    }
}