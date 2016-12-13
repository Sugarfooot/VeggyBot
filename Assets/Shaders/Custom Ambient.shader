Shader "Custom Ambient" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _AmbTex ("Ambient", 2D) = "white" {}
        _BumpMap ("Bump (RGB)", 2D) = "bump" {}
        _IndicatorTex ("Indicator Lights (RGB)", 2D) = "black" {}
        _Indicator ("Indicator Color", Color) = (1,1,1,1)
        _Glow ("Glow Color", Color) = (0,0,0,1)
        _Silhouette ("Silhouette Color", Color) = (0.5, 0.5, 0.5, 1)
    }
     
    SubShader {
        Tags { "Queue" = "Geometry+3" "RenderType" = "Opaque" }
       
        Pass {         
            Tags { "LightMode" = "Always" }
            AlphaTest Greater 0.0
            ZWrite Off
            ZTest Greater
 
            Color [_Silhouette]
        }
               
CGPROGRAM
#pragma surface surf BlinnPhong noambient exclude_path:prepass fullforwardshadows
#pragma target 3.0
 
        uniform float4 _Color;
        uniform float _Shininess;
        uniform float4 _Indicator;
        uniform float4 _Glow;
        uniform sampler2D _MainTex;
        uniform sampler2D _AmbTex;
        uniform sampler2D _BumpMap;
        uniform sampler2D _IndicatorTex;
 
        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
        };
 
        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 ambTex = tex2D( _AmbTex, IN.uv_MainTex);
           
            half3 ambC = saturate(UNITY_LIGHTMODEL_AMBIENT * 15.0).rgb;
 
            o.Albedo = tex.rgb * _Color.rgb - ambTex.rgb * ambC;
            o.Gloss = tex.a;
            o.Specular = _Shininess;
            o.Emission = tex2D ( _IndicatorTex, IN.uv_MainTex).rgb * _Indicator.rgb * 2.0;
           
            o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
           
            half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
            o.Emission += ambTex.rgb * (pow (rim, 3.0) + ambC);
            o.Emission += rim * 3.0 * _Glow;
        }
 
ENDCG
    }
   
    Fallback " Glossy", 0
}