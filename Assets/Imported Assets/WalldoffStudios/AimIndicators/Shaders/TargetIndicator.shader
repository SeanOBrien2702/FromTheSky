Shader "WalldoffStudios/TargetIndicator"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comparison", Float) = 8
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp("Stencil Operation", Float) = 0
        [Enum(Off,0,On,1)]_ZWrite ("ZWrite", Float) = 1.0
    }
    SubShader
    {
        Tags 
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        Stencil
		{
			Ref 1
			Comp [_StencilComp]
            Pass [_StencilOp]
		    fail keep
		}	
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite [_ZWrite]
        Cull off

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
                float4 settings : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainColor;
            float4 _FillColor;
            float _Fill;
            float _Brightness;
            float4 _VertPoints[4];

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(_VertPoints[v.settings.w].xyz);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 indicator = tex2D(_MainTex, i.uv);
                float4 mainColor = indicator * _MainColor;

                const float2 center = float2(0.5, 0.5);
                const float dist = distance(i.uv, center);
                
                const float fillDelta = saturate(sign(_Fill * 0.5f - dist));

                float3 fillColor = _FillColor * fillDelta;
                float3 lerpedColor = lerp(indicator.rgb, fillColor.rgb, fillDelta) * _Brightness;
                
                float alfaLerp = lerp(mainColor.a, _FillColor.a * indicator.a, fillDelta);
                return float4(lerpedColor, alfaLerp);
            }
            ENDCG
        }
    }
}
