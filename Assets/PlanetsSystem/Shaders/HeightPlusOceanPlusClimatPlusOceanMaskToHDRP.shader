// Upgrade NOTE: upgraded instancing buffer 'HeightPlusOceanPlusClimatPlusOceanMaskToHDRP' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HeightPlusOceanPlusClimatPlusOceanMaskToHDRP"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_OceanLevel("OceanLevel", Range( 0 , 1)) = 0.2588235
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

			uniform sampler2D _TextureSample0;
			UNITY_INSTANCING_BUFFER_START(HeightPlusOceanPlusClimatPlusOceanMaskToHDRP)
				UNITY_DEFINE_INSTANCED_PROP(float4, _TextureSample0_ST)
#define _TextureSample0_ST_arr HeightPlusOceanPlusClimatPlusOceanMaskToHDRP
				UNITY_DEFINE_INSTANCED_PROP(float, _OceanLevel)
#define _OceanLevel_arr HeightPlusOceanPlusClimatPlusOceanMaskToHDRP
			UNITY_INSTANCING_BUFFER_END(HeightPlusOceanPlusClimatPlusOceanMaskToHDRP)

			
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
				float4 _TextureSample0_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_TextureSample0_ST_arr, _TextureSample0_ST);
				float2 uv_TextureSample0 = i.ase_texcoord1.xy * _TextureSample0_ST_Instance.xy + _TextureSample0_ST_Instance.zw;
				float _OceanLevel_Instance = UNITY_ACCESS_INSTANCED_PROP(_OceanLevel_arr, _OceanLevel);
				float4 temp_cast_0 = (_OceanLevel_Instance).xxxx;
				float4 clampResult3 = clamp( tex2D( _TextureSample0, uv_TextureSample0 ) , temp_cast_0 , float4( 1,1,1,0 ) );
				float4 temp_cast_1 = (_OceanLevel_Instance).xxxx;
				float4 temp_cast_2 = (_OceanLevel_Instance).xxxx;
				
				
				finalColor = (float4( 0,0,0,0 ) + (clampResult3 - temp_cast_1) * (float4( 1,1,1,1 ) - float4( 0,0,0,0 )) / (temp_cast_2 - temp_cast_1));
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18912
6;5.5;1908;1013.5;1010;506.5;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;2;-596,118.25;Inherit;False;InstancedProperty;_OceanLevel;OceanLevel;1;0;Create;True;0;0;0;True;0;False;0.2588235;0.2117647;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-589,-211.75;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;fe41fb262c955084ab44b9e21776a55e;fe41fb262c955084ab44b9e21776a55e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;3;-161,-66.75;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;6;-66,176.75;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;241,-39;Float;False;True;-1;2;ASEMaterialInspector;100;1;HeightPlusOceanPlusClimatPlusOceanMaskToHDRP;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;6;0;3;0
WireConnection;6;1;2;0
WireConnection;6;2;2;0
WireConnection;0;0;6;0
ASEEND*/
//CHKSM=90F7B8B387F7806DA597552323492E5EE33C85A6