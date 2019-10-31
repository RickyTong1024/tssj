// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:False,qofs:1,qpre:3,rntp:2,fgom:True,fgoc:True,fgod:False,fgor:True,fgmd:1,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.005,fgrn:60,fgrf:150,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:35032,y:32507,varname:node_1,prsc:2|emission-29-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33323,y:32892,ptovrint:False,ptlb:DisT01,ptin:_DisT01,varname:node_52,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:899c5fd66853a0a4a976e047fe41bfd7,ntxv:0,isnm:False|UVIN-8-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3,x:33878,y:32872,ptovrint:False,ptlb:Tex01,ptin:_Tex01,varname:node_625,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e5c2d246f2b01444391ec767bb59399c,ntxv:0,isnm:False|UVIN-11-OUT;n:type:ShaderForge.SFN_Tex2d,id:4,x:33861,y:32544,ptovrint:False,ptlb:Tex02,ptin:_Tex02,varname:node_3713,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a9b9ff1d9e59c7c4f9a4c5ca16654838,ntxv:2,isnm:False|UVIN-20-UVOUT;n:type:ShaderForge.SFN_Time,id:5,x:32824,y:32891,varname:node_5,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:6,x:32824,y:33022,ptovrint:False,ptlb:PanY_DisT01,ptin:_PanY_DisT01,varname:node_1229,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7,x:33007,y:32912,varname:node_7,prsc:2|A-5-T,B-6-OUT;n:type:ShaderForge.SFN_Panner,id:8,x:33163,y:32892,varname:node_8,prsc:2,spu:0,spv:1|UVIN-4005-UVOUT,DIST-7-OUT;n:type:ShaderForge.SFN_Multiply,id:9,x:33509,y:32872,varname:node_9,prsc:2|A-10-OUT,B-2-RGB;n:type:ShaderForge.SFN_ValueProperty,id:10,x:33347,y:32807,ptovrint:False,ptlb:DisT01Power,ptin:_DisT01Power,varname:node_4091,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:11,x:33709,y:32872,varname:node_11,prsc:2|A-9-OUT,B-4005-UVOUT;n:type:ShaderForge.SFN_Color,id:13,x:33986,y:32743,ptovrint:False,ptlb:Color_T01,ptin:_Color_T01,varname:node_5192,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:14,x:34147,y:32853,varname:node_14,prsc:2|A-13-RGB,B-3-RGB;n:type:ShaderForge.SFN_Color,id:16,x:33982,y:32415,ptovrint:False,ptlb:Color_T02,ptin:_Color_T02,varname:node_1902,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:18,x:34143,y:32525,varname:node_18,prsc:2|A-16-RGB,B-4-RGB;n:type:ShaderForge.SFN_Panner,id:20,x:33686,y:32544,varname:node_20,prsc:2,spu:0,spv:1|UVIN-4005-UVOUT,DIST-22-OUT;n:type:ShaderForge.SFN_Multiply,id:22,x:33517,y:32564,varname:node_22,prsc:2|A-26-T,B-24-OUT;n:type:ShaderForge.SFN_ValueProperty,id:24,x:33346,y:32674,ptovrint:False,ptlb:PanY_T02,ptin:_PanY_T02,varname:node_6104,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Time,id:26,x:33346,y:32543,varname:node_26,prsc:2;n:type:ShaderForge.SFN_Add,id:27,x:34342,y:32672,varname:node_27,prsc:2|A-18-OUT,B-14-OUT;n:type:ShaderForge.SFN_VertexColor,id:28,x:34706,y:32675,varname:node_28,prsc:2;n:type:ShaderForge.SFN_Multiply,id:29,x:34869,y:32608,varname:node_29,prsc:2|A-90-OUT,B-28-RGB,C-28-A;n:type:ShaderForge.SFN_Multiply,id:41,x:34537,y:32606,varname:node_41,prsc:2|A-44-R,B-27-OUT;n:type:ShaderForge.SFN_Tex2d,id:44,x:34342,y:32489,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_5975,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a3de474356606594493c3c344cbf0f8f,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:50,x:34518,y:32413,ptovrint:False,ptlb:Intensity,ptin:_Intensity,varname:node_4624,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:90,x:34690,y:32499,varname:node_90,prsc:2|A-50-OUT,B-41-OUT;n:type:ShaderForge.SFN_TexCoord,id:4005,x:33092,y:32547,varname:node_4005,prsc:2,uv:0,uaff:False;proporder:3-2-10-6-4-24-44-50-13-16;pass:END;sub:END;*/

Shader "Shader Forge/Emission_2Tex_PanV" {
    Properties {
        _Tex01 ("Tex01", 2D) = "white" {}
        _DisT01 ("DisT01", 2D) = "white" {}
        _DisT01Power ("DisT01Power", Float ) = 0
        _PanY_DisT01 ("PanY_DisT01", Float ) = 1
        _Tex02 ("Tex02", 2D) = "black" {}
        _PanY_T02 ("PanY_T02", Float ) = 1
        _Mask ("Mask", 2D) = "white" {}
        _Intensity ("Intensity", Float ) = 1
        _Color_T01 ("Color_T01", Color) = (0.5,0.5,0.5,1)
        _Color_T02 ("Color_T02", Color) = (0.5,0.5,0.5,1)
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
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform sampler2D _DisT01; uniform float4 _DisT01_ST;
            uniform sampler2D _Tex01; uniform float4 _Tex01_ST;
            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform float _PanY_DisT01;
            uniform float _DisT01Power;
            uniform float4 _Color_T01;
            uniform float4 _Color_T02;
            uniform float _PanY_T02;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _Intensity;
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
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 node_26 = _Time;
                float2 node_20 = (i.uv0+(node_26.g*_PanY_T02)*float2(0,1));
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(node_20, _Tex02));
                float4 node_5 = _Time;
                float2 node_8 = (i.uv0+(node_5.g*_PanY_DisT01)*float2(0,1));
                float4 _DisT01_var = tex2D(_DisT01,TRANSFORM_TEX(node_8, _DisT01));
                float3 node_11 = ((_DisT01Power*_DisT01_var.rgb)+float3(i.uv0,0.0));
                float4 _Tex01_var = tex2D(_Tex01,TRANSFORM_TEX(node_11, _Tex01));
                float3 emissive = ((_Intensity*(_Mask_var.r*((_Color_T02.rgb*_Tex02_var.rgb)+(_Color_T01.rgb*_Tex01_var.rgb))))*i.vertexColor.rgb*i.vertexColor.a);
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
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
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
