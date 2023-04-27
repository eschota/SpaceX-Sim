// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/ClimatZonesShader"
{
	Properties
	{
		_base_climat_zones("base_climat_zones", 2D) = "white" {}
		_TextureSample1("Texture Sample 0", 2D) = "white" {}
		_NoiseClimat("NoiseClimat", Float) = 3.67
		_OceanLevel("OceanLevel", Range( 0 , 1)) = 0.2588235
		_seamlessMultMask("seamlessMultMask", 2D) = "white" {}
		_RenderID("RenderID", Int) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform int _RenderID;
			uniform sampler2D _base_climat_zones;
			uniform float _NoiseClimat;
			uniform sampler2D _seamlessMultMask;
			uniform float4 _seamlessMultMask_ST;
			uniform sampler2D _TextureSample1;
			uniform float4 _TextureSample1_ST;
			uniform float _OceanLevel;
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			
			float4 CalculateContrast( float contrastValue, float4 colorTarget )
			{
				float t = 0.5 * ( 1.0 - contrastValue );
				return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
			}
			float3 MyCustomExpression48( int RenderID, float3 In0, float3 In1, float3 In2, float3 In3, float3 In4 )
			{
				switch (RenderID)
				{
					case 0:
					return In0;
					break;
					case 1:
					return In1;
					break;
					case 2:
					return In2;
					break;
					case 3:
					return In3;
					break;
					case 4:
					return In4;
					break;
					default:
					return 0;
					break;
				}
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				int RenderID48 = _RenderID;
				float simplePerlin3D11 = snoise( float3( i.ase_texcoord1.xy ,  0.0 )*_NoiseClimat );
				simplePerlin3D11 = simplePerlin3D11*0.5 + 0.5;
				float2 uv_seamlessMultMask = i.ase_texcoord1.xy * _seamlessMultMask_ST.xy + _seamlessMultMask_ST.zw;
				float3 In048 = tex2D( _base_climat_zones, ( float4( i.ase_texcoord1.xy, 0.0 , 0.0 ) + CalculateContrast(0.4,( ( simplePerlin3D11 * tex2D( _seamlessMultMask, uv_seamlessMultMask ) ) + float4( -0.5,0,0,0 ) )) ).rg ).rgb;
				float2 uv_TextureSample1 = i.ase_texcoord1.xy * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
				float4 temp_cast_4 = (_OceanLevel).xxxx;
				float4 clampResult32 = clamp( tex2D( _TextureSample1, uv_TextureSample1 ) , temp_cast_4 , float4( 1,1,1,0 ) );
				float3 In148 = clampResult32.rgb;
				float4 color45 = IsGammaSpace() ? float4(0,0.0377028,1,0) : float4(0,0.002918173,1,0);
				float3 In248 = color45.rgb;
				float4 color46 = IsGammaSpace() ? float4(1,0,0.786839,0) : float4(1,0,0.58176,0);
				float3 In348 = color46.rgb;
				float4 color47 = IsGammaSpace() ? float4(1,0.7942005,0.004716992,0) : float4(1,0.5940441,0.0003650922,0);
				float3 In448 = color47.rgb;
				float3 localMyCustomExpression48 = MyCustomExpression48( RenderID48 , In048 , In148 , In248 , In348 , In448 );
				
				
				finalColor = float4( localMyCustomExpression48 , 0.0 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18912
6;11;1908;1008;2720.359;315.6674;1.157138;True;True
Node;AmplifyShaderEditor.TexCoordVertexDataNode;12;-1801.588,-1346.202;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-2014.183,-977.2058;Inherit;False;Property;_NoiseClimat;NoiseClimat;2;0;Create;True;0;0;0;False;0;False;3.67;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;11;-2016.09,-818.4226;Inherit;True;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;22;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-1188.883,-414.506;Inherit;True;Property;_seamlessMultMask;seamlessMultMask;4;0;Create;True;0;0;0;False;0;False;-1;d8b1171af0931f34cac23763fc72ff3c;d8b1171af0931f34cac23763fc72ff3c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1478.783,-982.6057;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-1629.583,-679.7056;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;-0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;25;-1404.368,-679.1791;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0.4;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;31;-2127.136,118.2117;Inherit;True;Property;_TextureSample1;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;fe41fb262c955084ab44b9e21776a55e;fe41fb262c955084ab44b9e21776a55e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-2087.85,415.8118;Inherit;False;Property;_OceanLevel;OceanLevel;3;0;Create;True;0;0;0;True;0;False;0.2588235;0.2588235;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-1223.792,-907.5831;Inherit;True;2;2;0;FLOAT2;0,0;False;1;COLOR;-0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;45;-109.0144,406.0162;Inherit;False;Constant;_Color3;Color 0;6;0;Create;True;0;0;0;False;0;False;0,0.0377028,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;47;-107.2309,739.5298;Inherit;False;Constant;_Color5;Color 0;6;0;Create;True;0;0;0;False;0;False;1,0.7942005,0.004716992,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;32;-1699.137,263.2117;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;46;-107.2309,575.4481;Inherit;False;Constant;_Color4;Color 0;6;0;Create;True;0;0;0;False;0;False;1,0,0.786839,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;37;173.6762,423.766;Inherit;False;Property;_RenderID;RenderID;5;0;Create;True;0;0;0;False;0;False;0;4;False;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;10;-990.8414,-1084.818;Inherit;True;Property;_base_climat_zones;base_climat_zones;0;0;Create;True;0;0;0;False;0;False;-1;b421e413094084f42833df252352b751;b421e413094084f42833df252352b751;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GradientNode;27;-1484.302,-1403.745;Inherit;False;0;3;2;1,1,1,0;0,0,0,0.5000076;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1856.861,-290.8534;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;44;-107.231,233.0173;Inherit;False;Constant;_Color2;Color 0;6;0;Create;True;0;0;0;False;0;False;0,1,0.05708027,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;43;-110.7979,58.23485;Inherit;False;Constant;_Color0;Color 0;6;0;Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;33;-1604.137,506.7117;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;48;416.8196,437.2293;Inherit;False;switch (RenderID)${$	case 0:$	return In0@$	break@$$	case 1:$	return In1@$	break@$$	case 2:$	return In2@$	break@$$	case 3:$	return In3@$	break@$$	case 4:$	return In4@$	break@$$	default:$	return 0@$	break@$};3;Create;6;True;RenderID;INT;0;In;;Inherit;False;True;In0;FLOAT3;0,0,0;In;;Inherit;False;True;In1;FLOAT3;0,0,0;In;;Inherit;False;True;In2;FLOAT3;0,0,0;In;;Inherit;False;True;In3;FLOAT3;0,0,0;In;;Inherit;False;True;In4;FLOAT3;0,0,0;In;;Inherit;False;My Custom Expression;True;False;0;;False;6;0;INT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1192.503,-1231.328;Inherit;True;2;2;0;OBJECT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;13;-1620.89,-295.8221;Inherit;False;RadialUVDistortion;-1;;4;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;_Sampler6013;False;1;FLOAT2;10,10;False;11;FLOAT2;1,1;False;65;FLOAT;0.01;False;68;FLOAT2;0.9,0.76;False;47;FLOAT2;0,0;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StickyNoteNode;34;-2195.441,43.39172;Inherit;False;1491.642;1402.234;OceanAndDisplace;;1,1,1,1;;0;0
Node;AmplifyShaderEditor.StickyNoteNode;29;-2155.282,-1452.839;Inherit;False;1491.642;1402.234;ClimatZones;;1,1,1,1;;0;0
Node;AmplifyShaderEditor.ColorNode;20;-2089.783,-565.3056;Inherit;False;Constant;_Color1;Color 1;2;0;Create;True;0;0;0;False;0;False;1,1,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1785.583,-513.3057;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;17;730.701,453.6213;Float;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/ClimatZonesShader;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;11;0;12;0
WireConnection;11;1;19;0
WireConnection;18;0;11;0
WireConnection;18;1;24;0
WireConnection;22;0;18;0
WireConnection;25;1;22;0
WireConnection;23;0;12;0
WireConnection;23;1;25;0
WireConnection;32;0;31;0
WireConnection;32;1;30;0
WireConnection;10;1;23;0
WireConnection;33;0;32;0
WireConnection;33;1;30;0
WireConnection;33;2;30;0
WireConnection;48;0;37;0
WireConnection;48;1;10;0
WireConnection;48;2;32;0
WireConnection;48;3;45;0
WireConnection;48;4;46;0
WireConnection;48;5;47;0
WireConnection;28;0;27;0
WireConnection;28;1;12;0
WireConnection;13;68;11;0
WireConnection;13;29;15;0
WireConnection;21;0;11;0
WireConnection;21;1;20;1
WireConnection;17;0;48;0
ASEEND*/
//CHKSM=6A7854C0C76A8381FBC9949B239190D8A05687DD