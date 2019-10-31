// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:False,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:False,qofs:0,qpre:1,rntp:1,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.005,fgrn:60,fgrf:150,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1,x:34178,y:32549,varname:node_1,prsc:2|diff-1099-OUT,emission-153-OUT,alpha-4143-A;n:type:ShaderForge.SFN_Tex2d,id:2,x:31655,y:31624,ptovrint:False,ptlb:D01,ptin:_D01,varname:node_7097,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Fresnel,id:94,x:33202,y:33186,varname:node_94,prsc:2|EXP-3320-OUT;n:type:ShaderForge.SFN_Multiply,id:99,x:33702,y:33167,varname:node_99,prsc:2|A-100-RGB,B-3805-OUT;n:type:ShaderForge.SFN_Color,id:100,x:33551,y:33030,ptovrint:False,ptlb:RimColor,ptin:_RimColor,varname:node_7203,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Lerp,id:107,x:34119,y:31900,varname:node_107,prsc:2|A-1976-OUT,B-113-OUT,T-382-OUT;n:type:ShaderForge.SFN_Tex2d,id:109,x:32674,y:31920,ptovrint:False,ptlb:D02,ptin:_D02,varname:node_2757,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Color,id:111,x:33449,y:31783,ptovrint:False,ptlb:D02_Color,ptin:_D02_Color,varname:node_5079,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:113,x:33686,y:31904,varname:node_113,prsc:2|A-111-RGB,B-109-RGB;n:type:ShaderForge.SFN_Tex2d,id:114,x:33449,y:31997,ptovrint:False,ptlb:MaskD02,ptin:_MaskD02,varname:node_6007,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:117,x:33449,y:32185,ptovrint:False,ptlb:MaskD02_Bright,ptin:_MaskD02_Bright,varname:node_7843,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Power,id:130,x:34017,y:32301,varname:node_130,prsc:2|VAL-107-OUT,EXP-132-OUT;n:type:ShaderForge.SFN_ValueProperty,id:132,x:33835,y:32374,ptovrint:False,ptlb:Tex_power,ptin:_Tex_power,varname:node_3736,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:147,x:33048,y:32634,ptovrint:False,ptlb:MaskE01,ptin:_MaskE01,varname:node_7991,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:148,x:33048,y:32812,ptovrint:False,ptlb:E01,ptin:_E01,varname:node_6313,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False|UVIN-4265-UVOUT;n:type:ShaderForge.SFN_Multiply,id:149,x:33268,y:32761,varname:node_149,prsc:2|A-147-RGB,B-148-RGB;n:type:ShaderForge.SFN_Color,id:151,x:33214,y:32973,ptovrint:False,ptlb:E01_Color,ptin:_E01_Color,varname:node_8163,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:152,x:33463,y:32771,varname:node_152,prsc:2|A-149-OUT,B-151-RGB;n:type:ShaderForge.SFN_Add,id:153,x:34011,y:32770,varname:node_153,prsc:2|A-152-OUT,B-99-OUT;n:type:ShaderForge.SFN_Panner,id:159,x:32449,y:32755,varname:node_159,prsc:2,spu:1,spv:0|UVIN-4059-UVOUT,DIST-192-OUT;n:type:ShaderForge.SFN_Multiply,id:192,x:32149,y:32811,varname:node_192,prsc:2|A-334-T,B-233-OUT;n:type:ShaderForge.SFN_ValueProperty,id:233,x:32002,y:32918,ptovrint:False,ptlb:E01_UVpan_Speed,ptin:_E01_UVpan_Speed,varname:node_8437,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Time,id:334,x:31985,y:32751,varname:node_334,prsc:2;n:type:ShaderForge.SFN_Multiply,id:382,x:33686,y:32036,varname:node_382,prsc:2|A-114-R,B-117-OUT;n:type:ShaderForge.SFN_Code,id:1974,x:32137,y:31528,varname:node_1974,prsc:2,code:ZgBsAG8AYQB0ADQAIABrACAAPQBmAGwAbwBhAHQANAAoADAALgAwACwALQAxAC4AMAAvADMALgAwACwAMgAuADAALwAzAC4AMAAsAC0AMQAuADAAKQA7AAoAZgBsAG8AYQB0ADQAIABwACAAPQBSAEcAQgAuAGcAPABSAEcAQgAuAGIAPwBmAGwAbwBhAHQANAAoAFIARwBCAC4AYgAsAFIARwBCAC4AZwAsAGsALgB3ACwAawAuAHoAKQA6AGYAbABvAGEAdAA0ACgAUgBHAEIALgBnAGIALABrAC4AeAB5ACkAOwAKAGYAbABvAGEAdAA0ACAAcQAgAD0AUgBHAEIALgByADwAcAAuAHgAIAAgAD8AZgBsAG8AYQB0ADQAKABwAC4AeAAsAHAALgB5ACwAcAAuAHcALABSAEcAQgAuAHIAKQA6AGYAbABvAGEAdAA0ACgAUgBHAEIALgByACwAcAAuAHkAegB4ACkAOwAKAGYAbABvAGEAdAAgAGQAIAA9AHEALgB4AC0AbQBpAG4AKABxAC4AdwAsAHEALgB5ACkAOwAKAGYAbABvAGEAdAAgAGUAPQAxAC4AMABlAC0AMQAwADsACgByAGUAdAB1AHIAbgAgAGYAbABvAGEAdAAzACgAYQBiAHMAKABxAC4AegArACgAcQAuAHcALQBxAC4AeQApAC8AKAA2AC4AMAAqAGQAKwBlACkAKQAsAGQALwAoAHEALgB4ACsAZQApACwAcQAuAHgAKQA7AA==,output:2,fname:RGBtoHSV,width:716,height:154,input:2,input_1_label:RGB|A-3536-OUT;n:type:ShaderForge.SFN_Code,id:1976,x:33622,y:31398,varname:node_1976,prsc:2,code:ZgBsAG8AYQB0ADQAIABrACAAPQAgAGYAbABvAGEAdAA0ACgAMQAuADAALAAyAC4AMAAvADMALgAwACwAMQAuADAALwAzAC4AMAAsADMALgAwACkAOwAKAGYAbABvAGEAdAAzACAAcAAgAD0AYQBiAHMAKABmAHIAYQBjACgASABTAFYALgB4AHgAeAArAGsALgB4AHkAegApACoANgAuADAALQBrAC4AdwB3AHcAKQA7AAoAcgBlAHQAdQByAG4AIABIAFMAVgAuAHoAKgBsAGUAcgBwACgAawAuAHgAeAB4ACwAYwBsAGEAbQBwACgAcAAtAGsALgB4AHgAeAAsADAALgAwACwAMQAuADAAKQAsAEgAUwBWAC4AeQApADsA,output:2,fname:HSVtoRGB,width:699,height:127,input:2,input_1_label:HSV|A-1982-OUT;n:type:ShaderForge.SFN_Add,id:1978,x:33132,y:31295,varname:node_1978,prsc:2|A-1980-X,B-1990-R;n:type:ShaderForge.SFN_Vector4Property,id:1980,x:32921,y:31377,ptovrint:False,ptlb:HSV,ptin:_HSV,varname:node_6006,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1,v2:1,v3:1,v4:1;n:type:ShaderForge.SFN_Append,id:1982,x:33449,y:31397,varname:node_1982,prsc:2|A-1984-OUT,B-1988-OUT;n:type:ShaderForge.SFN_Append,id:1984,x:33286,y:31397,varname:node_1984,prsc:2|A-1978-OUT,B-1986-OUT;n:type:ShaderForge.SFN_Multiply,id:1986,x:33132,y:31416,varname:node_1986,prsc:2|A-1980-Y,B-1990-G;n:type:ShaderForge.SFN_Multiply,id:1988,x:33132,y:31548,varname:node_1988,prsc:2|A-1980-Z,B-1990-B;n:type:ShaderForge.SFN_ComponentMask,id:1990,x:32921,y:31527,varname:node_1990,prsc:2,cc1:0,cc2:1,cc3:2,cc4:-1|IN-1974-OUT;n:type:ShaderForge.SFN_TexCoord,id:4059,x:32260,y:32667,varname:node_4059,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:1097,x:32449,y:32910,varname:node_1097,prsc:2,spu:0,spv:1|UVIN-4059-UVOUT,DIST-192-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:4482,x:32618,y:32811,ptovrint:False,ptlb:E01_U/Vpan,ptin:_E01_UVpan,varname:node_4482,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-159-UVOUT,B-1097-UVOUT;n:type:ShaderForge.SFN_Rotator,id:4265,x:32827,y:32811,varname:node_4265,prsc:2|UVIN-4482-OUT,ANG-8026-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3770,x:32585,y:33043,ptovrint:False,ptlb:E01_UVangle,ptin:_E01_UVangle,varname:node_3770,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:8026,x:32767,y:33037,varname:node_8026,prsc:2|A-3770-OUT,B-5709-OUT;n:type:ShaderForge.SFN_Pi,id:5709,x:32585,y:33130,varname:node_5709,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1099,x:33940,y:32486,varname:node_1099,prsc:2|A-130-OUT,B-4143-RGB;n:type:ShaderForge.SFN_VertexColor,id:4143,x:33653,y:32574,varname:node_4143,prsc:2;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:2413,x:33376,y:33189,varname:node_2413,prsc:2|IN-94-OUT,IMIN-6394-OUT,IMAX-1379-OUT,OMIN-4351-OUT,OMAX-2628-OUT;n:type:ShaderForge.SFN_Clamp01,id:3805,x:33533,y:33189,varname:node_3805,prsc:2|IN-2413-OUT;n:type:ShaderForge.SFN_Vector1,id:6394,x:33087,y:33319,varname:node_6394,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:1379,x:33087,y:33367,varname:node_1379,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:4351,x:33045,y:33468,ptovrint:False,ptlb:RimPower_min,ptin:_RimPower_min,varname:node_4351,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:-5;n:type:ShaderForge.SFN_Vector1,id:3320,x:33016,y:33220,varname:node_3320,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:2628,x:33045,y:33560,ptovrint:False,ptlb:RimPower_max,ptin:_RimPower_max,varname:node_2628,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:5;n:type:ShaderForge.SFN_Multiply,id:3536,x:31899,y:31624,varname:node_3536,prsc:2|A-2-RGB,B-9645-RGB;n:type:ShaderForge.SFN_Color,id:9645,x:31655,y:31826,ptovrint:False,ptlb:D01_Color,ptin:_D01_Color,varname:node_9645,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;proporder:132-2-9645-1980-109-111-114-117-148-151-4482-3770-233-147-100-4351-2628;pass:END;sub:END;*/

Shader "yh/Diffuse/2DiffuseLerp_EmissUVpan_Rim" {
    Properties {
        _Tex_power ("Tex_power", Float ) = 1
        _D01 ("D01", 2D) = "black" {}
        _D01_Color ("D01_Color", Color) = (1,1,1,1)
        _HSV ("HSV", Vector) = (1,1,1,1)
        _D02 ("D02", 2D) = "black" {}
        _D02_Color ("D02_Color", Color) = (1,1,1,1)
        _MaskD02 ("MaskD02", 2D) = "white" {}
        _MaskD02_Bright ("MaskD02_Bright", Float ) = 0
        _E01 ("E01", 2D) = "black" {}
        _E01_Color ("E01_Color", Color) = (1,1,1,1)
        [MaterialToggle] _E01_UVpan ("E01_U/Vpan", Float ) = 0
        _E01_UVangle ("E01_UVangle", Float ) = 0
        _E01_UVpan_Speed ("E01_UVpan_Speed", Float ) = 1
        _MaskE01 ("MaskE01", 2D) = "white" {}
        _RimColor ("RimColor", Color) = (1,1,1,1)
        _RimPower_min ("RimPower_min", Range(0, -5)) = 0
        _RimPower_max ("RimPower_max", Range(1, 5)) = 1
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
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _D01; uniform float4 _D01_ST;
            uniform float4 _RimColor;
            uniform sampler2D _D02; uniform float4 _D02_ST;
            uniform float4 _D02_Color;
            uniform sampler2D _MaskD02; uniform float4 _MaskD02_ST;
            uniform float _MaskD02_Bright;
            uniform float _Tex_power;
            uniform sampler2D _MaskE01; uniform float4 _MaskE01_ST;
            uniform sampler2D _E01; uniform float4 _E01_ST;
            uniform float4 _E01_Color;
            uniform float _E01_UVpan_Speed;
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
            uniform fixed _E01_UVpan;
            uniform float _E01_UVangle;
            uniform float _RimPower_min;
            uniform float _RimPower_max;
            uniform float4 _D01_Color;
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
                float4 _D01_var = tex2D(_D01,TRANSFORM_TEX(i.uv0, _D01));
                float3 node_1990 = RGBtoHSV( (_D01_var.rgb*_D01_Color.rgb) ).rgb;
                float4 _D02_var = tex2D(_D02,TRANSFORM_TEX(i.uv0, _D02));
                float4 _MaskD02_var = tex2D(_MaskD02,TRANSFORM_TEX(i.uv0, _MaskD02));
                float3 diffuseColor = (pow(lerp(HSVtoRGB( float3(float2((_HSV.r+node_1990.r),(_HSV.g*node_1990.g)),(_HSV.b*node_1990.b)) ),(_D02_Color.rgb*_D02_var.rgb),(_MaskD02_var.r*_MaskD02_Bright)),_Tex_power)*i.vertexColor.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 _MaskE01_var = tex2D(_MaskE01,TRANSFORM_TEX(i.uv0, _MaskE01));
                float node_4265_ang = (_E01_UVangle*3.141592654);
                float node_4265_spd = 1.0;
                float node_4265_cos = cos(node_4265_spd*node_4265_ang);
                float node_4265_sin = sin(node_4265_spd*node_4265_ang);
                float2 node_4265_piv = float2(0.5,0.5);
                float4 node_334 = _Time;
                float node_192 = (node_334.g*_E01_UVpan_Speed);
                float2 node_4265 = (mul(lerp( (i.uv0+node_192*float2(1,0)), (i.uv0+node_192*float2(0,1)), _E01_UVpan )-node_4265_piv,float2x2( node_4265_cos, -node_4265_sin, node_4265_sin, node_4265_cos))+node_4265_piv);
                float4 _E01_var = tex2D(_E01,TRANSFORM_TEX(node_4265, _E01));
                float node_6394 = 0.0;
                float3 emissive = (((_MaskE01_var.rgb*_E01_var.rgb)*_E01_Color.rgb)+(_RimColor.rgb*saturate((_RimPower_min + ( (pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0) - node_6394) * (_RimPower_max - _RimPower_min) ) / (1.0 - node_6394)))));
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,i.vertexColor.a);
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
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _D01; uniform float4 _D01_ST;
            uniform float4 _RimColor;
            uniform sampler2D _D02; uniform float4 _D02_ST;
            uniform float4 _D02_Color;
            uniform sampler2D _MaskD02; uniform float4 _MaskD02_ST;
            uniform float _MaskD02_Bright;
            uniform float _Tex_power;
            uniform sampler2D _MaskE01; uniform float4 _MaskE01_ST;
            uniform sampler2D _E01; uniform float4 _E01_ST;
            uniform float4 _E01_Color;
            uniform float _E01_UVpan_Speed;
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
            uniform fixed _E01_UVpan;
            uniform float _E01_UVangle;
            uniform float _RimPower_min;
            uniform float _RimPower_max;
            uniform float4 _D01_Color;
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
                float4 _D01_var = tex2D(_D01,TRANSFORM_TEX(i.uv0, _D01));
                float3 node_1990 = RGBtoHSV( (_D01_var.rgb*_D01_Color.rgb) ).rgb;
                float4 _D02_var = tex2D(_D02,TRANSFORM_TEX(i.uv0, _D02));
                float4 _MaskD02_var = tex2D(_MaskD02,TRANSFORM_TEX(i.uv0, _MaskD02));
                float3 diffuseColor = (pow(lerp(HSVtoRGB( float3(float2((_HSV.r+node_1990.r),(_HSV.g*node_1990.g)),(_HSV.b*node_1990.b)) ),(_D02_Color.rgb*_D02_var.rgb),(_MaskD02_var.r*_MaskD02_Bright)),_Tex_power)*i.vertexColor.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * i.vertexColor.a,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
