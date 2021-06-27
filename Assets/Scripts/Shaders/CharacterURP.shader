Shader "Unlit/CharacterURP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Dissolve ("Dissolve", Range(0,1)) = 0.0
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        [HDR]_DissolveColor ("Edge dissolve color", Color) = (1,1,1,1)
        [HDR]_SecondDissolveColor ("Edge second dissolve color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
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
            sampler2D _DissolveTex;
            float4 _MainTex_ST;
            float4 _DissolveTex_ST;

            half _Dissolve;

            float4 _DissolveColor;
            float4 _SecondDissolveColor;
            fixed4 _Color;

            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 dissolveCol = tex2D(_DissolveTex, i.uv);

                half dissolveVal = dissolveCol.w - _Dissolve;

                clip(dissolveVal);

                if (dissolveVal <= 0.04){
                    return _DissolveColor;
                } else if (dissolveVal <= 0.06){
                    return _SecondDissolveColor;
                }

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}