// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Blended" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlendTex ("Alpha Blended (RGBA) ", 2D) = "white" {}
        _light ("Color", float) = 1.0
    }
    SubShader {
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		//Cull Off
		Lighting Off
		//ZWrite Off
		Fog { Mode Off }
		//Offset -1, -1
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"
	
				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};
	
				struct v2f
				{
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};
	
				sampler2D _MainTex;
				sampler2D _BlendTex;
				float4 _MainTex_ST;
				float _light;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR
				{
					fixed4 col = tex2D(_MainTex, i.texcoord) * _light;
					fixed4 alpha = tex2D(_BlendTex, i.texcoord);
					col.w = col.w * alpha.x;
					
					return col;
				}
			ENDCG
		}
    }
}