Shader "Unlit/ColorBlend"
{
	Properties
	{
		_Color1 ("Color 1", Color) = (1,1,1,1)
        _Color2 ("Color 2", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True"  "RenderType"="Transparent" }
		LOD 200
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Color1;
            float4 _Color2;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// interpolate colors
				fixed4 col = i.uv.x * _Color1 + (1-i.uv.x) * _Color2;
                
				return col;
			}
			ENDCG
		}
	}
}
