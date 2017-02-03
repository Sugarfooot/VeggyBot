// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "AQUAS/Mobile/Single-Colored" {
    Properties {
        [NoScaleOffset]_NormalTexture ("Normal Texture", 2D) = "white" {}
        _NormalTiling ("Normal Tiling", Float ) = 1
        _DeepWaterColor ("Deep Water Color", Color) = (0.04313726,0.2392157,0.4627451,1)
        _DepthTransparency ("Depth Transparency", Float ) = 1.5
        _ShoreFade ("Shore Fade", Float ) = 0.3
        _ShoreTransparency ("Shore Transparency", Float ) = 0
        [HideInInspector]_ReflectionTex ("Reflection Tex", 2D) = "white" {}
        [MaterialToggle] _UseReflections ("Enable Reflections", Float ) = 0.5215687
        _Reflectionintensity ("Reflection intensity", Range(0, 1)) = 0.5
        _Distortion ("Distortion", Range(0, 2)) = 0.3
        _Specular ("Specular", Float ) = 1
        _SpecularColor ("Specular Color", Color) = (0.6470588,0.6518166,0.6518166,1)
        _Gloss ("Gloss", Float ) = 0.7
        _LightWrapping ("Light Wrapping", Float ) = 2
        _Refraction ("Refraction", Range(0, 1)) = 0.67
        _WaveSpeed ("Wave Speed", Float ) = 40
        _EmissiveColor ("Emissive Color", Color) = (0.5,0.5,0.5,1)
        _EmissionIntensity ("Emission Intensity", Float ) = 0
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
            ColorMask RGB
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x 
            #pragma target 2.0
            uniform float4 _LightColor0;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float4 _DeepWaterColor;
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform float _Reflectionintensity;
            uniform fixed _UseReflections;
            uniform float _DepthTransparency;
            uniform float _Specular;
            uniform float _Gloss;
            uniform float _LightWrapping;
            uniform sampler2D _NormalTexture; uniform float4 _NormalTexture_ST;
            uniform float _Refraction;
            uniform float _WaveSpeed;
            uniform float _Distortion;
            uniform float4 _SpecularColor;
            uniform float _NormalTiling;
            uniform float4 _EmissiveColor;
            uniform float _EmissionIntensity;
            uniform float _ShoreFade;
            uniform float _ShoreTransparency;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 recipObjScale = float3( length(unity_WorldToObject[0].xyz), length(unity_WorldToObject[1].xyz), length(unity_WorldToObject[2].xyz) );
                float3 objScale = 1.0/recipObjScale;
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float _rotator_ang = 1.5708;
                float _rotator_spd = 1.0;
                float _rotator_cos = cos(_rotator_spd*_rotator_ang);
                float _rotator_sin = sin(_rotator_spd*_rotator_ang);
                float2 _rotator_piv = float2(0.5,0.5);
                float2 _rotator = (mul(i.uv0-_rotator_piv,float2x2( _rotator_cos, -_rotator_sin, _rotator_sin, _rotator_cos))+_rotator_piv);
                float2 _division = ((objScale.rb*_NormalTiling)/1000.0);
                float4 _timer = _Time + _TimeEditor;
                float3 _multiplier1 = (float3((_WaveSpeed/_division),0.0)*(_timer.r/100.0));
                float2 _multiplier3 = ((_rotator+_multiplier1)*_division);
                float4 _texture1 = tex2D(_NormalTexture,TRANSFORM_TEX(_multiplier3, _NormalTexture));
                float2 _multiplier2 = ((i.uv0+_multiplier1)*_division);
                float4 _texture2 = tex2D(_NormalTexture,TRANSFORM_TEX(_multiplier2, _NormalTexture));
                float3 _subtractor = (_texture1.rgb-_texture2.rgb);
                float3 _lerp = lerp(float3(0,0,1),_subtractor,_Refraction);
                float3 normalLocal = _lerp;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _Gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = (_Specular*_SpecularColor.rgb);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 w = float3(_LightWrapping,_LightWrapping,_LightWrapping)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = forwardLight * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float2 _componentMask = _subtractor.rg;
                float2 _remap = ((i.screenPos.rg+(float2(_componentMask.r,_componentMask.g)*_Distortion))*0.5+0.5);
                float4 _ReflectionTex_var = tex2D(_ReflectionTex,TRANSFORM_TEX(_remap, _ReflectionTex));
                float3 diffuseColor = lerp( _DeepWaterColor.rgb, lerp(_ReflectionTex_var.rgb,_DeepWaterColor.rgb,(1.0 - _Reflectionintensity)), _UseReflections );
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = saturate((_lerp+(_EmissiveColor.rgb*_EmissionIntensity)-1.0));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,(pow(saturate((sceneZ-partZ)/_DepthTransparency),_ShoreFade)*saturate((sceneZ-partZ)/_ShoreTransparency)));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
