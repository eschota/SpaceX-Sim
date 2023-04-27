// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/ClimatZonesShader"
{
	Properties
	{
		_OceanLevel("OceanLevel", Range( 0 , 1)) = 0.2588235
		_RenderID("RenderID", Int) = 0

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
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _OceanLevel;
			uniform int _RenderID;
			float4 MyCustomExpression48( int RenderID, float4 BaseMap, float4 MaskMap, float3 NormalMap, float3 HeightMap, float3 EmissiveMap )
			{
				switch (RenderID)
				{
					case 0:
					return BaseMap;
					break;
					case 1:
					return MaskMap;
					break;
					case 2:
					return NormalMap;
					break;
					case 3:
					return HeightMap;
					break;
					case 4:
					return EmissiveMap;
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
				float4 color43 = IsGammaSpace() ? float4(0.5,0.5,0.5,1) : float4(0.2140411,0.2140411,0.2140411,1);
				float4 BaseMap48 = color43;
				float4 color44 = IsGammaSpace() ? float4(0,1,1,0.6980392) : float4(0,1,1,0.6980392);
				float4 MaskMap48 = color44;
				float4 color45 = IsGammaSpace() ? float4(0.5,0.519608,1,1) : float4(0.2140411,0.2326409,1,1);
				float3 NormalMap48 = color45.rgb;
				float4 color47 = IsGammaSpace() ? float4(0,0,0,1) : float4(0,0,0,1);
				float3 HeightMap48 = color47.rgb;
				float3 EmissiveMap48 = color47.rgb;
				float4 localMyCustomExpression48 = MyCustomExpression48( RenderID48 , BaseMap48 , MaskMap48 , NormalMap48 , HeightMap48 , EmissiveMap48 );
				
				
				finalColor = localMyCustomExpression48;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18912
21;13;2094;1330;629.6073;262.6986;1;True;True
Node;AmplifyShaderEditor.ColorNode;47;-106.2309,737.5298;Inherit;False;Constant;_Color5;Color 5;6;0;Create;True;0;0;0;False;0;False;0,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;45;-109.0144,406.0162;Inherit;False;Constant;_Color3;Color 3;6;0;Create;True;0;0;0;False;0;False;0.5,0.519608,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;44;-107.231,233.0173;Inherit;False;Constant;_Color2;Color 2;6;0;Create;True;0;0;0;False;0;False;0,1,1,0.6980392;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;43;-110.7979,58.23485;Inherit;False;Constant;_Color0;Color 0;6;0;Create;True;0;0;0;False;0;False;0.5,0.5,0.5,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;37;-47.32379,-49.23401;Inherit;False;Property;_RenderID;RenderID;5;0;Create;True;0;0;0;False;0;False;0;4;False;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-1629.583,-679.7056;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;-0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StickyNoteNode;29;-2151.185,-1452.839;Inherit;False;1491.642;1402.234;ClimatZones;;1,1,1,1;;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1856.861,-290.8534;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1478.783,-982.6057;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2087.85,415.8118;Inherit;False;Property;_OceanLevel;OceanLevel;3;0;Create;True;0;0;0;True;0;False;0.2588235;0.311;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;33;-1604.137,506.7117;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1192.503,-1231.328;Inherit;True;2;2;0;OBJECT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;13;-1620.89,-295.8221;Inherit;False;RadialUVDistortion;-1;;4;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;_Sampler6013;False;1;FLOAT2;10,10;False;11;FLOAT2;1,1;False;65;FLOAT;0.01;False;68;FLOAT2;0.9,0.76;False;47;FLOAT2;0,0;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StickyNoteNode;34;-2195.441,43.39172;Inherit;False;1491.642;1402.234;OceanAndDisplace;;1,1,1,1;;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1785.583,-513.3057;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;-2089.783,-565.3056;Inherit;False;Constant;_Color1;Color 1;2;0;Create;True;0;0;0;False;0;False;1,1,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;24;-1188.883,-414.506;Inherit;True;Property;_seamlessMultMask;seamlessMultMask;4;0;Create;True;0;0;0;False;0;False;-1;d8b1171af0931f34cac23763fc72ff3c;d8b1171af0931f34cac23763fc72ff3c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;48;420.8196,434.2293;Inherit;False;switch (RenderID)${$	case 0:$	return BaseMap@$	break@$$	case 1:$	return MaskMap@$	break@$$	case 2:$	return NormalMap@$	break@$$	case 3:$	return HeightMap@$	break@$$	case 4:$	return EmissiveMap@$	break@$$	default:$	return 0@$	break@$};4;Create;6;True;RenderID;INT;0;In;;Inherit;False;True;BaseMap;FLOAT4;0,0,0,0;In;;Inherit;False;True;MaskMap;FLOAT4;0,0,0,0;In;;Inherit;False;True;NormalMap;FLOAT3;0,0,0;In;;Inherit;False;True;HeightMap;FLOAT3;0,0,0;In;;Inherit;False;True;EmissiveMap;FLOAT3;0,0,0;In;;Inherit;False;My Custom Expression;True;False;0;;False;6;0;INT;0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;46;-107.2309,575.4481;Inherit;False;Constant;_Color4;Color 4;6;0;Create;True;0;0;0;False;0;False;1,0,0.786839,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;11;-2016.09,-818.4226;Inherit;True;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;22;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-2014.183,-977.2058;Inherit;False;Property;_NoiseClimat;NoiseClimat;2;0;Create;True;0;0;0;False;0;False;3.67;12.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;27;-1484.302,-1403.745;Inherit;False;0;3;2;1,1,1,0;0,0,0,0.5000076;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;12;-1801.588,-1346.202;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;32;-1699.137,263.2117;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;10;-990.8414,-1084.818;Inherit;True;Property;_base_climat_zones;base_climat_zones;0;0;Create;True;0;0;0;False;0;False;-1;b421e413094084f42833df252352b751;b421e413094084f42833df252352b751;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;31;-2127.136,118.2117;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;fe41fb262c955084ab44b9e21776a55e;fe41fb262c955084ab44b9e21776a55e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;25;-1404.368,-679.1791;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0.4;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-1223.792,-907.5831;Inherit;True;2;2;0;FLOAT2;0,0;False;1;COLOR;-0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;58;717.701,430.6213;Float;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/ClimatZonesShader;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;22;0;18;0
WireConnection;18;0;11;0
WireConnection;18;1;24;0
WireConnection;33;0;32;0
WireConnection;33;1;30;0
WireConnection;33;2;30;0
WireConnection;28;0;27;0
WireConnection;28;1;12;0
WireConnection;13;68;11;0
WireConnection;13;29;15;0
WireConnection;21;0;11;0
WireConnection;21;1;20;1
WireConnection;48;0;37;0
WireConnection;48;1;43;0
WireConnection;48;2;44;0
WireConnection;48;3;45;0
WireConnection;48;4;47;0
WireConnection;48;5;47;0
WireConnection;11;0;12;0
WireConnection;11;1;19;0
WireConnection;32;0;31;0
WireConnection;32;1;30;0
WireConnection;10;1;23;0
WireConnection;25;1;22;0
WireConnection;23;0;12;0
WireConnection;23;1;25;0
WireConnection;58;0;48;0
ASEEND*/
//CHKSM=72996220EBB40E03396005F901E29857E9B762DF