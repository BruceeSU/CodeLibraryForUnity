Shader "LitonShaders/Test/TwoTextureCombine"
{
	Properties
	{
		//原图片
		_OrgTex ("Orginal Texture", 2D) = "white" {}

		//码图
		_MaskTex("Mosaic Texture",2D) = "white" {}

		//码图在水平方向的缩放比
		_UScale("Horizontal Split",Range(1,1000)) = 50

		//码图在竖直方向的缩放比
		_VScale("Vertical Split",Range(1,1000)) = 50

		_AddBright("Additonal Brightness", Range(-2,20)) = 0.2

		//采样偏移，越接近0.5，采样越靠近中心
		_Offset("Sampler Offset",Range(-10,10)) = 0.5

	}
	SubShader
	{
		Cull Off 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _OrgTex;
			sampler2D _MaskTex;
			float _UScale;
			float _VScale;
			float _AddBright;
			float _Offset;



			fixed4 frag (v2f i) : SV_Target
			{

				float baseX = i.uv.x - fmod(i.uv.x,1/_UScale) + (1 / _UScale ) * _Offset;
				float baseY = i.uv.y - fmod( i.uv.y, 1 / _VScale ) + (1 / _VScale) * _Offset;
				float2 orgUV = float2( baseX, baseY );
				float2 newUV = float2( i.uv.x*_UScale,i.uv.y * _VScale);
				
				fixed4 orgColor = tex2D(_OrgTex, orgUV);
				fixed4 maskColor = tex2D(_MaskTex,newUV);
				fixed luminance = Luminance(maskColor.rgb) + _AddBright;

				return fixed4(orgColor.rgb * luminance, 1);
				
			}
			ENDCG
		}
	}
}
