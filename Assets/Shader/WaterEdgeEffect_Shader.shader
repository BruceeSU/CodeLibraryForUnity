Shader "LitonShaders/Test/WaterEdgeEffect_Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{
		Queue = Transparent
		
		}
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 scrPos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//计算顶点的屏幕坐标，用于采样深度纹理，传入的是剪裁空间的坐标值
				o.scrPos = ComputeScreenPos(o.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				//根据像素的屏幕坐标来采样深度纹理，得到改像素对应的地方的深度值信息
				float depth = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, i.scrPos );
				//获取观察空间下的Z值
				float viewSpaceZ = LinearEyeDepth( depth );

				//计算改像素点在观察空间下的深度值
				fixed4 viewSpacePos = mul(UNITY_V,	i.vertex );


				col.rgb = 1 - col.rgb;
				return col;
			}
			ENDCG
		}
	}
}
