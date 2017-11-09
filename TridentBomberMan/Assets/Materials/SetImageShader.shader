Shader "Custom/SetImageShader" {
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
	};

	v2f vert(appdata_base v)
	{
		v2f o;
		o.position = UnityObjectToClipPos(v.vertex);
		o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
		return o;
	}

	float4 frag(v2f i) : COLOR
	{
		return tex2D(_MainTex, i.uv);
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}
