// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AtmosphereGlowShader"
{
	Properties
	{
		_EmissionColor("Emission Color", Color) = (0.4858491,0.7620791,1,1)
		_GlowScaleOutside("GlowScale Outside", Range( 0 , 1)) = 0.77
		_GlowScaleInside("GlowScale Inside", Range( 0 , 1)) = 0.14
		_DarkSideMult("DarkSideMult", Range( 0 , 1)) = 0.03
		_DayNightContrast("DayNight Contrast", Range( 0 , 50)) = 4
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha , SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			half3 worldNormal;
		};

		uniform half4 _EmissionColor;
		uniform half _GlowScaleOutside;
		uniform half _GlowScaleInside;
		uniform float _DayNightContrast;
		uniform float _DarkSideMult;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Emission = _EmissionColor.rgb;
			float3 ase_worldPos = i.worldPos;
			half3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			half3 ase_worldNormal = i.worldNormal;
			half fresnelNdotV57 = dot( ase_worldNormal, ase_worldViewDir );
			half fresnelNode57 = ( 0.0 + ( ( 1.0 - _GlowScaleOutside ) * 20.0 ) * pow( 1.0 - fresnelNdotV57, 8.0 ) );
			half clampResult103 = clamp( fresnelNode57 , 0.0 , 1.0 );
			half fresnelNdotV101 = dot( ase_worldNormal, ase_worldViewDir );
			half fresnelNode101 = ( 0.0 + ( _GlowScaleInside * 20.0 ) * pow( 1.0 - fresnelNdotV101, 3.0 ) );
			half clampResult104 = clamp( fresnelNode101 , 0.0 , 1.0 );
			half temp_output_102_0 = ( ( 1.0 - clampResult103 ) * clampResult104 );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			half3 ase_worldlightDir = 0;
			#else //aseld
			half3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			half dotResult17 = dot( ase_worldlightDir , ase_worldNormal );
			half4 temp_cast_1 = (dotResult17).xxxx;
			half4 clampResult96 = clamp( CalculateContrast(_DayNightContrast,temp_cast_1) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			half4 temp_cast_2 = (temp_output_102_0).xxxx;
			half4 lerpResult111 = lerp( ( temp_output_102_0 * clampResult96 ) , temp_cast_2 , _DarkSideMult);
			o.Alpha = lerpResult111.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
-1407;196;1404;1042;-3546.084;53.60425;1.640162;True;False
Node;AmplifyShaderEditor.RangedFloatNode;59;2593.541,788.6711;Inherit;False;Property;_GlowScaleOutside;GlowScale Outside;1;0;Create;True;0;0;0;False;0;False;0.77;1.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;105;2943.876,714.5966;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;2950.376,808.1962;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;3145.375,722.3966;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;2916.343,968.4615;Inherit;False;Constant;_PowerAtmoOutside;PowerAtmoOutside;2;0;Create;True;0;0;0;False;0;False;8;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;2607.499,1149.49;Inherit;False;Property;_GlowScaleInside;GlowScale Inside;2;0;Create;True;0;0;0;False;0;False;0.14;2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;15;3133.276,272.3289;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;16;3142.187,460.6284;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;3093.376,1146.196;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;57;3394.001,771.0703;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;2948.07,1318.895;Inherit;False;Constant;_PowerAtmoInside;PowerAtmoInside;1;0;Create;True;0;0;0;False;0;False;3;3;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;101;3423.126,1099.402;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;17;3452.234,278.4997;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;103;3778.864,786.9008;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;109;3355.639,601.6622;Float;False;Property;_DayNightContrast;DayNight Contrast;4;0;Create;True;0;0;0;False;0;False;4;3;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;94;3716.161,343.5614;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;104;3746.327,1121.234;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;97;4025.045,741.2275;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;96;4034.596,392.6775;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;4258.202,927.6207;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;112;4295.957,1227.418;Float;False;Property;_DarkSideMult;DarkSideMult;3;0;Create;True;0;0;0;False;0;False;0.03;3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;4520.44,698.1922;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;111;4740.533,902.8371;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;93;4377.544,298.5256;Inherit;False;Property;_EmissionColor;Emission Color;0;0;Create;True;0;0;0;False;0;False;0.4858491,0.7620791,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4992.959,469.3683;Half;False;True;-1;2;ASEMaterialInspector;0;0;Standard;AtmosphereGlowShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;False;Transparent;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;5;-1;0;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;105;0;59;0
WireConnection;106;0;105;0
WireConnection;106;1;107;0
WireConnection;108;0;100;0
WireConnection;108;1;107;0
WireConnection;57;2;106;0
WireConnection;57;3;60;0
WireConnection;101;2;108;0
WireConnection;101;3;99;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;103;0;57;0
WireConnection;94;1;17;0
WireConnection;94;0;109;0
WireConnection;104;0;101;0
WireConnection;97;0;103;0
WireConnection;96;0;94;0
WireConnection;102;0;97;0
WireConnection;102;1;104;0
WireConnection;110;0;102;0
WireConnection;110;1;96;0
WireConnection;111;0;110;0
WireConnection;111;1;102;0
WireConnection;111;2;112;0
WireConnection;0;2;93;0
WireConnection;0;9;111;0
ASEEND*/
//CHKSM=2641F3D7ED1B0CCBE4EDD79D999A8B503DC01A87