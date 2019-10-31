// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:False,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:False,qofs:0,qpre:1,rntp:2,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.005,fgrn:60,fgrf:150,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:34922,y:32549,varname:node_1,prsc:2|diff-225-OUT,alpha-393-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:33575,y:31962,ptovrint:False,ptlb:Tex01,ptin:_Tex01,varname:node_9811,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c04ff96383ef54c4493409512680fa18,ntxv:2,isnm:False|UVIN-164-OUT;n:type:ShaderForge.SFN_Color,id:3,x:33755,y:31847,ptovrint:False,ptlb:Color_T01,ptin:_Color_T01,varname:node_2584,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:4,x:33935,y:31942,varname:node_4,prsc:2|A-3-RGB,B-2-RGB;n:type:ShaderForge.SFN_Fresnel,id:94,x:34352,y:31930,varname:node_94,prsc:2|EXP-98-OUT;n:type:ShaderForge.SFN_ValueProperty,id:98,x:34186,y:31951,ptovrint:False,ptlb:RimPow,ptin:_RimPow,varname:node_9090,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2.5;n:type:ShaderForge.SFN_Multiply,id:99,x:34521,y:31910,varname:node_99,prsc:2|A-100-RGB,B-94-OUT;n:type:ShaderForge.SFN_Color,id:100,x:34370,y:31773,ptovrint:False,ptlb:RimColor,ptin:_RimColor,varname:node_5946,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:109,x:33599,y:32210,ptovrint:False,ptlb:Tex02,ptin:_Tex02,varname:node_9998,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4e51f181444356f458eac091f4ee2144,ntxv:2,isnm:False|UVIN-213-UVOUT;n:type:ShaderForge.SFN_Color,id:111,x:33750,y:32210,ptovrint:False,ptlb:Color_T02,ptin:_Color_T02,varname:node_5616,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:113,x:33930,y:32290,varname:node_113,prsc:2|A-111-RGB,B-109-RGB;n:type:ShaderForge.SFN_ValueProperty,id:117,x:34514,y:33078,ptovrint:False,ptlb:IntensityAlpha,ptin:_IntensityAlpha,varname:node_9716,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:140,x:34671,y:32162,varname:node_140,prsc:2|VAL-230-OUT,EXP-142-OUT;n:type:ShaderForge.SFN_ValueProperty,id:142,x:34510,y:32243,ptovrint:False,ptlb:Tex_power,ptin:_Tex_power,varname:node_3406,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Add,id:164,x:33395,y:31961,varname:node_164,prsc:2|A-166-OUT,B-165-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:165,x:33249,y:32074,varname:node_165,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:166,x:33232,y:31961,varname:node_166,prsc:2|A-170-OUT,B-174-RGB;n:type:ShaderForge.SFN_ValueProperty,id:170,x:33052,y:31905,ptovrint:False,ptlb:DisT01Power,ptin:_DisT01Power,varname:node_2474,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Tex2d,id:174,x:33052,y:31993,ptovrint:False,ptlb:DisT01,ptin:_DisT01,varname:node_786,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-202-UVOUT;n:type:ShaderForge.SFN_Time,id:187,x:32479,y:31986,varname:node_187,prsc:2;n:type:ShaderForge.SFN_Multiply,id:188,x:32718,y:32014,varname:node_188,prsc:2|A-187-T,B-189-OUT;n:type:ShaderForge.SFN_ValueProperty,id:189,x:32479,y:32129,ptovrint:False,ptlb:PanY_DisT01,ptin:_PanY_DisT01,varname:node_5570,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Time,id:194,x:32479,y:32211,varname:node_194,prsc:2;n:type:ShaderForge.SFN_Multiply,id:196,x:32718,y:32239,varname:node_196,prsc:2|A-194-T,B-198-OUT;n:type:ShaderForge.SFN_ValueProperty,id:198,x:32479,y:32354,ptovrint:False,ptlb:PanY_T02,ptin:_PanY_T02,varname:node_405,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Panner,id:202,x:32888,y:31993,varname:node_202,prsc:2,spu:0,spv:1|UVIN-6386-UVOUT,DIST-188-OUT;n:type:ShaderForge.SFN_Panner,id:213,x:32888,y:32217,varname:node_213,prsc:2,spu:0,spv:1|UVIN-6386-UVOUT,DIST-196-OUT;n:type:ShaderForge.SFN_Add,id:218,x:34115,y:32164,varname:node_218,prsc:2|A-4-OUT,B-113-OUT;n:type:ShaderForge.SFN_VertexColor,id:224,x:34720,y:32278,varname:node_224,prsc:2;n:type:ShaderForge.SFN_Multiply,id:225,x:34868,y:32162,varname:node_225,prsc:2|A-140-OUT,B-224-RGB;n:type:ShaderForge.SFN_Lerp,id:230,x:34337,y:32164,varname:node_230,prsc:2|A-316-OUT,B-419-OUT,T-232-OUT;n:type:ShaderForge.SFN_Multiply,id:232,x:34187,y:32528,varname:node_232,prsc:2|A-236-RGB,B-240-OUT;n:type:ShaderForge.SFN_Tex2d,id:233,x:33572,y:32572,ptovrint:False,ptlb:Tex03,ptin:_Tex03,varname:node_368,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:236,x:33960,y:32528,ptovrint:False,ptlb:MaskT03,ptin:_MaskT03,varname:node_8274,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f07f6c223aa93fc4e9b22cd13e73ea52,ntxv:2,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:240,x:34039,y:32738,ptovrint:False,ptlb:Intensity_Mask,ptin:_Intensity_Mask,varname:node_8929,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:278,x:34575,y:32913,varname:node_278,prsc:2|A-224-A,B-117-OUT;n:type:ShaderForge.SFN_Add,id:316,x:34232,y:32056,varname:node_316,prsc:2|A-99-OUT,B-218-OUT;n:type:ShaderForge.SFN_Add,id:393,x:34734,y:32835,varname:node_393,prsc:2|A-236-R,B-278-OUT;n:type:ShaderForge.SFN_Color,id:417,x:33578,y:32395,ptovrint:False,ptlb:Color_T03,ptin:_Color_T03,varname:node_2298,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:419,x:33758,y:32490,varname:node_419,prsc:2|A-417-RGB,B-233-RGB;n:type:ShaderForge.SFN_TexCoord,id:6386,x:32412,y:31718,varname:node_6386,prsc:2,uv:0,uaff:False;proporder:2-174-170-189-109-198-233-236-142-3-111-417-100-98-240-117;pass:END;sub:END;*/

Shader "Shader Forge/Trans_crystal" {
    Properties {
        _Tex01 ("Tex01", 2D) = "black" {}
        _DisT01 ("DisT01", 2D) = "white" {}
        _DisT01Power ("DisT01Power", Float ) = 0.2
        _PanY_DisT01 ("PanY_DisT01", Float ) = 1
        _Tex02 ("Tex02", 2D) = "black" {}
        _PanY_T02 ("PanY_T02", Float ) = 1
        _Tex03 ("Tex03", 2D) = "white" {}
        _MaskT03 ("MaskT03", 2D) = "black" {}
        _Tex_power ("Tex_power", Float ) = 1
        _Color_T01 ("Color_T01", Color) = (1,1,1,1)
        _Color_T02 ("Color_T02", Color) = (1,1,1,1)
        _Color_T03 ("Color_T03", Color) = (1,1,1,1)
        _RimColor ("RimColor", Color) = (1,1,1,1)
        _RimPow ("RimPow", Float ) = 2.5
        _Intensity_Mask ("Intensity_Mask", Float ) = 1
        _IntensityAlpha ("IntensityAlpha", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
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
            uniform float _IntensityAlpha;
            uniform float _Tex_power;
            uniform float _DisT01Power;
            uniform sampler2D _DisT01; uniform float4 _DisT01_ST;
            uniform float _PanY_DisT01;
            uniform float _PanY_T02;
            uniform sampler2D _Tex03; uniform float4 _Tex03_ST;
            uniform sampler2D _MaskT03; uniform float4 _MaskT03_ST;
            uniform float _Intensity_Mask;
            uniform float4 _Color_T03;
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
                float4 _Tex03_var = tex2D(_Tex03,TRANSFORM_TEX(i.uv0, _Tex03));
                float4 _MaskT03_var = tex2D(_MaskT03,TRANSFORM_TEX(i.uv0, _MaskT03));
                float3 diffuseColor = (pow(lerp(((_RimColor.rgb*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimPow))+((_Color_T01.rgb*_Tex01_var.rgb)+(_Color_T02.rgb*_Tex02_var.rgb))),(_Color_T03.rgb*_Tex03_var.rgb),(_MaskT03_var.rgb*_Intensity_Mask)),_Tex_power)*i.vertexColor.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,(_MaskT03_var.r+(i.vertexColor.a*_IntensityAlpha)));
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
            uniform float _IntensityAlpha;
            uniform float _Tex_power;
            uniform float _DisT01Power;
            uniform sampler2D _DisT01; uniform float4 _DisT01_ST;
            uniform float _PanY_DisT01;
            uniform float _PanY_T02;
            uniform sampler2D _Tex03; uniform float4 _Tex03_ST;
            uniform sampler2D _MaskT03; uniform float4 _MaskT03_ST;
            uniform float _Intensity_Mask;
            uniform float4 _Color_T03;
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
                float4 _Tex03_var = tex2D(_Tex03,TRANSFORM_TEX(i.uv0, _Tex03));
                float4 _MaskT03_var = tex2D(_MaskT03,TRANSFORM_TEX(i.uv0, _MaskT03));
                float3 diffuseColor = (pow(lerp(((_RimColor.rgb*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimPow))+((_Color_T01.rgb*_Tex01_var.rgb)+(_Color_T02.rgb*_Tex02_var.rgb))),(_Color_T03.rgb*_Tex03_var.rgb),(_MaskT03_var.rgb*_Intensity_Mask)),_Tex_power)*i.vertexColor.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * (_MaskT03_var.r+(i.vertexColor.a*_IntensityAlpha)),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
