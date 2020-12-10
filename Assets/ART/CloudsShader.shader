// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CloudsShader"
{
	Properties
	{
		_CloudsBaseColor("Clouds BaseColor", Color) = (1,1,1,0)
		_CloudMult("CloudMult", Range( 0 , 5)) = 1.5
		_CloudSmoothness("Cloud Smoothness", Range( 0 , 1)) = 0.2
		_DarkSideEmission("DarkSide Emission", Color) = (0.03875934,0.06443756,0.1226415,1)
		_DarkSideContrast("DarkSide Contrast", Range( 0 , 50)) = 5
		_CloudMap("CloudMap", 2D) = "white" {}
		_NormalMap("NormalMap", 2D) = "bump" {}
		_CloudNormal("CloudNormal", Range( 0 , 1)) = 0.1
		_Flowmapcloud("Flowmap cloud", 2D) = "white" {}
		_SpeedFlowmap("Speed Flow map", Float) = 0.2
		_PowerFlow("PowerFlow", Float) = 1
		_FlowTiling("FlowTiling", Float) = 1
		_FlowOffset("FlowOffset", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
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

		uniform sampler2D _NormalMap;
		uniform sampler2D _Flowmapcloud;
		uniform half4 _Flowmapcloud_ST;
		uniform half _SpeedFlowmap;
		uniform half _PowerFlow;
		uniform half _FlowTiling;
		uniform half _FlowOffset;
		uniform half _CloudNormal;
		uniform half _CloudMult;
		uniform half4 _CloudsBaseColor;
		uniform half4 _DarkSideEmission;
		uniform float _DarkSideContrast;
		uniform half _CloudSmoothness;
		uniform sampler2D _CloudMap;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Flowmapcloud = i.uv_texcoord * _Flowmapcloud_ST.xy + _Flowmapcloud_ST.zw;
			half2 blendOpSrc105 = i.uv_texcoord;
			half2 blendOpDest105 = (tex2D( _Flowmapcloud, uv_Flowmapcloud )).rg;
			half2 temp_output_105_0 = ( saturate( (( blendOpDest105 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest105 ) * ( 1.0 - blendOpSrc105 ) ) : ( 2.0 * blendOpDest105 * blendOpSrc105 ) ) ));
			half mulTime97 = _Time.y * 0.5;
			half temp_output_99_0 = ( mulTime97 * _SpeedFlowmap );
			half temp_output_1_0_g5 = temp_output_99_0;
			half temp_output_101_0 = (0.0 + (( ( temp_output_1_0_g5 - floor( ( temp_output_1_0_g5 + 0.5 ) ) ) * 2 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0));
			half timeA108 = ( -temp_output_101_0 * _PowerFlow );
			half2 lerpResult107 = lerp( i.uv_texcoord , temp_output_105_0 , timeA108);
			half2 temp_cast_0 = (_FlowTiling).xx;
			half2 temp_cast_1 = (_FlowOffset).xx;
			float2 uv_TexCoord215 = i.uv_texcoord * temp_cast_0 + temp_cast_1;
			half2 DifTiling216 = uv_TexCoord215;
			half2 flowA110 = ( lerpResult107 + DifTiling216 );
			half temp_output_1_0_g4 = (temp_output_99_0*1.0 + 0.5);
			half timeB151 = ( -(0.0 + (( ( temp_output_1_0_g4 - floor( ( temp_output_1_0_g4 + 0.5 ) ) ) * 2 ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) * _PowerFlow );
			half2 lerpResult157 = lerp( i.uv_texcoord , temp_output_105_0 , timeB151);
			half2 flowB160 = ( lerpResult157 + DifTiling216 );
			half Blend187 = saturate( abs( ( 1.0 - ( temp_output_101_0 / 0.5 ) ) ) );
			half3 lerpResult179 = lerp( UnpackNormal( tex2D( _NormalMap, flowA110 ) ) , UnpackNormal( tex2D( _NormalMap, flowB160 ) ) , Blend187);
			half CloudMult198 = _CloudMult;
			half clampResult121 = clamp( _CloudNormal , 0.0 , CloudMult198 );
			half4 lerpResult84 = lerp( float4(0,0,1,0) , half4( lerpResult179 , 0.0 ) , clampResult121);
			half4 Normal192 = lerpResult84;
			o.Normal = Normal192.rgb;
			half4 Albedo196 = ( _CloudsBaseColor * CloudMult198 );
			o.Albedo = Albedo196.rgb;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			half3 ase_worldlightDir = 0;
			#else //aseld
			half3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			half3 ase_worldNormal = WorldNormalVector( i, half3( 0, 0, 1 ) );
			half dotResult17 = dot( ase_worldlightDir , ase_worldNormal );
			half4 temp_cast_5 = (dotResult17).xxxx;
			half4 clampResult43 = clamp( CalculateContrast(_DarkSideContrast,temp_cast_5) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			half4 Emission194 = ( _DarkSideEmission * ( 1.0 - clampResult43 ) );
			o.Emission = Emission194.rgb;
			o.Smoothness = _CloudSmoothness;
			half4 lerpResult164 = lerp( tex2D( _CloudMap, flowA110 ) , tex2D( _CloudMap, flowB160 ) , Blend187);
			half4 Cloud113 = lerpResult164;
			half grayscale122 = Luminance(Cloud113.rgb);
			half clampResult127 = clamp( ( CloudMult198 * grayscale122 ) , 0.0 , 1.0 );
			half Opacity190 = clampResult127;
			o.Alpha = Opacity190;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
1943;10;1872;1049;-1183.295;1102.613;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;207;967.1468,-355.8619;Inherit;False;2023.786;537.1597;TimeFlowMap;20;97;98;99;139;148;100;101;149;204;150;102;203;202;108;182;151;184;185;186;187;;0.9339623,0.4459249,0.07489321,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;97;1032.05,-305.8619;Inherit;False;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;1017.147,-157.019;Inherit;False;Property;_SpeedFlowmap;Speed Flow map;9;0;Create;True;0;0;0;False;0;False;0.2;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;1222.111,-237.3577;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;139;1454.961,-26.96364;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;100;1641.992,-238.7706;Inherit;False;Sawtooth Wave;-1;;5;289adb816c3ac6d489f255fc3caf5016;0;1;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;148;1647.496,-26.49148;Inherit;False;Sawtooth Wave;-1;;4;289adb816c3ac6d489f255fc3caf5016;0;1;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;149;1833.006,-25.70241;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;101;1824.906,-237.9814;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;150;2125.152,-26.85842;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;204;2243.342,-72.2322;Inherit;False;Property;_PowerFlow;PowerFlow;10;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;102;2116.788,-239.7635;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;217;2207.349,-2196.399;Inherit;False;709.4012;253.5134;DifTiling;4;216;215;214;222;;0,1,0.7793248,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;206;1415.26,-942.7993;Inherit;False;1555.113;567.4478;FlowMap;13;160;110;218;219;157;220;107;158;105;109;103;92;91;;0.8584906,0.6773725,0,1;0;0
Node;AmplifyShaderEditor.SamplerNode;91;1476.26,-626.9247;Inherit;True;Property;_Flowmapcloud;Flowmap cloud;8;0;Create;True;0;0;0;False;0;False;-1;None;35cac8ffad82c5048bc85886a6df7c90;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;203;2426.342,-26.2322;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;214;2257.349,-2120.646;Inherit;False;Property;_FlowTiling;FlowTiling;11;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;2425.342,-240.2321;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;2252.378,-2041.363;Inherit;False;Property;_FlowOffset;FlowOffset;12;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;108;2565.282,-245.4335;Inherit;False;timeA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;92;1790.795,-627.6357;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;2565.436,-31.77553;Inherit;False;timeB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;215;2456.796,-2140.886;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;103;1779.203,-858.041;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;158;2089.708,-539.238;Inherit;False;151;timeB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;109;2084.448,-892.7993;Inherit;False;108;timeA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;182;2120.14,-140.6815;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;216;2692.75,-2146.399;Inherit;False;DifTiling;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BlendOpsNode;105;2064.418,-722.8725;Inherit;False;Overlay;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;184;2251.869,-140.2382;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;107;2324.839,-853.3891;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;157;2327.242,-627.1826;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;220;2358.631,-724.3062;Inherit;False;216;DifTiling;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;185;2428.314,-140.2379;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;219;2572.631,-626.3062;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;218;2563.631,-852.3062;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;2758.978,-860.4628;Inherit;True;flowA;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;186;2569.73,-141.5355;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;160;2763.363,-634.3151;Inherit;True;flowB;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;211;1257.998,-1581.659;Inherit;False;1704.085;622.9906;Albedo;12;111;112;162;188;68;161;164;113;123;118;196;200;;0.1847264,1,0,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;111;1309.998,-1413.185;Inherit;True;Property;_CloudMap;CloudMap;5;0;Create;True;0;0;0;False;0;False;None;040b5d14c0963fd47981e050448e69c3;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RegisterLocalVarNode;187;2766.933,-145.4274;Inherit;True;Blend;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;1367.286,-1531.659;Inherit;False;110;flowA;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;1366.506,-1197.146;Inherit;False;160;flowB;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;68;1558.663,-1501.948;Inherit;True;Global;CloudMap002;CloudMap002;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;208;1397.377,209.0846;Inherit;False;1602.011;586.0936;Emission;10;15;16;17;22;8;43;21;125;126;194;;0,0.9167204,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;161;1561.464,-1277.909;Inherit;True;Global;TextureSample0;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;188;1702.976,-1081.669;Inherit;False;187;Blend;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;15;1447.377,349.4547;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;212;3091.954,682.7902;Inherit;False;669.9207;740.0803;Main;8;195;119;191;197;193;0;77;198;;1,0,0.008262634,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;16;1480.288,505.7549;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;164;1890.967,-1385.497;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;209;1396.612,820.6158;Inherit;False;1609.926;607.5467;Normal;13;192;84;3;179;121;120;201;181;83;189;174;176;175;;0.8301887,0,0.6649203,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;17;1715.382,433.584;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;210;2007.796,-1928.172;Inherit;False;941.917;337.9794;Opacity;6;190;127;79;122;199;114;;0,0.00298214,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;22;1647.004,652.1782;Float;False;Property;_DarkSideContrast;DarkSide Contrast;4;0;Create;True;0;0;0;False;0;False;5;3;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;3222.147,1306.566;Inherit;False;Property;_CloudMult;CloudMult;1;0;Create;True;0;0;0;False;0;False;1.5;1.32;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;113;2083.626,-1392.238;Inherit;True;Cloud;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;114;2037.308,-1790.678;Inherit;False;113;Cloud;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;175;1500.331,898.8839;Inherit;False;110;flowA;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;176;1506.197,1156.406;Inherit;False;160;flowB;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;198;3522.892,1306.872;Inherit;False;CloudMult;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;174;1446.612,970.9325;Inherit;True;Property;_NormalMap;NormalMap;6;0;Create;True;0;0;0;False;0;False;None;8bf30880a0e617b4caad7837d0649bbe;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleContrastOpNode;8;1968.12,434.6932;Inherit;True;2;1;COLOR;0,0,0,0;False;0;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;199;2248.854,-1876.764;Inherit;False;198;CloudMult;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;122;2232.691,-1791.465;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;43;2244.436,435.3899;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;83;1745.262,899.6991;Inherit;True;Property;_CloudsNormalMap;Clouds NormalMap;2;0;Create;True;0;0;0;False;0;False;-1;None;8bf30880a0e617b4caad7837d0649bbe;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;181;1747.531,1110.755;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;8bf30880a0e617b4caad7837d0649bbe;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;189;1872.891,1300.02;Inherit;False;187;Blend;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;201;2185.223,1238.89;Inherit;False;198;CloudMult;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;2084.49,1164.225;Inherit;False;Property;_CloudNormal;CloudNormal;7;0;Create;True;0;0;0;False;0;False;0.1;0.163;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;2432.482,-1811.35;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;121;2377.466,1170.155;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;200;2393.967,-1218.618;Inherit;False;198;CloudMult;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;118;2359.626,-1398.469;Inherit;False;Property;_CloudsBaseColor;Clouds BaseColor;0;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;179;2090.63,1025.975;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;125;2369.592,259.0846;Inherit;False;Property;_DarkSideEmission;DarkSide Emission;3;0;Create;True;0;0;0;False;0;False;0.03875934,0.06443756,0.1226415,1;0.03875934,0.06443756,0.1226415,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;2316.363,875.4738;Float;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;0;False;0;False;0,0,1,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;21;2427.656,434.8282;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;84;2543.927,1004.146;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;127;2584.378,-1811.865;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;2593.084,-1304.868;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;2609.813,343.821;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;190;2748.243,-1817.414;Inherit;True;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;194;2775.388,339.9306;Inherit;True;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;192;2797.11,998.3663;Inherit;True;Normal;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;196;2738.083,-1309.62;Inherit;True;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;197;3242.473,732.7902;Inherit;False;196;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;119;3141.954,962.4839;Inherit;False;Property;_CloudSmoothness;Cloud Smoothness;2;0;Create;True;0;0;0;False;0;False;0.2;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;193;3245.789,810.0806;Inherit;False;192;Normal;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;195;3244.719,886.2832;Inherit;False;194;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;3244.989,1037.982;Inherit;False;190;Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3506.874,794.4524;Half;False;True;-1;2;ASEMaterialInspector;0;0;Standard;CloudsShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;99;0;97;0
WireConnection;99;1;98;0
WireConnection;139;0;99;0
WireConnection;100;1;99;0
WireConnection;148;1;139;0
WireConnection;149;0;148;0
WireConnection;101;0;100;0
WireConnection;150;0;149;0
WireConnection;102;0;101;0
WireConnection;203;0;150;0
WireConnection;203;1;204;0
WireConnection;202;0;102;0
WireConnection;202;1;204;0
WireConnection;108;0;202;0
WireConnection;92;0;91;0
WireConnection;151;0;203;0
WireConnection;215;0;214;0
WireConnection;215;1;222;0
WireConnection;182;0;101;0
WireConnection;216;0;215;0
WireConnection;105;0;103;0
WireConnection;105;1;92;0
WireConnection;184;0;182;0
WireConnection;107;0;103;0
WireConnection;107;1;105;0
WireConnection;107;2;109;0
WireConnection;157;0;103;0
WireConnection;157;1;105;0
WireConnection;157;2;158;0
WireConnection;185;0;184;0
WireConnection;219;0;157;0
WireConnection;219;1;220;0
WireConnection;218;0;107;0
WireConnection;218;1;220;0
WireConnection;110;0;218;0
WireConnection;186;0;185;0
WireConnection;160;0;219;0
WireConnection;187;0;186;0
WireConnection;68;0;111;0
WireConnection;68;1;112;0
WireConnection;161;0;111;0
WireConnection;161;1;162;0
WireConnection;164;0;68;0
WireConnection;164;1;161;0
WireConnection;164;2;188;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;113;0;164;0
WireConnection;198;0;77;0
WireConnection;8;1;17;0
WireConnection;8;0;22;0
WireConnection;122;0;114;0
WireConnection;43;0;8;0
WireConnection;83;0;174;0
WireConnection;83;1;175;0
WireConnection;181;0;174;0
WireConnection;181;1;176;0
WireConnection;79;0;199;0
WireConnection;79;1;122;0
WireConnection;121;0;120;0
WireConnection;121;2;201;0
WireConnection;179;0;83;0
WireConnection;179;1;181;0
WireConnection;179;2;189;0
WireConnection;21;0;43;0
WireConnection;84;0;3;0
WireConnection;84;1;179;0
WireConnection;84;2;121;0
WireConnection;127;0;79;0
WireConnection;123;0;118;0
WireConnection;123;1;200;0
WireConnection;126;0;125;0
WireConnection;126;1;21;0
WireConnection;190;0;127;0
WireConnection;194;0;126;0
WireConnection;192;0;84;0
WireConnection;196;0;123;0
WireConnection;0;0;197;0
WireConnection;0;1;193;0
WireConnection;0;2;195;0
WireConnection;0;4;119;0
WireConnection;0;9;191;0
ASEEND*/
//CHKSM=9F51F4B780BDA61ED645E03142D79D476C61C748