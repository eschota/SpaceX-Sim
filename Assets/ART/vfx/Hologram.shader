// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hologram"
{
	Properties
	{
		_Primarylayerimage("Primary layer image", 2D) = "white" {}
		_Texture0("Texture 0", 2D) = "white" {}
		_Color0("Color 0", Color) = (0.4878055,0.7924528,0.3102527,0)
		_speed("speed", Float) = 0
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_frenel("frenel", Color) = (0,0,0,0)
		_powerEmission("powerEmission", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Unlit alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
		};

		uniform sampler2D _Primarylayerimage;
		uniform float4 _Primarylayerimage_ST;
		uniform float4 _frenel;
		uniform float4 _Color0;
		uniform sampler2D _Texture0;
		uniform float2 _Tiling;
		uniform float _speed;
		uniform float _powerEmission;


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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float mulTime64 = _Time.y * 0.1;
			float2 temp_cast_0 = (mulTime64).xx;
			float dotResult4_g1 = dot( temp_cast_0 , float2( 12.9898,78.233 ) );
			float lerpResult10_g1 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g1 ) * 43758.55 ) ));
			float temp_output_63_0 = lerpResult10_g1;
			float2 temp_cast_1 = (( ase_screenPosNorm.y * temp_output_63_0 )).xx;
			float simplePerlin2D69 = snoise( temp_cast_1*25.0 );
			float lerpResult68 = lerp( simplePerlin2D69 , 0.0 , ( temp_output_63_0 < 0.95 ? 1.0 : 0.8 ));
			float3 temp_cast_2 = (( lerpResult68 * 0.025 )).xxx;
			v.vertex.xyz += temp_cast_2;
			v.vertex.w = 1;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Primarylayerimage = i.uv_texcoord * _Primarylayerimage_ST.xy + _Primarylayerimage_ST.zw;
			float4 tex2DNode3 = tex2D( _Primarylayerimage, uv_Primarylayerimage );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV54 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode54 = ( 0.0 + 4.0 * pow( max( 1.0 - fresnelNdotV54 , 0.0001 ), 1.0 ) );
			float4 lerpResult59 = lerp( ( _frenel * fresnelNode54 ) , _Color0 , float4( 0.1603774,0.1581079,0.1581079,0 ));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 temp_cast_0 = (( _Time.y * _speed )).xx;
			float2 uv_TexCoord44 = i.uv_texcoord * _Tiling + temp_cast_0;
			float4 temp_output_60_0 = ( 1.0 - tex2D( _Texture0, ( ase_screenPosNorm + float4( uv_TexCoord44, 0.0 , 0.0 ) ).xy ) );
			float mulTime64 = _Time.y * 0.1;
			float2 temp_cast_3 = (mulTime64).xx;
			float dotResult4_g1 = dot( temp_cast_3 , float2( 12.9898,78.233 ) );
			float lerpResult10_g1 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g1 ) * 43758.55 ) ));
			float temp_output_63_0 = lerpResult10_g1;
			o.Emission = ( ( tex2DNode3 * ( ( lerpResult59 + ( temp_output_60_0 * _Color0 ) ) * ( temp_output_63_0 > 0.98 ? 1.0 : 0.8 ) ) ) * _powerEmission ).rgb;
			o.Alpha = tex2DNode3.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
78;441;1424;708;-1085.666;138.7317;2.222461;True;False
Node;AmplifyShaderEditor.CommentaryNode;51;229.9415,1001.724;Inherit;False;1306.518;517.6039;Comment;9;42;41;45;47;44;46;49;48;50;Opacity;0.4056491,1,0.08962262,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;50;279.9416,1403.328;Inherit;False;Property;_speed;speed;3;0;Create;True;0;0;0;False;0;False;0;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;48;291.8714,1230.061;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;509.3394,1380.678;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;46;504.3099,1229.994;Inherit;False;Property;_Tiling;Tiling;4;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;702.1608,1333.486;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenPosInputsNode;47;677.3099,1063.994;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;45;930.31,1243.994;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexturePropertyNode;42;901.4128,1053.858;Inherit;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;0;False;0;False;None;d2d32bc1ef9f5634db6ce64a6beae91e;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;53;1358.518,173.8414;Inherit;False;Property;_frenel;frenel;5;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0.5117955,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;1216.459,1051.724;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;54;1357.418,467.626;Inherit;False;Standard;WorldNormal;ViewDir;True;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;4;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;64;1780.902,1407.96;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;60;1735.772,889.436;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;43;1631.333,470.9957;Inherit;False;Property;_Color0;Color 0;2;0;Create;True;0;0;0;False;0;False;0.4878055,0.7924528,0.3102527,0;0.3973638,0.2122642,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;1621.064,179.9372;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;63;1969.738,1408.383;Inherit;False;Random Range;-1;;1;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;71;2219.961,1206.735;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;1983.609,449.5297;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;59;1874.722,181.0975;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.1603774,0.1581079,0.1581079,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;2138.987,326.1797;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;2499.071,1380.964;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;65;2185.902,1401.96;Inherit;True;2;4;0;FLOAT;0;False;1;FLOAT;0.98;False;2;FLOAT;1;False;3;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;1525.136,-78.06966;Inherit;True;Property;_Primarylayerimage;Primary layer image;0;0;Create;True;0;0;0;False;0;False;b273dbd84408a344da543d1a41388641;efdb0af2e57947741b90a4dda6ed8d3e;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;78;2453.892,1506.908;Inherit;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;0;False;0;False;25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;69;2697.977,1376.262;Inherit;True;Simplex2D;False;False;2;0;FLOAT2;1,1;False;1;FLOAT;3.54;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;2398.251,325.4369;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Compare;70;2196.717,1653.317;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0.95;False;2;FLOAT;1;False;3;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;1803.406,-74.17439;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;82;3362.48,860.4056;Inherit;False;Property;_powerEmission;powerEmission;6;0;Create;True;0;0;0;False;0;False;1;1.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;2772.772,305.221;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;80;3615.188,1812.042;Inherit;False;Constant;_Float1;Float 1;11;0;Create;True;0;0;0;False;0;False;0.025;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;68;2956.26,1606.334;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;3605.705,599.4103;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;3886.882,1522.882;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;3707.578,1053.291;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;85;3593.488,1186.398;Inherit;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;0;False;0;False;0.43;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;28;4093.425,866.678;Float;False;True;-1;6;ASEMaterialInspector;0;0;Unlit;Hologram;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.71;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;8;5;False;-1;1;False;-1;1;False;-1;0;False;-1;1;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;49;0;48;0
WireConnection;49;1;50;0
WireConnection;44;0;46;0
WireConnection;44;1;49;0
WireConnection;45;0;47;0
WireConnection;45;1;44;0
WireConnection;41;0;42;0
WireConnection;41;1;45;0
WireConnection;60;0;41;0
WireConnection;55;0;53;0
WireConnection;55;1;54;0
WireConnection;63;1;64;0
WireConnection;61;0;60;0
WireConnection;61;1;43;0
WireConnection;59;0;55;0
WireConnection;59;1;43;0
WireConnection;62;0;59;0
WireConnection;62;1;61;0
WireConnection;76;0;71;2
WireConnection;76;1;63;0
WireConnection;65;0;63;0
WireConnection;69;0;76;0
WireConnection;69;1;78;0
WireConnection;66;0;62;0
WireConnection;66;1;65;0
WireConnection;70;0;63;0
WireConnection;3;0;4;0
WireConnection;81;0;3;0
WireConnection;81;1;66;0
WireConnection;68;0;69;0
WireConnection;68;2;70;0
WireConnection;83;0;81;0
WireConnection;83;1;82;0
WireConnection;79;0;68;0
WireConnection;79;1;80;0
WireConnection;84;0;60;0
WireConnection;84;1;85;0
WireConnection;28;2;83;0
WireConnection;28;9;3;0
WireConnection;28;11;79;0
ASEEND*/
//CHKSM=F31985CADB969BAD053FE6FA48D0CAD94C2187BD