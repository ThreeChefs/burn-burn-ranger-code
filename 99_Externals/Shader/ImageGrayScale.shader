Shader "Custom/GrayscaleSpriteUI"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Gray ("Grayscale Amount", Range(0,1)) = 1
        _GrayBrightness ("Gray Brightness", Range(0,2)) = 1
        _GrayOffset ("Gray Offset", Range(-1,1)) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
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
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _Gray;
            float _GrayBrightness;
            float _GrayOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.texcoord) * i.color;

                fixed lum = dot(c.rgb, fixed3(0.2126, 0.7152, 0.0722));

                // 명도 조절 (곱 + 오프셋)
                lum = saturate(lum * _GrayBrightness + _GrayOffset);

                fixed3 gray = fixed3(lum, lum, lum);
                c.rgb = lerp(c.rgb, gray, saturate(_Gray));

                return c;
            }
            ENDCG
        }
    }
}