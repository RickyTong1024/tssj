// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:False,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:False,qofs:0,qpre:1,rntp:1,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.005,fgrn:60,fgrf:150,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:33679,y:32549,varname:node_1,prsc:2|diff-1991-OUT,spec-91-OUT,gloss-89-OUT,emission-99-OUT,alpha-1993-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:32832,y:32685,ptovrint:False,ptlb:Tex01,ptin:_Tex01,varname:node_5970,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:6,x:33060,y:32879,ptovrint:False,ptlb:shininess,ptin:_shininess,varname:node_7716,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_ValueProperty,id:89,x:33490,y:32610,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:node_3399,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:90,x:33045,y:32702,varname:node_90,prsc:2|A-2-R,B-2-G;n:type:ShaderForge.SFN_Multiply,id:91,x:33208,y:32702,varname:node_91,prsc:2|A-90-OUT,B-6-OUT;n:type:ShaderForge.SFN_Fresnel,id:94,x:33337,y:33084,varname:node_94,prsc:2|EXP-98-OUT;n:type:ShaderForge.SFN_ValueProperty,id:98,x:33171,y:33105,ptovrint:False,ptlb:RimPow,ptin:_RimPow,varname:node_3302,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2.5;n:type:ShaderForge.SFN_Multiply,id:99,x:33506,y:33064,varname:node_99,prsc:2|A-100-RGB,B-94-OUT;n:type:ShaderForge.SFN_Color,id:100,x:33355,y:32927,ptovrint:False,ptlb:RimColor,ptin:_RimColor,varname:node_8269,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Power,id:105,x:33450,y:32371,varname:node_105,prsc:2|VAL-1987-OUT,EXP-106-OUT;n:type:ShaderForge.SFN_ValueProperty,id:106,x:33301,y:32449,ptovrint:False,ptlb:Tex_power,ptin:_Tex_power,varname:node_9792,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Code,id:1763,x:31766,y:31611,varname:node_1763,prsc:2,code:ZgBsAG8AYQB0ADQAIABrACAAPQBmAGwAbwBhAHQANAAoADAALgAwACwALQAxAC4AMAAvADMALgAwACwAMgAuADAALwAzAC4AMAAsAC0AMQAuADAAKQA7AAoAZgBsAG8AYQB0ADQAIABwACAAPQBSAEcAQgAuAGcAPABSAEcAQgAuAGIAPwBmAGwAbwBhAHQANAAoAFIARwBCAC4AYgAsAFIARwBCAC4AZwAsAGsALgB3ACwAawAuAHoAKQA6AGYAbABvAGEAdAA0ACgAUgBHAEIALgBnAGIALABrAC4AeAB5ACkAOwAKAGYAbABvAGEAdAA0ACAAcQAgAD0AUgBHAEIALgByADwAcAAuAHgAIAAgAD8AZgBsAG8AYQB0ADQAKABwAC4AeAAsAHAALgB5ACwAcAAuAHcALABSAEcAQgAuAHIAKQA6AGYAbABvAGEAdAA0ACgAUgBHAEIALgByACwAcAAuAHkAegB4ACkAOwAKAGYAbABvAGEAdAAgAGQAIAA9AHEALgB4AC0AbQBpAG4AKABxAC4AdwAsAHEALgB5ACkAOwAKAGYAbABvAGEAdAAgAGUAPQAxAC4AMABlAC0AMQAwADsACgByAGUAdAB1AHIAbgAgAGYAbABvAGEAdAAzACgAYQBiAHMAKABxAC4AegArACgAcQAuAHcALQBxAC4AeQApAC8AKAA2AC4AMAAqAGQAKwBlACkAKQAsAGQALwAoAHEALgB4ACsAZQApACwAcQAuAHgAKQA7AA==,output:2,fname:RGBtoHSV,width:716,height:154,input:2,input_1_label:RGB|A-2-RGB;n:type:ShaderForge.SFN_Code,id:1765,x:33251,y:31481,varname:node_1765,prsc:2,code:ZgBsAG8AYQB0ADQAIABrACAAPQAgAGYAbABvAGEAdAA0ACgAMQAuADAALAAyAC4AMAAvADMALgAwACwAMQAuADAALwAzAC4AMAAsADMALgAwACkAOwAKAGYAbABvAGEAdAAzACAAcAAgAD0AYQBiAHMAKABmAHIAYQBjACgASABTAFYALgB4AHgAeAArAGsALgB4AHkAegApACoANgAuADAALQBrAC4AdwB3AHcAKQA7AAoAcgBlAHQAdQByAG4AIABIAFMAVgAuAHoAKgBsAGUAcgBwACgAawAuAHgAeAB4ACwAYwBsAGEAbQBwACgAcAAtAGsALgB4AHgAeAAsADAALgAwACwAMQAuADAAKQAsAEgAUwBWAC4AeQApADsA,output:2,fname:HSVtoRGB,width:699,height:127,input:2,input_1_label:HSV|A-1959-OUT;n:type:ShaderForge.SFN_Add,id:1938,x:32761,y:31378,varname:node_1938,prsc:2|A-1952-X,B-1968-R;n:type:ShaderForge.SFN_Vector4Property,id:1952,x:32550,y:31460,ptovrint:False,ptlb:HSV,ptin:_HSV,varname:node_1277,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1,v2:1,v3:1,v4:1;n:type:ShaderForge.SFN_Append,id:1959,x:33078,y:31480,varname:node_1959,prsc:2|A-1961-OUT,B-1966-OUT;n:type:ShaderForge.SFN_Append,id:1961,x:32915,y:31480,varname:node_1961,prsc:2|A-1938-OUT,B-1962-OUT;n:type:ShaderForge.SFN_Multiply,id:1962,x:32761,y:31499,varname:node_1962,prsc:2|A-1952-Y,B-1968-G;n:type:ShaderForge.SFN_Multiply,id:1966,x:32761,y:31631,varname:node_1966,prsc:2|A-1952-Z,B-1968-B;n:type:ShaderForge.SFN_ComponentMask,id:1968,x:32550,y:31610,varname:node_1968,prsc:2,cc1:0,cc2:1,cc3:2,cc4:-1|IN-1763-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1977,x:33330,y:32793,ptovrint:False,ptlb:Alpha,ptin:_Alpha,varname:node_9820,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Color,id:1986,x:33144,y:32134,ptovrint:False,ptlb:Color_Tex01,ptin:_Color_Tex01,varname:node_470,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:1987,x:33301,y:32246,varname:node_1987,prsc:2|A-1986-RGB,B-1765-OUT;n:type:ShaderForge.SFN_VertexColor,id:1990,x:33330,y:32540,varname:node_1990,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1991,x:33672,y:32371,varname:node_1991,prsc:2|A-105-OUT,B-1990-RGB;n:type:ShaderForge.SFN_Multiply,id:1993,x:33500,y:32771,varname:node_1993,prsc:2|A-1990-A,B-1977-OUT;proporder:106-2-1986-1952-100-98-89-6-1977;pass:END;sub:END;*/

Shader "Shader Forge/diffuse_Rim" {
    Properties {
        _Tex_power ("Tex_power", Float ) = 1
        _Tex01 ("Tex01", 2D) = "white" {}
        _Color_Tex01 ("Color_Tex01", Color) = (1,1,1,1)
        _HSV ("HSV", Vector) = (1,1,1,1)
        _RimColor ("RimColor", Color) = (1,1,1,1)
        _RimPow ("RimPow", Float ) = 2.5
        _Gloss ("Gloss", Float ) = 1
        _shininess ("shininess", Float ) = 5
        _Alpha ("Alpha", Float ) = 1
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
            
            uniform float4 _HSV;
            uniform float _Alpha;
            uniform float4 _Color_Tex01;
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
                float node_91 = ((_Tex01_var.r*_Tex01_var.g)*_shininess);
                float3 specularColor = float3(node_91,node_91,node_91);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 node_1968 = RGBtoHSV( _Tex01_var.rgb ).rgb;
                float3 diffuseColor = (pow((_Color_Tex01.rgb*HSVtoRGB( float3(float2((_HSV.r+node_1968.r),(_HSV.g*node_1968.g)),(_HSV.b*node_1968.b)) )),_Tex_power)*i.vertexColor.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (_RimColor.rgb*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimPow));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,(i.vertexColor.a*_Alpha));
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
            
            uniform float4 _HSV;
            uniform float _Alpha;
            uniform float4 _Color_Tex01;
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
                float node_91 = ((_Tex01_var.r*_Tex01_var.g)*_shininess);
                float3 specularColor = float3(node_91,node_91,node_91);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 node_1968 = RGBtoHSV( _Tex01_var.rgb ).rgb;
                float3 diffuseColor = (pow((_Color_Tex01.rgb*HSVtoRGB( float3(float2((_HSV.r+node_1968.r),(_HSV.g*node_1968.g)),(_HSV.b*node_1968.b)) )),_Tex_power)*i.vertexColor.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * (i.vertexColor.a*_Alpha),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
