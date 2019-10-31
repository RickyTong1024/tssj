// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:True,qofs:1,qpre:3,rntp:2,fgom:True,fgoc:True,fgod:False,fgor:True,fgmd:1,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:45,fgrf:100,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:34900,y:32712,varname:node_1,prsc:2|emission-654-OUT,alpha-679-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33884,y:32696,ptovrint:False,ptlb:T01,ptin:_T01,varname:node_2175,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False|UVIN-554-OUT;n:type:ShaderForge.SFN_Tex2d,id:7,x:33349,y:32654,ptovrint:False,ptlb:niuqu,ptin:_niuqu,varname:node_2772,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-15-UVOUT;n:type:ShaderForge.SFN_Panner,id:15,x:33185,y:32615,varname:node_15,prsc:2,spu:0,spv:1|UVIN-9798-UVOUT,DIST-631-OUT;n:type:ShaderForge.SFN_Multiply,id:217,x:33557,y:32591,varname:node_217,prsc:2|A-589-OUT,B-7-RGB;n:type:ShaderForge.SFN_TexCoord,id:496,x:33557,y:32716,varname:node_496,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:554,x:33719,y:32696,varname:node_554,prsc:2|A-217-OUT,B-496-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:589,x:33349,y:32569,ptovrint:False,ptlb:niuqudu,ptin:_niuqudu,varname:node_5500,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Time,id:630,x:32849,y:32567,varname:node_630,prsc:2;n:type:ShaderForge.SFN_Multiply,id:631,x:33024,y:32635,varname:node_631,prsc:2|A-630-T,B-633-OUT;n:type:ShaderForge.SFN_ValueProperty,id:633,x:32700,y:32720,ptovrint:False,ptlb:niuqu_sudu,ptin:_niuqu_sudu,varname:node_8412,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:643,x:34061,y:32555,varname:node_643,prsc:2|A-647-RGB,B-2-RGB;n:type:ShaderForge.SFN_Tex2d,id:647,x:33884,y:32526,ptovrint:False,ptlb:mask,ptin:_mask,varname:node_5360,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:3a5a96df060a5cf4a9cc0c59e13486b7,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:653,x:34540,y:32673,varname:node_653,prsc:2;n:type:ShaderForge.SFN_Multiply,id:654,x:34724,y:32635,varname:node_654,prsc:2|A-669-OUT,B-653-RGB,C-653-A;n:type:ShaderForge.SFN_Multiply,id:659,x:34208,y:32633,varname:node_659,prsc:2|A-643-OUT,B-664-RGB;n:type:ShaderForge.SFN_Color,id:664,x:34059,y:32819,ptovrint:False,ptlb:Color_T01,ptin:_Color_T01,varname:node_1818,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:669,x:34395,y:32633,varname:node_669,prsc:2|A-659-OUT,B-675-OUT;n:type:ShaderForge.SFN_ValueProperty,id:675,x:34246,y:32798,ptovrint:False,ptlb:Intensity_T01,ptin:_Intensity_T01,varname:node_8124,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:679,x:34648,y:32933,varname:node_679,prsc:2|A-647-R,B-2-R;n:type:ShaderForge.SFN_TexCoord,id:9798,x:32964,y:32414,varname:node_9798,prsc:2,uv:0,uaff:False;proporder:2-664-7-589-633-647-675;pass:END;sub:END;*/

Shader "Shader Forge/FireA" {
    Properties {
        _T01 ("T01", 2D) = "white" {}
        _Color_T01 ("Color_T01", Color) = (1,1,1,1)
        _niuqu ("niuqu", 2D) = "white" {}
        _niuqudu ("niuqudu", Float ) = 0.2
        _niuqu_sudu ("niuqu_sudu", Float ) = 1
        _mask ("mask", 2D) = "white" {}
        _Intensity_T01 ("Intensity_T01", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
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
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _T01; uniform float4 _T01_ST;
            uniform sampler2D _niuqu; uniform float4 _niuqu_ST;
            uniform float _niuqudu;
            uniform float _niuqu_sudu;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform float4 _Color_T01;
            uniform float _Intensity_T01;
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
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(i.uv0, _mask));
                float4 node_630 = _Time;
                float2 node_15 = (i.uv0+(node_630.g*_niuqu_sudu)*float2(0,1));
                float4 _niuqu_var = tex2D(_niuqu,TRANSFORM_TEX(node_15, _niuqu));
                float3 node_554 = ((_niuqudu*_niuqu_var.rgb)+float3(i.uv0,0.0));
                float4 _T01_var = tex2D(_T01,TRANSFORM_TEX(node_554, _T01));
                float3 emissive = ((((_mask_var.rgb*_T01_var.rgb)*_Color_T01.rgb)*_Intensity_T01)*i.vertexColor.rgb*i.vertexColor.a);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(_mask_var.r*_T01_var.r));
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
