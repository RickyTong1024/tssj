Shader "Custom/login_mm" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MainTex2 ("Base (RGB)", 2D) = "white" {}
		_BackColor ("Back Color", Color) = (1.0,1.0,1.0,0.0)
		_Color ("Color", Color) = (1.0,1.0,1.0,0.0)	
		_jiaohuan ("alpha", Float) = 0.0
	}
    SubShader {
    	Tags { "Queue"="Transparent-10" }
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off
		CGPROGRAM
		#pragma surface surf Lambert
		
		float _jiaohuan;
		uniform float4 _BackColor;
		uniform float4 _Color;
		sampler2D _MainTex;
		sampler2D _MainTex2;
		struct Input {
			float2 uv_MainTex;
			float2 uv_LightTex;
			float3 viewDir;
		};
			
		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);

			half4 b = tex2D (_MainTex2, IN.uv_MainTex);

			o.Albedo = lerp(b,c,_Color.a) * _Color;
			//o.Emission = o.Albedo;
			
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission += _BackColor.rgb * pow (rim, 1.8f);
			//o.Emission += o.Albedo * 0.2;
			
			o.Alpha = 1.0f;
		}
		ENDCG
    }
    Fallback "Diffuse"    
}
