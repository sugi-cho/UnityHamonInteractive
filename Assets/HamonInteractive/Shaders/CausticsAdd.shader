Shader "Hidden/Hamon/CausticsAdd"
{
    Properties
    {
        _NormalInfluence ("Normal Influence (intensity remap)", Range(0,1)) = 1
        _ColorTint ("Color Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        Blend One One
        ZWrite Off
        ZTest Always
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            StructuredBuffer<float4> _HitBuffer; // xy=uv, z=intensity, w=valid
            float _NormalInfluence; // 未使用（将来用）
            float4 _ColorTint;      // 省略時 (1,1,1,1)

            struct v2f
            {
                float4 posCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float intensity : TEXCOORD1;
            };

            v2f vert(uint id : SV_VertexID)
            {
                float4 h = _HitBuffer[id];
                v2f o;
                if (h.w < 0.5)
                {
                    o.posCS = 0;
                    o.uv = 0;
                    o.intensity = 0;
                    return o;
                }

                float2 uv = h.xy;
                // 直接UV空間をクリップ座標にマッピングし、カメラ行列に依存しない描画
                o.posCS = float4(uv * 2.0 - 1.0, 0, 1);
                o.uv = uv;
                o.intensity = h.z;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                if (i.intensity <= 0) discard;
                float e = i.intensity;
                float4 tint = (_ColorTint == 0) ? float4(1,1,1,1) : _ColorTint;
                return float4(tint.rgb * e, 1);
            }
            ENDHLSL
        }
    }
}
