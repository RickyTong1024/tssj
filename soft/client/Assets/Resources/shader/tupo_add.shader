Shader "Custom/tupo_add" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BackColor ("Back Color", Color) = (1.0,1.0,1.0,0.0)	
		_alpha ("alpha", Float) = 1.0
		_while ("while", Float) = 0.0
		_back ("back", Float) = 0
		_level ("level", Float) = 1.0	
	}
    SubShader {

		Tags { "Queue"="Transparent" }
		Blend One One
		ZWrite Off
		
		Lighting Off
		CGPROGRAM
		#pragma surface surf Lambert
		
		float _alpha;
		float _while;
		uniform float _back;
		uniform float _level;
		uniform float4 _BackColor;
		
		sampler2D _MainTex;

		struct Input {
			half4 color : COLOR;
			float2 uv_MainTex;
			float2 uv_LightTex;
			float3 viewDir;
		};
			
		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _BackColor * IN.color * _alpha;
			o.Emission = o.Albedo;
			o.Alpha = _alpha;
		}
		ENDCG
    }
    Fallback "Diffuse"    
}