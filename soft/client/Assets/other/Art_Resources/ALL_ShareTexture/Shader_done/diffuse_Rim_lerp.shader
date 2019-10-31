// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:False,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:False,qofs:0,qpre:1,rntp:1,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.005,fgrn:60,fgrf:150,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:33926,y:32549,varname:node_1,prsc:2|diff-130-OUT,spec-91-OUT,gloss-89-OUT,emission-99-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:32918,y:31977,ptovrint:False,ptlb:Tex01,ptin:_Tex01,varname:node_9021,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:6,x:33396,y:32982,ptovrint:False,ptlb:shininess,ptin:_shininess,varname:node_2997,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:89,x:33736,y:32611,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:node_7629,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:90,x:33436,y:32788,varname:node_90,prsc:2|A-2-R,B-109-R;n:type:ShaderForge.SFN_Multiply,id:91,x:33632,y:32788,varname:node_91,prsc:2|A-90-OUT,B-6-OUT;n:type:ShaderForge.SFN_Fresnel,id:94,x:33584,y:33084,varname:node_94,prsc:2|EXP-98-OUT;n:type:ShaderForge.SFN_ValueProperty,id:98,x:33418,y:33105,ptovrint:False,ptlb:RimPow,ptin:_RimPow,varname:node_2465,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2.5;n:type:ShaderForge.SFN_Multiply,id:99,x:33753,y:33064,varname:node_99,prsc:2|A-100-RGB,B-94-OUT;n:type:ShaderForge.SFN_Color,id:100,x:33602,y:32927,ptovrint:False,ptlb:RimColor,ptin:_RimColor,varname:node_8556,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Lerp,id:107,x:33467,y:32242,varname:node_107,prsc:2|A-1819-OUT,B-113-OUT,T-139-OUT;n:type:ShaderForge.SFN_Tex2d,id:109,x:32789,y:32246,ptovrint:False,ptlb:Tex02,ptin:_Tex02,varname:node_2215,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Color,id:111,x:32937,y:32246,ptovrint:False,ptlb:Color_Tex02,ptin:_Color_Tex02,varname:node_3616,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:113,x:33229,y:32263,varname:node_113,prsc:2|A-111-RGB,B-109-RGB;n:type:ShaderForge.SFN_Tex2d,id:114,x:32996,y:32424,ptovrint:False,ptlb:Mask_Tex02,ptin:_Mask_Tex02,varname:node_2087,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f07f6c223aa93fc4e9b22cd13e73ea52,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:116,x:33229,y:32458,varname:node_116,prsc:2|A-114-RGB,B-117-OUT;n:type:ShaderForge.SFN_ValueProperty,id:117,x:33080,y:32615,ptovrint:False,ptlb:alpha_mask,ptin:_alpha_mask,varname:node_5156,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Power,id:130,x:33765,y:32301,varname:node_130,prsc:2|VAL-107-OUT,EXP-132-OUT;n:type:ShaderForge.SFN_ValueProperty,id:132,x:33614,y:32395,ptovrint:False,ptlb:Tex_power,ptin:_Tex_power,varname:node_2517,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:139,x:33416,y:32458,varname:node_139,prsc:2|A-2-R,B-116-OUT;n:type:ShaderForge.SFN_Code,id:1817,x:31949,y:31675,varname:node_1817,prsc:2,code:ZgBsAG8AYQB0ADQAIABrACAAPQBmAGwAbwBhAHQANAAoADAALgAwACwALQAxAC4AMAAvADMALgAwACwAMgAuADAALwAzAC4AMAAsAC0AMQAuADAAKQA7AAoAZgBsAG8AYQB0ADQAIABwACAAPQBSAEcAQgAuAGcAPABSAEcAQgAuAGIAPwBmAGwAbwBhAHQANAAoAFIARwBCAC4AYgAsAFIARwBCAC4AZwAsAGsALgB3ACwAawAuAHoAKQA6AGYAbABvAGEAdAA0ACgAUgBHAEIALgBnAGIALABrAC4AeAB5ACkAOwAKAGYAbABvAGEAdAA0ACAAcQAgAD0AUgBHAEIALgByADwAcAAuAHgAIAAgAD8AZgBsAG8AYQB0ADQAKABwAC4AeAAsAHAALgB5ACwAcAAuAHcALABSAEcAQgAuAHIAKQA6AGYAbABvAGEAdAA0ACgAUgBHAEIALgByACwAcAAuAHkAegB4ACkAOwAKAGYAbABvAGEAdAAgAGQAIAA9AHEALgB4AC0AbQBpAG4AKABxAC4AdwAsAHEALgB5ACkAOwAKAGYAbABvAGEAdAAgAGUAPQAxAC4AMABlAC0AMQAwADsACgByAGUAdAB1AHIAbgAgAGYAbABvAGEAdAAzACgAYQBiAHMAKABxAC4AegArACgAcQAuAHcALQBxAC4AeQApAC8AKAA2AC4AMAAqAGQAKwBlACkAKQAsAGQALwAoAHEALgB4ACsAZQApACwAcQAuAHgAKQA7AA==,output:2,fname:RGBtoHSV,width:716,height:154,input:2,input_1_label:RGB|A-2-RGB;n:type:ShaderForge.SFN_Code,id:1819,x:33434,y:31545,varname:node_1819,prsc:2,code:ZgBsAG8AYQB0ADQAIABrACAAPQAgAGYAbABvAGEAdAA0ACgAMQAuADAALAAyAC4AMAAvADMALgAwACwAMQAuADAALwAzAC4AMAAsADMALgAwACkAOwAKAGYAbABvAGEAdAAzACAAcAAgAD0AYQBiAHMAKABmAHIAYQBjACgASABTAFYALgB4AHgAeAArAGsALgB4AHkAegApACoANgAuADAALQBrAC4AdwB3AHcAKQA7AAoAcgBlAHQAdQByAG4AIABIAFMAVgAuAHoAKgBsAGUAcgBwACgAawAuAHgAeAB4ACwAYwBsAGEAbQBwACgAcAAtAGsALgB4AHgAeAAsADAALgAwACwAMQAuADAAKQAsAEgAUwBWAC4AeQApADsA,output:2,fname:HSVtoRGB,width:699,height:127,input:2,input_1_label:HSV|A-1960-OUT;n:type:ShaderForge.SFN_Add,id:1939,x:32944,y:31442,varname:node_1939,prsc:2|A-1953-X,B-1969-R;n:type:ShaderForge.SFN_Vector4Property,id:1953,x:32733,y:31524,ptovrint:False,ptlb:Hsv,ptin:_Hsv,varname:node_2577,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1,v2:1,v3:1,v4:1;n:type:ShaderForge.SFN_Append,id:1960,x:33261,y:31544,varname:node_1960,prsc:2|A-1962-OUT,B-1967-OUT;n:type:ShaderForge.SFN_Append,id:1962,x:33098,y:31544,varname:node_1962,prsc:2|A-1939-OUT,B-1964-OUT;n:type:ShaderForge.SFN_Multiply,id:1964,x:32944,y:31563,varname:node_1964,prsc:2|A-1953-Y,B-1969-G;n:type:ShaderForge.SFN_Multiply,id:1967,x:32944,y:31695,varname:node_1967,prsc:2|A-1953-Z,B-1969-B;n:type:ShaderForge.SFN_ComponentMask,id:1969,x:32733,y:31674,varname:node_1969,prsc:2,cc1:0,cc2:1,cc3:2,cc4:-1|IN-1817-OUT;proporder:132-2-1953-109-114-111-117-100-98-89-6;pass:END;sub:END;*/

Shader "Shader Forge/diffuse_Rim_lerp" {
    Properties {
        _Tex_power ("Tex_power", Float ) = 1
        _Tex01 ("Tex01", 2D) = "black" {}
        _Hsv ("Hsv", Vector) = (1,1,1,1)
        _Tex02 ("Tex02", 2D) = "black" {}
        _Mask_Tex02 ("Mask_Tex02", 2D) = "white" {}
        _Color_Tex02 ("Color_Tex02", Color) = (1,1,1,1)
        _alpha_mask ("alpha_mask", Float ) = 1
        _RimColor ("RimColor", Color) = (1,1,1,1)
        _RimPow ("RimPow", Float ) = 2.5
        _Gloss ("Gloss", Float ) = 1
        _shininess ("shininess", Float ) = 1
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
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Tex01; uniform float4 _Tex01_ST;
            uniform float _shininess;
            uniform float _Gloss;
            uniform float _RimPow;
            uniform float4 _RimColor;
            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform float4 _Color_Tex02;
            uniform sampler2D _Mask_Tex02; uniform float4 _Mask_Tex02_ST;
            uniform float _alpha_mask;
            uniform float _Tex_power;
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
            
            uniform float4 _Hsv;
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
                float4 _Tex01_var = tex2D(_Tex01,TRANSFORM_TEX(i.uv0, _Tex01));
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(i.uv0, _Tex02));
                float node_91 = ((_Tex01_var.r*_Tex02_var.r)*_shininess);
                float3 specularColor = float3(node_91,node_91,node_91);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 node_1969 = RGBtoHSV( _Tex01_var.rgb ).rgb;
                float4 _Mask_Tex02_var = tex2D(_Mask_Tex02,TRANSFORM_TEX(i.uv0, _Mask_Tex02));
                float3 diffuseColor = pow(lerp(HSVtoRGB( float3(float2((_Hsv.r+node_1969.r),(_Hsv.g*node_1969.g)),(_Hsv.b*node_1969.b)) ),(_Color_Tex02.rgb*_Tex02_var.rgb),(_Tex01_var.r*(_Mask_Tex02_var.rgb*_alpha_mask))),_Tex_power);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (_RimColor.rgb*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimPow));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
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
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Tex01; uniform float4 _Tex01_ST;
            uniform float _shininess;
            uniform float _Gloss;
            uniform float _RimPow;
            uniform float4 _RimColor;
            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform float4 _Color_Tex02;
            uniform sampler2D _Mask_Tex02; uniform float4 _Mask_Tex02_ST;
            uniform float _alpha_mask;
            uniform float _Tex_power;
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
            
            uniform float4 _Hsv;
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
                float4 _Tex01_var = tex2D(_Tex01,TRANSFORM_TEX(i.uv0, _Tex01));
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(i.uv0, _Tex02));
                float node_91 = ((_Tex01_var.r*_Tex02_var.r)*_shininess);
                float3 specularColor = float3(node_91,node_91,node_91);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 node_1969 = RGBtoHSV( _Tex01_var.rgb ).rgb;
                float4 _Mask_Tex02_var = tex2D(_Mask_Tex02,TRANSFORM_TEX(i.uv0, _Mask_Tex02));
                float3 diffuseColor = pow(lerp(HSVtoRGB( float3(float2((_Hsv.r+node_1969.r),(_Hsv.g*node_1969.g)),(_Hsv.b*node_1969.b)) ),(_Color_Tex02.rgb*_Tex02_var.rgb),(_Tex01_var.r*(_Mask_Tex02_var.rgb*_alpha_mask))),_Tex_power);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
