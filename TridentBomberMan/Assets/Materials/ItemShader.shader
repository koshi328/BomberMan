Shader "Custom/ItemShader" {
	Properties
	{
		_MainTex("MainTexture",2D) = "white"{}
	}

	SubShader
	{
		Pass
		{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 3.0
#include "UnityCG.cginc"

		sampler2D _MainTex;

		struct v2f
		{
			float4 position : POSITION;
			float2 uv : TEXCOORD0;
			float3 worldPos : TEXCOORD1;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
			o.position = UnityObjectToClipPos(v.vertex);
			o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex);
			return o;
		}

		float4 frag(v2f i) : COLOR
		{
			float dest = distance(i.worldPos, float3(0,0,0));
		dest = abs(sin(dest / 10 - _Time * 40));
		dest = 0.3 - dest / 3;
		//dest = clamp(dest,0.9,1.0) - 0.9f;
		//dest *= 5;
		float4 color = tex2D(_MainTex, i.uv) + float4(0.0, 0.0, dest, 0.0);
			return color;
		}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
