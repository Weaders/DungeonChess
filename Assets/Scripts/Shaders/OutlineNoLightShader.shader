Shader "Custom/OutlineNoLightShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1, 0, 0, 1)
        _Progress ("Progress", Range(0, 1)) = 0
        _MaxHeight ("Max Height", float) = 0.2
        _WidthOutLine ("Width outline", float) = 0.01
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
         

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;
            fixed4 _OutlineColor;
            float _Progress;
            float _MaxHeight;
            float _WidthOutLine;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.y -= _Progress * _MaxHeight;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                if (i.uv.y < _WidthOutLine || i.uv.y > 1 - _WidthOutLine || i.uv.x < _WidthOutLine || i.uv.x > 1 - _WidthOutLine) {

                    fixed4 col = _OutlineColor;
                    col.a = 1 - (_Progress + 0.2);

                    if (col.a < 0)
                        col.a = 0;

                    return col;

                } else {
                
                    fixed4 col = tex2D(_MainTex, i.uv) * _Color ;
                    return col;

                }

            }
            ENDCG
        }
    }
}
