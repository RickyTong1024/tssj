// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ZpGame/WeavedBg"{
Properties{
	_RB("RB",Int) = 0
	_SwitchLR("switch LR texture",Int) = 0
	_CloseWeave("Close weave",Int) = 0
	_MainTex ("Texture", 2D) = "white" {}
	_LRTexture("LR (RGB)", RECT) = "white" {}
	_LTexture("L (RGB)", RECT) = "white" {}
	_RTexture("R (RGB)", RECT) = "white" {}
}

CGINCLUDE

	#include "UnityCG.cginc"


	uniform sampler2D _MainTex;
	uniform sampler2D _LRTexture;
	uniform sampler2D _LTexture;
	uniform sampler2D _RTexture;
	uniform int _RB;
	uniform int _SwitchLR;
	uniform int _CloseWeave;


	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};


	v2f vert(appdata_img v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
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
			fixed4 bg;
			float2 wcoord = i.uv.xy * _ScreenParams.xy;
			float screenx = floor(abs(float(wcoord.x) - 0.0));
			float fraction = screenx - 2.0*(floor(screenx*0.5));

			if(_CloseWeave)
			{
				o = tex2D(_MainTex, i.uv);
				bg = tex2D(_LRTexture, i.uv);
				return bg * bg.a*(1 - o.a) + o * o.a;
			}

			if (fraction >= 1.0 )
			{
				if(_SwitchLR == 0)
				{
					o = tex2D(_LTexture, i.uv);
					bg = tex2D(_LRTexture, i.uv.xy*float2(0.5,1));
				}
				else
				{
					o = tex2D(_RTexture, i.uv);
					bg = tex2D(_LRTexture, i.uv.xy*float2(0.5,1)+float2(0.5,0));
				}

				if (_RB)
					o = fixed4(1, 0, 0, 1);
			}
			else
			{
				if(_SwitchLR == 0)
				{
					o = tex2D(_RTexture, i.uv);
					bg = tex2D(_LRTexture, i.uv.xy*float2(0.5,1)+float2(0.5,0));
				}
				else
				{
					o = tex2D(_LTexture, i.uv);
					bg = tex2D(_LRTexture, i.uv.xy*float2(0.5,1));
				}
				if (_RB)
					o = fixed4(0, 0, 1, 1);
			}

			return bg *bg.a*(1 - o.a) + o * o.a;
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

			return tex2 *(1 - tex1.a) + tex1 * tex1.a;

		}
		ENDCG
	}
*/
}
Fallback off
}