Shader "Custom/FadeShader" {
	Properties
	{
		_MainTex("MainTexture",2D) = "white"{}
		_Mask("Mask",2D) = "white"{}
		_Threshold("Threshold",Range(0.0,1.0)) = 0.0
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

		struct v2f
		{
			float4 position : POSITION;
			float2 uv : TEXCOORD0;
		};
		sampler2D _MainTex;
		sampler2D _Mask;
		float _Threshold;

		v2f vert(appdata_base v)
		{
			v2f o;
			o.position = UnityObjectToClipPos(v.vertex);
			o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
			return o;
		}
		float4 frag(v2f i) : COLOR
		{
			float4 maskTex = tex2D(_Mask,i.uv);
			float gray = (maskTex.r * 0.3) + (maskTex.g * 0.6) + (maskTex.b * 0.1);

			if (gray <= _Threshold)
			{
				discard;
			}
			return tex2D(_MainTex,i.uv);
		}

		ENDCG
		}
	}
	FallBack "Diffuse"
}
