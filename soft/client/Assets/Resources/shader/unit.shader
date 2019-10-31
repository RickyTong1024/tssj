Shader "Custom/unit" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_LightTex ("Light (RGB)", 2D) = "white" {}
		_alphaTex ("alpha (RGB)", 2D) = "white" {}
		_range ("range", Float) = 0.5
		_RimColor ("Rim Color", Color) = (1.0,1.0,0.0,0.0)
		_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
		_uvc ("uvc", Range(0.0,1.0)) = 0.0
	}
    SubShader {
    
		Tags { "Queue"="Transparent-2" }
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _LightTex;
		float _range;

		struct Input {
			float2 uv_MainTex;
			float2 uv_LightTex;
			float3 viewDir;
		};
			float4 _RimColor;
			float _RimPower;
			float  _uvc;
		void surf (Input IN, inout SurfaceOutput o) {

			IN.uv_LightTex.x += _uvc;
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 l = tex2D (_LightTex, IN.uv_LightTex);
			o.Albedo = c + l * _range;

			//o.Albedo = lerp(c,float4(0, 0, 0, 1.0),_range);
			//o.Albedo = c;
			o.Alpha = c.a;
			if (o.Alpha < 0.5)
               // alpha value less than user-specified threshold?
            {
               discard; // yes: discard this fragment
            }
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow (rim, _RimPower);

		}
		ENDCG
    }
	Fallback "Diffuse"
}
