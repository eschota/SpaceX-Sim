// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Antonio/Ocean"
{
	Properties
	{
		_WaveStreach("Wave Streach", Vector) = (1,1,0,0)
		_WaveTile("Wave Tile", Float) = 0
		_WaveDirection("WaveDirection", Vector) = (0,-1,0,0)
		_WaveSpeed("Wave Speed", Float) = 1
		_WaveHeight("Wave Height", Float) = 0.07
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		AlphaToMask On
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _WaveHeight;
		uniform float _WaveSpeed;
		uniform float2 _WaveDirection;
		uniform float2 _WaveStreach;
		uniform float _WaveTile;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Smoothness;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float4 temp_cast_3 = (20.0).xxxx;
			return temp_cast_3;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float temp_output_208_0 = ( _Time.y * _WaveSpeed );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult222 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceTile223 = appendResult222;
			float4 WaveTileUV233 = ( ( WorldSpaceTile223 * float4( _WaveStreach, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner204 = ( temp_output_208_0 * _WaveDirection + WaveTileUV233.xy);
			float simplePerlin2D202 = snoise( panner204 );
			float2 panner235 = ( temp_output_208_0 * _WaveDirection + ( WaveTileUV233 * float4( 0.25,0.25,0,0 ) ).xy);
			float simplePerlin2D236 = snoise( panner235*2.0 );
			float temp_output_237_0 = ( simplePerlin2D202 + simplePerlin2D236 );
			float3 WaveHeight246 = ( ( float3(0,1,0) * _WaveHeight ) * temp_output_237_0 );
			v.vertex.xyz += WaveHeight246;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Normal = tex2D( _TextureSample0, uv_TextureSample0 ).rgb;
			float4 color252 = IsGammaSpace() ? float4(0.02024743,0.3975856,0.6132076,0) : float4(0.001567139,0.1311825,0.3341808,0);
			float4 color253 = IsGammaSpace() ? float4(0.1532574,0.6736281,0.7924528,0) : float4(0.02036268,0.411347,0.5911142,0);
			float temp_output_208_0 = ( _Time.y * _WaveSpeed );
			float3 ase_worldPos = i.worldPos;
			float4 appendResult222 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceTile223 = appendResult222;
			float4 WaveTileUV233 = ( ( WorldSpaceTile223 * float4( _WaveStreach, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner204 = ( temp_output_208_0 * _WaveDirection + WaveTileUV233.xy);
			float simplePerlin2D202 = snoise( panner204 );
			float2 panner235 = ( temp_output_208_0 * _WaveDirection + ( WaveTileUV233 * float4( 0.25,0.25,0,0 ) ).xy);
			float simplePerlin2D236 = snoise( panner235*2.0 );
			float temp_output_237_0 = ( simplePerlin2D202 + simplePerlin2D236 );
			float WavePattern242 = temp_output_237_0;
			float clampResult257 = clamp( WavePattern242 , 0.0 , 1.0 );
			float4 lerpResult255 = lerp( color252 , color253 , clampResult257);
			float4 Albedo258 = lerpResult255;
			o.Albedo = Albedo258.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
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
				surfIN.worldPos = worldPos;
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
71;180;1200;923;3778.615;188.3199;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;224;-1527.893,-277.5724;Inherit;False;652.0988;341.5;Comment;3;222;221;223;WorlSpaceTile;1,0,0,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;221;-1477.893,-227.5725;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;222;-1259.494,-191.1725;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;248;-3566.692,1713.366;Inherit;False;1191.964;578.381;Comment;11;225;227;226;229;228;233;244;232;245;246;231;WaveUV;0.4436305,0.9528302,0.1842737,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;223;-1104.794,-195.0725;Inherit;True;WorldSpaceTile;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;225;-3516.692,1763.366;Inherit;False;223;WorldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;227;-3493.482,1854.422;Inherit;False;Property;_WaveStreach;Wave Streach;0;0;Create;True;0;0;0;False;0;False;1,1;0.15,0.02;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;229;-3301.057,1907.81;Inherit;False;Property;_WaveTile;Wave Tile;1;0;Create;True;0;0;0;False;0;False;0;0.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;-3280.64,1807.196;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;228;-3133.203,1808.137;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;249;-3705.295,2336.173;Inherit;False;1323.115;479.9194;Comment;12;242;234;207;209;211;238;208;235;204;236;202;237;WavePattern;0.1273585,0.424782,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;233;-2992.762,1802.483;Inherit;False;WaveTileUV;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;234;-3592.601,2386.173;Inherit;False;233;WaveTileUV;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;207;-3655.295,2611.182;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;209;-3645.654,2691.212;Inherit;False;Property;_WaveSpeed;Wave Speed;3;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;208;-3460.244,2609.136;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;211;-3507.622,2490.362;Inherit;False;Property;_WaveDirection;WaveDirection;2;0;Create;True;0;0;0;False;0;False;0,-1;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;238;-3350.813,2394.652;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.25,0.25,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;235;-3175.393,2642.571;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;204;-3177.799,2498.963;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;236;-2973.998,2637.115;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;1,1;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;202;-2974.819,2494.605;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;237;-2763.675,2563.362;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;242;-2606.181,2557.092;Inherit;True;WavePattern;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;260;-1755.99,87.5298;Inherit;False;874.6725;541.1918;Comment;6;255;253;257;256;252;258;Albedo;1,0,0.8021469,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;256;-1705.99,465.6032;Inherit;False;242;WavePattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-3141.728,2074.747;Inherit;False;Property;_WaveHeight;Wave Height;4;0;Create;True;0;0;0;False;0;False;0.07;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;231;-3140.014,1919.046;Inherit;False;Constant;_WaveUP;WaveUP;4;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;-2953.076,2039.275;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;252;-1545.411,137.5298;Inherit;False;Constant;_WaterColor;Water Color;7;0;Create;True;0;0;0;False;0;False;0.02024743,0.3975856,0.6132076,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;257;-1474.161,469.7214;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;253;-1545.601,303.5124;Inherit;False;Constant;_TopColor;Top Color;7;0;Create;True;0;0;0;False;0;False;0.1532574,0.6736281,0.7924528,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;245;-2769.728,2038.747;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;255;-1277.335,257.4844;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;246;-2598.728,2032.747;Inherit;True;WaveHeight;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;258;-1105.318,252.9281;Inherit;True;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;247;-467.4331,1649.054;Inherit;False;246;WaveHeight;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;259;-558.1637,1235.628;Inherit;False;258;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;230;-448.168,1790.11;Inherit;False;Constant;_Tesselation;Tesselation;5;0;Create;True;0;0;0;False;0;False;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;251;-1230.238,1357.736;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;0;False;0;False;-1;None;70b136ca3ad6a3741aa08f7ae1d770aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;250;-643.7008,1460.153;Inherit;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;0;False;0;False;0;0.85;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;243;-847.0253,1692.472;Inherit;False;242;WavePattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;261;-3141.277,125.5396;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;263;-2866.615,125.6801;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;262;-3375.615,144.6801;Inherit;False;Property;_EdgeDistance;Edge Distance;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-226.0691,1367.611;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Antonio/Ocean;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;50;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;5;False;-1;5;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;222;0;221;1
WireConnection;222;1;221;3
WireConnection;223;0;222;0
WireConnection;226;0;225;0
WireConnection;226;1;227;0
WireConnection;228;0;226;0
WireConnection;228;1;229;0
WireConnection;233;0;228;0
WireConnection;208;0;207;0
WireConnection;208;1;209;0
WireConnection;238;0;234;0
WireConnection;235;0;238;0
WireConnection;235;2;211;0
WireConnection;235;1;208;0
WireConnection;204;0;234;0
WireConnection;204;2;211;0
WireConnection;204;1;208;0
WireConnection;236;0;235;0
WireConnection;202;0;204;0
WireConnection;237;0;202;0
WireConnection;237;1;236;0
WireConnection;242;0;237;0
WireConnection;232;0;231;0
WireConnection;232;1;244;0
WireConnection;257;0;256;0
WireConnection;245;0;232;0
WireConnection;245;1;237;0
WireConnection;255;0;252;0
WireConnection;255;1;253;0
WireConnection;255;2;257;0
WireConnection;246;0;245;0
WireConnection;258;0;255;0
WireConnection;261;0;262;0
WireConnection;263;0;261;0
WireConnection;0;0;259;0
WireConnection;0;1;251;0
WireConnection;0;4;250;0
WireConnection;0;11;247;0
WireConnection;0;14;230;0
ASEEND*/
//CHKSM=3668897C111BFD8148D794DEA672EA20943C4846