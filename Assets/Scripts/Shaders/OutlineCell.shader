Shader "Custom/OutlineCell"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1, 0, 0, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;
        fixed4 _OutlineColor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            if (IN.uv_MainTex.y < 0.01 || IN.uv_MainTex.y > 0.99 || IN.uv_MainTex.x < 0.01 || IN.uv_MainTex.x > 0.99) {
                
                o.Alpha = 1;
                o.Albedo = _OutlineColor.rgb;

            }
            else {

                fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
                o.Alpha = _Color.a;
                o.Albedo = c.rgb;

            }
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
