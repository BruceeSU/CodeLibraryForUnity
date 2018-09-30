Shader "LitonShaders/Test/GlassShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScaleValue("Scale value",Range(-10,10)) = 0.1
	}
	SubShader
	{
		Cull off
		Tags
		{
			Queue = Transparent  RenderType = Opaque
		}

		//抓取屏幕图像的pass
		GrabPass{ "_ScreenTex" }

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
				float2 screenUV : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				//计算采样屏幕图像的纹理坐标
				o.screenUV = ComputeGrabScreenPos(o.vertex);
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _ScreenTex;
			fixed _ScaleValue;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed2 toCenter = i.uv - fixed2(0.5,0.5);
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed2 newUV = i.screenUV  - normalize(toCenter) * sin(_ScaleValue *length(toCenter));
				newUV = fmod( newUV , 1 );
				fixed4 backgroundColor = tex2D( _ScreenTex, newUV);
				col.rgb *= backgroundColor.rgb;
				return col;
			}
			ENDCG
		}
	}
}
