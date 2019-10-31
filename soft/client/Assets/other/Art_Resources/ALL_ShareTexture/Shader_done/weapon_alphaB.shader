// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:False,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:False,qofs:0,qpre:1,rntp:1,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.005,fgrn:60,fgrf:150,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:34908,y:32549,varname:node_1,prsc:2|diff-130-OUT,spec-91-OUT,gloss-89-OUT,emission-99-OUT,alpha-109-A,clip-458-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:32625,y:32438,ptovrint:False,ptlb:Tex01,ptin:_Tex01,varname:node_1664,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:169301305f2ac824cb471334d4b86e99,ntxv:2,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:6,x:34453,y:32663,ptovrint:False,ptlb:shininess,ptin:_shininess,varname:node_1898,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:89,x:34718,y:32611,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:node_976,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:90,x:34424,y:32508,varname:node_90,prsc:2|A-443-OUT,B-443-OUT;n:type:ShaderForge.SFN_Multiply,id:91,x:34601,y:32508,varname:node_91,prsc:2|A-90-OUT,B-6-OUT;n:type:ShaderForge.SFN_Fresnel,id:94,x:34376,y:33001,varname:node_94,prsc:2|EXP-98-OUT;n:type:ShaderForge.SFN_ValueProperty,id:98,x:34203,y:33021,ptovrint:False,ptlb:RimPow,ptin:_RimPow,varname:node_3165,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:99,x:34545,y:32981,varname:node_99,prsc:2|A-100-RGB,B-94-OUT;n:type:ShaderForge.SFN_Color,id:100,x:34394,y:32844,ptovrint:False,ptlb:RimColor,ptin:_RimColor,varname:node_8516,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:109,x:33523,y:31976,ptovrint:False,ptlb:Tex02,ptin:_Tex02,varname:node_4992,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:169301305f2ac824cb471334d4b86e99,ntxv:2,isnm:False|UVIN-398-UVOUT;n:type:ShaderForge.SFN_Color,id:111,x:33674,y:31976,ptovrint:False,ptlb:Color_Tex02,ptin:_Color_Tex02,varname:node_7071,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:113,x:33854,y:31976,varname:node_113,prsc:2|A-111-RGB,B-109-RGB;n:type:ShaderForge.SFN_Tex2d,id:114,x:33674,y:32145,ptovrint:False,ptlb:mask_T02,ptin:_mask_T02,varname:node_2955,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:3bb27cdf3aaefd24a8e983230d2f77b3,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Power,id:130,x:34747,y:32301,varname:node_130,prsc:2|VAL-419-OUT,EXP-132-OUT;n:type:ShaderForge.SFN_ValueProperty,id:132,x:34596,y:32395,ptovrint:False,ptlb:Tex_power,ptin:_Tex_power,varname:node_6934,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:382,x:34081,y:31976,varname:node_382,prsc:2|A-113-OUT,B-114-RGB;n:type:ShaderForge.SFN_Panner,id:398,x:33338,y:31976,varname:node_398,prsc:2,spu:1,spv:0|UVIN-4038-UVOUT,DIST-400-OUT;n:type:ShaderForge.SFN_Time,id:399,x:33026,y:31940,varname:node_399,prsc:2;n:type:ShaderForge.SFN_Multiply,id:400,x:33182,y:31996,varname:node_400,prsc:2|A-399-T,B-402-OUT;n:type:ShaderForge.SFN_ValueProperty,id:402,x:33026,y:32109,ptovrint:False,ptlb:PanU_T02,ptin:_PanU_T02,varname:node_4243,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Add,id:419,x:34230,y:32196,varname:node_419,prsc:2|A-382-OUT,B-443-OUT;n:type:ShaderForge.SFN_Multiply,id:443,x:34072,y:32355,varname:node_443,prsc:2|A-477-OUT,B-1766-OUT;n:type:ShaderForge.SFN_Subtract,id:458,x:34030,y:32788,varname:node_458,prsc:2|A-109-A,B-459-OUT;n:type:ShaderForge.SFN_Slider,id:459,x:33654,y:32831,ptovrint:False,ptlb:clip,ptin:_clip,varname:node_213,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_Subtract,id:477,x:33923,y:32197,varname:node_477,prsc:2|A-478-OUT,B-114-RGB;n:type:ShaderForge.SFN_Vector1,id:478,x:33870,y:32133,varname:node_478,prsc:2,v1:1;n:type:ShaderForge.SFN_Code,id:1764,x:31806,y:31762,varname:node_1764,prsc:2,code:ZgBsAG8AYQB0ADQAIABrACAAPQBmAGwAbwBhAHQANAAoADAALgAwACwALQAxAC4AMAAvADMALgAwACwAMgAuADAALwAzAC4AMAAsAC0AMQAuADAAKQA7AAoAZgBsAG8AYQB0ADQAIABwACAAPQBSAEcAQgAuAGcAPABSAEcAQgAuAGIAPwBmAGwAbwBhAHQANAAoAFIARwBCAC4AYgAsAFIARwBCAC4AZwAsAGsALgB3ACwAawAuAHoAKQA6AGYAbABvAGEAdAA0ACgAUgBHAEIALgBnAGIALABrAC4AeAB5ACkAOwAKAGYAbABvAGEAdAA0ACAAcQAgAD0AUgBHAEIALgByADwAcAAuAHgAIAAgAD8AZgBsAG8AYQB0ADQAKABwAC4AeAAsAHAALgB5ACwAcAAuAHcALABSAEcAQgAuAHIAKQA6AGYAbABvAGEAdAA0ACgAUgBHAEIALgByACwAcAAuAHkAegB4ACkAOwAKAGYAbABvAGEAdAAgAGQAIAA9AHEALgB4AC0AbQBpAG4AKABxAC4AdwAsAHEALgB5ACkAOwAKAGYAbABvAGEAdAAgAGUAPQAxAC4AMABlAC0AMQAwADsACgByAGUAdAB1AHIAbgAgAGYAbABvAGEAdAAzACgAYQBiAHMAKABxAC4AegArACgAcQAuAHcALQBxAC4AeQApAC8AKAA2AC4AMAAqAGQAKwBlACkAKQAsAGQALwAoAHEALgB4ACsAZQApACwAcQAuAHgAKQA7AA==,output:2,fname:RGBtoHSV,width:938,height:154,input:2,input_1_label:RGB|A-2-RGB;n:type:ShaderForge.SFN_Code,id:1766,x:33513,y:31632,varname:node_1766,prsc:2,code:ZgBsAG8AYQB0ADQAIABrACAAPQAgAGYAbABvAGEAdAA0ACgAMQAuADAALAAyAC4AMAAvADMALgAwACwAMQAuADAALwAzAC4AMAAsADMALgAwACkAOwAKAGYAbABvAGEAdAAzACAAcAAgAD0AYQBiAHMAKABmAHIAYQBjACgASABTAFYALgB4AHgAeAArAGsALgB4AHkAegApACoANgAuADAALQBrAC4AdwB3AHcAKQA7AAoAcgBlAHQAdQByAG4AIABIAFMAVgAuAHoAKgBsAGUAcgBwACgAawAuAHgAeAB4ACwAYwBsAGEAbQBwACgAcAAtAGsALgB4AHgAeAAsADAALgAwACwAMQAuADAAKQAsAEgAUwBWAC4AeQApADsA,output:2,fname:HSVtoRGB,width:699,height:127,input:2,input_1_label:HSV|A-1960-OUT;n:type:ShaderForge.SFN_Add,id:1939,x:33023,y:31529,varname:node_1939,prsc:2|A-1953-X,B-1969-R;n:type:ShaderForge.SFN_Vector4Property,id:1953,x:32812,y:31611,ptovrint:False,ptlb:HSV,ptin:_HSV,varname:node_2330,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1,v2:1,v3:1,v4:1;n:type:ShaderForge.SFN_Append,id:1960,x:33340,y:31631,varname:node_1960,prsc:2|A-1962-OUT,B-1967-OUT;n:type:ShaderForge.SFN_Append,id:1962,x:33177,y:31631,varname:node_1962,prsc:2|A-1939-OUT,B-1964-OUT;n:type:ShaderForge.SFN_Multiply,id:1964,x:33023,y:31650,varname:node_1964,prsc:2|A-1953-Y,B-1969-G;n:type:ShaderForge.SFN_Multiply,id:1967,x:33023,y:31782,varname:node_1967,prsc:2|A-1953-Z,B-1969-B;n:type:ShaderForge.SFN_ComponentMask,id:1969,x:32812,y:31761,varname:node_1969,prsc:2,cc1:0,cc2:1,cc3:2,cc4:-1|IN-1764-OUT;n:type:ShaderForge.SFN_TexCoord,id:4038,x:32659,y:31937,varname:node_4038,prsc:2,uv:0,uaff:False;proporder:132-2-1953-109-402-114-111-100-98-89-6-459;pass:END;sub:END;*/

Shader "Shader Forge/weapon_alphaB" {
    Properties {
        _Tex_power ("Tex_power", Float ) = 1
        _Tex01 ("Tex01", 2D) = "black" {}
        _HSV ("HSV", Vector) = (1,1,1,1)
        _Tex02 ("Tex02", 2D) = "black" {}
        _PanU_T02 ("PanU_T02", Float ) = 0.1
        _mask_T02 ("mask_T02", 2D) = "white" {}
        _Color_Tex02 ("Color_Tex02", Color) = (1,1,1,1)
        _RimColor ("RimColor", Color) = (1,1,1,1)
        _RimPow ("RimPow", Float ) = 3
        _Gloss ("Gloss", Float ) = 1
        _shininess ("shininess", Float ) = 1
        _clip ("clip", Range(-1, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            
            
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
            uniform float _shininess;
            uniform float _Gloss;
            uniform float _RimPow;
            uniform float4 _RimColor;
            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform float4 _Color_Tex02;
            uniform sampler2D _mask_T02; uniform float4 _mask_T02_ST;
            uniform float _Tex_power;
            uniform float _PanU_T02;
            uniform float _clip;
            float3 RGBtoHSV( float3 RGB ){
            float4 k =float4(0.0,-1.0/3.0,2.0/3.0,-1.0);
            float4 p =RGB.g<RGB.b?float4(RGB.b,RGB.g,k.w,k.z):float4(RGB.gb,k.xy);
            float4 q =RGB.r<p.x  ?float4(p.x,p.y,p.w,RGB.r):float4(RGB.r,p.yzx);
            float d =q.x-min(q.w,q.y);
            float e=1.0e-10;
            return float3(abs(q.z+(q.w-q.y)/(6.0*d+e)),d/(q.x+e),q.x);
            }
            
            float3 HSVtoRGB( float3 HSV ){
            float4 k = float4(1.0,2.0/3.0,1.0/3.0,3.0);
            float3 p =abs(frac(HSV.xxx+k.xyz)*6.0-k.www);
            return HSV.z*lerp(k.xxx,clamp(p-k.xxx,0.0,1.0),HSV.y);
            }
            
            uniform float4 _HSV;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
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
                float4 node_399 = _Time;
                float2 node_398 = (i.uv0+(node_399.g*_PanU_T02)*float2(1,0));
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(node_398, _Tex02));
                clip((_Tex02_var.a-_clip) - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float4 _mask_T02_var = tex2D(_mask_T02,TRANSFORM_TEX(i.uv0, _mask_T02));
                float4 _Tex01_var = tex2D(_Tex01,TRANSFORM_TEX(i.uv0, _Tex01));
                float3 node_1969 = RGBtoHSV( _Tex01_var.rgb ).rgb;
                float3 node_443 = ((1.0-_mask_T02_var.rgb)*HSVtoRGB( float3(float2((_HSV.r+node_1969.r),(_HSV.g*node_1969.g)),(_HSV.b*node_1969.b)) ));
                float3 specularColor = ((node_443*node_443)*_shininess);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = pow((((_Color_Tex02.rgb*_Tex02_var.rgb)*_mask_T02_var.rgb)+node_443),_Tex_power);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (_RimColor.rgb*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimPow));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,_Tex02_var.a);
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
            uniform float _shininess;
            uniform float _Gloss;
            uniform float _RimPow;
            uniform float4 _RimColor;
            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform float4 _Color_Tex02;
            uniform sampler2D _mask_T02; uniform float4 _mask_T02_ST;
            uniform float _Tex_power;
            uniform float _PanU_T02;
            uniform float _clip;
            float3 RGBtoHSV( float3 RGB ){
            float4 k =float4(0.0,-1.0/3.0,2.0/3.0,-1.0);
            float4 p =RGB.g<RGB.b?float4(RGB.b,RGB.g,k.w,k.z):float4(RGB.gb,k.xy);
            float4 q =RGB.r<p.x  ?float4(p.x,p.y,p.w,RGB.r):float4(RGB.r,p.yzx);
            float d =q.x-min(q.w,q.y);
            float e=1.0e-10;
            return float3(abs(q.z+(q.w-q.y)/(6.0*d+e)),d/(q.x+e),q.x);
            }
            
            float3 HSVtoRGB( float3 HSV ){
            float4 k = float4(1.0,2.0/3.0,1.0/3.0,3.0);
            float3 p =abs(frac(HSV.xxx+k.xyz)*6.0-k.www);
            return HSV.z*lerp(k.xxx,clamp(p-k.xxx,0.0,1.0),HSV.y);
            }
            
            uniform float4 _HSV;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
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
                float4 node_399 = _Time;
                float2 node_398 = (i.uv0+(node_399.g*_PanU_T02)*float2(1,0));
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(node_398, _Tex02));
                clip((_Tex02_var.a-_clip) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float4 _mask_T02_var = tex2D(_mask_T02,TRANSFORM_TEX(i.uv0, _mask_T02));
                float4 _Tex01_var = tex2D(_Tex01,TRANSFORM_TEX(i.uv0, _Tex01));
                float3 node_1969 = RGBtoHSV( _Tex01_var.rgb ).rgb;
                float3 node_443 = ((1.0-_mask_T02_var.rgb)*HSVtoRGB( float3(float2((_HSV.r+node_1969.r),(_HSV.g*node_1969.g)),(_HSV.b*node_1969.b)) ));
                float3 specularColor = ((node_443*node_443)*_shininess);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuseColor = pow((((_Color_Tex02.rgb*_Tex02_var.rgb)*_mask_T02_var.rgb)+node_443),_Tex_power);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * _Tex02_var.a,0);
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
            Cull Back
            
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
            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform float _PanU_T02;
            uniform float _clip;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 node_399 = _Time;
                float2 node_398 = (i.uv0+(node_399.g*_PanU_T02)*float2(1,0));
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(node_398, _Tex02));
                clip((_Tex02_var.a-_clip) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
