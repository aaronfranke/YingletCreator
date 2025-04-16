Shader "Custom/Unlit-Offset"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DepthOffset ("Depth Offset", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            

            
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            float _DepthOffset;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                float camDist = length(o.worldPos.xyz - _WorldSpaceCameraPos);
                
                // Offset the depth just a bit, so it renders in front of the hair
                // This isn't actually a proper way of turning depth into distance
                // If we wanted to, we'd need more math like in this article:
                // https://www.cyanilux.com/tutorials/depth/#eye-depth
                o.vertex.z += _DepthOffset * 1 / (camDist * camDist) * o.vertex.w;
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float4 col = mainTex;
                return col;
            }
            ENDHLSL
        }
    }
}
