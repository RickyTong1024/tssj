// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:False,qofs:1,qpre:3,rntp:2,fgom:True,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.005,fgrn:60,fgrf:150,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:35312,y:32507,varname:node_1,prsc:2|emission-29-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33389,y:32685,ptovrint:False,ptlb:DisT01,ptin:_DisT01,varname:node_1449,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:899c5fd66853a0a4a976e047fe41bfd7,ntxv:0,isnm:False|UVIN-8-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3,x:33908,y:32645,ptovrint:False,ptlb:Tex01,ptin:_Tex01,varname:node_284,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:98162de2fd8acd845995a2a109fad096,ntxv:0,isnm:False|UVIN-11-OUT;n:type:ShaderForge.SFN_Tex2d,id:4,x:33893,y:33057,ptovrint:False,ptlb:Tex02,ptin:_Tex02,varname:node_6138,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d1ee51aa13469654eaffaaa70f68ec25,ntxv:2,isnm:False|UVIN-20-UVOUT;n:type:ShaderForge.SFN_Time,id:5,x:32890,y:32684,varname:node_5,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:6,x:32890,y:32815,ptovrint:False,ptlb:PanY_DisT01,ptin:_PanY_DisT01,varname:node_3527,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7,x:33073,y:32705,varname:node_7,prsc:2|A-5-T,B-6-OUT;n:type:ShaderForge.SFN_Panner,id:8,x:33229,y:32685,varname:node_8,prsc:2,spu:0,spv:1|UVIN-3712-UVOUT,DIST-7-OUT;n:type:ShaderForge.SFN_Multiply,id:9,x:33575,y:32665,varname:node_9,prsc:2|A-10-OUT,B-2-RGB;n:type:ShaderForge.SFN_ValueProperty,id:10,x:33413,y:32600,ptovrint:False,ptlb:DisT01Power,ptin:_DisT01Power,varname:node_879,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:11,x:33739,y:32645,varname:node_11,prsc:2|A-9-OUT,B-12-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:12,x:33575,y:32796,varname:node_12,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Color,id:13,x:34016,y:32497,ptovrint:False,ptlb:Color_T01,ptin:_Color_T01,varname:node_9966,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:14,x:34187,y:32614,varname:node_14,prsc:2|A-13-RGB,B-3-RGB;n:type:ShaderForge.SFN_Multiply,id:18,x:34175,y:33038,varname:node_18,prsc:2|A-109-OUT,B-4-RGB;n:type:ShaderForge.SFN_Panner,id:20,x:33705,y:33057,varname:node_20,prsc:2,spu:1,spv:0|UVIN-3712-UVOUT,DIST-102-OUT;n:type:ShaderForge.SFN_ValueProperty,id:24,x:33351,y:33155,ptovrint:False,ptlb:PanX_T02,ptin:_PanX_T02,varname:node_8060,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_VertexColor,id:28,x:34954,y:32674,varname:node_28,prsc:2;n:type:ShaderForge.SFN_Multiply,id:29,x:35148,y:32607,varname:node_29,prsc:2|A-119-OUT,B-28-RGB;n:type:ShaderForge.SFN_Multiply,id:102,x:33516,y:33085,varname:node_102,prsc:2|A-28-A,B-24-OUT;n:type:ShaderForge.SFN_ValueProperty,id:109,x:34024,y:32969,ptovrint:False,ptlb:Intensity_T02,ptin:_Intensity_T02,varname:node_4273,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:119,x:34720,y:32615,varname:node_119,prsc:2|A-156-OUT,B-131-OUT;n:type:ShaderForge.SFN_Tex2d,id:120,x:34608,y:33350,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_5084,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:61768f446f8ba9244a57115e4b0712bc,ntxv:2,isnm:False|UVIN-121-UVOUT;n:type:ShaderForge.SFN_Panner,id:121,x:34434,y:33350,varname:node_121,prsc:2,spu:0,spv:1|UVIN-3712-UVOUT,DIST-127-OUT;n:type:ShaderForge.SFN_Time,id:123,x:34077,y:33350,varname:node_123,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:125,x:34077,y:33481,ptovrint:False,ptlb:PanY_TexAlpha,ptin:_PanY_TexAlpha,varname:node_9243,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:127,x:34260,y:33371,varname:node_127,prsc:2|A-123-T,B-125-OUT;n:type:ShaderForge.SFN_Multiply,id:131,x:34867,y:33073,varname:node_131,prsc:2|A-132-OUT,B-120-RGB;n:type:ShaderForge.SFN_ValueProperty,id:132,x:34606,y:33104,ptovrint:False,ptlb:Intensity_Mask,ptin:_Intensity_Mask,varname:node_7182,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:156,x:34505,y:32615,varname:node_156,prsc:2|A-14-OUT,B-18-OUT;n:type:ShaderForge.SFN_TexCoord,id:3712,x:32911,y:33007,varname:node_3712,prsc:2,uv:0,uaff:False;proporder:3-13-2-10-6-4-109-24-120-125-132;pass:END;sub:END;*/

Shader "Shader Forge/Emission_daoguang01" {
    Properties {
        _Tex01 ("Tex01", 2D) = "white" {}
        _Color_T01 ("Color_T01", Color) = (1,1,1,1)
        _DisT01 ("DisT01", 2D) = "white" {}
        _DisT01Power ("DisT01Power", Float ) = 0
        _PanY_DisT01 ("PanY_DisT01", Float ) = 1
        _Tex02 ("Tex02", 2D) = "black" {}
        _Intensity_T02 ("Intensity_T02", Float ) = 1
        _PanX_T02 ("PanX_T02", Float ) = 1
        _Mask ("Mask", 2D) = "black" {}
        _PanY_TexAlpha ("PanY_TexAlpha", Float ) = 0
        _Intensity_Mask ("Intensity_Mask", Float ) = 1
    }
    SubShader {
        Tags {
            "Queue"="Transparent+1"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _DisT01; uniform float4 _DisT01_ST;
            uniform sampler2D _Tex01; uniform float4 _Tex01_ST;
            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform float _PanY_DisT01;
            uniform float _DisT01Power;
            uniform float4 _Color_T01;
            uniform float _PanX_T02;
            uniform float _Intensity_T02;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _PanY_TexAlpha;
            uniform float _Intensity_Mask;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_5 = _Time;
                float2 node_8 = (i.uv0+(node_5.g*_PanY_DisT01)*float2(0,1));
                float4 _DisT01_var = tex2D(_DisT01,TRANSFORM_TEX(node_8, _DisT01));
                float3 node_11 = ((_DisT01Power*_DisT01_var.rgb)+float3(i.uv0,0.0));
                float4 _Tex01_var = tex2D(_Tex01,TRANSFORM_TEX(node_11, _Tex01));
                float2 node_20 = (i.uv0+(i.vertexColor.a*_PanX_T02)*float2(1,0));
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(node_20, _Tex02));
                float4 node_123 = _Time;
                float2 node_121 = (i.uv0+(node_123.g*_PanY_TexAlpha)*float2(0,1));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(node_121, _Mask));
                float3 emissive = ((((_Color_T01.rgb*_Tex01_var.rgb)*(_Intensity_T02*_Tex02_var.rgb))*(_Intensity_Mask*_Mask_var.rgb))*i.vertexColor.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
