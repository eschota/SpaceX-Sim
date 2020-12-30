// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "EarthShader"
{
	Properties
	{
		_EarthMap("EarthMap", 2D) = "white" {}
		_NightMap("NightMap", 2D) = "white" {}
		_NightMult("NightMult", Range( 0 , 10)) = 1
		_DayNightContrast("DayNight Contrast", Range( 0 , 50)) = 5
		_WaterMask("WaterMask", 2D) = "white" {}
		_EarthNormalMap("EarthNormalMap", 2D) = "bump" {}
		_WaterNormal("WaterNormal", Range( 0 , 1)) = 0.03
		_LandNormal("LandNormal", Range( 0 , 1)) = 0.03
		_WaterSmoothness("WaterSmoothness", Range( 0 , 1)) = 0.7
		_LandSmoothness("LandSmoothness", Range( 0 , 1)) = 0.4
		_AtmosphereColor("AtmosphereColor", Color) = (0,0,0,0)
		_AtmospherePower("AtmospherePower", Range( 0 , 10)) = 3
		_AtmosphereScale("AtmosphereScale", Range( 0 , 10)) = 1.5
		_AtmosphereDarkSide("AtmosphereDarkSide", Range( 0 , 1)) = 0.35
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
			float2 uv_texcoord;
			float3 worldPos;
			half3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _EarthNormalMap;
		uniform half4 _EarthNormalMap_ST;
		uniform float _LandNormal;
		uniform float _WaterNormal;
		uniform sampler2D _WaterMask;
		uniform half4 _WaterMask_ST;
		uniform sampler2D _NightMap;
		uniform half4 _NightMap_ST;
		uniform float _NightMult;
		uniform sampler2D _EarthMap;
		uniform half4 _EarthMap_ST;
		uniform float _DayNightContrast;
		uniform half _AtmosphereScale;
		uniform half _AtmospherePower;
		uniform half4 _AtmosphereColor;
		uniform half _AtmosphereDarkSide;
		uniform float _LandSmoothness;
		uniform float _WaterSmoothness;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _Color0 = float4(0,0,1,0);
			float2 uv_EarthNormalMap = i.uv_texcoord * _EarthNormalMap_ST.xy + _EarthNormalMap_ST.zw;
			half3 tex2DNode23 = UnpackNormal( tex2D( _EarthNormalMap, uv_EarthNormalMap ) );
			half4 lerpResult49 = lerp( _Color0 , half4( tex2DNode23 , 0.0 ) , _LandNormal);
			half4 lerpResult47 = lerp( _Color0 , half4( tex2DNode23 , 0.0 ) , _WaterNormal);
			float2 uv_WaterMask = i.uv_texcoord * _WaterMask_ST.xy + _WaterMask_ST.zw;
			half4 tex2DNode24 = tex2D( _WaterMask, uv_WaterMask );
			half4 lerpResult51 = lerp( lerpResult49 , lerpResult47 , tex2DNode24);
			o.Normal = lerpResult51.rgb;
			float2 uv_NightMap = i.uv_texcoord * _NightMap_ST.xy + _NightMap_ST.zw;
			half4 temp_output_18_0 = ( tex2D( _NightMap, uv_NightMap ) * _NightMult );
			float2 uv_EarthMap = i.uv_texcoord * _EarthMap_ST.xy + _EarthMap_ST.zw;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			half3 ase_worldlightDir = 0;
			#else //aseld
			half3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			half3 ase_worldNormal = WorldNormalVector( i, half3( 0, 0, 1 ) );
			half dotResult17 = dot( ase_worldlightDir , ase_worldNormal );
			half4 temp_cast_3 = (dotResult17).xxxx;
			half4 clampResult43 = clamp( CalculateContrast(_DayNightContrast,temp_cast_3) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			half4 lerpResult4 = lerp( temp_output_18_0 , tex2D( _EarthMap, uv_EarthMap ) , clampResult43);
			o.Albedo = lerpResult4.rgb;
			half3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			half fresnelNdotV57 = dot( ase_worldNormal, ase_worldViewDir );
			half fresnelNode57 = ( 0.0 + _AtmosphereScale * pow( 1.0 - fresnelNdotV57, _AtmospherePower ) );
			half4 temp_output_63_0 = ( fresnelNode57 * _AtmosphereColor );
			half4 lerpResult65 = lerp( temp_output_63_0 , ( dotResult17 * temp_output_63_0 ) , ( 1.0 - _AtmosphereDarkSide ));
			o.Emission = ( ( ( 1.0 - clampResult43 ) * temp_output_18_0 ) + lerpResult65 ).rgb;
			half lerpResult54 = lerp( _LandSmoothness , _WaterSmoothness , tex2DNode24.r);
			o.Smoothness = lerpResult54;
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
Version=18712
169;251;1424;708;-1711.405;362.9292;1;True;False
Node;AmplifyShaderEditor.WorldNormalVector;16;-185.1312,-555.8136;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;15;-194.0422,-744.1138;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;17;73.96294,-659.9844;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-0.2740768,-411.0257;Float;False;Property;_DayNightContrast;DayNight Contrast;3;0;Create;True;0;0;0;False;0;False;5;3;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;1148.682,-216.53;Inherit;False;Property;_AtmospherePower;AtmospherePower;11;0;Create;True;0;0;0;False;0;False;3;3;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;1141.782,-309.7791;Inherit;False;Property;_AtmosphereScale;AtmosphereScale;12;0;Create;True;0;0;0;False;0;False;1.5;1.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;62;1154.534,-105.8389;Inherit;False;Property;_AtmosphereColor;AtmosphereColor;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.240566,0.6392689,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;57;1474.291,-346.0873;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;8;385.9743,-672.3835;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;1748.695,-218.5221;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;19;530.6058,-772.3463;Float;False;Property;_NightMult;NightMult;2;0;Create;True;0;0;0;False;0;False;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;478.6091,-978.6226;Inherit;True;Property;_NightMap;NightMap;1;0;Create;True;0;0;0;False;0;False;-1;None;14abb0596b07d1f41b3c28fc41d0b5a7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;1518.295,18.21486;Inherit;False;Property;_AtmosphereDarkSide;AtmosphereDarkSide;13;0;Create;True;0;0;0;False;0;False;0.35;0.4;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;43;763.9479,-578.6563;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;1061.933,-855.17;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;3;485.8174,392.9316;Float;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;0;False;0;False;0,0,1,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;23;440.1975,611.9122;Inherit;True;Property;_EarthNormalMap;EarthNormalMap;5;0;Create;True;0;0;0;False;0;False;-1;None;1db77afdc9b714c49a4728890de35977;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;21;1015.843,-614.9991;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;50;479.0299,946.8795;Float;False;Property;_LandNormal;LandNormal;7;0;Create;True;0;0;0;False;0;False;0.03;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;480.3878,827.3744;Float;False;Property;_WaterNormal;WaterNormal;6;0;Create;True;0;0;0;False;0;False;0.03;0.03;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;67;1819.773,33.99439;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;1833.928,-71.21263;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;53;1040.786,209.9158;Float;False;Property;_LandSmoothness;LandSmoothness;9;0;Create;True;0;0;0;False;0;False;0.4;0.4;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;903.4131,339.4816;Inherit;True;Property;_WaterMask;WaterMask;4;0;Create;True;0;0;0;False;0;False;-1;None;1f014093a422aa94683d2581a9d9019b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;47;1126.833,595.6949;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.567;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;49;1118.932,777.2336;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;1042.162,91.05327;Float;False;Property;_WaterSmoothness;WaterSmoothness;8;0;Create;True;0;0;0;False;0;False;0.7;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;609.1972,-1259.141;Inherit;True;Property;_EarthMap;EarthMap;0;0;Create;True;0;0;0;False;0;False;-1;None;286b6e6f63cf3dc479d05fdb0cb7a020;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;1393.746,-601.2408;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;65;2041.781,-331.5087;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;4;1426.49,-1093.966;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;51;1514.922,587.8098;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;54;1425.965,129.3924;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;61;2299.155,-436.5639;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2762.967,-230.8175;Half;False;True;-1;2;ASEMaterialInspector;0;0;Standard;EarthShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;57;2;59;0
WireConnection;57;3;60;0
WireConnection;8;1;17;0
WireConnection;8;0;22;0
WireConnection;63;0;57;0
WireConnection;63;1;62;0
WireConnection;43;0;8;0
WireConnection;18;0;9;0
WireConnection;18;1;19;0
WireConnection;21;0;43;0
WireConnection;67;0;66;0
WireConnection;64;0;17;0
WireConnection;64;1;63;0
WireConnection;47;0;3;0
WireConnection;47;1;23;0
WireConnection;47;2;48;0
WireConnection;49;0;3;0
WireConnection;49;1;23;0
WireConnection;49;2;50;0
WireConnection;20;0;21;0
WireConnection;20;1;18;0
WireConnection;65;0;63;0
WireConnection;65;1;64;0
WireConnection;65;2;67;0
WireConnection;4;0;18;0
WireConnection;4;1;10;0
WireConnection;4;2;43;0
WireConnection;51;0;49;0
WireConnection;51;1;47;0
WireConnection;51;2;24;0
WireConnection;54;0;53;0
WireConnection;54;1;25;0
WireConnection;54;2;24;0
WireConnection;61;0;20;0
WireConnection;61;1;65;0
WireConnection;0;0;4;0
WireConnection;0;1;51;0
WireConnection;0;2;61;0
WireConnection;0;4;54;0
ASEEND*/
//CHKSM=D0CA7095E5840B0D64210BFFF8778F5F62BA222F