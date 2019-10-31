// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.06 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.06;sub:START;pass:START;ps:flbk:,lico:0,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:False,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,dith:0,ufog:True,aust:False,igpj:True,qofs:1,qpre:3,rntp:2,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.005,fgrn:60,fgrf:150,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:35032,y:32507,varname:node_1,prsc:2|emission-29-OUT,alpha-28-A;n:type:ShaderForge.SFN_Tex2d,id:2,x:33130,y:32552,ptovrint:False,ptlb:DisEmiss,ptin:_DisEmiss,varname:node_4280,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3,x:33859,y:33200,ptovrint:False,ptlb:E02,ptin:_E02,varname:node_6513,prsc:2,ntxv:2,isnm:False|UVIN-2534-OUT;n:type:ShaderForge.SFN_Tex2d,id:4,x:33855,y:32140,ptovrint:False,ptlb:E01,ptin:_E01,varname:node_315,prsc:2,ntxv:2,isnm:False|UVIN-11-OUT;n:type:ShaderForge.SFN_Time,id:5,x:32466,y:33226,varname:node_5,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:6,x:32466,y:33358,ptovrint:False,ptlb:E02_UVpan_Speed,ptin:_E02_UVpan_Speed,varname:node_7105,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7,x:32649,y:33247,varname:node_7,prsc:2|A-5-T,B-6-OUT;n:type:ShaderForge.SFN_Panner,id:8,x:33142,y:33145,varname:node_8,prsc:2,spu:1,spv:0|UVIN-457-UVOUT,DIST-7-OUT;n:type:ShaderForge.SFN_Multiply,id:9,x:33333,y:32490,varname:node_9,prsc:2|A-10-OUT,B-2-R;n:type:ShaderForge.SFN_ValueProperty,id:10,x:33130,y:32470,ptovrint:False,ptlb:DisE01_Value,ptin:_DisE01_Value,varname:node_8436,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Add,id:11,x:33592,y:32145,varname:node_11,prsc:2|A-4345-OUT,B-9-OUT;n:type:ShaderForge.SFN_Color,id:13,x:33958,y:33070,ptovrint:False,ptlb:E02_Color,ptin:_E02_Color,varname:node_239,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:14,x:34119,y:33180,varname:node_14,prsc:2|A-13-RGB,B-3-RGB;n:type:ShaderForge.SFN_Color,id:16,x:33960,y:32014,ptovrint:False,ptlb:E01_Color,ptin:_E01_Color,varname:node_7026,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:18,x:34121,y:32124,varname:node_18,prsc:2|A-16-RGB,B-4-RGB;n:type:ShaderForge.SFN_Panner,id:20,x:33124,y:32011,varname:node_20,prsc:2,spu:1,spv:0|UVIN-9737-UVOUT,DIST-22-OUT;n:type:ShaderForge.SFN_Multiply,id:22,x:32714,y:32351,varname:node_22,prsc:2|A-26-T,B-24-OUT;n:type:ShaderForge.SFN_ValueProperty,id:24,x:32543,y:32517,ptovrint:False,ptlb:E01_UVpan_Speed,ptin:_E01_UVpan_Speed,varname:node_254,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Time,id:26,x:32543,y:32330,varname:node_26,prsc:2;n:type:ShaderForge.SFN_Add,id:27,x:34342,y:32672,varname:node_27,prsc:2|A-18-OUT,B-14-OUT,C-4995-OUT;n:type:ShaderForge.SFN_VertexColor,id:28,x:34706,y:32675,varname:node_28,prsc:2;n:type:ShaderForge.SFN_Multiply,id:29,x:34869,y:32608,varname:node_29,prsc:2|A-90-OUT,B-28-RGB,C-28-A;n:type:ShaderForge.SFN_Multiply,id:41,x:34537,y:32606,varname:node_41,prsc:2|A-44-R,B-27-OUT;n:type:ShaderForge.SFN_Tex2d,id:44,x:34342,y:32489,ptovrint:False,ptlb:MaskTex,ptin:_MaskTex,varname:node_3970,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:50,x:34518,y:32413,ptovrint:False,ptlb:Tex_Bright,ptin:_Tex_Bright,varname:node_1396,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:90,x:34690,y:32499,varname:node_90,prsc:2|A-50-OUT,B-41-OUT;n:type:ShaderForge.SFN_TexCoord,id:787,x:32538,y:31993,varname:node_787,prsc:2,uv:0;n:type:ShaderForge.SFN_Add,id:2534,x:33540,y:33199,varname:node_2534,prsc:2|A-9424-OUT,B-8728-OUT;n:type:ShaderForge.SFN_Multiply,id:9424,x:33333,y:32702,varname:node_9424,prsc:2|A-2-R,B-3816-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3816,x:33130,y:32769,ptovrint:False,ptlb:DisE02_Value,ptin:_DisE02_Value,varname:node_3816,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Rotator,id:9737,x:32868,y:32005,varname:node_9737,prsc:2|UVIN-787-UVOUT,ANG-1122-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7424,x:32538,y:32160,ptovrint:False,ptlb:E01_UVangle,ptin:_E01_UVangle,varname:node_7424,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:1122,x:32714,y:32160,varname:node_1122,prsc:2|A-7424-OUT,B-2529-OUT;n:type:ShaderForge.SFN_Pi,id:2529,x:32538,y:32226,varname:node_2529,prsc:2;n:type:ShaderForge.SFN_Rotator,id:457,x:32820,y:32837,varname:node_457,prsc:2|UVIN-6712-UVOUT,ANG-4320-OUT;n:type:ShaderForge.SFN_Multiply,id:4320,x:32666,y:32992,varname:node_4320,prsc:2|A-1608-OUT,B-1315-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1608,x:32490,y:32992,ptovrint:False,ptlb:E02_UVangle,ptin:_E02_UVangle,varname:_E01_UVangle_copy,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Pi,id:1315,x:32490,y:33058,varname:node_1315,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:6712,x:32490,y:32825,varname:node_6712,prsc:2,uv:0;n:type:ShaderForge.SFN_Fresnel,id:6731,x:33537,y:33619,varname:node_6731,prsc:2|EXP-689-OUT;n:type:ShaderForge.SFN_Multiply,id:4995,x:34140,y:33644,varname:node_4995,prsc:2|A-9441-RGB,B-7061-OUT;n:type:ShaderForge.SFN_Color,id:9441,x:33959,y:33520,ptovrint:False,ptlb:RimColor,ptin:_RimColor,varname:node_9441,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Clamp01,id:7061,x:33958,y:33708,varname:node_7061,prsc:2|IN-9533-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:9533,x:33792,y:33712,varname:node_9533,prsc:2|IN-6731-OUT,IMIN-2794-OUT,IMAX-4357-OUT,OMIN-2049-OUT,OMAX-9204-OUT;n:type:ShaderForge.SFN_Vector1,id:2794,x:33537,y:33746,varname:node_2794,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:4357,x:33537,y:33801,varname:node_4357,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:689,x:33380,y:33639,varname:node_689,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:2049,x:33380,y:33878,ptovrint:False,ptlb:RimPower_min,ptin:_RimPower_min,varname:node_2049,prsc:2,min:0,cur:0,max:-5;n:type:ShaderForge.SFN_Slider,id:9204,x:33380,y:33990,ptovrint:False,ptlb:RimPower_max,ptin:_RimPower_max,varname:_node_2049_copy,prsc:2,min:1,cur:1,max:5;n:type:ShaderForge.SFN_Panner,id:3343,x:33142,y:33289,varname:node_3343,prsc:2,spu:0,spv:1|UVIN-457-UVOUT,DIST-7-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:8728,x:33336,y:33235,ptovrint:False,ptlb:E02_U/Vpan,ptin:_E02_UVpan,varname:node_8728,prsc:2,on:False|A-8-UVOUT,B-3343-UVOUT;n:type:ShaderForge.SFN_Panner,id:1023,x:33124,y:32164,varname:node_1023,prsc:2,spu:0,spv:1|UVIN-9737-UVOUT,DIST-22-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:4345,x:33305,y:32100,ptovrint:False,ptlb:E01_U/Vpan,ptin:_E01_UVpan,varname:node_4345,prsc:2,on:False|A-20-UVOUT,B-1023-UVOUT;proporder:50-4-16-4345-7424-24-3-13-8728-1608-6-44-2-10-3816-9441-2049-9204;pass:END;sub:END;*/

Shader "yh/Emissive/2EmissionUVpan_Rim" {
    Properties {
        _Tex_Bright ("Tex_Bright", Float ) = 1
        _E01 ("E01", 2D) = "black" {}
        _E01_Color ("E01_Color", Color) = (1,1,1,1)
        [MaterialToggle] _E01_UVpan ("E01_U/Vpan", Float ) = 0
        _E01_UVangle ("E01_UVangle", Float ) = 0
        _E01_UVpan_Speed ("E01_UVpan_Speed", Float ) = 1
        _E02 ("E02", 2D) = "black" {}
        _E02_Color ("E02_Color", Color) = (1,1,1,1)
        [MaterialToggle] _E02_UVpan ("E02_U/Vpan", Float ) = 0
        _E02_UVangle ("E02_UVangle", Float ) = 0
        _E02_UVpan_Speed ("E02_UVpan_Speed", Float ) = 1
        _MaskTex ("MaskTex", 2D) = "white" {}
        _DisEmiss ("DisEmiss", 2D) = "white" {}
        _DisE01_Value ("DisE01_Value", Float ) = 0
        _DisE02_Value ("DisE02_Value", Float ) = 0
        _RimColor ("RimColor", Color) = (1,1,1,1)
        _RimPower_min ("RimPower_min", Range(0, -5)) = 0
        _RimPower_max ("RimPower_max", Range(1, 5)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent+1"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            Fog {Mode Global}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _DisEmiss; uniform float4 _DisEmiss_ST;
            uniform sampler2D _E02; uniform float4 _E02_ST;
            uniform sampler2D _E01; uniform float4 _E01_ST;
            uniform float _E02_UVpan_Speed;
            uniform float _DisE01_Value;
            uniform float4 _E02_Color;
            uniform float4 _E01_Color;
            uniform float _E01_UVpan_Speed;
            uniform sampler2D _MaskTex; uniform float4 _MaskTex_ST;
            uniform float _Tex_Bright;
            uniform float _DisE02_Value;
            uniform float _E01_UVangle;
            uniform float _E02_UVangle;
            uniform float4 _RimColor;
            uniform float _RimPower_min;
            uniform float _RimPower_max;
            uniform fixed _E02_UVpan;
            uniform fixed _E01_UVpan;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
////// Lighting:
////// Emissive:
                float4 _MaskTex_var = tex2D(_MaskTex,TRANSFORM_TEX(i.uv0, _MaskTex));
                float4 node_26 = _Time + _TimeEditor;
                float node_22 = (node_26.g*_E01_UVpan_Speed);
                float node_9737_ang = (_E01_UVangle*3.141592654);
                float node_9737_spd = 1.0;
                float node_9737_cos = cos(node_9737_spd*node_9737_ang);
                float node_9737_sin = sin(node_9737_spd*node_9737_ang);
                float2 node_9737_piv = float2(0.5,0.5);
                float2 node_9737 = (mul(i.uv0-node_9737_piv,float2x2( node_9737_cos, -node_9737_sin, node_9737_sin, node_9737_cos))+node_9737_piv);
                float4 _DisEmiss_var = tex2D(_DisEmiss,TRANSFORM_TEX(i.uv0, _DisEmiss));
                float2 node_11 = (lerp( (node_9737+node_22*float2(1,0)), (node_9737+node_22*float2(0,1)), _E01_UVpan )+(_DisE01_Value*_DisEmiss_var.r));
                float4 _E01_var = tex2D(_E01,TRANSFORM_TEX(node_11, _E01));
                float4 node_5 = _Time + _TimeEditor;
                float node_7 = (node_5.g*_E02_UVpan_Speed);
                float node_457_ang = (_E02_UVangle*3.141592654);
                float node_457_spd = 1.0;
                float node_457_cos = cos(node_457_spd*node_457_ang);
                float node_457_sin = sin(node_457_spd*node_457_ang);
                float2 node_457_piv = float2(0.5,0.5);
                float2 node_457 = (mul(i.uv0-node_457_piv,float2x2( node_457_cos, -node_457_sin, node_457_sin, node_457_cos))+node_457_piv);
                float2 node_2534 = ((_DisEmiss_var.r*_DisE02_Value)+lerp( (node_457+node_7*float2(1,0)), (node_457+node_7*float2(0,1)), _E02_UVpan ));
                float4 _E02_var = tex2D(_E02,TRANSFORM_TEX(node_2534, _E02));
                float node_2794 = 0.0;
                float3 emissive = ((_Tex_Bright*(_MaskTex_var.r*((_E01_Color.rgb*_E01_var.rgb)+(_E02_Color.rgb*_E02_var.rgb)+(_RimColor.rgb*saturate((_RimPower_min + ( (pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0) - node_2794) * (_RimPower_max - _RimPower_min) ) / (1.0 - node_2794)))))))*i.vertexColor.rgb*i.vertexColor.a);
                float3 finalColor = emissive;
                return fixed4(finalColor,i.vertexColor.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
