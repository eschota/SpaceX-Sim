// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "EarthShader"
{
	Properties
	{
		_NightMap("NightMap", 2D) = "white" {}
		_EarthMap("EarthMap", 2D) = "white" {}
		_NightMult("NightMult", Float) = 5
		_Rough("Rough", Float) = 1.27
		_SunMask("SunMask", Float) = 5
		_Height("Height", 2D) = "bump" {}
		_OceanInverted("OceanInverted", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			half2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Height;
		uniform half4 _Height_ST;
		uniform sampler2D _NightMap;
		uniform half _NightMult;
		uniform sampler2D _EarthMap;
		uniform half _SunMask;
		uniform sampler2D _OceanInverted;
		uniform half4 _OceanInverted_ST;
		uniform half _Rough;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Height = i.uv_texcoord * _Height_ST.xy + _Height_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Height, uv_Height ) );
			float4 temp_output_18_0 = ( tex2D( _NightMap, i.uv_texcoord ) * _NightMult );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			half3 ase_worldNormal = WorldNormalVector( i, half3( 0, 0, 1 ) );
			float dotResult17 = dot( ase_worldlightDir , ase_worldNormal );
			half4 temp_cast_0 = (dotResult17).xxxx;
			float4 temp_output_8_0 = CalculateContrast(_SunMask,temp_cast_0);
			float4 lerpResult4 = lerp( temp_output_18_0 , tex2D( _EarthMap, i.uv_texcoord ) , temp_output_8_0);
			half4 temp_cast_1 = (500.0).xxxx;
			float4 clampResult36 = clamp( lerpResult4 , float4( 0,0,0,0 ) , temp_cast_1 );
			o.Albedo = clampResult36.rgb;
			half4 temp_cast_3 = (500.0).xxxx;
			float4 clampResult30 = clamp( ( ( 1.0 - temp_output_8_0 ) * temp_output_18_0 ) , float4( 0,0,0,0 ) , temp_cast_3 );
			o.Emission = clampResult30.rgb;
			float2 uv_OceanInverted = i.uv_texcoord * _OceanInverted_ST.xy + _OceanInverted_ST.zw;
			float4 temp_output_26_0 = ( tex2D( _OceanInverted, uv_OceanInverted ) + 0.2 );
			float4 clampResult32 = clamp( ( temp_output_26_0 * 0.1 ) , float4( 0,0,0,0 ) , float4( 1,0.9590455,0.8066038,0 ) );
			o.Metallic = clampResult32.r;
			float4 clampResult34 = clamp( ( temp_output_26_0 * _Rough ) , float4( 0.3207547,0.3207547,0.3207547,0 ) , float4( 0.8018868,0.8018868,0.8018868,0 ) );
			o.Smoothness = clampResult34.r;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16900
80.44444;518.2222;719;401;249.8695;423.1559;2.023874;True;False
Node;AmplifyShaderEditor.WorldNormalVector;16;768.5679,599.3965;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;15;726.1146,433.2439;Float;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-548.3547,204.4862;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;17;1024.401,487.1647;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;62.93933,385.7089;Float;False;Property;_SunMask;SunMask;4;0;Create;True;0;0;False;0;5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;19.13311,-420.3526;Float;False;Constant;_OceanAdd;OceanAdd;5;0;Create;True;0;0;False;0;0.2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;190.3777,-538.7404;Float;True;Property;_OceanInverted;OceanInverted;6;0;Create;True;0;0;False;0;2dee9f6d59a2f3d4f83294106fc55f21;2dee9f6d59a2f3d4f83294106fc55f21;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-232.7833,252.0394;Float;True;Property;_NightMap;NightMap;0;0;Create;True;0;0;False;0;1acca5db1f29cce49a7405a590cd8a5a;1acca5db1f29cce49a7405a590cd8a5a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;8;239.2065,248.7708;Float;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-103.6212,187.1425;Float;False;Property;_NightMult;NightMult;2;0;Create;True;0;0;False;0;5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-321.1066,26.49005;Float;True;Property;_EarthMap;EarthMap;1;0;Create;True;0;0;False;0;991c4f70204a84e4ebd88a0e6eb4249c;991c4f70204a84e4ebd88a0e6eb4249c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;26;503.3034,-485.9651;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;28;64.75366,-301.325;Float;False;Constant;_OceanMult;OceanMult;4;0;Create;True;0;0;False;0;0.1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;86.57549,-172.8801;Float;False;Property;_Rough;Rough;3;0;Create;True;0;0;False;0;1.27;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;21;516.9465,252.6506;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;189.3147,10.3228;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;4;303.5095,-300.9599;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;431.9395,29.1408;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;432.3998,-189.405;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;31;795.0397,242.7914;Float;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;500;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;745.5881,-482.055;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;32;951.2338,-453.6052;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0.9590455,0.8066038,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;5;-76.26315,514.1739;Float;False;Constant;_Color1;Color 1;0;0;Create;True;0;0;False;0;0.112679,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;188.5643,502.6982;Float;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;23;237.2401,758.8596;Float;True;Property;_Height;Height;5;0;Create;True;0;0;False;0;af3c061b8dd6f7041a065811cc087e7c;af3c061b8dd6f7041a065811cc087e7c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;34;645.5392,-89.4369;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.3207547,0.3207547,0.3207547,0;False;2;COLOR;0.8018868,0.8018868,0.8018868,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;30;724.4667,56.6922;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;36;790.4016,-226.8401;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1026.199,-223.5494;Half;False;True;2;Half;ASEMaterialInspector;0;0;Standard;EarthShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;9;1;12;0
WireConnection;8;1;17;0
WireConnection;8;0;22;0
WireConnection;10;1;12;0
WireConnection;26;0;24;0
WireConnection;26;1;29;0
WireConnection;21;0;8;0
WireConnection;18;0;9;0
WireConnection;18;1;19;0
WireConnection;4;0;18;0
WireConnection;4;1;10;0
WireConnection;4;2;8;0
WireConnection;20;0;21;0
WireConnection;20;1;18;0
WireConnection;33;0;26;0
WireConnection;33;1;25;0
WireConnection;27;0;26;0
WireConnection;27;1;28;0
WireConnection;32;0;27;0
WireConnection;34;0;33;0
WireConnection;30;0;20;0
WireConnection;30;2;31;0
WireConnection;36;0;4;0
WireConnection;36;2;31;0
WireConnection;0;0;36;0
WireConnection;0;1;23;0
WireConnection;0;2;30;0
WireConnection;0;3;32;0
WireConnection;0;4;34;0
ASEEND*/
//CHKSM=8878C3EA90C6227C4348B64329D11F02DF9BD515