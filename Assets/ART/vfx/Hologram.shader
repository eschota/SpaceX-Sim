// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hologram"
{
	Properties
	{
		_Primarylayerimage("Primary layer image", 2D) = "white" {}
		_PrimaryLight("Primary Light", Color) = (0,0,0,0)
		_PrimaryDark("Primary Dark", Color) = (0,0,0,0)
		_PrimaryPower("Primary Power", Float) = 0.1
		_OpacityContrast("Opacity Contrast", Float) = 0.59
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _PrimaryDark;
		uniform float4 _PrimaryLight;
		uniform sampler2D _Primarylayerimage;
		uniform float4 _Primarylayerimage_ST;
		uniform float _PrimaryPower;
		uniform float _OpacityContrast;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Primarylayerimage = i.uv_texcoord * _Primarylayerimage_ST.xy + _Primarylayerimage_ST.zw;
			float4 tex2DNode3 = tex2D( _Primarylayerimage, uv_Primarylayerimage );
			float4 lerpResult9 = lerp( _PrimaryDark , _PrimaryLight , tex2DNode3);
			float4 lerpResult32 = lerp( ( lerpResult9 * _PrimaryPower ) , float4( 0,0,0,0 ) , float4( 0.509434,0.509434,0.509434,0 ));
			o.Emission = lerpResult32.rgb;
			float4 temp_cast_1 = (_OpacityContrast).xxxx;
			float4 clampResult34 = clamp( tex2DNode3 , temp_cast_1 , float4( 1,0,0,0 ) );
			o.Alpha = clampResult34.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
105;234;1200;915;214.0705;-223.9102;1.59845;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;4;24.27016,754.547;Inherit;True;Property;_Primarylayerimage;Primary layer image;0;0;Create;True;0;0;0;False;0;False;b273dbd84408a344da543d1a41388641;abb99dd7fedacb2479ef84e5147786e8;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;6;387.1183,582.2667;Inherit;False;Property;_PrimaryLight;Primary Light;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0.2350664,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;7;384.3652,401.5127;Inherit;False;Property;_PrimaryDark;Primary Dark;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;318.5868,754.9686;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;9;732.1182,497.2658;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;734.1763,651.2061;Inherit;False;Property;_PrimaryPower;Primary Power;3;0;Create;True;0;0;0;False;0;False;0.1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;948.0484,566.4402;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;35;868.3591,895.7792;Inherit;False;Property;_OpacityContrast;Opacity Contrast;4;0;Create;True;0;0;0;False;0;False;0.59;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;32;1124.509,566.2593;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.509434,0.509434,0.509434,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;34;1135.967,784.8674;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;28;1457.864,565.9677;Float;False;True;-1;6;ASEMaterialInspector;0;0;Unlit;Hologram;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.71;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;1;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;4;0
WireConnection;9;0;7;0
WireConnection;9;1;6;0
WireConnection;9;2;3;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;32;0;10;0
WireConnection;34;0;3;0
WireConnection;34;1;35;0
WireConnection;28;2;32;0
WireConnection;28;9;34;0
ASEEND*/
//CHKSM=0B5BAC6D2D7606FE494BD6B82A8727F57225F356