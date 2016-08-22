Shader "Unlit/outlineGlow"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Range("Range", float) = 1
		_OutlineColor("Outline Color" , Color) = (1,1,1,1)

	}
		SubShader
		{
			LOD 200
			//Tags { "RenderType" = "Opaque" }



			Pass//outline pass seen through objects
			{
			Tags{"Queue" = "Overlay" "RenderType" = "Opaque"}
			ZTest Always
			Cull Back
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency
												//Blend One OneMinusSrcAlpha // Premultiplied transparency
												//Blend One One // Additive
												//Blend OneMinusDstColor One // Soft Additive
												//Blend DstColor Zero // Multiplicative
												//Blend DstColor SrcColor // 2x Multiplicative
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

			#include "UnityCG.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			//sampler2D _MainTex;
			//float4 _MainTex_ST;
			float _Range;
			float4 _OutlineColor;

			v2f vert(appdata v)
			{
				v2f o;
				float3 val = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 off = TransformViewToProjection(val.xy);//ignore all but xy
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.vertex.xy += _Range *  off;// Removed o.vertex.z * for ortho

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _OutlineColor;
				return col;
			}
			ENDCG
		}
			//normal default pass
				Tags{"RenderType" = "Opaque"}
				ZWrite On
				ZTest Less
				Cull Back

				CGPROGRAM
				// Physically based Standard lighting model, and enable shadows on all light types
				#pragma surface surf Standard fullforwardshadows

				// Use shader model 3.0 target, to get nicer looking lighting
				#pragma target 3.0

				sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			void surf(Input IN, inout SurfaceOutputStandard o) {
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
