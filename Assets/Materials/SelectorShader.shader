Shader "Unlit/SelectorShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color1 ("Color1", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

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
				float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Color1;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.screenPos = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
				float4 sPos = i.screenPos;
				float uv2 = step(frac((sPos.x + sPos.y) * 25 + _Time.x * 4), 0.5);
				float a = (_SinTime.w * 0.1 + 0.3) * uv2;

				fixed4 col = fixed4(uv2.xxx * _Color1.xyz, a);
				fixed2 pixSize = 1.0 / _ScreenParams.xy;
				
				// border
				fixed bw = 20;
				fixed2 b = bw * pixSize;
				fixed3 inv = (_Color1.yyy + 0.1) * (1 - uv2) + 0.5;
				if (i.uv.x <= b.x && i.uv.x > 0.001)
				{
					col = fixed4(inv * i.uv.x / b.x, 1);
				}
				if (i.uv.x >= 0.999 - b.x)
				{
					col = fixed4(inv * (1 - i.uv.x) / b.x, 1);
				}
				if (i.uv.y <= b.y && i.uv.y > 0.001)
				{
					col = fixed4(inv * i.uv.y / b.y, 1);
				}
				if (i.uv.y >= 0.999 - b.y)
				{
					col = fixed4(inv * (1 - i.uv.y) / b.y, 1);
				}

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
