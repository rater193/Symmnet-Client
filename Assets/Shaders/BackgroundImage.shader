Shader "Unlit/BackgroundImage"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_CloudTex1("Texture", 2D) = "white" {}
		_CloudTex2("Texture", 2D) = "white" {}
		_WaveTex("Texture", 2D) = "white" {}
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
				//float2 screenPos : TEXCOORD2;
				float4 screenPos : TEXCOORD1;
			};

			struct v2f
			{
				//float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 scrPos : TEXCOORD2;
			};

			sampler2D _MainTex;
			sampler2D _CloudTex1;
			sampler2D _CloudTex2;
			sampler2D _WaveTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.scrPos = o.vertex;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 baseWorldPos = unity_ObjectToWorld._m30_m31_m32;

				fixed4 col2 = tex2D(_CloudTex1, i.scrPos + _SinTime);
				fixed4 col3 = tex2D(_CloudTex2, i.scrPos + _CosTime);
				fixed4 col4 = (tex2D(_WaveTex, i.scrPos + (_Time/3)))/4;
				fixed4 col = tex2D(_MainTex, (i.scrPos + float2(col2.r, col2.r) + float2(col3.r, col3.r) + float2(col4.r, col4.r))/4 + baseWorldPos);
				return col;
			}
			ENDCG
		}
	}
}
