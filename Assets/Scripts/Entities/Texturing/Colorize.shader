Shader "Hidden/Colorize"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" { }
        _MixTex ("Mix Texture", 2D) = "white" { }

        _HueShift ("Hue Shift", Range(0, 360)) = 0
        _Multiplication ("Multiplication", Range(0, 2)) = 1
        _Contrast ("Contrast", Range(0, 2)) = 1
        _Saturation ("Saturation", Range(0, 2)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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
            sampler2D _MainTex;
            sampler2D _MixTex;
            float _HueShift;
            float _Multiplication;
            float _Contrast;
            float _Saturation;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


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
            
            void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Contrast_float(float3 In, float Contrast, out float3 Out)
            {
                float midpoint = pow(0.5, 2.2);
                Out =  (In - midpoint) * Contrast + midpoint;
            }
            
            void Unity_Saturation_float(float3 In, float Saturation, out float3 Out)
            {
                float luma = dot(In, float3(0.2126729, 0.7151522, 0.0721750));
                Out =  luma.xxx + Saturation.xxx * (In - luma.xxx);
            }


            float4 frag(v2f i) : SV_Target
            {
                float4 mainTexColor = tex2D(_MainTex, i.uv);
                float4 mixTexColor = tex2D(_MixTex, i.uv);
                
                // Hue
                Unity_Hue_Degrees_float(mixTexColor.rgb, _HueShift, mixTexColor.rgb);
                // Multiplication
                Unity_Multiply_float3_float3(mixTexColor.rgb, _Multiplication, mixTexColor.rgb);
                // Contrast
                Unity_Contrast_float(mixTexColor.rgb, _Contrast, mixTexColor.rgb);
                // Saturation
                Unity_Saturation_float(mixTexColor.rgb, _Saturation, mixTexColor.rgb);


                float4 outColor = mainTexColor;
                outColor.rgb = lerp(mainTexColor.rgb, mixTexColor.rgb, mixTexColor.a);
                outColor.a = max(mainTexColor.a, mixTexColor.a);
                return outColor;
            }

            ENDCG
        }
    }
}