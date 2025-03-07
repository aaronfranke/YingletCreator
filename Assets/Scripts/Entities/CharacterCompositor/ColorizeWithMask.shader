Shader "Hidden/ColorizeWithMask"
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
            sampler2D _MainTex;
            sampler2D _MixTex;
            sampler2D _MaskTex;
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

            float4 frag(v2f i) : SV_Target
            {
                float4 mainTexColor = tex2D(_MainTex, i.uv);
                float4 mixTexColor = tex2D(_MixTex, i.uv);
                float4 maskTexColor = tex2D(_MaskTex, i.uv);
                
                // Hue
                Unity_Hue_Degrees_float(mixTexColor.rgb, _HueShift, mixTexColor.rgb);
                // Multiplication
                Unity_Multiply_float3_float3(mixTexColor.rgb, _Multiplication, mixTexColor.rgb);
                // Contrast
                Unity_Contrast_float(mixTexColor.rgb, _Contrast, mixTexColor.rgb);
                // Saturation
                Unity_Saturation_float(mixTexColor.rgb, _Saturation, mixTexColor.rgb);


                float4 outColor = mainTexColor;
                mixTexColor.a = mixTexColor.a * maskTexColor.a;
                outColor.rgb = lerp(mainTexColor.rgb, mixTexColor.rgb, mixTexColor.a);
                outColor.a = max(mainTexColor.a, mixTexColor.a);
                return outColor;
            }

            ENDCG
        }
    }
}