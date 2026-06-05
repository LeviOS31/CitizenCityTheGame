Shader "UI/TutorialRectHighlighter"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Overlay Color", Color) = (0,0,0,0.75)
        _CutoutPos ("Cutout Center (Normalized 0-1)", Vector) = (0.5, 0.5, 0, 0)
        _Size ("Cutout Size (Width, Height)", Vector) = (0.2, 0.1, 0, 0)
        _Feather ("Edge Softness", Range(0.001, 0.1)) = 0.005
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off Lighting Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord  : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            fixed4 _Color;
            float4 _CutoutPos;
            float4 _Size;
            float _Feather;
            sampler2D _MainTex;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Get normalized screen coordinates (0 to 1)
                float2 uv = i.screenPos.xy / i.screenPos.w;
                
                // Account for screen aspect ratio so width and height feel intuitive
                float aspect = _ScreenParams.x / _ScreenParams.y;
                float2 aspectUV = float2(uv.x * aspect, uv.y);
                float2 aspectCenter = float2(_CutoutPos.x * aspect, _CutoutPos.y);
                float2 aspectSize = float2(_Size.x * aspect, _Size.y);

                // Signed distance field (SDF) for a rectangle
                float2 d = abs(aspectUV - aspectCenter) - (aspectSize * 0.5);
                float dist = length(max(d, 0.0)) + min(max(d.x, d.y), 0.0);

                // Smoothly blend the alpha at the boundaries of the rectangle
                float alpha = smoothstep(0.0, _Feather, dist);

                fixed4 col = _Color;
                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}