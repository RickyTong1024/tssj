// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:False,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:45,fgrf:100,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:34900,y:32712,varname:node_1,prsc:2|emission-654-OUT,alpha-3184-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33848,y:32694,ptovrint:False,ptlb:E01,ptin:_E01,varname:_E01,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False|UVIN-2473-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:7,x:33090,y:32652,ptovrint:False,ptlb:DisE01,ptin:_DisE01,varname:_DisE01,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-7443-OUT;n:type:ShaderForge.SFN_Panner,id:15,x:32776,y:32564,varname:node_15,prsc:2,spu:1,spv:0|UVIN-6331-UVOUT,DIST-631-OUT;n:type:ShaderForge.SFN_Multiply,id:217,x:33298,y:32589,varname:node_217,prsc:2|A-589-OUT,B-7-R;n:type:ShaderForge.SFN_TexCoord,id:496,x:33298,y:32714,varname:node_496,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:554,x:33460,y:32694,varname:node_554,prsc:2|A-217-OUT,B-496-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:589,x:33090,y:32567,ptovrint:False,ptlb:DisE01_value,ptin:_DisE01_value,varname:_DisE01_value,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:630,x:32440,y:32567,varname:node_630,prsc:2;n:type:ShaderForge.SFN_Multiply,id:631,x:32615,y:32635,varname:node_631,prsc:2|A-630-T,B-633-OUT;n:type:ShaderForge.SFN_ValueProperty,id:633,x:32440,y:32758,ptovrint:False,ptlb:DisE01_UVpan_Speed,ptin:_DisE01_UVpan_Speed,varname:_DisE01_UVpan_Speed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Tex2d,id:647,x:33979,y:32965,ptovrint:False,ptlb:Alpha01,ptin:_Alpha01,varname:_MaskE01,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:3a5a96df060a5cf4a9cc0c59e13486b7,ntxv:0,isnm:False|UVIN-7860-UVOUT;n:type:ShaderForge.SFN_VertexColor,id:653,x:34540,y:32673,varname:node_653,prsc:2;n:type:ShaderForge.SFN_Multiply,id:654,x:34724,y:32635,cmnt:在UI里和背景做透明,varname:node_654,prsc:2|A-669-OUT,B-653-RGB;n:type:ShaderForge.SFN_Multiply,id:659,x:34208,y:32633,varname:node_659,prsc:2|A-664-RGB,B-2-RGB;n:type:ShaderForge.SFN_Color,id:664,x:34035,y:32483,ptovrint:False,ptlb:E01_Color,ptin:_E01_Color,varname:_E01_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:669,x:34395,y:32633,varname:node_669,prsc:2|A-659-OUT,B-675-OUT;n:type:ShaderForge.SFN_ValueProperty,id:675,x:34246,y:32798,ptovrint:False,ptlb:E01_Bright,ptin:_E01_Bright,varname:_E01_Bright,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Panner,id:4833,x:32776,y:32785,varname:node_4833,prsc:2,spu:0,spv:1|UVIN-6331-UVOUT,DIST-631-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:7443,x:32932,y:32701,ptovrint:False,ptlb:DisE01_U/Vpan,ptin:_DisE01_UVpan,varname:_DisE01_UVpan,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-15-UVOUT,B-4833-UVOUT;n:type:ShaderForge.SFN_Rotator,id:2473,x:33688,y:32694,varname:node_2473,prsc:2|UVIN-554-OUT,ANG-9784-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5289,x:33437,y:32963,ptovrint:False,ptlb:E01_UVangle,ptin:_E01_UVangle,varname:_E01_UVangle,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_TexCoord,id:6331,x:32589,y:32812,varname:node_6331,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:3184,x:34618,y:32942,varname:node_3184,prsc:2|A-653-A,B-6968-OUT;n:type:ShaderForge.SFN_Multiply,id:6968,x:34326,y:32968,varname:node_6968,prsc:2|A-2-R,B-647-R,C-7035-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7035,x:33999,y:33178,ptovrint:False,ptlb:Alpha01_Bright,ptin:_Alpha01_Bright,varname:node_7035,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:9784,x:33621,y:32963,varname:node_9784,prsc:2|A-5289-OUT,B-3295-OUT;n:type:ShaderForge.SFN_Pi,id:3295,x:33431,y:33086,varname:node_3295,prsc:2;n:type:ShaderForge.SFN_Rotator,id:7860,x:33801,y:33132,varname:node_7860,prsc:2|UVIN-8138-UVOUT,ANG-6966-OUT;n:type:ShaderForge.SFN_TexCoord,id:8138,x:33608,y:33132,varname:node_8138,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:6966,x:33662,y:33302,varname:node_6966,prsc:2|A-4576-OUT,B-7811-OUT;n:type:ShaderForge.SFN_Pi,id:4576,x:33475,y:33311,varname:node_4576,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:7811,x:33475,y:33437,ptovrint:False,ptlb:Alpha01_UVangle,ptin:_Alpha01_UVangle,varname:node_1273,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:675-2-664-5289-7-589-633-7443-647-7811-7035;pass:END;sub:END;*/

Shader "yh/Particle/Particle_Blend" {
    Properties {
        _E01_Bright ("E01_Bright", Float ) = 1
        _E01 ("E01", 2D) = "white" {}
        _E01_Color ("E01_Color", Color) = (1,1,1,1)
        _E01_UVangle ("E01_UVangle", Float ) = 0
        _DisE01 ("DisE01", 2D) = "white" {}
        _DisE01_value ("DisE01_value", Float ) = 0
        _DisE01_UVpan_Speed ("DisE01_UVpan_Speed", Float ) = 0
        [MaterialToggle] _DisE01_UVpan ("DisE01_U/Vpan", Float ) = 0
        _Alpha01 ("Alpha01", 2D) = "white" {}
        _Alpha01_UVangle ("Alpha01_UVangle", Float ) = 0
        _Alpha01_Bright ("Alpha01_Bright", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _E01; uniform float4 _E01_ST;
            uniform sampler2D _DisE01; uniform float4 _DisE01_ST;
            uniform float _DisE01_value;
            uniform float _DisE01_UVpan_Speed;
            uniform sampler2D _Alpha01; uniform float4 _Alpha01_ST;
            uniform float4 _E01_Color;
            uniform float _E01_Bright;
            uniform fixed _DisE01_UVpan;
            uniform float _E01_UVangle;
            uniform float _Alpha01_Bright;
            uniform float _Alpha01_UVangle;
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
                float node_2473_ang = (_E01_UVangle*3.141592654);
                float node_2473_spd = 1.0;
                float node_2473_cos = cos(node_2473_spd*node_2473_ang);
                float node_2473_sin = sin(node_2473_spd*node_2473_ang);
                float2 node_2473_piv = float2(0.5,0.5);
                float4 node_630 = _Time;
                float node_631 = (node_630.g*_DisE01_UVpan_Speed);
                float2 _DisE01_UVpan_var = lerp( (i.uv0+node_631*float2(1,0)), (i.uv0+node_631*float2(0,1)), _DisE01_UVpan );
                float4 _DisE01_var = tex2D(_DisE01,TRANSFORM_TEX(_DisE01_UVpan_var, _DisE01));
                float2 node_2473 = (mul(((_DisE01_value*_DisE01_var.r)+i.uv0)-node_2473_piv,float2x2( node_2473_cos, -node_2473_sin, node_2473_sin, node_2473_cos))+node_2473_piv);
                float4 _E01_var = tex2D(_E01,TRANSFORM_TEX(node_2473, _E01));
                float3 emissive = (((_E01_Color.rgb*_E01_var.rgb)*_E01_Bright)*i.vertexColor.rgb);
                float3 finalColor = emissive;
                float node_7860_ang = (3.141592654*_Alpha01_UVangle);
                float node_7860_spd = 1.0;
                float node_7860_cos = cos(node_7860_spd*node_7860_ang);
                float node_7860_sin = sin(node_7860_spd*node_7860_ang);
                float2 node_7860_piv = float2(0.5,0.5);
                float2 node_7860 = (mul(i.uv0-node_7860_piv,float2x2( node_7860_cos, -node_7860_sin, node_7860_sin, node_7860_cos))+node_7860_piv);
                float4 _Alpha01_var = tex2D(_Alpha01,TRANSFORM_TEX(node_7860, _Alpha01));
                fixed4 finalRGBA = fixed4(finalColor,(i.vertexColor.a*(_E01_var.r*_Alpha01_var.r*_Alpha01_Bright)));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
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
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
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
