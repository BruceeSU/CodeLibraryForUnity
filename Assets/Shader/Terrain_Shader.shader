// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "LitonShaders/Test/Terrain_Shader"
{
	Properties
	{

		_TerrainSize("TerrainSize",vector) = (500,500,600,1)
		_GrassMaxHeight("Grass Max Height",float) = 5.0
		_GrassMinHeight("Grass Min Height",float) = 3.0
		_GrassSoilTexBlendThreshold("Grass Soil Blend Threshold",range(0,1)) = 0.5
		_SnowThikness("SnowThinkness",Range(0,10)) = 2
		_SnowAngle("SnowStackableAngle",Range(0,90))=50
		_SnowEdgeBlendRange("Snow Edge Blend Range",range(0,0.5)) = 0.1
		_GrassTex ("Grass", 2D) = "white" {}
		_SoilTex ("Soil", 2D) = "white" {}
		_SnowTex ("Snow", 2D) = "white" {}
		_NoiseTex("Noise",2D) = "white"{}
		_NoiseTex1("Noise 1",2D) = "white"{}
	}
	SubShader
	{

		Pass
		{

			Tags
			{
				//设置正确的渲染路径才能得到正确的内置变量的值,
				//如果不写这个，_WorldSpaceLightPos0的值会不正确
				"LightMode"="ForwardBase"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			//包含一些内置的光照变量：_LightColor0
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal: TEXCOORD1;
				float3 pos: TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			fixed _GrassMaxHeight;
			float _GrassMinHeight;
			fixed _GrassSoilTexBlendThreshold;
			fixed _SnowThikness;
			fixed _SnowAngle;
			fixed _SnowEdgeBlendRange;
			sampler2D _GrassTex;
			sampler2D _SoilTex;
			sampler2D _SnowTex;
			sampler2D _NoiseTex;
			sampler2D _NoiseTex1;
			
			fixed4 _TerrainSize;
			fixed4 _GrassTex_ST;
			fixed4 _SoilTex_ST;

			

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.pos = v.vertex.xyz;
				o.normal = v.normal;
				o.uv = v.uv;
				return o;
			}
		

			fixed4 frag (v2f i) : SV_Target
			{
				fixed2 newUV = i.uv  * _TerrainSize.xy;

				float3 worldNormal = normalize( mul( i.normal,unity_ObjectToWorld ));
				float3 worldLight = normalize( _WorldSpaceLightPos0.xyz );
				float colorRate = dot( worldNormal, worldLight ) * 0.5 + 0.5;

				fixed grassRate = 1.0 - min( i.pos.y / _GrassMaxHeight,1.0);
				
				fixed noiseLumimance = tex2D(_NoiseTex,newUV).r * tex2D(_NoiseTex1, newUV).r ;
				//fixed noise1Luminance = ;
				
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT;
				fixed4 grassColor = tex2D(_GrassTex, newUV);
				fixed4 soilColor = tex2D(_SoilTex, newUV);

				fixed4 col = soilColor;
				//计算坡度
				fixed slope = dot( worldNormal, fixed3(0,1,0) );
				

				if(i.pos.y < _GrassMaxHeight)
				{
						if(slope* grassRate  > noiseLumimance * _GrassSoilTexBlendThreshold) col = grassColor;
				}		
				fixed3 snowColor = tex2D( _SnowTex,newUV);
				fixed snowValue =slope - cos(radians(_SnowAngle));
				snowValue = ceil(snowValue + _SnowEdgeBlendRange - noiseLumimance * 0.5);
				


				col =  col* (1 - snowValue) + fixed4(snowColor * snowValue * _SnowThikness,1);
				col = _LightColor0 * colorRate * col + fixed4(ambient,1);
				return col;
			}
			ENDCG
		}
	}
}
