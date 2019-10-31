// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:False,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:False,qofs:1,qpre:3,rntp:2,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.005,fgrn:60,fgrf:150,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:34630,y:32549,varname:node_1,prsc:2|diff-225-OUT,emission-99-OUT,alpha-116-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33559,y:31962,ptovrint:False,ptlb:Tex01,ptin:_Tex01,varname:node_9440,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False|UVIN-164-OUT;n:type:ShaderForge.SFN_Color,id:3,x:33739,y:31847,ptovrint:False,ptlb:Color_T01,ptin:_Color_T01,varname:node_9469,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:4,x:33919,y:31942,varname:node_4,prsc:2|A-3-RGB,B-2-RGB;n:type:ShaderForge.SFN_Fresnel,id:94,x:34252,y:32669,varname:node_94,prsc:2|EXP-98-OUT;n:type:ShaderForge.SFN_ValueProperty,id:98,x:34086,y:32690,ptovrint:False,ptlb:RimPow,ptin:_RimPow,varname:node_163,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2.5;n:type:ShaderForge.SFN_Multiply,id:99,x:34421,y:32649,varname:node_99,prsc:2|A-100-RGB,B-94-OUT;n:type:ShaderForge.SFN_Color,id:100,x:34270,y:32512,ptovrint:False,ptlb:RimColor,ptin:_RimColor,varname:node_9429,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:109,x:33583,y:32210,ptovrint:False,ptlb:Tex02,ptin:_Tex02,varname:node_7945,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False|UVIN-213-UVOUT;n:type:ShaderForge.SFN_Color,id:111,x:33734,y:32210,ptovrint:False,ptlb:Color_T02,ptin:_Color_T02,varname:node_7797,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:113,x:33914,y:32290,varname:node_113,prsc:2|A-111-RGB,B-109-RGB;n:type:ShaderForge.SFN_Tex2d,id:114,x:34237,y:33003,ptovrint:False,ptlb:MaskAlpha,ptin:_MaskAlpha,varname:node_8558,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f07f6c223aa93fc4e9b22cd13e73ea52,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:116,x:34411,y:33020,varname:node_116,prsc:2|A-114-R,B-117-OUT;n:type:ShaderForge.SFN_ValueProperty,id:117,x:34256,y:33203,ptovrint:False,ptlb:IntensityA,ptin:_IntensityA,varname:node_5407,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:140,x:34379,y:32162,varname:node_140,prsc:2|VAL-218-OUT,EXP-142-OUT;n:type:ShaderForge.SFN_ValueProperty,id:142,x:34218,y:32243,ptovrint:False,ptlb:Tex_power,ptin:_Tex_power,varname:node_3446,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Add,id:164,x:33382,y:31961,varname:node_164,prsc:2|A-166-OUT,B-165-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:165,x:33233,y:32074,varname:node_165,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:166,x:33216,y:31961,varname:node_166,prsc:2|A-170-OUT,B-174-RGB;n:type:ShaderForge.SFN_ValueProperty,id:170,x:33036,y:31905,ptovrint:False,ptlb:DisT01Power,ptin:_DisT01Power,varname:node_6851,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Tex2d,id:174,x:33036,y:31993,ptovrint:False,ptlb:DisT01,ptin:_DisT01,varname:node_7624,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-202-UVOUT;n:type:ShaderForge.SFN_Time,id:187,x:32463,y:31986,varname:node_187,prsc:2;n:type:ShaderForge.SFN_Multiply,id:188,x:32702,y:32014,varname:node_188,prsc:2|A-187-T,B-189-OUT;n:type:ShaderForge.SFN_ValueProperty,id:189,x:32463,y:32129,ptovrint:False,ptlb:PanY_DisT01,ptin:_PanY_DisT01,varname:node_8384,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Time,id:194,x:32463,y:32211,varname:node_194,prsc:2;n:type:ShaderForge.SFN_Multiply,id:196,x:32702,y:32239,varname:node_196,prsc:2|A-194-T,B-198-OUT;n:type:ShaderForge.SFN_ValueProperty,id:198,x:32463,y:32354,ptovrint:False,ptlb:PanY_T02,ptin:_PanY_T02,varname:node_1530,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Panner,id:202,x:32872,y:31993,varname:node_202,prsc:2,spu:0,spv:1|UVIN-5961-UVOUT,DIST-188-OUT;n:type:ShaderForge.SFN_Panner,id:213,x:32872,y:32217,varname:node_213,prsc:2,spu:0,spv:1|UVIN-5961-UVOUT,DIST-196-OUT;n:type:ShaderForge.SFN_Add,id:218,x:34099,y:32164,varname:node_218,prsc:2|A-4-OUT,B-113-OUT;n:type:ShaderForge.SFN_VertexColor,id:224,x:34428,y:32278,varname:node_224,prsc:2;n:type:ShaderForge.SFN_Multiply,id:225,x:34576,y:32162,varname:node_225,prsc:2|A-140-OUT,B-224-RGB;n:type:ShaderForge.SFN_TexCoord,id:5961,x:32396,y:31732,varname:node_5961,prsc:2,uv:0,uaff:False;proporder:2-109-198-174-114-117-142-170-189-3-111-100-98;pass:END;sub:END;*/

Shader "Shader Forge/Trans_2TexPanYRimAmask" {
    Properties {
        _Tex01 ("Tex01", 2D) = "black" {}
        _Tex02 ("Tex02", 2D) = "black" {}
        _PanY_T02 ("PanY_T02", Float ) = 1
        _DisT01 ("DisT01", 2D) = "white" {}
        _MaskAlpha ("MaskAlpha", 2D) = "white" {}
        _IntensityA ("IntensityA", Float ) = 1
        _Tex_power ("Tex_power", Float ) = 1
        _DisT01Power ("DisT01Power", Float ) = 0.2
        _PanY_DisT01 ("PanY_DisT01", Float ) = 1
        _Color_T01 ("Color_T01", Color) = (1,1,1,1)
        _Color_T02 ("Color_T02", Color) = (1,1,1,1)
        _RimColor ("RimColor", Color) = (1,1,1,1)
        _RimPow ("RimPow", Float ) = 2.5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend One Zero
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Tex01; uniform float4 _Tex01_ST;
            uniform float4 _Color_T01;
            uniform float _RimPow;
            uniform float4 _RimColor;
            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform float4 _Color_T02;
            uniform sampler2D _MaskAlpha; uniform float4 _MaskAlpha_ST;
            uniform float _IntensityA;
            uniform float _Tex_power;
            uniform float _DisT01Power;
            uniform sampler2D _DisT01; uniform float4 _DisT01_ST;
            uniform float _PanY_DisT01;
            uniform float _PanY_T02;
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
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 node_187 = _Time;
                float2 node_202 = (i.uv0+(node_187.g*_PanY_DisT01)*float2(0,1));
                float4 _DisT01_var = tex2D(_DisT01,TRANSFORM_TEX(node_202, _DisT01));
                float3 node_164 = ((_DisT01Power*_DisT01_var.rgb)+float3(i.uv0,0.0));
                float4 _Tex01_var = tex2D(_Tex01,TRANSFORM_TEX(node_164, _Tex01));
                float4 node_194 = _Time;
                float2 node_213 = (i.uv0+(node_194.g*_PanY_T02)*float2(0,1));
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(node_213, _Tex02));
                float3 diffuseColor = (pow(((_Color_T01.rgb*_Tex01_var.rgb)+(_Color_T02.rgb*_Tex02_var.rgb)),_Tex_power)*i.vertexColor.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (_RimColor.rgb*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimPow));
/// Final Color:
                float3 finalColor = diffuse + emissive;
                float4 _MaskAlpha_var = tex2D(_MaskAlpha,TRANSFORM_TEX(i.uv0, _MaskAlpha));
                fixed4 finalRGBA = fixed4(finalColor,(_MaskAlpha_var.r*_IntensityA));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Tex01; uniform float4 _Tex01_ST;
            uniform float4 _Color_T01;
            uniform float _RimPow;
            uniform float4 _RimColor;
            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform float4 _Color_T02;
            uniform sampler2D _MaskAlpha; uniform float4 _MaskAlpha_ST;
            uniform float _IntensityA;
            uniform float _Tex_power;
            uniform float _DisT01Power;
            uniform sampler2D _DisT01; uniform float4 _DisT01_ST;
            uniform float _PanY_DisT01;
            uniform float _PanY_T02;
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
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 node_187 = _Time;
                float2 node_202 = (i.uv0+(node_187.g*_PanY_DisT01)*float2(0,1));
                float4 _DisT01_var = tex2D(_DisT01,TRANSFORM_TEX(node_202, _DisT01));
                float3 node_164 = ((_DisT01Power*_DisT01_var.rgb)+float3(i.uv0,0.0));
                float4 _Tex01_var = tex2D(_Tex01,TRANSFORM_TEX(node_164, _Tex01));
                float4 node_194 = _Time;
                float2 node_213 = (i.uv0+(node_194.g*_PanY_T02)*float2(0,1));
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(node_213, _Tex02));
                float3 diffuseColor = (pow(((_Color_T01.rgb*_Tex01_var.rgb)+(_Color_T02.rgb*_Tex02_var.rgb)),_Tex_power)*i.vertexColor.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                float4 _MaskAlpha_var = tex2D(_MaskAlpha,TRANSFORM_TEX(i.uv0, _MaskAlpha));
                fixed4 finalRGBA = fixed4(finalColor * (_MaskAlpha_var.r*_IntensityA),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
