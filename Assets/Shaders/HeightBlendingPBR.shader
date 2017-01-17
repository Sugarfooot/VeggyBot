// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:35266,y:32272,varname:node_2865,prsc:2|diff-6380-OUT,spec-2548-OUT,gloss-3890-OUT,normal-3266-OUT;n:type:ShaderForge.SFN_Tex2d,id:3101,x:34106,y:32030,ptovrint:False,ptlb:baseColor_Height_Black,ptin:_baseColor_Height_Black,varname:_baseColor_Height_Black,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c9094d6cca0265440a4eeceee6cf526f,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5015,x:33957,y:32272,ptovrint:False,ptlb:baseColor_Height_R,ptin:_baseColor_Height_R,varname:_baseColor_Height_R,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:15c9cf773166dcc45b7d06a660fe1e4c,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Lerp,id:1312,x:33813,y:32796,varname:node_1312,prsc:2;n:type:ShaderForge.SFN_VertexColor,id:7639,x:34032,y:32578,varname:node_7639,prsc:2;n:type:ShaderForge.SFN_Subtract,id:8237,x:34386,y:32349,varname:node_8237,prsc:2|A-9833-OUT,B-7639-R;n:type:ShaderForge.SFN_ConstantClamp,id:7447,x:34721,y:32357,varname:node_7447,prsc:2,min:0,max:1|IN-9270-OUT;n:type:ShaderForge.SFN_Divide,id:9270,x:34534,y:32445,varname:node_9270,prsc:2|A-8237-OUT,B-4915-OUT;n:type:ShaderForge.SFN_Vector1,id:4915,x:34274,y:32712,varname:node_4915,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Lerp,id:6380,x:34927,y:32173,varname:node_6380,prsc:2|A-5015-RGB,B-3101-RGB,T-7447-OUT;n:type:ShaderForge.SFN_Tex2d,id:7469,x:34123,y:33297,ptovrint:False,ptlb:Metallic_Smoothness_black,ptin:_Metallic_Smoothness_black,varname:_Metallic_Smoothness_black,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d0bd04d8035088644adf597806c4fcd8,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9686,x:34287,y:32835,ptovrint:False,ptlb:Norm_red,ptin:_Norm_red,varname:_Norm_red,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0f04b6c8fc9c9e649a31ce55b9455386,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Lerp,id:3266,x:35035,y:32688,varname:node_3266,prsc:2|A-9686-RGB,B-5351-RGB,T-7447-OUT;n:type:ShaderForge.SFN_Tex2d,id:495,x:34123,y:33545,ptovrint:False,ptlb:Metallic_Smoothness_red,ptin:_Metallic_Smoothness_red,varname:_Metallic_Smoothness_red,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:613db3a6e5776564fa883f400fef806f,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Lerp,id:2548,x:34945,y:32496,varname:node_2548,prsc:2|A-495-R,B-7469-R,T-7447-OUT;n:type:ShaderForge.SFN_Lerp,id:3890,x:34720,y:32844,varname:node_3890,prsc:2|A-495-G,B-7469-G,T-7447-OUT;n:type:ShaderForge.SFN_Tex2d,id:6087,x:33701,y:32992,ptovrint:False,ptlb:node_6087,ptin:_node_6087,varname:_node_6087,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0f04b6c8fc9c9e649a31ce55b9455386,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:5351,x:34093,y:32795,ptovrint:False,ptlb:Norm_red_copy,ptin:_Norm_red_copy,varname:_Norm_red_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2cf225950dc09e54b93233794823a7ba,ntxv:3,isnm:True;n:type:ShaderForge.SFN_ConstantClamp,id:9833,x:34264,y:32092,varname:node_9833,prsc:2,min:0,max:0.9|IN-3101-A;proporder:3101-5015-7469-9686-495-5351;pass:END;sub:END;*/

Shader "" {
    Properties {
        _baseColor_Height_Black ("baseColor_Height_Black", 2D) = "white" {}
        _baseColor_Height_R ("baseColor_Height_R", 2D) = "bump" {}
        _Metallic_Smoothness_black ("Metallic_Smoothness_black", 2D) = "black" {}
        _Norm_red ("Norm_red", 2D) = "bump" {}
        _Metallic_Smoothness_red ("Metallic_Smoothness_red", 2D) = "black" {}
        _Norm_red_copy ("Norm_red_copy", 2D) = "bump" {}
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
            uniform sampler2D _baseColor_Height_Black; uniform float4 _baseColor_Height_Black_ST;
            uniform sampler2D _baseColor_Height_R; uniform float4 _baseColor_Height_R_ST;
            uniform sampler2D _Metallic_Smoothness_black; uniform float4 _Metallic_Smoothness_black_ST;
            uniform sampler2D _Norm_red; uniform float4 _Norm_red_ST;
            uniform sampler2D _Metallic_Smoothness_red; uniform float4 _Metallic_Smoothness_red_ST;
            uniform sampler2D _Norm_red_copy; uniform float4 _Norm_red_copy_ST;
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
                float3 _Norm_red_var = UnpackNormal(tex2D(_Norm_red,TRANSFORM_TEX(i.uv0, _Norm_red)));
                float3 _Norm_red_copy_var = UnpackNormal(tex2D(_Norm_red_copy,TRANSFORM_TEX(i.uv0, _Norm_red_copy)));
                float4 _baseColor_Height_Black_var = tex2D(_baseColor_Height_Black,TRANSFORM_TEX(i.uv0, _baseColor_Height_Black));
                float node_7447 = clamp(((clamp(_baseColor_Height_Black_var.a,0,0.9)-i.vertexColor.r)/0.2),0,1);
                float3 normalLocal = lerp(_Norm_red_var.rgb,_Norm_red_copy_var.rgb,node_7447);
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
                float4 _Metallic_Smoothness_red_var = tex2D(_Metallic_Smoothness_red,TRANSFORM_TEX(i.uv0, _Metallic_Smoothness_red));
                float4 _Metallic_Smoothness_black_var = tex2D(_Metallic_Smoothness_black,TRANSFORM_TEX(i.uv0, _Metallic_Smoothness_black));
                float gloss = lerp(_Metallic_Smoothness_red_var.g,_Metallic_Smoothness_black_var.g,node_7447);
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
                float3 specularColor = lerp(_Metallic_Smoothness_red_var.r,_Metallic_Smoothness_black_var.r,node_7447);
                float specularMonochrome;
                float4 _baseColor_Height_R_var = tex2D(_baseColor_Height_R,TRANSFORM_TEX(i.uv0, _baseColor_Height_R));
                float3 diffuseColor = lerp(_baseColor_Height_R_var.rgb,_baseColor_Height_Black_var.rgb,node_7447); // Need this for specular when using metallic
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
            uniform sampler2D _baseColor_Height_Black; uniform float4 _baseColor_Height_Black_ST;
            uniform sampler2D _baseColor_Height_R; uniform float4 _baseColor_Height_R_ST;
            uniform sampler2D _Metallic_Smoothness_black; uniform float4 _Metallic_Smoothness_black_ST;
            uniform sampler2D _Norm_red; uniform float4 _Norm_red_ST;
            uniform sampler2D _Metallic_Smoothness_red; uniform float4 _Metallic_Smoothness_red_ST;
            uniform sampler2D _Norm_red_copy; uniform float4 _Norm_red_copy_ST;
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
                float3 _Norm_red_var = UnpackNormal(tex2D(_Norm_red,TRANSFORM_TEX(i.uv0, _Norm_red)));
                float3 _Norm_red_copy_var = UnpackNormal(tex2D(_Norm_red_copy,TRANSFORM_TEX(i.uv0, _Norm_red_copy)));
                float4 _baseColor_Height_Black_var = tex2D(_baseColor_Height_Black,TRANSFORM_TEX(i.uv0, _baseColor_Height_Black));
                float node_7447 = clamp(((clamp(_baseColor_Height_Black_var.a,0,0.9)-i.vertexColor.r)/0.2),0,1);
                float3 normalLocal = lerp(_Norm_red_var.rgb,_Norm_red_copy_var.rgb,node_7447);
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
                float4 _Metallic_Smoothness_red_var = tex2D(_Metallic_Smoothness_red,TRANSFORM_TEX(i.uv0, _Metallic_Smoothness_red));
                float4 _Metallic_Smoothness_black_var = tex2D(_Metallic_Smoothness_black,TRANSFORM_TEX(i.uv0, _Metallic_Smoothness_black));
                float gloss = lerp(_Metallic_Smoothness_red_var.g,_Metallic_Smoothness_black_var.g,node_7447);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = lerp(_Metallic_Smoothness_red_var.r,_Metallic_Smoothness_black_var.r,node_7447);
                float specularMonochrome;
                float4 _baseColor_Height_R_var = tex2D(_baseColor_Height_R,TRANSFORM_TEX(i.uv0, _baseColor_Height_R));
                float3 diffuseColor = lerp(_baseColor_Height_R_var.rgb,_baseColor_Height_Black_var.rgb,node_7447); // Need this for specular when using metallic
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
            uniform sampler2D _baseColor_Height_Black; uniform float4 _baseColor_Height_Black_ST;
            uniform sampler2D _baseColor_Height_R; uniform float4 _baseColor_Height_R_ST;
            uniform sampler2D _Metallic_Smoothness_black; uniform float4 _Metallic_Smoothness_black_ST;
            uniform sampler2D _Metallic_Smoothness_red; uniform float4 _Metallic_Smoothness_red_ST;
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
                
                float4 _baseColor_Height_R_var = tex2D(_baseColor_Height_R,TRANSFORM_TEX(i.uv0, _baseColor_Height_R));
                float4 _baseColor_Height_Black_var = tex2D(_baseColor_Height_Black,TRANSFORM_TEX(i.uv0, _baseColor_Height_Black));
                float node_7447 = clamp(((clamp(_baseColor_Height_Black_var.a,0,0.9)-i.vertexColor.r)/0.2),0,1);
                float3 diffColor = lerp(_baseColor_Height_R_var.rgb,_baseColor_Height_Black_var.rgb,node_7447);
                float specularMonochrome;
                float3 specColor;
                float4 _Metallic_Smoothness_red_var = tex2D(_Metallic_Smoothness_red,TRANSFORM_TEX(i.uv0, _Metallic_Smoothness_red));
                float4 _Metallic_Smoothness_black_var = tex2D(_Metallic_Smoothness_black,TRANSFORM_TEX(i.uv0, _Metallic_Smoothness_black));
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, lerp(_Metallic_Smoothness_red_var.r,_Metallic_Smoothness_black_var.r,node_7447), specColor, specularMonochrome );
                float roughness = 1.0 - lerp(_Metallic_Smoothness_red_var.g,_Metallic_Smoothness_black_var.g,node_7447);
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
