Shader "Hidden/VertexColorBakingAmbientOcclusion" {
	SubShader { Pass {
		cull off
		fog {mode off}
		CGPROGRAM 
		#pragma vertex vert 
		#pragma fragment frag
		half4 vert(half4 vertexPos : POSITION) : SV_POSITION { return UnityObjectToClipPos(vertexPos); }
		half4 frag(void) : COLOR { return (0,0,0,0); }
		ENDCG
	} }
}
