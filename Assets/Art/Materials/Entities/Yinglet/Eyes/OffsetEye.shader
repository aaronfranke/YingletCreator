Shader "Custom/OffsetEye"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Outline ("Outline", 2D) = "white" {}
        _Pupil ("Pupil", 2D) = "white" {}
        _AlphaCutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _DepthOffset ("Depth Offset", Float) = 0.0
        _PupilOffsetX ("Pupil Offset X", Range(-0.35,0.2)) = 0.0
        _PupilOffsetY ("Pupil Offset Y", Range(-0.31,.22)) = 0.0
        _Expression ("Expression", Integer) = 1
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            

            
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_Outline); SAMPLER(sampler_Outline);
            TEXTURE2D(_Pupil); SAMPLER(sampler_Pupil);
            float _AlphaCutoff;
            float _DepthOffset;
            float _PupilOffsetX;
            float _PupilOffsetY;
            int _Expression;

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
                const int MAX_EXPRESSIONS = 3; 

                float2 pupilOffset = float2(_PupilOffsetX, _PupilOffsetY);
                float2 expressionUV = i.uv;
                expressionUV.x += _Expression;

                expressionUV.x /= MAX_EXPRESSIONS;

                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, expressionUV);
                float4 outline = SAMPLE_TEXTURE2D(_Outline, sampler_Outline, expressionUV);
                float4 pupil = SAMPLE_TEXTURE2D(_Pupil, sampler_Pupil, i.uv + pupilOffset);


                float4 col = mainTex;
                col.rgb = lerp(col.rgb, pupil.rgb, pupil.a);
                col.rgb = lerp(col.rgb, outline.rgb, outline.a);
                col.a = max(mainTex.a, outline.a);
                
                if (col.a < _AlphaCutoff)
                    discard;
                return col;
            }
            ENDHLSL
        }
    }
}
