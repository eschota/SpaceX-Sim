// Upgrade NOTE: upgraded instancing buffer 'UnlitClimatZonesShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/ClimatZonesShader"
{
	Properties
	{
		_OceanLevel1("OceanLevel", Range( 0 , 1)) = 0.2588235
		_RenderID("RenderID", Int) = 4

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

			uniform int _RenderID;
			UNITY_INSTANCING_BUFFER_START(UnlitClimatZonesShader)
				UNITY_DEFINE_INSTANCED_PROP(float, _OceanLevel1)
#define _OceanLevel1_arr UnlitClimatZonesShader
			UNITY_INSTANCING_BUFFER_END(UnlitClimatZonesShader)

			
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
				float _OceanLevel1_Instance = UNITY_ACCESS_INSTANCED_PROP(_OceanLevel1_arr, _OceanLevel1);
				float4 color43 = IsGammaSpace() ? float4(1,0,0,0) : float4(1,0,0,0);
				float4 color44 = IsGammaSpace() ? float4(0,1,0.05708027,0) : float4(0,1,0.004603244,0);
				float ifLocalVar38 = 0;
				float4 lerpResult35 = lerp( color43 , color44 , ifLocalVar38);
				float4 color45 = IsGammaSpace() ? float4(0,0.0377028,1,0) : float4(0,0.002918173,1,0);
				float4 lerpResult39 = lerp( lerpResult35 , color45 , ifLocalVar38);
				float4 color46 = IsGammaSpace() ? float4(1,0,0.786839,0) : float4(1,0,0.58176,0);
				float4 lerpResult40 = lerp( lerpResult39 , color46 , ifLocalVar38);
				float4 lerpResult41 = lerp( lerpResult40 , float4( 0,0,0,0 ) , ifLocalVar38);
				float4 color47 = IsGammaSpace() ? float4(1,0.7942005,0.004716992,0) : float4(1,0.5940443,0.0003650923,0);
				float4 lerpResult42 = lerp( lerpResult41 , color47 , ifLocalVar38);
				
				
				finalColor = lerpResult42;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18912
51;126.5;1908;1013.5;578.7546;28.6702;1.183493;True;True
Node;AmplifyShaderEditor.IntNode;37;127.6762,475.766;Inherit;False;Property;_RenderID;RenderID;5;0;Create;True;0;0;0;False;0;False;4;0;False;0;1;INT;0
Node;AmplifyShaderEditor.ConditionalIfNode;38;355.7743,443.8112;Inherit;False;False;5;0;INT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;44;-107.231,233.0173;Inherit;False;Constant;_Color2;Color 0;6;0;Create;True;0;0;0;False;0;False;0,1,0.05708027,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;43;-110.7979,58.23485;Inherit;False;Constant;_Color0;Color 0;6;0;Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;35;604.0932,149.1057;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;45;-109.0144,406.0162;Inherit;False;Constant;_Color3;Color 0;6;0;Create;True;0;0;0;False;0;False;0,0.0377028,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;46;-107.2309,575.4481;Inherit;False;Constant;_Color4;Color 0;6;0;Create;True;0;0;0;False;0;False;1,0,0.786839,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;39;605.1551,268.121;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;40;605.1552,377.2684;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;41;605.1555,493.9954;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;47;-107.2309,739.5298;Inherit;False;Constant;_Color5;Color 0;6;0;Create;True;0;0;0;False;0;False;1,0.7942005,0.004716992,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;31;-2127.136,118.2117;Inherit;True;Property;_TextureSample1;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;fe41fb262c955084ab44b9e21776a55e;fe41fb262c955084ab44b9e21776a55e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;32;-1699.137,263.2117;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-1629.583,-679.7056;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;-0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;10;-990.8414,-1084.818;Inherit;True;Property;_base_climat_zones;base_climat_zones;0;0;Create;True;0;0;0;False;0;False;-1;b421e413094084f42833df252352b751;b421e413094084f42833df252352b751;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;11;-2016.09,-818.4226;Inherit;True;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;22;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-1188.883,-414.506;Inherit;True;Property;_seamlessMultMask;seamlessMultMask;4;0;Create;True;0;0;0;False;0;False;-1;d8b1171af0931f34cac23763fc72ff3c;d8b1171af0931f34cac23763fc72ff3c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;25;-1404.368,-679.1791;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0.4;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2134.136,448.2117;Inherit;False;InstancedProperty;_OceanLevel1;OceanLevel;3;0;Create;True;0;0;0;True;0;False;0.2588235;0.2117647;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-1223.792,-907.5831;Inherit;True;2;2;0;FLOAT2;0,0;False;1;COLOR;-0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1478.783,-982.6057;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientNode;27;-1484.302,-1403.745;Inherit;False;0;3;2;1,1,1,0;0,0,0,0.5000076;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;12;-1801.588,-1346.202;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;20;-2089.783,-565.3056;Inherit;False;Constant;_Color1;Color 1;2;0;Create;True;0;0;0;False;0;False;1,1,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1856.861,-290.8534;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StickyNoteNode;29;-2155.282,-1452.839;Inherit;False;1491.642;1402.234;ClimatZones;;1,1,1,1;;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1785.583,-513.3057;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StickyNoteNode;34;-2195.441,43.39172;Inherit;False;1491.642;1402.234;OceanAndDisplace;;1,1,1,1;;0;0
Node;AmplifyShaderEditor.LerpOp;42;602.1233,607.6908;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;33;-1604.137,506.7117;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1192.503,-1231.328;Inherit;True;2;2;0;OBJECT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1970.183,-985.2058;Inherit;False;Property;_noise;noise;2;0;Create;True;0;0;0;False;0;False;3.67;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;13;-1620.89,-295.8221;Inherit;False;RadialUVDistortion;-1;;4;051d65e7699b41a4c800363fd0e822b2;0;7;60;SAMPLER2D;_Sampler6013;False;1;FLOAT2;10,10;False;11;FLOAT2;1,1;False;65;FLOAT;0.01;False;68;FLOAT2;0.9,0.76;False;47;FLOAT2;0,0;False;29;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;17;1087.701,629.6213;Float;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/ClimatZonesShader;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;38;0;37;0
WireConnection;35;0;43;0
WireConnection;35;1;44;0
WireConnection;35;2;38;0
WireConnection;39;0;35;0
WireConnection;39;1;45;0
WireConnection;39;2;38;0
WireConnection;40;0;39;0
WireConnection;40;1;46;0
WireConnection;40;2;38;0
WireConnection;41;0;40;0
WireConnection;41;2;38;0
WireConnection;32;0;31;0
WireConnection;32;1;30;0
WireConnection;22;0;18;0
WireConnection;10;1;23;0
WireConnection;11;0;12;0
WireConnection;11;1;19;0
WireConnection;25;1;22;0
WireConnection;23;0;12;0
WireConnection;23;1;25;0
WireConnection;18;0;11;0
WireConnection;18;1;24;0
WireConnection;21;0;11;0
WireConnection;21;1;20;1
WireConnection;42;0;41;0
WireConnection;42;1;47;0
WireConnection;42;2;38;0
WireConnection;33;0;32;0
WireConnection;33;1;30;0
WireConnection;33;2;30;0
WireConnection;28;0;27;0
WireConnection;28;1;12;0
WireConnection;13;68;11;0
WireConnection;13;29;15;0
WireConnection;17;0;42;0
ASEEND*/
//CHKSM=D4042A73881513EBDDAB95C342A0EA93D8AFA614