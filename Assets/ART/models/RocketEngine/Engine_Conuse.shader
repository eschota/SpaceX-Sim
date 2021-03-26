// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Antonio/Engine_Conuse"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15.5
		_Speed("Speed", Float) = 1
		_Float3("Float 3", Float) = 3
		_Color_back("Color_back", Color) = (0.9339623,0.6564341,0.1982467,0)
		_Float4("Float 4", Float) = 0.49
		_Texture0("Texture 0", 2D) = "white" {}
		_Texture2("Texture 2", 2D) = "white" {}
		_Texture1("Texture 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		Blend One Zero , SrcAlpha OneMinusSrcAlpha
		BlendOp Add , Add
		AlphaToMask On
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture1;
		uniform float4 _Texture1_ST;
		uniform float4 _Color_back;
		uniform sampler2D _Texture0;
		uniform float _Speed;
		uniform sampler2D _Texture2;
		uniform float _Float3;
		uniform float _Float4;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture1 = i.uv_texcoord * _Texture1_ST.xy + _Texture1_ST.zw;
			float2 temp_cast_0 = (_Speed).xx;
			float2 uv_TexCoord9 = i.uv_texcoord * float2( 1,4.39 );
			float2 panner8 = ( 1.0 * _Time.y * temp_cast_0 + uv_TexCoord9);
			float4 temp_output_15_0 = ( tex2D( _Texture0, panner8 ) * 0.1 );
			float4 blendOpSrc11 = _Color_back;
			float4 blendOpDest11 = temp_output_15_0;
			float4 lerpBlendMode11 = lerp(blendOpDest11, (( blendOpSrc11 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpSrc11 - 0.5 ) ) * ( 1.0 - blendOpDest11 ) ) : ( 2.0 * blendOpSrc11 * blendOpDest11 ) ),0.5);
			float4 blendOpSrc20 = tex2D( _Texture1, uv_Texture1 );
			float4 blendOpDest20 = ( saturate( lerpBlendMode11 ));
			float4 lerpBlendMode20 = lerp(blendOpDest20, (( blendOpSrc20 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpSrc20 - 0.5 ) ) * ( 1.0 - blendOpDest20 ) ) : ( 2.0 * blendOpSrc20 * blendOpDest20 ) ),0.5);
			float2 temp_cast_1 = (_Float3).xx;
			float2 panner36 = ( 1.0 * _Time.y * temp_cast_1 + float2( 0,0 ));
			float4 appendResult39 = (float4(0.0 , panner36.x , 0.0 , 0.0));
			float2 uv_TexCoord35 = i.uv_texcoord * float2( 2,2 ) + appendResult39.xy;
			float4 blendOpSrc42 = ( saturate( lerpBlendMode20 ));
			float4 blendOpDest42 = ( tex2D( _Texture2, uv_TexCoord35 ) * _Float4 );
			float4 temp_output_42_0 = ( saturate( ( 1.0 - ( 1.0 - blendOpSrc42 ) * ( 1.0 - blendOpDest42 ) ) ));
			o.Albedo = temp_output_42_0.rgb;
			o.Emission = ( temp_output_42_0 * 1.36 ).rgb;
			float4 color14 = IsGammaSpace() ? float4(0.6415094,0.6415094,0.6415094,0) : float4(0.3691636,0.3691636,0.3691636,0);
			float4 blendOpSrc13 = color14;
			float4 blendOpDest13 = temp_output_15_0;
			o.Alpha = ( saturate(  (( blendOpSrc13 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpSrc13 - 0.5 ) ) * ( 1.0 - blendOpDest13 ) ) : ( 2.0 * blendOpSrc13 * blendOpDest13 ) ) )).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			AlphaToMask Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
0;154;1318;995;708.6608;659.2559;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;34;-497.9175,-1302.606;Inherit;False;Property;_Float3;Float 3;7;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;36;-281.9434,-1323.321;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-703.7071,-390.5013;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,4.39;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-651.336,-207.7407;Inherit;False;Property;_Speed;Speed;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;8;-459.8636,-309.3354;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;38;-91.73969,-1322.671;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TexturePropertyNode;1;-706.5042,-588.4821;Inherit;True;Property;_Texture0;Texture 0;10;0;Create;True;0;0;0;False;0;False;02f71419854c0d845a930c9e0a0bf775;0158973838fc4c84a9209105f979a780;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;12;-96.34963,-124.724;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;39;47.21178,-1345.127;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;2;-227.9164,-337.7123;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;21;200.9482,-1009.92;Inherit;True;Property;_Texture1;Texture 1;12;0;Create;True;0;0;0;False;0;False;02f71419854c0d845a930c9e0a0bf775;f4d98881c3ab0274dad227a6f1bb8e66;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;10;116.5748,-769.4983;Inherit;False;Property;_Color_back;Color_back;8;0;Create;True;0;0;0;False;0;False;0.9339623,0.6564341,0.1982467,0;0.9339623,0.6564341,0.1982467,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;128.1929,-323.6534;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;196.1436,-1392.226;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;32;180.7515,-1588.108;Inherit;True;Property;_Texture2;Texture 2;11;0;Create;True;0;0;0;False;0;False;02f71419854c0d845a930c9e0a0bf775;aafb42eeeb90de44e9745f0492821c9c;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.BlendOpsNode;11;503.3761,-769.0288;Inherit;True;HardLight;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.5;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;44;545.5955,-1115.076;Inherit;False;Property;_Float4;Float 4;9;0;Create;True;0;0;0;False;0;False;0.49;0.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;33;420.1022,-1421.016;Inherit;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;468.1467,-1011.581;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;20;825.0341,-907.7847;Inherit;True;HardLight;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.5;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;759.8077,-1424.729;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;42;1099.89,-909.8666;Inherit;True;Screen;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;17;1144.566,-688.8546;Inherit;False;Constant;_IntensiveEmmisive;IntensiveEmmisive;2;0;Create;True;0;0;0;False;0;False;1.36;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;119.016,-505.9073;Inherit;False;Constant;_Color1;Color 1;2;0;Create;True;0;0;0;False;0;False;0.6415094,0.6415094,0.6415094,0;0.6415094,0.6415094,0.6415094,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;13;505.1934,-364.2278;Inherit;True;HardLight;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;1423.674,-809.7477;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1652.182,-900.1844;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Antonio/Engine_Conuse;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15.5;10;25;False;0.5;True;0;0;False;-1;0;False;-1;2;5;False;-1;10;False;-1;0;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;36;2;34;0
WireConnection;8;0;9;0
WireConnection;8;2;7;0
WireConnection;38;0;36;0
WireConnection;39;1;38;0
WireConnection;2;0;1;0
WireConnection;2;1;8;0
WireConnection;15;0;2;0
WireConnection;15;1;12;0
WireConnection;35;1;39;0
WireConnection;11;0;10;0
WireConnection;11;1;15;0
WireConnection;33;0;32;0
WireConnection;33;1;35;0
WireConnection;22;0;21;0
WireConnection;20;0;22;0
WireConnection;20;1;11;0
WireConnection;43;0;33;0
WireConnection;43;1;44;0
WireConnection;42;0;20;0
WireConnection;42;1;43;0
WireConnection;13;0;14;0
WireConnection;13;1;15;0
WireConnection;45;0;42;0
WireConnection;45;1;17;0
WireConnection;0;0;42;0
WireConnection;0;2;45;0
WireConnection;0;9;13;0
ASEEND*/
//CHKSM=905AC5F0A9E830582648716DE8D4B243B519DB49