Shader "BIGMESS/OutlineShaded" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_Outline("Outline Color", Color) = (0,0,0,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_OutlineTex("Outline Texture", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Size("Outline Thickness", Float) = 1.5
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			// render outline

			Pass {


			Cull Off
			ZWrite Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				half _Size;
				fixed4 _Outline;
				sampler2D _OutlineTex;

				struct appdata {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};
				v2f vert(appdata v) {
					v2f o;
					v.vertex.xyz *= _Size;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}
				half4 frag(v2f i) : SV_Target
				{
					float4 texColor = tex2D(_OutlineTex, i.uv);
					return _Outline;
				}
				ENDCG
			}

			Tags { "RenderType" = "Opaque" }
			LOD 200

					// render model

					Stencil {
						Ref 1
						Comp always
						Pass replace
					}


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
					fixed4 _Color;
					void surf(Input IN, inout SurfaceOutputStandard o) {
						// Albedo comes from a texture tinted by color
						fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
						o.Albedo = c.rgb;
						// Metallic and smoothness come from slider variables
						o.Smoothness = _Glossiness;
						o.Alpha = c.a;
					}
					ENDCG
		}
			FallBack "Diffuse"
}