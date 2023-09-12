// Made with Amplify Shader Editor v1.9.1.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Vray2PBRMasks"
{
	Properties
	{
		_Fresnel("_Fresnel", Int) = 1
		_IOR("_IOR", Range( 0 , 10)) = 1
		_Spec_Color("_Spec_Color", Color) = (1,1,1,0)
		_SpecGlossMap("_SpecGlossMap", 2D) = "white" {}
		_Glossiness("_Glossiness", Range( 0 , 1)) = 1
		_SmoothMap("_SmoothMap", 2D) = "white" {}
		_Reflection_metalness("_Reflection_metalness", Range( 0 , 1)) = 1
		_Reflection_metalness_tex("_Reflection_metalness_tex", 2D) = "white" {}
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

			uniform float _Reflection_metalness;
			uniform sampler2D _Reflection_metalness_tex;
			uniform float4 _Reflection_metalness_tex_ST;
			uniform int _Fresnel;
			uniform float _IOR;
			uniform float _Glossiness;
			uniform sampler2D _SmoothMap;
			uniform float4 _SmoothMap_ST;
			uniform float4 _Spec_Color;
			uniform sampler2D _SpecGlossMap;
			uniform float4 _SpecGlossMap_ST;
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
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
				float2 uv_Reflection_metalness_tex = i.ase_texcoord1.xy * _Reflection_metalness_tex_ST.xy + _Reflection_metalness_tex_ST.zw;
				float3 hsvTorgb24 = RGBToHSV( tex2D( _Reflection_metalness_tex, uv_Reflection_metalness_tex ).rgb );
				float temp_output_25_0 = ( _Reflection_metalness * hsvTorgb24.z );
				int clampResult29 = clamp( _Fresnel , 0 , 1 );
				float lerpResult30 = lerp( (float)1 , temp_output_25_0 , (float)clampResult29);
				float clampResult32 = clamp( _IOR , 3.0 , 10.0 );
				float temp_output_33_0 = (0.0 + (clampResult32 - 3.0) * (1.0 - 0.0) / (10.0 - 3.0));
				float lerpResult34 = lerp( lerpResult30 , (float)1 , temp_output_33_0);
				float2 uv_SmoothMap = i.ase_texcoord1.xy * _SmoothMap_ST.xy + _SmoothMap_ST.zw;
				float3 hsvTorgb17 = RGBToHSV( tex2D( _SmoothMap, uv_SmoothMap ).rgb );
				float3 hsvTorgb19 = RGBToHSV( _Spec_Color.rgb );
				float2 uv_SpecGlossMap = i.ase_texcoord1.xy * _SpecGlossMap_ST.xy + _SpecGlossMap_ST.zw;
				float3 hsvTorgb20 = RGBToHSV( tex2D( _SpecGlossMap, uv_SpecGlossMap ).rgb );
				float temp_output_23_0 = ( (0.5 + (hsvTorgb19.z - 0.0) * (1.0 - 0.5) / (1.0 - 0.0)) * (0.9 + (hsvTorgb20.z - 0.0) * (1.0 - 0.9) / (1.0 - 0.0)) );
				float lerpResult26 = lerp( temp_output_23_0 , (float)1 , temp_output_25_0);
				float temp_output_28_0 = min( ( _Glossiness * hsvTorgb17.z ) , lerpResult26 );
				float4 appendResult42 = (float4(lerpResult34 , (float)1 , (float)1 , temp_output_28_0));
				
				
				finalColor = appendResult42;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19102
Node;AmplifyShaderEditor.SamplerNode;4;-1764.184,-356.6419;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-1748.708,589.244;Inherit;True;Property;_TextureSample2;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-1765.601,88.15426;Inherit;True;Property;_TextureSample1;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-1725.247,1066.404;Inherit;True;Property;_TextureSample3;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-1998.77,-553.0157;Inherit;False;Property;_BaseColor;_BaseColor;0;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-1982.555,-30.16669;Inherit;False;Property;_Glossiness;_Glossiness;6;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;7;-1999.635,87.98147;Inherit;True;Property;_SmoothMap;_SmoothMap;7;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;5;-1998.217,-356.8147;Inherit;True;Property;_BaseColorMap;_BaseColorMap;1;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RGBToHSVNode;17;-1462.798,94.89685;Inherit;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RGBToHSVNode;19;-1431.984,362.2353;Inherit;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TFHCRemapNode;21;-1125.711,387.3178;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;22;-1129.671,601.1804;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.9;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;20;-1433.304,578.7383;Inherit;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RGBToHSVNode;24;-1382.928,1050.186;Inherit;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-887.9967,-550.4492;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-879.6441,-17.89618;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-883.5892,493.8797;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-873.7212,952.4578;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;13;-1984.618,1066.231;Inherit;True;Property;_Reflection_metalness_tex;_Reflection_metalness_tex;9;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;26;-455.0068,1026.025;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1967.538,948.0828;Inherit;False;Property;_Reflection_metalness;_Reflection_metalness;8;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;29;-527.8674,495.9475;Inherit;False;3;0;INT;0;False;1;INT;0;False;2;INT;1;False;1;INT;0
Node;AmplifyShaderEditor.LerpOp;30;-235.0254,613.2062;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;14;-679.4867,492.8641;Inherit;False;Property;_Fresnel;_Fresnel;2;0;Create;True;0;0;0;False;0;False;1;0;False;0;1;INT;0
Node;AmplifyShaderEditor.TFHCRemapNode;33;395.5108,499.667;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;3;False;2;FLOAT;10;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-30.35783,511.7524;Inherit;False;Property;_IOR;_IOR;3;0;Create;True;0;0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;32;248.6106,516.5673;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;3;False;2;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;35;-619.7859,-67.13083;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;37;-653.5858,-254.3296;Inherit;False;Constant;_Color0;Color 0;10;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;36;-337.6848,-254.3302;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;38;-2.284757,-285.5295;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StickyNoteNode;41;1445.225,65.73065;Inherit;False;263.1;279.4001;MaskMap;HDRP Maskmap;1,1,1,1;;0;0
Node;AmplifyShaderEditor.StickyNoteNode;43;846.379,12.17741;Inherit;False;200.7;200.1;Base Color;All Render pipelines Base Color;1,1,1,1;;0;0
Node;AmplifyShaderEditor.StickyNoteNode;44;730.2051,468.3899;Inherit;False;200.7;200.1;Metallic;;1,1,1,1;;0;0
Node;AmplifyShaderEditor.StickyNoteNode;45;675.0151,928.6701;Inherit;False;200.7;200.1;Smoothness;;1,1,1,1;;0;0
Node;AmplifyShaderEditor.SimpleMinOpNode;28;720.2845,989.9533;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;39;572.6174,-2.130071;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;27;-678.2045,963.8832;Inherit;False;Constant;_Int1;Int 1;10;0;Create;True;0;0;0;False;0;False;1;0;False;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;46;-439.0855,641.3705;Inherit;False;Constant;_Int2;Int 1;10;0;Create;True;0;0;0;False;0;False;1;0;False;0;1;INT;0
Node;AmplifyShaderEditor.LerpOp;34;750.4102,512.6675;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;47;591.8153,606.27;Inherit;False;Constant;_Int3;Int 1;10;0;Create;True;0;0;0;False;0;False;1;0;False;0;1;INT;0
Node;AmplifyShaderEditor.StickyNoteNode;49;1446.524,483.0303;Inherit;False;263.1;279.4001;MaskMap;URP and BuiltIn Metallic;1,1,1,1;;0;0
Node;AmplifyShaderEditor.IntNode;48;1282.724,155.4307;Inherit;False;Constant;_Int4;Int 1;10;0;Create;True;0;0;0;False;0;False;1;0;False;0;1;INT;0
Node;AmplifyShaderEditor.DynamicAppendNode;42;1479.023,109.9308;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;50;1481.623,527.2307;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;40;875.2797,64.17722;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;10;-1982.743,589.0712;Inherit;True;Property;_SpecGlossMap;_SpecGlossMap;5;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;2;-1974.741,390.0279;Inherit;False;Property;_Spec_Color;_Spec_Color;4;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;51;1471.673,890.9661;Float;False;True;-1;2;ASEMaterialInspector;100;5;Vray2PBRMasks;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;;0;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
WireConnection;4;0;5;0
WireConnection;9;0;10;0
WireConnection;8;0;7;0
WireConnection;12;0;13;0
WireConnection;17;0;8;0
WireConnection;19;0;2;0
WireConnection;21;0;19;3
WireConnection;22;0;20;3
WireConnection;20;0;9;0
WireConnection;24;0;12;0
WireConnection;16;0;1;0
WireConnection;16;1;4;0
WireConnection;18;0;6;0
WireConnection;18;1;17;3
WireConnection;23;0;21;0
WireConnection;23;1;22;0
WireConnection;25;0;11;0
WireConnection;25;1;24;3
WireConnection;26;0;23;0
WireConnection;26;1;27;0
WireConnection;26;2;25;0
WireConnection;29;0;14;0
WireConnection;30;0;46;0
WireConnection;30;1;25;0
WireConnection;30;2;29;0
WireConnection;33;0;32;0
WireConnection;32;0;15;0
WireConnection;35;0;23;0
WireConnection;36;0;16;0
WireConnection;36;1;37;0
WireConnection;36;2;35;0
WireConnection;38;0;36;0
WireConnection;38;1;16;0
WireConnection;38;2;29;0
WireConnection;28;0;18;0
WireConnection;28;1;26;0
WireConnection;39;0;38;0
WireConnection;39;1;36;0
WireConnection;39;2;33;0
WireConnection;34;0;30;0
WireConnection;34;1;47;0
WireConnection;34;2;33;0
WireConnection;42;0;34;0
WireConnection;42;1;48;0
WireConnection;42;2;48;0
WireConnection;42;3;28;0
WireConnection;50;0;34;0
WireConnection;50;1;34;0
WireConnection;50;2;34;0
WireConnection;50;3;28;0
WireConnection;40;0;39;0
WireConnection;40;1;16;0
WireConnection;40;2;25;0
WireConnection;51;0;42;0
ASEEND*/
//CHKSM=1AAC3C5186B9EE58B2F216F1DDB0465C55477620