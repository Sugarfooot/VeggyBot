// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:33936,y:32330,varname:node_2865,prsc:2|diff-5713-OUT,spec-2230-OUT,gloss-7754-OUT,normal-3936-RGB,emission-2670-OUT,alpha-3656-OUT,voffset-6105-OUT;n:type:ShaderForge.SFN_Tex2d,id:745,x:31363,y:33404,ptovrint:False,ptlb:heightMap,ptin:_heightMap,varname:_heightMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:59b4ce678b8889d43b2671cf8b8a4892,ntxv:0,isnm:False|UVIN-9118-OUT;n:type:ShaderForge.SFN_Tex2d,id:3936,x:31376,y:33057,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:_NormalMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:5c812e1122126a94e8157bd20106d153,ntxv:3,isnm:True|UVIN-9118-OUT;n:type:ShaderForge.SFN_NormalVector,id:1238,x:32562,y:33493,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:7938,x:32813,y:33447,varname:node_7938,prsc:2|A-2963-OUT,B-1238-OUT;n:type:ShaderForge.SFN_Slider,id:1799,x:32716,y:33724,ptovrint:False,ptlb:offset_Size,ptin:_offset_Size,varname:_offset_Size,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:6105,x:33050,y:33545,varname:node_6105,prsc:2|A-7938-OUT,B-1799-OUT;n:type:ShaderForge.SFN_Slider,id:7754,x:32540,y:33141,ptovrint:False,ptlb:Roughness,ptin:_Roughness,varname:_Roughness,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:6564,x:31968,y:32658,ptovrint:False,ptlb:Color1,ptin:_Color1,varname:_Color1,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2521626,c2:0.7794118,c3:0.4085193,c4:1;n:type:ShaderForge.SFN_Color,id:361,x:31985,y:32910,ptovrint:False,ptlb:Color2,ptin:_Color2,varname:_Color2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.3893927,c2:0.4485294,c3:0.3660792,c4:1;n:type:ShaderForge.SFN_Lerp,id:5713,x:32529,y:32618,varname:node_5713,prsc:2|A-6564-RGB,B-361-RGB,T-9200-OUT;n:type:ShaderForge.SFN_Power,id:9200,x:32301,y:33098,varname:node_9200,prsc:2|VAL-2963-OUT,EXP-4993-OUT;n:type:ShaderForge.SFN_Slider,id:4993,x:31826,y:33682,ptovrint:False,ptlb:baseColor_slope,ptin:_baseColor_slope,varname:_baseColor_slope,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:10;n:type:ShaderForge.SFN_Power,id:2670,x:32441,y:32464,varname:node_2670,prsc:2|VAL-2963-OUT,EXP-6293-OUT;n:type:ShaderForge.SFN_Slider,id:6293,x:31766,y:32467,ptovrint:False,ptlb:node_4993_copy,ptin:_node_4993_copy,varname:_node_4993_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8935443,max:10;n:type:ShaderForge.SFN_TexCoord,id:950,x:30500,y:33117,varname:node_950,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:3915,x:30035,y:32838,varname:node_3915,prsc:2;n:type:ShaderForge.SFN_Add,id:8709,x:30860,y:32971,varname:node_8709,prsc:2|A-950-U,B-165-OUT;n:type:ShaderForge.SFN_Append,id:9118,x:31078,y:33177,varname:node_9118,prsc:2|A-8709-OUT,B-950-V;n:type:ShaderForge.SFN_Slider,id:2230,x:32730,y:32844,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:_Metallic,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Fresnel,id:2299,x:33075,y:32400,varname:node_2299,prsc:2|EXP-7882-OUT;n:type:ShaderForge.SFN_Lerp,id:3656,x:33457,y:32215,varname:node_3656,prsc:2|A-3034-OUT,B-6227-OUT,T-2299-OUT;n:type:ShaderForge.SFN_Slider,id:3034,x:32901,y:32127,ptovrint:False,ptlb:opacity_Min,ptin:_opacity_Min,varname:_opacity_Min,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:6227,x:32944,y:32260,ptovrint:False,ptlb:opacity_Max,ptin:_opacity_Max,varname:_opacity_Max,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_ValueProperty,id:7882,x:32825,y:32341,ptovrint:False,ptlb:Fresnel_power,ptin:_Fresnel_power,varname:_Fresnel_power,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Add,id:1601,x:30779,y:33539,varname:node_1601,prsc:2|A-950-V,B-165-OUT;n:type:ShaderForge.SFN_Append,id:2765,x:30999,y:33539,varname:node_2765,prsc:2|A-950-U,B-1601-OUT;n:type:ShaderForge.SFN_Clamp01,id:2963,x:31736,y:33505,varname:node_2963,prsc:2|IN-1972-OUT;n:type:ShaderForge.SFN_Distance,id:1556,x:33189,y:32715,varname:node_1556,prsc:2;n:type:ShaderForge.SFN_Multiply,id:165,x:30246,y:32746,varname:node_165,prsc:2|A-3915-TSL,B-3191-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3191,x:29764,y:32667,ptovrint:False,ptlb:speed,ptin:_speed,varname:_speed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:2762,x:31306,y:33657,ptovrint:False,ptlb:heightMap_copy,ptin:_heightMap_copy,varname:_heightMap_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:59b4ce678b8889d43b2671cf8b8a4892,ntxv:0,isnm:False|UVIN-2765-OUT;n:type:ShaderForge.SFN_Add,id:1972,x:31579,y:33505,varname:node_1972,prsc:2|A-745-R,B-2762-R;proporder:3936-745-1799-7754-6564-361-4993-6293-2230-3034-6227-7882-3191-2762;pass:END;sub:END;*/

Shader "Shader Forge/TestOcean" {
    Properties {
        _NormalMap ("NormalMap", 2D) = "bump" {}
        _heightMap ("heightMap", 2D) = "white" {}
        _offset_Size ("offset_Size", Range(0, 1)) = 1
        _Roughness ("Roughness", Range(0, 1)) = 0
        _Color1 ("Color1", Color) = (0.2521626,0.7794118,0.4085193,1)
        _Color2 ("Color2", Color) = (0.3893927,0.4485294,0.3660792,1)
        _baseColor_slope ("baseColor_slope", Range(0, 10)) = 0
        _node_4993_copy ("node_4993_copy", Range(0, 10)) = 0.8935443
        _Metallic ("Metallic", Range(0, 1)) = 0
        _opacity_Min ("opacity_Min", Range(0, 1)) = 0
        _opacity_Max ("opacity_Max", Range(0, 1)) = 1
        _Fresnel_power ("Fresnel_power", Float ) = 0.5
        _speed ("speed", Float ) = 1
        _heightMap_copy ("heightMap_copy", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _heightMap; uniform float4 _heightMap_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _offset_Size;
            uniform float _Roughness;
            uniform float4 _Color1;
            uniform float4 _Color2;
            uniform float _baseColor_slope;
            uniform float _node_4993_copy;
            uniform float _Metallic;
            uniform float _opacity_Min;
            uniform float _opacity_Max;
            uniform float _Fresnel_power;
            uniform float _speed;
            uniform sampler2D _heightMap_copy; uniform float4 _heightMap_copy_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
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
                UNITY_FOG_COORDS(7)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD8;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
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
                float4 node_3915 = _Time + _TimeEditor;
                float node_165 = (node_3915.r*_speed);
                float2 node_9118 = float2((o.uv0.r+node_165),o.uv0.g);
                float4 _heightMap_var = tex2Dlod(_heightMap,float4(TRANSFORM_TEX(node_9118, _heightMap),0.0,0));
                float2 node_2765 = float2(o.uv0.r,(o.uv0.g+node_165));
                float4 _heightMap_copy_var = tex2Dlod(_heightMap_copy,float4(TRANSFORM_TEX(node_2765, _heightMap_copy),0.0,0));
                float node_2963 = saturate((_heightMap_var.r+_heightMap_copy_var.r));
                v.vertex.xyz += ((node_2963*v.normal)*_offset_Size);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_3915 = _Time + _TimeEditor;
                float node_165 = (node_3915.r*_speed);
                float2 node_9118 = float2((i.uv0.r+node_165),i.uv0.g);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_9118, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
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
                float4 _heightMap_var = tex2D(_heightMap,TRANSFORM_TEX(node_9118, _heightMap));
                float2 node_2765 = float2(i.uv0.r,(i.uv0.g+node_165));
                float4 _heightMap_copy_var = tex2D(_heightMap_copy,TRANSFORM_TEX(node_2765, _heightMap_copy));
                float node_2963 = saturate((_heightMap_var.r+_heightMap_copy_var.r));
                float3 diffuseColor = lerp(_Color1.rgb,_Color2.rgb,pow(node_2963,_baseColor_slope)); // Need this for specular when using metallic
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
////// Emissive:
                float node_2670 = pow(node_2963,_node_4993_copy);
                float3 emissive = float3(node_2670,node_2670,node_2670);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,lerp(_opacity_Min,_opacity_Max,pow(1.0-max(0,dot(normalDirection, viewDirection)),_Fresnel_power)));
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
            ZWrite Off
            
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
            #pragma multi_compile_fwdadd
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _heightMap; uniform float4 _heightMap_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _offset_Size;
            uniform float _Roughness;
            uniform float4 _Color1;
            uniform float4 _Color2;
            uniform float _baseColor_slope;
            uniform float _node_4993_copy;
            uniform float _Metallic;
            uniform float _opacity_Min;
            uniform float _opacity_Max;
            uniform float _Fresnel_power;
            uniform float _speed;
            uniform sampler2D _heightMap_copy; uniform float4 _heightMap_copy_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
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
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_3915 = _Time + _TimeEditor;
                float node_165 = (node_3915.r*_speed);
                float2 node_9118 = float2((o.uv0.r+node_165),o.uv0.g);
                float4 _heightMap_var = tex2Dlod(_heightMap,float4(TRANSFORM_TEX(node_9118, _heightMap),0.0,0));
                float2 node_2765 = float2(o.uv0.r,(o.uv0.g+node_165));
                float4 _heightMap_copy_var = tex2Dlod(_heightMap_copy,float4(TRANSFORM_TEX(node_2765, _heightMap_copy),0.0,0));
                float node_2963 = saturate((_heightMap_var.r+_heightMap_copy_var.r));
                v.vertex.xyz += ((node_2963*v.normal)*_offset_Size);
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
                float4 node_3915 = _Time + _TimeEditor;
                float node_165 = (node_3915.r*_speed);
                float2 node_9118 = float2((i.uv0.r+node_165),i.uv0.g);
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_9118, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
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
                float4 _heightMap_var = tex2D(_heightMap,TRANSFORM_TEX(node_9118, _heightMap));
                float2 node_2765 = float2(i.uv0.r,(i.uv0.g+node_165));
                float4 _heightMap_copy_var = tex2D(_heightMap_copy,TRANSFORM_TEX(node_2765, _heightMap_copy));
                float node_2963 = saturate((_heightMap_var.r+_heightMap_copy_var.r));
                float3 diffuseColor = lerp(_Color1.rgb,_Color2.rgb,pow(node_2963,_baseColor_slope)); // Need this for specular when using metallic
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
                fixed4 finalRGBA = fixed4(finalColor * lerp(_opacity_Min,_opacity_Max,pow(1.0-max(0,dot(normalDirection, viewDirection)),_Fresnel_power)),0);
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
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _heightMap; uniform float4 _heightMap_ST;
            uniform float _offset_Size;
            uniform float _speed;
            uniform sampler2D _heightMap_copy; uniform float4 _heightMap_copy_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float2 uv1 : TEXCOORD2;
                float2 uv2 : TEXCOORD3;
                float4 posWorld : TEXCOORD4;
                float3 normalDir : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_3915 = _Time + _TimeEditor;
                float node_165 = (node_3915.r*_speed);
                float2 node_9118 = float2((o.uv0.r+node_165),o.uv0.g);
                float4 _heightMap_var = tex2Dlod(_heightMap,float4(TRANSFORM_TEX(node_9118, _heightMap),0.0,0));
                float2 node_2765 = float2(o.uv0.r,(o.uv0.g+node_165));
                float4 _heightMap_copy_var = tex2Dlod(_heightMap_copy,float4(TRANSFORM_TEX(node_2765, _heightMap_copy),0.0,0));
                float node_2963 = saturate((_heightMap_var.r+_heightMap_copy_var.r));
                v.vertex.xyz += ((node_2963*v.normal)*_offset_Size);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
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
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _heightMap; uniform float4 _heightMap_ST;
            uniform float _offset_Size;
            uniform float _Roughness;
            uniform float4 _Color1;
            uniform float4 _Color2;
            uniform float _baseColor_slope;
            uniform float _node_4993_copy;
            uniform float _Metallic;
            uniform float _speed;
            uniform sampler2D _heightMap_copy; uniform float4 _heightMap_copy_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_3915 = _Time + _TimeEditor;
                float node_165 = (node_3915.r*_speed);
                float2 node_9118 = float2((o.uv0.r+node_165),o.uv0.g);
                float4 _heightMap_var = tex2Dlod(_heightMap,float4(TRANSFORM_TEX(node_9118, _heightMap),0.0,0));
                float2 node_2765 = float2(o.uv0.r,(o.uv0.g+node_165));
                float4 _heightMap_copy_var = tex2Dlod(_heightMap_copy,float4(TRANSFORM_TEX(node_2765, _heightMap_copy),0.0,0));
                float node_2963 = saturate((_heightMap_var.r+_heightMap_copy_var.r));
                v.vertex.xyz += ((node_2963*v.normal)*_offset_Size);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 node_3915 = _Time + _TimeEditor;
                float node_165 = (node_3915.r*_speed);
                float2 node_9118 = float2((i.uv0.r+node_165),i.uv0.g);
                float4 _heightMap_var = tex2D(_heightMap,TRANSFORM_TEX(node_9118, _heightMap));
                float2 node_2765 = float2(i.uv0.r,(i.uv0.g+node_165));
                float4 _heightMap_copy_var = tex2D(_heightMap_copy,TRANSFORM_TEX(node_2765, _heightMap_copy));
                float node_2963 = saturate((_heightMap_var.r+_heightMap_copy_var.r));
                float node_2670 = pow(node_2963,_node_4993_copy);
                o.Emission = float3(node_2670,node_2670,node_2670);
                
                float3 diffColor = lerp(_Color1.rgb,_Color2.rgb,pow(node_2963,_baseColor_slope));
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
