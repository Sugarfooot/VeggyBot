// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:34747,y:32689,varname:node_2865,prsc:2|diff-5548-OUT,spec-9616-OUT,gloss-3677-OUT,normal-7344-OUT;n:type:ShaderForge.SFN_Tex2d,id:922,x:32516,y:31683,ptovrint:False,ptlb:base Albedo Height,ptin:_baseAlbedoHeight,varname:_baseAlbedoHeight,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0aad627bce521b948ba62b84ffaa9c6e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3638,x:32536,y:32757,ptovrint:False,ptlb:base Metallic Smoothness,ptin:_baseMetallicSmoothness,varname:_baseMetallicSmoothness,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cf06ddd608432ed40af438bf9f75bf76,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3405,x:33271,y:33077,ptovrint:False,ptlb:base Normal Map,ptin:_baseNormalMap,varname:_baseNormalMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:04c8605a22ab9f6448831a9e302b5f3d,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:4066,x:33110,y:33293,ptovrint:False,ptlb:detail Normal Map,ptin:_detailNormalMap,varname:_detailNormalMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:3d5760742e9578c438381802f7a5baea,ntxv:3,isnm:True;n:type:ShaderForge.SFN_NormalBlend,id:9712,x:33960,y:33193,varname:node_9712,prsc:2|BSE-3405-RGB,DTL-3513-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8600,x:33313,y:33390,varname:node_8600,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-4066-RGB;n:type:ShaderForge.SFN_Multiply,id:4334,x:33558,y:33390,varname:node_4334,prsc:2|A-8600-OUT,B-3848-OUT;n:type:ShaderForge.SFN_Slider,id:2049,x:32957,y:33610,ptovrint:False,ptlb:normal Strength,ptin:_normalStrength,varname:_normalStrength,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Append,id:3513,x:33755,y:33484,varname:node_3513,prsc:2|A-4334-OUT,B-8006-OUT;n:type:ShaderForge.SFN_Vector1,id:8006,x:33114,y:33722,varname:node_8006,prsc:2,v1:1;n:type:ShaderForge.SFN_Lerp,id:8767,x:33944,y:32007,varname:node_8767,prsc:2|A-1661-OUT,B-5470-OUT,T-4260-OUT;n:type:ShaderForge.SFN_Tex2d,id:9103,x:32344,y:32256,ptovrint:False,ptlb:detail Albedo,ptin:_detailAlbedo,varname:_detailAlbedo,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0baf9ec8db561f14d89f30d8ba6ba0c6,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:2140,x:33268,y:32377,ptovrint:False,ptlb:albedo Intensity,ptin:_albedoIntensity,varname:_albedoIntensity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.05208128,max:1;n:type:ShaderForge.SFN_Color,id:2470,x:32395,y:31945,ptovrint:False,ptlb:baseColor Tint,ptin:_baseColorTint,varname:_baseColorTint,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Color,id:6059,x:32860,y:32418,ptovrint:False,ptlb:detailColor Tint_copy,ptin:_detailColorTint_copy,varname:_detailColorTint_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:1661,x:32927,y:31799,varname:node_1661,prsc:2|A-922-RGB,B-2470-RGB;n:type:ShaderForge.SFN_Multiply,id:5470,x:32956,y:32244,varname:node_5470,prsc:2|A-9103-RGB,B-6059-RGB;n:type:ShaderForge.SFN_VertexColor,id:5427,x:32626,y:33048,varname:node_5427,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4260,x:33689,y:32370,varname:node_4260,prsc:2|A-2140-OUT,B-5427-R;n:type:ShaderForge.SFN_Multiply,id:3848,x:33541,y:33599,varname:node_3848,prsc:2|A-2049-OUT,B-5427-R;n:type:ShaderForge.SFN_Tex2d,id:537,x:33861,y:32239,ptovrint:False,ptlb:dirt Albedo Height,ptin:_dirtAlbedoHeight,varname:_dirtAlbedoHeight,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:15c9cf773166dcc45b7d06a660fe1e4c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3982,x:33984,y:33440,ptovrint:False,ptlb:dirt Normal,ptin:_dirtNormal,varname:_dirtNormal,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0f04b6c8fc9c9e649a31ce55b9455386,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Lerp,id:5548,x:34414,y:32346,varname:node_5548,prsc:2|A-8767-OUT,B-537-RGB,T-2657-OUT;n:type:ShaderForge.SFN_Multiply,id:3383,x:32981,y:32686,varname:node_3383,prsc:2|A-1043-OUT,B-743-OUT;n:type:ShaderForge.SFN_Multiply,id:2657,x:33399,y:32555,varname:node_2657,prsc:2|A-2038-OUT,B-5427-G;n:type:ShaderForge.SFN_Power,id:1043,x:32584,y:32484,varname:node_1043,prsc:2|VAL-3326-OUT,EXP-7541-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7541,x:32336,y:32607,ptovrint:False,ptlb:detailHeight Strengtht,ptin:_detailHeightStrengtht,varname:_detailHeightStrengtht,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:2984,x:32566,y:32056,ptovrint:False,ptlb:baseHeight Strength,ptin:_baseHeightStrength,varname:_baseHeightStrength,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Power,id:743,x:32956,y:31997,varname:node_743,prsc:2|VAL-1100-OUT,EXP-2984-OUT;n:type:ShaderForge.SFN_OneMinus,id:1100,x:32741,y:31905,varname:node_1100,prsc:2|IN-922-A;n:type:ShaderForge.SFN_OneMinus,id:3326,x:32295,y:32464,varname:node_3326,prsc:2|IN-9103-A;n:type:ShaderForge.SFN_Lerp,id:2038,x:33056,y:32540,varname:node_2038,prsc:2|A-1043-OUT,B-743-OUT,T-5427-R;n:type:ShaderForge.SFN_Lerp,id:7344,x:34278,y:33248,varname:node_7344,prsc:2|A-9712-OUT,B-3982-RGB,T-2657-OUT;n:type:ShaderForge.SFN_Tex2d,id:7641,x:33565,y:32820,ptovrint:False,ptlb:dirt Metallic Smoothness,ptin:_dirtMetallicSmoothness,varname:_dirtMetallicSmoothness,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:613db3a6e5776564fa883f400fef806f,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:9616,x:34014,y:32546,varname:node_9616,prsc:2|A-3638-R,B-7641-R,T-2657-OUT;n:type:ShaderForge.SFN_Lerp,id:3677,x:33915,y:32681,varname:node_3677,prsc:2|A-2446-OUT,B-7641-A,T-2657-OUT;n:type:ShaderForge.SFN_Slider,id:81,x:32397,y:32983,ptovrint:False,ptlb:base Smoothness Strength,ptin:_baseSmoothnessStrength,varname:_baseSmoothnessStrength,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:10;n:type:ShaderForge.SFN_Multiply,id:2446,x:33111,y:32929,varname:node_2446,prsc:2|A-3638-A,B-81-OUT;proporder:922-3638-3405-4066-2049-9103-2140-2470-6059-537-7541-2984-3982-7641-81;pass:END;sub:END;*/

Shader "" {
    Properties {
        _baseAlbedoHeight ("base Albedo Height", 2D) = "white" {}
        _baseMetallicSmoothness ("base Metallic Smoothness", 2D) = "white" {}
        _baseNormalMap ("base Normal Map", 2D) = "bump" {}
        _detailNormalMap ("detail Normal Map", 2D) = "bump" {}
        _normalStrength ("normal Strength", Range(0, 1)) = 1
        _detailAlbedo ("detail Albedo", 2D) = "white" {}
        _albedoIntensity ("albedo Intensity", Range(0, 1)) = 0.05208128
        _baseColorTint ("baseColor Tint", Color) = (1,1,1,1)
        _detailColorTint_copy ("detailColor Tint_copy", Color) = (1,1,1,1)
        _dirtAlbedoHeight ("dirt Albedo Height", 2D) = "white" {}
        _detailHeightStrengtht ("detailHeight Strengtht", Float ) = 2
        _baseHeightStrength ("baseHeight Strength", Float ) = 2
        _dirtNormal ("dirt Normal", 2D) = "bump" {}
        _dirtMetallicSmoothness ("dirt Metallic Smoothness", 2D) = "white" {}
        _baseSmoothnessStrength ("base Smoothness Strength", Range(0, 10)) = 1
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
            uniform sampler2D _baseAlbedoHeight; uniform float4 _baseAlbedoHeight_ST;
            uniform sampler2D _baseMetallicSmoothness; uniform float4 _baseMetallicSmoothness_ST;
            uniform sampler2D _baseNormalMap; uniform float4 _baseNormalMap_ST;
            uniform sampler2D _detailNormalMap; uniform float4 _detailNormalMap_ST;
            uniform float _normalStrength;
            uniform sampler2D _detailAlbedo; uniform float4 _detailAlbedo_ST;
            uniform float _albedoIntensity;
            uniform float4 _baseColorTint;
            uniform float4 _detailColorTint_copy;
            uniform sampler2D _dirtAlbedoHeight; uniform float4 _dirtAlbedoHeight_ST;
            uniform sampler2D _dirtNormal; uniform float4 _dirtNormal_ST;
            uniform float _detailHeightStrengtht;
            uniform float _baseHeightStrength;
            uniform sampler2D _dirtMetallicSmoothness; uniform float4 _dirtMetallicSmoothness_ST;
            uniform float _baseSmoothnessStrength;
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
                float3 _baseNormalMap_var = UnpackNormal(tex2D(_baseNormalMap,TRANSFORM_TEX(i.uv0, _baseNormalMap)));
                float3 _detailNormalMap_var = UnpackNormal(tex2D(_detailNormalMap,TRANSFORM_TEX(i.uv0, _detailNormalMap)));
                float3 node_9712_nrm_base = _baseNormalMap_var.rgb + float3(0,0,1);
                float3 node_9712_nrm_detail = float3((_detailNormalMap_var.rgb.rg*(_normalStrength*i.vertexColor.r)),1.0) * float3(-1,-1,1);
                float3 node_9712_nrm_combined = node_9712_nrm_base*dot(node_9712_nrm_base, node_9712_nrm_detail)/node_9712_nrm_base.z - node_9712_nrm_detail;
                float3 node_9712 = node_9712_nrm_combined;
                float3 _dirtNormal_var = UnpackNormal(tex2D(_dirtNormal,TRANSFORM_TEX(i.uv0, _dirtNormal)));
                float4 _detailAlbedo_var = tex2D(_detailAlbedo,TRANSFORM_TEX(i.uv0, _detailAlbedo));
                float node_1043 = pow((1.0 - _detailAlbedo_var.a),_detailHeightStrengtht);
                float4 _baseAlbedoHeight_var = tex2D(_baseAlbedoHeight,TRANSFORM_TEX(i.uv0, _baseAlbedoHeight));
                float node_743 = pow((1.0 - _baseAlbedoHeight_var.a),_baseHeightStrength);
                float node_2657 = (lerp(node_1043,node_743,i.vertexColor.r)*i.vertexColor.g);
                float3 normalLocal = lerp(node_9712,_dirtNormal_var.rgb,node_2657);
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
                float4 _baseMetallicSmoothness_var = tex2D(_baseMetallicSmoothness,TRANSFORM_TEX(i.uv0, _baseMetallicSmoothness));
                float4 _dirtMetallicSmoothness_var = tex2D(_dirtMetallicSmoothness,TRANSFORM_TEX(i.uv0, _dirtMetallicSmoothness));
                float gloss = lerp((_baseMetallicSmoothness_var.a*_baseSmoothnessStrength),_dirtMetallicSmoothness_var.a,node_2657);
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
                float3 specularColor = lerp(_baseMetallicSmoothness_var.r,_dirtMetallicSmoothness_var.r,node_2657);
                float specularMonochrome;
                float4 _dirtAlbedoHeight_var = tex2D(_dirtAlbedoHeight,TRANSFORM_TEX(i.uv0, _dirtAlbedoHeight));
                float3 diffuseColor = lerp(lerp((_baseAlbedoHeight_var.rgb*_baseColorTint.rgb),(_detailAlbedo_var.rgb*_detailColorTint_copy.rgb),(_albedoIntensity*i.vertexColor.r)),_dirtAlbedoHeight_var.rgb,node_2657); // Need this for specular when using metallic
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
            uniform sampler2D _baseAlbedoHeight; uniform float4 _baseAlbedoHeight_ST;
            uniform sampler2D _baseMetallicSmoothness; uniform float4 _baseMetallicSmoothness_ST;
            uniform sampler2D _baseNormalMap; uniform float4 _baseNormalMap_ST;
            uniform sampler2D _detailNormalMap; uniform float4 _detailNormalMap_ST;
            uniform float _normalStrength;
            uniform sampler2D _detailAlbedo; uniform float4 _detailAlbedo_ST;
            uniform float _albedoIntensity;
            uniform float4 _baseColorTint;
            uniform float4 _detailColorTint_copy;
            uniform sampler2D _dirtAlbedoHeight; uniform float4 _dirtAlbedoHeight_ST;
            uniform sampler2D _dirtNormal; uniform float4 _dirtNormal_ST;
            uniform float _detailHeightStrengtht;
            uniform float _baseHeightStrength;
            uniform sampler2D _dirtMetallicSmoothness; uniform float4 _dirtMetallicSmoothness_ST;
            uniform float _baseSmoothnessStrength;
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
                float3 _baseNormalMap_var = UnpackNormal(tex2D(_baseNormalMap,TRANSFORM_TEX(i.uv0, _baseNormalMap)));
                float3 _detailNormalMap_var = UnpackNormal(tex2D(_detailNormalMap,TRANSFORM_TEX(i.uv0, _detailNormalMap)));
                float3 node_9712_nrm_base = _baseNormalMap_var.rgb + float3(0,0,1);
                float3 node_9712_nrm_detail = float3((_detailNormalMap_var.rgb.rg*(_normalStrength*i.vertexColor.r)),1.0) * float3(-1,-1,1);
                float3 node_9712_nrm_combined = node_9712_nrm_base*dot(node_9712_nrm_base, node_9712_nrm_detail)/node_9712_nrm_base.z - node_9712_nrm_detail;
                float3 node_9712 = node_9712_nrm_combined;
                float3 _dirtNormal_var = UnpackNormal(tex2D(_dirtNormal,TRANSFORM_TEX(i.uv0, _dirtNormal)));
                float4 _detailAlbedo_var = tex2D(_detailAlbedo,TRANSFORM_TEX(i.uv0, _detailAlbedo));
                float node_1043 = pow((1.0 - _detailAlbedo_var.a),_detailHeightStrengtht);
                float4 _baseAlbedoHeight_var = tex2D(_baseAlbedoHeight,TRANSFORM_TEX(i.uv0, _baseAlbedoHeight));
                float node_743 = pow((1.0 - _baseAlbedoHeight_var.a),_baseHeightStrength);
                float node_2657 = (lerp(node_1043,node_743,i.vertexColor.r)*i.vertexColor.g);
                float3 normalLocal = lerp(node_9712,_dirtNormal_var.rgb,node_2657);
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
                float4 _baseMetallicSmoothness_var = tex2D(_baseMetallicSmoothness,TRANSFORM_TEX(i.uv0, _baseMetallicSmoothness));
                float4 _dirtMetallicSmoothness_var = tex2D(_dirtMetallicSmoothness,TRANSFORM_TEX(i.uv0, _dirtMetallicSmoothness));
                float gloss = lerp((_baseMetallicSmoothness_var.a*_baseSmoothnessStrength),_dirtMetallicSmoothness_var.a,node_2657);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = lerp(_baseMetallicSmoothness_var.r,_dirtMetallicSmoothness_var.r,node_2657);
                float specularMonochrome;
                float4 _dirtAlbedoHeight_var = tex2D(_dirtAlbedoHeight,TRANSFORM_TEX(i.uv0, _dirtAlbedoHeight));
                float3 diffuseColor = lerp(lerp((_baseAlbedoHeight_var.rgb*_baseColorTint.rgb),(_detailAlbedo_var.rgb*_detailColorTint_copy.rgb),(_albedoIntensity*i.vertexColor.r)),_dirtAlbedoHeight_var.rgb,node_2657); // Need this for specular when using metallic
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
            uniform sampler2D _baseAlbedoHeight; uniform float4 _baseAlbedoHeight_ST;
            uniform sampler2D _baseMetallicSmoothness; uniform float4 _baseMetallicSmoothness_ST;
            uniform sampler2D _detailAlbedo; uniform float4 _detailAlbedo_ST;
            uniform float _albedoIntensity;
            uniform float4 _baseColorTint;
            uniform float4 _detailColorTint_copy;
            uniform sampler2D _dirtAlbedoHeight; uniform float4 _dirtAlbedoHeight_ST;
            uniform float _detailHeightStrengtht;
            uniform float _baseHeightStrength;
            uniform sampler2D _dirtMetallicSmoothness; uniform float4 _dirtMetallicSmoothness_ST;
            uniform float _baseSmoothnessStrength;
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
                
                float4 _baseAlbedoHeight_var = tex2D(_baseAlbedoHeight,TRANSFORM_TEX(i.uv0, _baseAlbedoHeight));
                float4 _detailAlbedo_var = tex2D(_detailAlbedo,TRANSFORM_TEX(i.uv0, _detailAlbedo));
                float4 _dirtAlbedoHeight_var = tex2D(_dirtAlbedoHeight,TRANSFORM_TEX(i.uv0, _dirtAlbedoHeight));
                float node_1043 = pow((1.0 - _detailAlbedo_var.a),_detailHeightStrengtht);
                float node_743 = pow((1.0 - _baseAlbedoHeight_var.a),_baseHeightStrength);
                float node_2657 = (lerp(node_1043,node_743,i.vertexColor.r)*i.vertexColor.g);
                float3 diffColor = lerp(lerp((_baseAlbedoHeight_var.rgb*_baseColorTint.rgb),(_detailAlbedo_var.rgb*_detailColorTint_copy.rgb),(_albedoIntensity*i.vertexColor.r)),_dirtAlbedoHeight_var.rgb,node_2657);
                float specularMonochrome;
                float3 specColor;
                float4 _baseMetallicSmoothness_var = tex2D(_baseMetallicSmoothness,TRANSFORM_TEX(i.uv0, _baseMetallicSmoothness));
                float4 _dirtMetallicSmoothness_var = tex2D(_dirtMetallicSmoothness,TRANSFORM_TEX(i.uv0, _dirtMetallicSmoothness));
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, lerp(_baseMetallicSmoothness_var.r,_dirtMetallicSmoothness_var.r,node_2657), specColor, specularMonochrome );
                float roughness = 1.0 - lerp((_baseMetallicSmoothness_var.a*_baseSmoothnessStrength),_dirtMetallicSmoothness_var.a,node_2657);
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
