Shader "Custom/map_base" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BackColor ("Back Color", Color) = (1.0,1.0,1.0,0.0)	
		_alpha ("alpha", Float) = 1.0
		_while ("while", Float) = 0.0
		_back ("back", Float) = 0
		_level ("level", Float) = 1.0	
	}
    SubShader {
		 Tags {"Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting On
		CGPROGRAM
		#pragma surface surf Lambert
		
		float _alpha;
		float _while;
		uniform float _back;
		uniform float _level;
		uniform float4 _BackColor;
		
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_LightTex;
			float3 viewDir;
			float3 worldPos;
		};
			
		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _alpha;
			o.Alpha = c.a;

			if(_while > 0.0)
			{
				half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
				o.Emission += float4(1.0, 1.0, 1.0, 1.0) * pow (rim, 0.5)  * _while;	
			}
			
			o.Alpha *= _alpha;

			if(_back > 0.0)
			{
				half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
				o.Emission += _BackColor.rgb * pow (rim, _back)  * _level;
			}
			
			float3 _pos;
			
			_pos.x = 8.805742;
			_pos.y = -1.160232; 
			_pos.z = -2.214648;
			
			//_pos.y = 0;
			
			float _dis = distance(_pos,IN.worldPos);
			
			o.Alpha = 1.0 - _dis * 0.3;
		}
		ENDCG
    }
    Fallback "Diffuse"    
}
