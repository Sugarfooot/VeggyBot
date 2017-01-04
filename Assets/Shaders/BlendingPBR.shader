// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:33263,y:33034,varname:node_2865,prsc:2|diff-4024-OUT,spec-358-OUT,gloss-1813-OUT,normal-906-OUT;n:type:ShaderForge.SFN_Multiply,id:6343,x:32126,y:32087,varname:node_6343,prsc:2|A-5482-RGB,B-6665-RGB;n:type:ShaderForge.SFN_Color,id:6665,x:31823,y:32225,ptovrint:False,ptlb:Black Color,ptin:_BlackColor,varname:_BlackColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5019608,c2:0.5019608,c3:0.5019608,c4:1;n:type:ShaderForge.SFN_Tex2d,id:5964,x:32161,y:33447,ptovrint:True,ptlb:Normal Map Base,ptin:_BumpMap,varname:_BumpMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:04e050e17aa96aa45a687c724383c153,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:358,x:31998,y:33203,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:_Metallic,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:32015,y:33326,ptovrint:False,ptlb:Roughness,ptin:_Roughness,varname:_Roughness,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8,max:1;n:type:ShaderForge.SFN_Lerp,id:6462,x:32412,y:32218,varname:node_6462,prsc:2|A-6343-OUT,B-899-OUT,T-6693-OUT;n:type:ShaderForge.SFN_VertexColor,id:2422,x:32386,y:32068,varname:node_2422,prsc:2;n:type:ShaderForge.SFN_Lerp,id:4024,x:32402,y:32472,varname:node_4024,prsc:2|A-6462-OUT,B-6541-OUT,T-6749-OUT;n:type:ShaderForge.SFN_Multiply,id:899,x:32126,y:32393,varname:node_899,prsc:2|A-2078-RGB,B-2689-RGB;n:type:ShaderForge.SFN_Color,id:2689,x:31826,y:32603,ptovrint:False,ptlb:Red Color,ptin:_RedColor,varname:_RedColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5019608,c2:0.5019608,c3:0.5019608,c4:1;n:type:ShaderForge.SFN_Multiply,id:6541,x:32076,y:32717,varname:node_6541,prsc:2|A-6377-RGB,B-8921-RGB;n:type:ShaderForge.SFN_Color,id:8921,x:31809,y:32991,ptovrint:False,ptlb:Green Color,ptin:_GreenColor,varname:_GreenColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5019608,c2:0.5019608,c3:0.5019608,c4:1;n:type:ShaderForge.SFN_NormalBlend,id:906,x:32891,y:33399,varname:node_906,prsc:2|BSE-5964-RGB,DTL-8759-OUT;n:type:ShaderForge.SFN_Lerp,id:24,x:32337,y:33749,varname:node_24,prsc:2|A-614-RGB,B-2713-RGB,T-6693-OUT;n:type:ShaderForge.SFN_VertexColor,id:2376,x:32420,y:33552,varname:node_2376,prsc:2;n:type:ShaderForge.SFN_Lerp,id:8759,x:32347,y:33983,varname:node_8759,prsc:2|A-24-OUT,B-9884-RGB,T-6749-OUT;n:type:ShaderForge.SFN_Tex2d,id:5482,x:31823,y:31979,ptovrint:False,ptlb:Black Base Color,ptin:_BlackBaseColor,varname:_BlackBaseColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0baf9ec8db561f14d89f30d8ba6ba0c6,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:2078,x:31836,y:32410,ptovrint:False,ptlb:Red Base Color,ptin:_RedBaseColor,varname:_RedBaseColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:88233d9326edb4041b97a489b139c63f,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6377,x:31809,y:32784,ptovrint:False,ptlb:Green Base Color,ptin:_GreenBaseColor,varname:_GreenBaseColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:614,x:31731,y:33733,ptovrint:False,ptlb:Black Normal Map,ptin:_BlackNormalMap,varname:_BlackNormalMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2c8658a7c60139b4a9024d879624925f,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:2713,x:31743,y:33926,ptovrint:False,ptlb:Red Normal Map,ptin:_RedNormalMap,varname:_RedNormalMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c5e6c619477d5654ca110aba8f3bbab0,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:9884,x:31743,y:34131,ptovrint:False,ptlb:Green Normal Map,ptin:_GreenNormalMap,varname:_GreenNormalMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Step,id:6749,x:32126,y:32522,varname:node_6749,prsc:2|A-2422-G,B-2078-A;n:type:ShaderForge.SFN_Step,id:6693,x:32126,y:32238,varname:node_6693,prsc:2|A-2422-R,B-5482-A;proporder:5964-6665-358-1813-2689-8921-5482-2078-6377-9884-2713-614;pass:END;sub:END;*/

Shader "" {
    Properties {
        _BumpMap ("Normal Map Base", 2D) = "bump" {}
        _BlackColor ("Black Color", Color) = (0.5019608,0.5019608,0.5019608,1)
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Roughness ("Roughness", Range(0, 1)) = 0.8
        _RedColor ("Red Color", Color) = (0.5019608,0.5019608,0.5019608,1)
        _GreenColor ("Green Color", Color) = (0.5019608,0.5019608,0.5019608,1)
        _BlackBaseColor ("Black Base Color", 2D) = "white" {}
        _RedBaseColor ("Red Base Color", 2D) = "white" {}
        _GreenBaseColor ("Green Base Color", 2D) = "white" {}
        _GreenNormalMap ("Green Normal Map", 2D) = "white" {}
        _RedNormalMap ("Red Normal Map", 2D) = "bump" {}
        _BlackNormalMap ("Black Normal Map", 2D) = "bump" {}
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
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _BlackColor;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _Metallic;
            uniform float _Roughness;
            uniform float4 _RedColor;
            uniform float4 _GreenColor;
            uniform sampler2D _BlackBaseColor; uniform float4 _BlackBaseColor_ST;
            uniform sampler2D _RedBaseColor; uniform float4 _RedBaseColor_ST;
            uniform sampler2D _GreenBaseColor; uniform float4 _GreenBaseColor_ST;
            uniform sampler2D _BlackNormalMap; uniform float4 _BlackNormalMap_ST;
            uniform sampler2D _RedNormalMap; uniform float4 _RedNormalMap_ST;
            uniform sampler2D _GreenNormalMap; uniform float4 _GreenNormalMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 _BlackNormalMap_var = UnpackNormal(tex2D(_BlackNormalMap,TRANSFORM_TEX(i.uv0, _BlackNormalMap)));
                float3 _RedNormalMap_var = UnpackNormal(tex2D(_RedNormalMap,TRANSFORM_TEX(i.uv0, _RedNormalMap)));
                float4 _BlackBaseColor_var = tex2D(_BlackBaseColor,TRANSFORM_TEX(i.uv0, _BlackBaseColor));
                float node_6693 = step(i.vertexColor.r,_BlackBaseColor_var.a);
                float4 _GreenNormalMap_var = tex2D(_GreenNormalMap,TRANSFORM_TEX(i.uv0, _GreenNormalMap));
                float4 _RedBaseColor_var = tex2D(_RedBaseColor,TRANSFORM_TEX(i.uv0, _RedBaseColor));
                float node_6749 = step(i.vertexColor.g,_RedBaseColor_var.a);
                float3 node_906_nrm_base = _BumpMap_var.rgb + float3(0,0,1);
                float3 node_906_nrm_detail = lerp(lerp(_BlackNormalMap_var.rgb,_RedNormalMap_var.rgb,node_6693),_GreenNormalMap_var.rgb,node_6749) * float3(-1,-1,1);
                float3 node_906_nrm_combined = node_906_nrm_base*dot(node_906_nrm_base, node_906_nrm_detail)/node_906_nrm_base.z - node_906_nrm_detail;
                float3 node_906 = node_906_nrm_combined;
                float3 normalLocal = node_906;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = 1.0 - _Roughness; // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                d.boxMax[0] = unity_SpecCube0_BoxMax;
                d.boxMin[0] = unity_SpecCube0_BoxMin;
                d.probePosition[0] = unity_SpecCube0_ProbePosition;
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.boxMax[1] = unity_SpecCube1_BoxMax;
                d.boxMin[1] = unity_SpecCube1_BoxMin;
                d.probePosition[1] = unity_SpecCube1_ProbePosition;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float4 _GreenBaseColor_var = tex2D(_GreenBaseColor,TRANSFORM_TEX(i.uv0, _GreenBaseColor));
                float3 diffuseColor = lerp(lerp((_BlackBaseColor_var.rgb*_BlackColor.rgb),(_RedBaseColor_var.rgb*_RedColor.rgb),node_6693),(_GreenBaseColor_var.rgb*_GreenColor.rgb),node_6749); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, GGXTerm(NdotH, 1.0-gloss));
                float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
                if (IsGammaSpace())
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                specularPBL = max(0, specularPBL * NdotL);
                float3 directSpecular = 1*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
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
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _BlackColor;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _Metallic;
            uniform float _Roughness;
            uniform float4 _RedColor;
            uniform float4 _GreenColor;
            uniform sampler2D _BlackBaseColor; uniform float4 _BlackBaseColor_ST;
            uniform sampler2D _RedBaseColor; uniform float4 _RedBaseColor_ST;
            uniform sampler2D _GreenBaseColor; uniform float4 _GreenBaseColor_ST;
            uniform sampler2D _BlackNormalMap; uniform float4 _BlackNormalMap_ST;
            uniform sampler2D _RedNormalMap; uniform float4 _RedNormalMap_ST;
            uniform sampler2D _GreenNormalMap; uniform float4 _GreenNormalMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 _BlackNormalMap_var = UnpackNormal(tex2D(_BlackNormalMap,TRANSFORM_TEX(i.uv0, _BlackNormalMap)));
                float3 _RedNormalMap_var = UnpackNormal(tex2D(_RedNormalMap,TRANSFORM_TEX(i.uv0, _RedNormalMap)));
                float4 _BlackBaseColor_var = tex2D(_BlackBaseColor,TRANSFORM_TEX(i.uv0, _BlackBaseColor));
                float node_6693 = step(i.vertexColor.r,_BlackBaseColor_var.a);
                float4 _GreenNormalMap_var = tex2D(_GreenNormalMap,TRANSFORM_TEX(i.uv0, _GreenNormalMap));
                float4 _RedBaseColor_var = tex2D(_RedBaseColor,TRANSFORM_TEX(i.uv0, _RedBaseColor));
                float node_6749 = step(i.vertexColor.g,_RedBaseColor_var.a);
                float3 node_906_nrm_base = _BumpMap_var.rgb + float3(0,0,1);
                float3 node_906_nrm_detail = lerp(lerp(_BlackNormalMap_var.rgb,_RedNormalMap_var.rgb,node_6693),_GreenNormalMap_var.rgb,node_6749) * float3(-1,-1,1);
                float3 node_906_nrm_combined = node_906_nrm_base*dot(node_906_nrm_base, node_906_nrm_detail)/node_906_nrm_base.z - node_906_nrm_detail;
                float3 node_906 = node_906_nrm_combined;
                float3 normalLocal = node_906;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = 1.0 - _Roughness; // Convert roughness to gloss
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float4 _GreenBaseColor_var = tex2D(_GreenBaseColor,TRANSFORM_TEX(i.uv0, _GreenBaseColor));
                float3 diffuseColor = lerp(lerp((_BlackBaseColor_var.rgb*_BlackColor.rgb),(_RedBaseColor_var.rgb*_RedColor.rgb),node_6693),(_GreenBaseColor_var.rgb*_GreenColor.rgb),node_6749); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, GGXTerm(NdotH, 1.0-gloss));
                float specularPBL = (NdotL*visTerm*normTerm) * (UNITY_PI / 4);
                if (IsGammaSpace())
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                specularPBL = max(0, specularPBL * NdotL);
                float3 directSpecular = attenColor*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _BlackColor;
            uniform float _Metallic;
            uniform float _Roughness;
            uniform float4 _RedColor;
            uniform float4 _GreenColor;
            uniform sampler2D _BlackBaseColor; uniform float4 _BlackBaseColor_ST;
            uniform sampler2D _RedBaseColor; uniform float4 _RedBaseColor_ST;
            uniform sampler2D _GreenBaseColor; uniform float4 _GreenBaseColor_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float4 _BlackBaseColor_var = tex2D(_BlackBaseColor,TRANSFORM_TEX(i.uv0, _BlackBaseColor));
                float4 _RedBaseColor_var = tex2D(_RedBaseColor,TRANSFORM_TEX(i.uv0, _RedBaseColor));
                float node_6693 = step(i.vertexColor.r,_BlackBaseColor_var.a);
                float4 _GreenBaseColor_var = tex2D(_GreenBaseColor,TRANSFORM_TEX(i.uv0, _GreenBaseColor));
                float node_6749 = step(i.vertexColor.g,_RedBaseColor_var.a);
                float3 diffColor = lerp(lerp((_BlackBaseColor_var.rgb*_BlackColor.rgb),(_RedBaseColor_var.rgb*_RedColor.rgb),node_6693),(_GreenBaseColor_var.rgb*_GreenColor.rgb),node_6749);
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metallic, specColor, specularMonochrome );
                float roughness = _Roughness;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
