Shader "Custom/scene_alpha_light" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_alpha ("alpha", Float) = 1.0
		_while ("while", Float) = 0
	}
    SubShader {
		Tags { "Queue"="Transparent-1" }
		Blend SrcAlpha OneMinusSrcAlpha

		Lighting On
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		sampler2D _LightTex;
		sampler2D _LightAlpha;

		struct Input {
			float2 uv_MainTex;
			float2 uv_LightTex;
			float2 uv_LightAlpha;
			float3 viewDir;
		};
			
		uniform float _alpha;
		uniform float _while;
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _while;
			o.Alpha = c.a;
			o.Alpha *= _alpha;           
		}
		ENDCG
    }
    Fallback "Diffuse"
}
