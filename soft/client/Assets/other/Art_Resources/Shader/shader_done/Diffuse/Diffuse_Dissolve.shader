// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:False,qofs:0,qpre:1,rntp:1,fgom:True,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.2784314,fgcg:0.2784314,fgcb:0.2784314,fgca:1,fgde:0.01,fgrn:30,fgrf:150,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:796,x:32828,y:32694,varname:node_796,prsc:2|diff-1377-OUT,alpha-9096-A,clip-8340-OUT;n:type:ShaderForge.SFN_Tex2d,id:7438,x:30655,y:32920,ptovrint:False,ptlb:Alpha02,ptin:_Alpha02,varname:_Alpha02,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_If,id:5985,x:31888,y:32747,varname:node_5985,prsc:2|A-2590-OUT,B-238-OUT,GT-242-OUT,EQ-242-OUT,LT-7596-OUT;n:type:ShaderForge.SFN_Vector1,id:242,x:31336,y:33057,varname:node_242,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:7596,x:31336,y:33120,varname:node_7596,prsc:2,v1:1;n:type:ShaderForge.SFN_Tex2d,id:9096,x:31044,y:32266,ptovrint:False,ptlb:D01,ptin:_D01,varname:_E01,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_If,id:7270,x:31888,y:32914,varname:node_7270,prsc:2|A-2590-OUT,B-2525-OUT,GT-242-OUT,EQ-242-OUT,LT-7596-OUT;n:type:ShaderForge.SFN_Vector1,id:2525,x:31341,y:32938,varname:node_2525,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Tex2d,id:2307,x:30655,y:32743,ptovrint:False,ptlb:Alpha01,ptin:_Alpha01,varname:_Alpha01,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Subtract,id:6335,x:32094,y:32832,varname:node_6335,prsc:2|A-5985-OUT,B-7270-OUT;n:type:ShaderForge.SFN_VertexColor,id:2070,x:30526,y:32286,varname:node_2070,prsc:2;n:type:ShaderForge.SFN_Vector1,id:2866,x:31341,y:32763,varname:node_2866,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:2590,x:31224,y:32640,varname:node_2590,prsc:2|A-5994-OUT,B-1182-OUT;n:type:ShaderForge.SFN_Multiply,id:6964,x:32315,y:32812,varname:node_6964,prsc:2|A-9773-OUT,B-6335-OUT;n:type:ShaderForge.SFN_Add,id:1377,x:32591,y:32794,varname:node_1377,prsc:2|A-1940-OUT,B-6964-OUT;n:type:ShaderForge.SFN_Color,id:4345,x:32011,y:32448,ptovrint:False,ptlb:Dissovle_Color,ptin:_Dissovle_Color,varname:_Dissovle_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Vector1,id:627,x:30655,y:33105,varname:node_627,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Add,id:1182,x:31082,y:32840,varname:node_1182,prsc:2|A-4115-OUT,B-3170-OUT;n:type:ShaderForge.SFN_Add,id:3170,x:30833,y:33153,varname:node_3170,prsc:2|A-627-OUT,B-6305-OUT;n:type:ShaderForge.SFN_Add,id:238,x:31514,y:32763,varname:node_238,prsc:2|A-2866-OUT,B-6305-OUT;n:type:ShaderForge.SFN_Slider,id:6305,x:30503,y:33383,ptovrint:False,ptlb:Dissovle_Width,ptin:_Dissovle_Width,varname:_Dissovle_Width,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.002,cur:0.002,max:0.08;n:type:ShaderForge.SFN_Add,id:4115,x:30895,y:32842,varname:node_4115,prsc:2|A-2307-R,B-7438-R;n:type:ShaderForge.SFN_OneMinus,id:5994,x:31046,y:32640,varname:node_5994,prsc:2|IN-2196-OUT;n:type:ShaderForge.SFN_Add,id:9773,x:32166,y:32536,varname:node_9773,prsc:2|A-4345-RGB,B-5866-OUT;n:type:ShaderForge.SFN_Multiply,id:4618,x:32342,y:32110,varname:node_4618,prsc:2|A-2222-OUT,B-9096-RGB,C-2070-RGB;n:type:ShaderForge.SFN_Multiply,id:8340,x:32560,y:32958,varname:node_8340,prsc:2|A-9096-A,B-5985-OUT;n:type:ShaderForge.SFN_Code,id:6105,x:30645,y:31968,varname:node_6105,prsc:2,code:ZgBsAG8AYQB0ADQAIABrACAAPQBmAGwAbwBhAHQANAAoADAALgAwACwALQAxAC4AMAAvADMALgAwACwAMgAuADAALwAzAC4AMAAsAC0AMQAuADAAKQA7AAoAZgBsAG8AYQB0ADQAIABwACAAPQBSAEcAQgAuAGcAPABSAEcAQgAuAGIAPwBmAGwAbwBhAHQANAAoAFIARwBCAC4AYgAsAFIARwBCAC4AZwAsAGsALgB3ACwAawAuAHoAKQA6AGYAbABvAGEAdAA0ACgAUgBHAEIALgBnAGIALABrAC4AeAB5ACkAOwAKAGYAbABvAGEAdAA0ACAAcQAgAD0AUgBHAEIALgByADwAcAAuAHgAIAAgAD8AZgBsAG8AYQB0ADQAKABwAC4AeAAsAHAALgB5ACwAcAAuAHcALABSAEcAQgAuAHIAKQA6AGYAbABvAGEAdAA0ACgAUgBHAEIALgByACwAcAAuAHkAegB4ACkAOwAKAGYAbABvAGEAdAAgAGQAIAA9AHEALgB4AC0AbQBpAG4AKABxAC4AdwAsAHEALgB5ACkAOwAKAGYAbABvAGEAdAAgAGUAPQAxAC4AMABlAC0AMQAwADsACgByAGUAdAB1AHIAbgAgAGYAbABvAGEAdAAzACgAYQBiAHMAKABxAC4AegArACgAcQAuAHcALQBxAC4AeQApAC8AKAA2AC4AMAAqAGQAKwBlACkAKQAsAGQALwAoAHEALgB4ACsAZQApACwAcQAuAHgAKQA7AA==,output:2,fname:RGBtoHSV,width:716,height:154,input:2,input_1_label:RGB|A-9096-RGB;n:type:ShaderForge.SFN_Code,id:2222,x:32130,y:31838,varname:node_2222,prsc:2,code:ZgBsAG8AYQB0ADQAIABrACAAPQAgAGYAbABvAGEAdAA0ACgAMQAuADAALAAyAC4AMAAvADMALgAwACwAMQAuADAALwAzAC4AMAAsADMALgAwACkAOwAKAGYAbABvAGEAdAAzACAAcAAgAD0AYQBiAHMAKABmAHIAYQBjACgASABTAFYALgB4AHgAeAArAGsALgB4AHkAegApACoANgAuADAALQBrAC4AdwB3AHcAKQA7AAoAcgBlAHQAdQByAG4AIABIAFMAVgAuAHoAKgBsAGUAcgBwACgAawAuAHgAeAB4ACwAYwBsAGEAbQBwACgAcAAtAGsALgB4AHgAeAAsADAALgAwACwAMQAuADAAKQAsAEgAUwBWAC4AeQApADsA,output:2,fname:HSVtoRGB,width:699,height:127,input:2,input_1_label:HSV|A-3732-OUT;n:type:ShaderForge.SFN_Add,id:9458,x:31640,y:31735,varname:node_9458,prsc:2|A-7772-X,B-1076-R;n:type:ShaderForge.SFN_Vector4Property,id:7772,x:31429,y:31817,ptovrint:False,ptlb:HSV,ptin:_HSV,varname:node_1277,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1,v2:1,v3:1,v4:1;n:type:ShaderForge.SFN_Append,id:3732,x:31957,y:31837,varname:node_3732,prsc:2|A-8016-OUT,B-2462-OUT;n:type:ShaderForge.SFN_Append,id:8016,x:31794,y:31837,varname:node_8016,prsc:2|A-9458-OUT,B-7580-OUT;n:type:ShaderForge.SFN_Multiply,id:7580,x:31640,y:31856,varname:node_7580,prsc:2|A-7772-Y,B-1076-G;n:type:ShaderForge.SFN_Multiply,id:2462,x:31640,y:31988,varname:node_2462,prsc:2|A-7772-Z,B-1076-B;n:type:ShaderForge.SFN_ComponentMask,id:1076,x:31429,y:31967,varname:node_1076,prsc:2,cc1:0,cc2:1,cc3:2,cc4:-1|IN-6105-OUT;n:type:ShaderForge.SFN_Slider,id:5866,x:31827,y:32619,ptovrint:False,ptlb:Dissovle_Bright,ptin:_Dissovle_Bright,varname:node_5866,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:0;n:type:ShaderForge.SFN_Slider,id:7604,x:30298,y:32544,ptovrint:False,ptlb:Dissovle_Time,ptin:_Dissovle_Time,varname:node_7604,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_SwitchProperty,id:2196,x:30702,y:32492,ptovrint:False,ptlb:Anim/VertexAlpha,ptin:_AnimVertexAlpha,varname:node_2196,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-7604-OUT,B-2070-A;n:type:ShaderForge.SFN_Power,id:1940,x:32544,y:32345,varname:node_1940,prsc:2|VAL-4618-OUT,EXP-879-OUT;n:type:ShaderForge.SFN_ValueProperty,id:879,x:32312,y:32407,ptovrint:False,ptlb:Tex_Power,ptin:_Tex_Power,varname:node_879,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:879-9096-7772-2307-7438-4345-6305-5866-2196-7604;pass:END;sub:END;*/

Shader "yh/Diffuse/Diffuse_Dissolve" {
    Properties {
        _Tex_Power ("Tex_Power", Float ) = 1
        _D01 ("D01", 2D) = "white" {}
        _HSV ("HSV", Vector) = (1,1,1,1)
        _Alpha01 ("Alpha01", 2D) = "black" {}
        _Alpha02 ("Alpha02", 2D) = "black" {}
        _Dissovle_Color ("Dissovle_Color", Color) = (1,1,1,1)
        _Dissovle_Width ("Dissovle_Width", Range(0.002, 0.08)) = 0.002
        _Dissovle_Bright ("Dissovle_Bright", Range(-1, 0)) = 0
        [MaterialToggle] _AnimVertexAlpha ("Anim/VertexAlpha", Float ) = 1
        _Dissovle_Time ("Dissovle_Time", Range(0, 1)) = 1
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
            uniform sampler2D _Alpha02; uniform float4 _Alpha02_ST;
            uniform sampler2D _D01; uniform float4 _D01_ST;
            uniform sampler2D _Alpha01; uniform float4 _Alpha01_ST;
            uniform float4 _Dissovle_Color;
            uniform float _Dissovle_Width;
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
            uniform float _Dissovle_Bright;
            uniform float _Dissovle_Time;
            uniform fixed _AnimVertexAlpha;
            uniform float _Tex_Power;
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
                float3 normalDirection = i.normalDir;
                float4 _D01_var = tex2D(_D01,TRANSFORM_TEX(i.uv0, _D01));
                float4 _Alpha01_var = tex2D(_Alpha01,TRANSFORM_TEX(i.uv0, _Alpha01));
                float4 _Alpha02_var = tex2D(_Alpha02,TRANSFORM_TEX(i.uv0, _Alpha02));
                float node_2590 = ((1.0 - lerp( _Dissovle_Time, i.vertexColor.a, _AnimVertexAlpha ))*((_Alpha01_var.r+_Alpha02_var.r)+(0.1+_Dissovle_Width)));
                float node_5985_if_leA = step(node_2590,(0.1+_Dissovle_Width));
                float node_5985_if_leB = step((0.1+_Dissovle_Width),node_2590);
                float node_7596 = 1.0;
                float node_242 = 0.0;
                float node_5985 = lerp((node_5985_if_leA*node_7596)+(node_5985_if_leB*node_242),node_242,node_5985_if_leA*node_5985_if_leB);
                clip((_D01_var.a*node_5985) - 0.5);
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
                float3 node_1076 = RGBtoHSV( _D01_var.rgb ).rgb;
                float node_7270_if_leA = step(node_2590,0.1);
                float node_7270_if_leB = step(0.1,node_2590);
                float3 diffuseColor = (pow((HSVtoRGB( float3(float2((_HSV.r+node_1076.r),(_HSV.g*node_1076.g)),(_HSV.b*node_1076.b)) )*_D01_var.rgb*i.vertexColor.rgb),_Tex_Power)+((_Dissovle_Color.rgb+_Dissovle_Bright)*(node_5985-lerp((node_7270_if_leA*node_7596)+(node_7270_if_leB*node_242),node_242,node_7270_if_leA*node_7270_if_leB))));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,_D01_var.a);
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
            uniform sampler2D _Alpha02; uniform float4 _Alpha02_ST;
            uniform sampler2D _D01; uniform float4 _D01_ST;
            uniform sampler2D _Alpha01; uniform float4 _Alpha01_ST;
            uniform float4 _Dissovle_Color;
            uniform float _Dissovle_Width;
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
            uniform float _Dissovle_Bright;
            uniform float _Dissovle_Time;
            uniform fixed _AnimVertexAlpha;
            uniform float _Tex_Power;
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
                float3 normalDirection = i.normalDir;
                float4 _D01_var = tex2D(_D01,TRANSFORM_TEX(i.uv0, _D01));
                float4 _Alpha01_var = tex2D(_Alpha01,TRANSFORM_TEX(i.uv0, _Alpha01));
                float4 _Alpha02_var = tex2D(_Alpha02,TRANSFORM_TEX(i.uv0, _Alpha02));
                float node_2590 = ((1.0 - lerp( _Dissovle_Time, i.vertexColor.a, _AnimVertexAlpha ))*((_Alpha01_var.r+_Alpha02_var.r)+(0.1+_Dissovle_Width)));
                float node_5985_if_leA = step(node_2590,(0.1+_Dissovle_Width));
                float node_5985_if_leB = step((0.1+_Dissovle_Width),node_2590);
                float node_7596 = 1.0;
                float node_242 = 0.0;
                float node_5985 = lerp((node_5985_if_leA*node_7596)+(node_5985_if_leB*node_242),node_242,node_5985_if_leA*node_5985_if_leB);
                clip((_D01_var.a*node_5985) - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 node_1076 = RGBtoHSV( _D01_var.rgb ).rgb;
                float node_7270_if_leA = step(node_2590,0.1);
                float node_7270_if_leB = step(0.1,node_2590);
                float3 diffuseColor = (pow((HSVtoRGB( float3(float2((_HSV.r+node_1076.r),(_HSV.g*node_1076.g)),(_HSV.b*node_1076.b)) )*_D01_var.rgb*i.vertexColor.rgb),_Tex_Power)+((_Dissovle_Color.rgb+_Dissovle_Bright)*(node_5985-lerp((node_7270_if_leA*node_7596)+(node_7270_if_leB*node_242),node_242,node_7270_if_leA*node_7270_if_leB))));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * _D01_var.a,0);
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
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _Alpha02; uniform float4 _Alpha02_ST;
            uniform sampler2D _D01; uniform float4 _D01_ST;
            uniform sampler2D _Alpha01; uniform float4 _Alpha01_ST;
            uniform float _Dissovle_Width;
            uniform float _Dissovle_Time;
            uniform fixed _AnimVertexAlpha;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _D01_var = tex2D(_D01,TRANSFORM_TEX(i.uv0, _D01));
                float4 _Alpha01_var = tex2D(_Alpha01,TRANSFORM_TEX(i.uv0, _Alpha01));
                float4 _Alpha02_var = tex2D(_Alpha02,TRANSFORM_TEX(i.uv0, _Alpha02));
                float node_2590 = ((1.0 - lerp( _Dissovle_Time, i.vertexColor.a, _AnimVertexAlpha ))*((_Alpha01_var.r+_Alpha02_var.r)+(0.1+_Dissovle_Width)));
                float node_5985_if_leA = step(node_2590,(0.1+_Dissovle_Width));
                float node_5985_if_leB = step((0.1+_Dissovle_Width),node_2590);
                float node_7596 = 1.0;
                float node_242 = 0.0;
                float node_5985 = lerp((node_5985_if_leA*node_7596)+(node_5985_if_leB*node_242),node_242,node_5985_if_leA*node_5985_if_leB);
                clip((_D01_var.a*node_5985) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
