Shader "Custom/test"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_col("Color",Color) = (1,1,1,1)
		_offx("X offset", float) = 0
		_offy("Y offset", float) = 0

		_posTex("Position Texture",2D) = "white" {}
		_Poffx("Position offset", Range(-4,4)) = 0
		_sampleoffx("Sampler x Offset", Range(0.5,1.5)) = 0
		_sampleoffy("Sampler y Offset",Range(0.5,1.5)) = 0
		_Cup("Clamp upper bounds", float) = 0
		_Cdown("Clamp Lower Bounds", float) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

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
					float3 norm : NORMAL;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float3 norm : NORMAL;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST,_col;

				float _offx,_offy;

				float _Poffx,_Cup,_Cdown, _sampleoffx, _sampleoffy;
				sampler2D _posTex;

				v2f vert(appdata v)
				{
					v2f o;
					#if !defined(SHADER_API_OPENGL)//allows text to be loaded in vert func
					
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					float4 posEdit = tex2Dlod(_posTex, float4(v.uv.x + _sampleoffx -1, v.uv.y + _sampleoffy - 1 ,0,0));
					v.vertex.xyz += v.norm.xyz * clamp(mul(posEdit , /*sin(_Time.w)**/_Poffx),_Cdown,_Cup);
					o.norm = v.norm;
					o.vertex = mul(UNITY_MATRIX_MVP , v.vertex);
					#endif
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					i.uv.x = i.uv.x + _offx * sin(_Time.y);
					i.uv.y = i.uv.y + _offy * cos(_Time.y);
					fixed4 col = tex2D(_MainTex, i.uv);
					col = col * _col;
					return col;
				}
				ENDCG
			}
		}
}
