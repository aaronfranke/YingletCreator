Shader "Custom/UnlitAlphaCutoff"
{
    Properties
    {
        _Fill ("Fill", 2D) = "white" {}
        _Outline ("Outline", 2D) = "white" {}
        _Pupil ("Pupil", 2D) = "white" {}
        _AlphaCutoff ("Alpha Cutoff", Range(0,1)) = 0.5
        _DepthOffset ("Depth Offset", Float) = 0.0
        _PupilOffsetX ("Pupil Offset X", Float) = 0.0
        _PupilOffsetY ("Pupil Offset Y", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog
            
            sampler2D _Fill;
            sampler2D _Outline;
            sampler2D _Pupil;
            float _AlphaCutoff;
            float _DepthOffset;
            float _PupilOffsetX;
            float _PupilOffsetY;

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
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                float camDist = length(o.worldPos.xyz - _WorldSpaceCameraPos);
                
                // Offset the depth just a bit, so it renders in front of the hair
                // This isn't actually a proper way of turning depth into distance
                // If we wanted to, we'd need more math like in this article:
                // https://www.cyanilux.com/tutorials/depth/#eye-depth
                o.vertex.z += (_DepthOffset / camDist) * o.vertex.w;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 fill = tex2D(_Fill, i.uv);
                fixed4 outline = tex2D(_Outline, i.uv);
                fixed2 pupilOffset = float2(_PupilOffsetX, _PupilOffsetY);
                fixed4 pupil = tex2D(_Pupil, i.uv + pupilOffset);

                fixed4 col = (0,0,0,0);
                col.rgb = lerp(col.rgb, fill.rgb, fill.a);
                col.rgb = lerp(col.rgb, pupil.rgb, pupil.a);
                col.rgb = lerp(col.rgb, outline.rgb, outline.a);
                col.a = max(fill.a, outline.a);
                
                if (col.a < _AlphaCutoff)
                    discard;
                return col;
            }
            ENDCG
        }
    }
}
