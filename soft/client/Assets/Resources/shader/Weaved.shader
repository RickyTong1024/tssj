// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 //Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ZpGame/Weaved"{
Properties{
	_MainTex("", 2D) = "" {}
	_RB("RB",Int) = 0
	_SwitchLR("switch LR texture",Int) = 0
	_LRTexture("LR (RGB)", RECT) = "white" {}
	_LTexture("L (RGB)", RECT) = "white" {}
	_RTexture("R (RGB)", RECT) = "white" {}
}

CGINCLUDE

	#include "UnityCG.cginc"

	uniform sampler2D _LRTexture;
	uniform sampler2D _LTexture;
	uniform sampler2D _RTexture;
	uniform int _RB;
	uniform int _SwitchLR;
	float4 _MainTex_TexelSize;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};


	v2f vert(appdata_img v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			v.texcoord.y = 1 - v.texcoord.y;
		#endif
		o.uv = v.texcoord;
		return o;
	}
	
ENDCG

SubShader
{
	ZTest Always Cull Off ZWrite Off Fog{ Mode Off }

	// COL Fragment Shader (Pass No. 0)
	Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		fixed4 frag(v2f i) : COLOR{
			fixed4 o;
			float2 wcoord = i.uv.xy * _ScreenParams.xy;
			float screenx = floor(abs(float(wcoord.x) - 0.0));
			float fraction = screenx - 2.0*(floor(screenx*0.5));
			if (fraction >= 1.0 )
			{
				if(_SwitchLR == 0)
					o = tex2D(_LTexture, i.uv);
				else
					o = tex2D(_RTexture, i.uv);
				if (_RB)
					o = fixed4(1, 0, 0, 1);
			}
			else
			{
				if(_SwitchLR == 0)
					o = tex2D(_RTexture, i.uv);
				else
					o = tex2D(_LTexture, i.uv);
				if (_RB)
					o = fixed4(0, 0, 1, 1);
			}
			return o;
		}
		ENDCG
	}
/*
	Pass{	
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		float4 frag(v2f i) : COLOR{

			float4 tex1;
			float4 tex2;
			float2 wcoord = i.uv.xy * _ScreenParams.xy;
			float screenx = floor(abs(float(wcoord.x) - 0.0));
			float fraction = screenx - 2.0*(floor(screenx*0.5));
			if (fraction >= 1.0)
			{
				tex1 = tex2D(_LTexture, i.uv);
				tex2 = tex2D(_LRTexture, float2(i.uv.x * 0.5, i.uv.y));
				if (_RB)
				{
					tex1 = float4(1, 0, 0, 1);
				}
			}
			else
			{
				tex1 = tex2D(_RTexture, i.uv);
				tex2 = tex2D(_LRTexture, float2(i.uv.x + 0.5, i.uv.y));
				if (_RB)
				{
					tex1 = float4(0, 0, 1, 1);
				}
			}

			return tex2 * tex2.a*(1 - tex1.a) + tex1 * tex1.a;

		}
		ENDCG
	}
*/
}
Fallback off
}