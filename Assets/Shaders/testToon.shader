// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|emission-544-OUT,custl-821-OUT;n:type:ShaderForge.SFN_Tex2d,id:851,x:32187,y:32266,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:455d2c68146951741afae15843bdbba9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1941,x:32736,y:32947,cmnt:Diffuse Contribution,varname:node_1941,prsc:2|A-544-OUT,B-2184-RGB;n:type:ShaderForge.SFN_Color,id:5927,x:32132,y:32510,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5588235,c2:0.5588235,c3:0.5588235,c4:1;n:type:ShaderForge.SFN_Multiply,id:544,x:32434,y:32411,cmnt:Diffuse Color,varname:node_544,prsc:2|A-851-RGB,B-5927-RGB;n:type:ShaderForge.SFN_Tex2d,id:2184,x:32538,y:33013,ptovrint:False,ptlb:ramp,ptin:_ramp,varname:_ramp,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9c2f65ff92a734b4e90d4c0e445271e3,ntxv:0,isnm:False|UVIN-5659-OUT;n:type:ShaderForge.SFN_LightVector,id:1951,x:31726,y:32880,varname:node_1951,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:1796,x:32527,y:33225,varname:node_1796,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6107,x:32755,y:33296,varname:node_6107,prsc:2|A-1796-OUT,B-2142-RGB;n:type:ShaderForge.SFN_Dot,id:7403,x:32122,y:32808,varname:node_7403,prsc:2,dt:0|A-1951-OUT,B-8875-OUT;n:type:ShaderForge.SFN_NormalVector,id:6974,x:31674,y:32671,prsc:2,pt:False;n:type:ShaderForge.SFN_Append,id:5659,x:32358,y:32872,varname:node_5659,prsc:2|A-7403-OUT,B-7403-OUT;n:type:ShaderForge.SFN_LightColor,id:2142,x:32509,y:33378,varname:node_2142,prsc:2;n:type:ShaderForge.SFN_Multiply,id:821,x:32948,y:33078,varname:node_821,prsc:2|A-1941-OUT,B-6107-OUT;n:type:ShaderForge.SFN_Tex2d,id:1110,x:31350,y:32371,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:_NormalMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:7f7a36ef47cbcbe45bb3a74cfd7c7801,ntxv:2,isnm:False;n:type:ShaderForge.SFN_NormalBlend,id:8875,x:31901,y:32534,varname:node_8875,prsc:2|BSE-6974-OUT,DTL-1110-RGB;n:type:ShaderForge.SFN_OneMinus,id:519,x:31657,y:32344,varname:node_519,prsc:2|IN-1110-G;proporder:851-5927-2184-1110;pass:END;sub:END;*/

Shader "Shader Forge/testToon" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Color ("Color", Color) = (0.5588235,0.5588235,0.5588235,1)
        _ramp ("ramp", 2D) = "white" {}
        _NormalMap ("NormalMap", 2D) = "black" {}
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
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _Color;
            uniform sampler2D _ramp; uniform float4 _ramp_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
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
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_544 = (_Diffuse_var.rgb*_Color.rgb); // Diffuse Color
                float3 emissive = node_544;
                float4 _NormalMap_var = tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap));
                float3 node_8875_nrm_base = i.normalDir + float3(0,0,1);
                float3 node_8875_nrm_detail = _NormalMap_var.rgb * float3(-1,-1,1);
                float3 node_8875_nrm_combined = node_8875_nrm_base*dot(node_8875_nrm_base, node_8875_nrm_detail)/node_8875_nrm_base.z - node_8875_nrm_detail;
                float3 node_8875 = node_8875_nrm_combined;
                float node_7403 = dot(lightDirection,node_8875);
                float2 node_5659 = float2(node_7403,node_7403);
                float4 _ramp_var = tex2D(_ramp,TRANSFORM_TEX(node_5659, _ramp));
                float3 node_1941 = (node_544*_ramp_var.rgb); // Diffuse Contribution
                float3 finalColor = emissive + (node_1941*(attenuation*_LightColor0.rgb));
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
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _Color;
            uniform sampler2D _ramp; uniform float4 _ramp_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
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
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_544 = (_Diffuse_var.rgb*_Color.rgb); // Diffuse Color
                float4 _NormalMap_var = tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap));
                float3 node_8875_nrm_base = i.normalDir + float3(0,0,1);
                float3 node_8875_nrm_detail = _NormalMap_var.rgb * float3(-1,-1,1);
                float3 node_8875_nrm_combined = node_8875_nrm_base*dot(node_8875_nrm_base, node_8875_nrm_detail)/node_8875_nrm_base.z - node_8875_nrm_detail;
                float3 node_8875 = node_8875_nrm_combined;
                float node_7403 = dot(lightDirection,node_8875);
                float2 node_5659 = float2(node_7403,node_7403);
                float4 _ramp_var = tex2D(_ramp,TRANSFORM_TEX(node_5659, _ramp));
                float3 node_1941 = (node_544*_ramp_var.rgb); // Diffuse Contribution
                float3 finalColor = (node_1941*(attenuation*_LightColor0.rgb));
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
