// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:35189,y:32738,varname:node_2865,prsc:2|diff-1078-OUT,spec-6409-OUT,gloss-743-OUT,normal-6549-OUT;n:type:ShaderForge.SFN_Tex2d,id:5694,x:34143,y:32213,ptovrint:False,ptlb:BC_base,ptin:_BC_base,varname:_BC_base,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5233-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:1790,x:34128,y:32465,ptovrint:False,ptlb:BC_red,ptin:_BC_red,varname:_BC_red,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-930-OUT;n:type:ShaderForge.SFN_Tex2d,id:6216,x:34134,y:32753,ptovrint:False,ptlb:dirtBC_green,ptin:_dirtBC_green,varname:_dirtBC_green,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5233-UVOUT;n:type:ShaderForge.SFN_Lerp,id:1078,x:34853,y:32351,varname:node_1078,prsc:2|A-2469-OUT,B-7985-OUT,T-3750-R;n:type:ShaderForge.SFN_VertexColor,id:3750,x:34130,y:32052,varname:node_3750,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:2668,x:34134,y:33004,ptovrint:False,ptlb:MS_base,ptin:_MS_base,varname:_MS_base,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5233-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6762,x:34134,y:33235,ptovrint:False,ptlb:MS_red,ptin:_MS_red,varname:_MS_red,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-930-OUT;n:type:ShaderForge.SFN_Tex2d,id:6209,x:34134,y:33462,ptovrint:False,ptlb:dirtMS_green,ptin:_dirtMS_green,varname:_dirtMS_green,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5233-UVOUT;n:type:ShaderForge.SFN_Lerp,id:8670,x:34510,y:32665,varname:node_8670,prsc:2|A-2668-R,B-6762-R,T-3750-R;n:type:ShaderForge.SFN_Lerp,id:7259,x:34498,y:32991,varname:node_7259,prsc:2|A-2668-A,B-6762-A,T-3750-R;n:type:ShaderForge.SFN_Tex2d,id:8752,x:34134,y:33721,ptovrint:False,ptlb:NM_base,ptin:_NM_base,varname:_NM_base,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True|UVIN-5233-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6185,x:34134,y:33952,ptovrint:False,ptlb:NM_red,ptin:_NM_red,varname:_NM_red,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True|UVIN-930-OUT;n:type:ShaderForge.SFN_Lerp,id:6549,x:34513,y:33361,varname:node_6549,prsc:2|A-8752-RGB,B-6185-RGB,T-3750-R;n:type:ShaderForge.SFN_TexCoord,id:2868,x:33301,y:32932,varname:node_2868,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:368,x:33558,y:33131,varname:node_368,prsc:2|A-2868-U,B-1410-OUT;n:type:ShaderForge.SFN_Multiply,id:4663,x:33558,y:33290,varname:node_4663,prsc:2|A-2868-V,B-6921-OUT;n:type:ShaderForge.SFN_Append,id:1845,x:33724,y:33155,varname:node_1845,prsc:2|A-368-OUT,B-4663-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1410,x:33269,y:33336,ptovrint:False,ptlb:U_tiling_base,ptin:_U_tiling_base,varname:_U_tiling_base,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:6921,x:33269,y:33476,ptovrint:False,ptlb:V_tiling_base,ptin:_V_tiling_base,varname:_V_tiling_base,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:4322,x:33563,y:33486,varname:node_4322,prsc:2|A-2868-U,B-3535-OUT;n:type:ShaderForge.SFN_Multiply,id:3582,x:33563,y:33645,varname:node_3582,prsc:2|A-2868-V,B-3803-OUT;n:type:ShaderForge.SFN_Append,id:930,x:33729,y:33510,varname:node_930,prsc:2|A-4322-OUT,B-3582-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3535,x:33274,y:33691,ptovrint:False,ptlb:U_tiling_red,ptin:_U_tiling_red,varname:_U_tiling_red,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:3803,x:33274,y:33831,ptovrint:False,ptlb:V_tiling_red,ptin:_V_tiling_red,varname:_V_tiling_red,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Rotator,id:5233,x:33770,y:32863,varname:node_5233,prsc:2|UVIN-1845-OUT,ANG-8177-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8177,x:33552,y:32833,ptovrint:False,ptlb:UV_angle_base,ptin:_UV_angle_base,varname:_UV_angle_base,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:6409,x:34730,y:32739,varname:node_6409,prsc:2|A-8670-OUT,B-60-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4831,x:34498,y:33145,ptovrint:False,ptlb:Smoothness_intensity,ptin:_Smoothness_intensity,varname:_Smoothness_intensity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:60,x:34510,y:32866,ptovrint:False,ptlb:Metallic_intensity,ptin:_Metallic_intensity,varname:_Metallic_intensity,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:743,x:34665,y:32997,varname:node_743,prsc:2|A-7259-OUT,B-4831-OUT;n:type:ShaderForge.SFN_Color,id:8121,x:34240,y:32600,ptovrint:False,ptlb:tint_red,ptin:_tint_red,varname:_tint_red,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Color,id:8729,x:34240,y:32349,ptovrint:False,ptlb:tint_base,ptin:_tint_base,varname:_tint_base,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:2469,x:34458,y:32290,varname:node_2469,prsc:2|A-5694-RGB,B-8729-RGB;n:type:ShaderForge.SFN_Multiply,id:7985,x:34480,y:32553,varname:node_7985,prsc:2|A-1790-RGB,B-8121-RGB;proporder:5694-1790-2668-6762-8752-6185-1410-6921-3535-3803-8177-4831-60-8729-8121;pass:END;sub:END;*/

Shader "Shader Forge/LayeredPBR" {
    Properties {
        _BC_base ("BC_base", 2D) = "white" {}
        _BC_red ("BC_red", 2D) = "white" {}
        _MS_base ("MS_base", 2D) = "white" {}
        _MS_red ("MS_red", 2D) = "white" {}
        _NM_base ("NM_base", 2D) = "bump" {}
        _NM_red ("NM_red", 2D) = "bump" {}
        _U_tiling_base ("U_tiling_base", Float ) = 1
        _V_tiling_base ("V_tiling_base", Float ) = 1
        _U_tiling_red ("U_tiling_red", Float ) = 1
        _V_tiling_red ("V_tiling_red", Float ) = 1
        _UV_angle_base ("UV_angle_base", Float ) = 0
        _Smoothness_intensity ("Smoothness_intensity", Float ) = 1
        _Metallic_intensity ("Metallic_intensity", Float ) = 1
        _tint_base ("tint_base", Color) = (1,1,1,1)
        _tint_red ("tint_red", Color) = (1,1,1,1)
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
            uniform sampler2D _BC_base; uniform float4 _BC_base_ST;
            uniform sampler2D _BC_red; uniform float4 _BC_red_ST;
            uniform sampler2D _MS_base; uniform float4 _MS_base_ST;
            uniform sampler2D _MS_red; uniform float4 _MS_red_ST;
            uniform sampler2D _NM_base; uniform float4 _NM_base_ST;
            uniform sampler2D _NM_red; uniform float4 _NM_red_ST;
            uniform float _U_tiling_base;
            uniform float _V_tiling_base;
            uniform float _U_tiling_red;
            uniform float _V_tiling_red;
            uniform float _UV_angle_base;
            uniform float _Smoothness_intensity;
            uniform float _Metallic_intensity;
            uniform float4 _tint_red;
            uniform float4 _tint_base;
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
                float node_5233_ang = _UV_angle_base;
                float node_5233_spd = 1.0;
                float node_5233_cos = cos(node_5233_spd*node_5233_ang);
                float node_5233_sin = sin(node_5233_spd*node_5233_ang);
                float2 node_5233_piv = float2(0.5,0.5);
                float2 node_5233 = (mul(float2((i.uv0.r*_U_tiling_base),(i.uv0.g*_V_tiling_base))-node_5233_piv,float2x2( node_5233_cos, -node_5233_sin, node_5233_sin, node_5233_cos))+node_5233_piv);
                float3 _NM_base_var = UnpackNormal(tex2D(_NM_base,TRANSFORM_TEX(node_5233, _NM_base)));
                float2 node_930 = float2((i.uv0.r*_U_tiling_red),(i.uv0.g*_V_tiling_red));
                float3 _NM_red_var = UnpackNormal(tex2D(_NM_red,TRANSFORM_TEX(node_930, _NM_red)));
                float3 normalLocal = lerp(_NM_base_var.rgb,_NM_red_var.rgb,i.vertexColor.r);
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
                float4 _MS_base_var = tex2D(_MS_base,TRANSFORM_TEX(node_5233, _MS_base));
                float4 _MS_red_var = tex2D(_MS_red,TRANSFORM_TEX(node_930, _MS_red));
                float gloss = (lerp(_MS_base_var.a,_MS_red_var.a,i.vertexColor.r)*_Smoothness_intensity);
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
                float3 specularColor = (lerp(_MS_base_var.r,_MS_red_var.r,i.vertexColor.r)*_Metallic_intensity);
                float specularMonochrome;
                float4 _BC_base_var = tex2D(_BC_base,TRANSFORM_TEX(node_5233, _BC_base));
                float4 _BC_red_var = tex2D(_BC_red,TRANSFORM_TEX(node_930, _BC_red));
                float3 diffuseColor = lerp((_BC_base_var.rgb*_tint_base.rgb),(_BC_red_var.rgb*_tint_red.rgb),i.vertexColor.r); // Need this for specular when using metallic
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
            uniform sampler2D _BC_base; uniform float4 _BC_base_ST;
            uniform sampler2D _BC_red; uniform float4 _BC_red_ST;
            uniform sampler2D _MS_base; uniform float4 _MS_base_ST;
            uniform sampler2D _MS_red; uniform float4 _MS_red_ST;
            uniform sampler2D _NM_base; uniform float4 _NM_base_ST;
            uniform sampler2D _NM_red; uniform float4 _NM_red_ST;
            uniform float _U_tiling_base;
            uniform float _V_tiling_base;
            uniform float _U_tiling_red;
            uniform float _V_tiling_red;
            uniform float _UV_angle_base;
            uniform float _Smoothness_intensity;
            uniform float _Metallic_intensity;
            uniform float4 _tint_red;
            uniform float4 _tint_base;
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
                float node_5233_ang = _UV_angle_base;
                float node_5233_spd = 1.0;
                float node_5233_cos = cos(node_5233_spd*node_5233_ang);
                float node_5233_sin = sin(node_5233_spd*node_5233_ang);
                float2 node_5233_piv = float2(0.5,0.5);
                float2 node_5233 = (mul(float2((i.uv0.r*_U_tiling_base),(i.uv0.g*_V_tiling_base))-node_5233_piv,float2x2( node_5233_cos, -node_5233_sin, node_5233_sin, node_5233_cos))+node_5233_piv);
                float3 _NM_base_var = UnpackNormal(tex2D(_NM_base,TRANSFORM_TEX(node_5233, _NM_base)));
                float2 node_930 = float2((i.uv0.r*_U_tiling_red),(i.uv0.g*_V_tiling_red));
                float3 _NM_red_var = UnpackNormal(tex2D(_NM_red,TRANSFORM_TEX(node_930, _NM_red)));
                float3 normalLocal = lerp(_NM_base_var.rgb,_NM_red_var.rgb,i.vertexColor.r);
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
                float4 _MS_base_var = tex2D(_MS_base,TRANSFORM_TEX(node_5233, _MS_base));
                float4 _MS_red_var = tex2D(_MS_red,TRANSFORM_TEX(node_930, _MS_red));
                float gloss = (lerp(_MS_base_var.a,_MS_red_var.a,i.vertexColor.r)*_Smoothness_intensity);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = (lerp(_MS_base_var.r,_MS_red_var.r,i.vertexColor.r)*_Metallic_intensity);
                float specularMonochrome;
                float4 _BC_base_var = tex2D(_BC_base,TRANSFORM_TEX(node_5233, _BC_base));
                float4 _BC_red_var = tex2D(_BC_red,TRANSFORM_TEX(node_930, _BC_red));
                float3 diffuseColor = lerp((_BC_base_var.rgb*_tint_base.rgb),(_BC_red_var.rgb*_tint_red.rgb),i.vertexColor.r); // Need this for specular when using metallic
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
            uniform sampler2D _BC_base; uniform float4 _BC_base_ST;
            uniform sampler2D _BC_red; uniform float4 _BC_red_ST;
            uniform sampler2D _MS_base; uniform float4 _MS_base_ST;
            uniform sampler2D _MS_red; uniform float4 _MS_red_ST;
            uniform float _U_tiling_base;
            uniform float _V_tiling_base;
            uniform float _U_tiling_red;
            uniform float _V_tiling_red;
            uniform float _UV_angle_base;
            uniform float _Smoothness_intensity;
            uniform float _Metallic_intensity;
            uniform float4 _tint_red;
            uniform float4 _tint_base;
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
                
                float node_5233_ang = _UV_angle_base;
                float node_5233_spd = 1.0;
                float node_5233_cos = cos(node_5233_spd*node_5233_ang);
                float node_5233_sin = sin(node_5233_spd*node_5233_ang);
                float2 node_5233_piv = float2(0.5,0.5);
                float2 node_5233 = (mul(float2((i.uv0.r*_U_tiling_base),(i.uv0.g*_V_tiling_base))-node_5233_piv,float2x2( node_5233_cos, -node_5233_sin, node_5233_sin, node_5233_cos))+node_5233_piv);
                float4 _BC_base_var = tex2D(_BC_base,TRANSFORM_TEX(node_5233, _BC_base));
                float2 node_930 = float2((i.uv0.r*_U_tiling_red),(i.uv0.g*_V_tiling_red));
                float4 _BC_red_var = tex2D(_BC_red,TRANSFORM_TEX(node_930, _BC_red));
                float3 diffColor = lerp((_BC_base_var.rgb*_tint_base.rgb),(_BC_red_var.rgb*_tint_red.rgb),i.vertexColor.r);
                float specularMonochrome;
                float3 specColor;
                float4 _MS_base_var = tex2D(_MS_base,TRANSFORM_TEX(node_5233, _MS_base));
                float4 _MS_red_var = tex2D(_MS_red,TRANSFORM_TEX(node_930, _MS_red));
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, (lerp(_MS_base_var.r,_MS_red_var.r,i.vertexColor.r)*_Metallic_intensity), specColor, specularMonochrome );
                float roughness = 1.0 - (lerp(_MS_base_var.a,_MS_red_var.a,i.vertexColor.r)*_Smoothness_intensity);
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
