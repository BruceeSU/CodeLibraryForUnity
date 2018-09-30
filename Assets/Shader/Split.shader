Shader "LitonShaders/Test/SplitShader"
{
	Properties
	{
		//原图片
		_OrgTex ("Orginal Texture", 2D) = "white" {}

		_MaskTex ("Orginal Texture", 2D) = "white" {}

		//码图在水平方向的缩放比
		_UScale("Horizontal Split",Range(1,1000)) = 50

		//码图在竖直方向的缩放比
		_VScale("Vertical Split",Range(1,1000)) = 50

		//采样偏移，越接近0.5，采样越靠近中心
		_Threshold("Threshold",Range(0,1)) = 0.5

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

			//计算颜色的明度
			float CalcuBrightness(fixed3 color)
			{
				return color.r * 0.2125 + color.g *0.7154 + color.b * 0.0721;
			}

			float2 MapToGrid( float2 uv, float2 grid )
			{
				return float2( fmod(uv.x,grid.x), fmod(uv.y,grid.y) );
			}

			//将UV映射到一个尺寸的格子里，scale的两个分量的取值范围是( 0,1 )
			float2 MapToScale(float2 uv, float2 scale )
			{
				return float2(fmod( uv.x, scale.x )/scale.x, fmod(uv.y, scale.y)/scale.y);
			}

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
			float _Threshold;

			fixed4 frag (v2f i) : SV_Target
			{			
				fixed4 orgColor = tex2D(_OrgTex, i.uv);
				float2 mUV = MapToScale(i.uv, float2(1/_UScale, 1/_VScale));

				//每个格子是一个完整的maskTexture
				fixed3 maskColor = tex2D( _MaskTex,mUV );

				//整个画面是一个完整的maskTexture
				//fixed3 maskColor = tex2D( _MaskTex,i.uv );
				if(mUV.x > _Threshold || mUV.y > _Threshold) return orgColor;
				else return fixed4(maskColor,1);
			}
			ENDCG
		}
	}
}
