// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "wow"
{
	Properties
	{
		_Flowmapcloud("Flowmap cloud", 2D) = "white" {}
		_CloudMap("CloudMap", 2D) = "white" {}
		_SpeedFlowmap("Speed Flow map", Range( 0 , 1)) = 0.05
		_TilingFlow("TilingFlow", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _CloudMap;
		uniform sampler2D _Flowmapcloud;
		uniform float4 _Flowmapcloud_ST;
		uniform float _SpeedFlowmap;
		uniform float _TilingFlow;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Flowmapcloud = i.uv_texcoord * _Flowmapcloud_ST.xy + _Flowmapcloud_ST.zw;
			float2 blendOpSrc20 = i.uv_texcoord;
			float2 blendOpDest20 = (tex2D( _Flowmapcloud, uv_Flowmapcloud )).rg;
			float2 temp_output_20_0 = ( saturate( (( blendOpDest20 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest20 ) * ( 1.0 - blendOpSrc20 ) ) : ( 2.0 * blendOpDest20 * blendOpSrc20 ) ) ));
			float temp_output_3_0 = ( _Time.y * _SpeedFlowmap );
			float temp_output_1_0_g3 = temp_output_3_0;
			float timeA16 = -(0.0 + (( ( temp_output_1_0_g3 - floor( ( temp_output_1_0_g3 + 0.5 ) ) ) * 2 ) - -1.0) * (1.0 - 0.0) / (0.0 - -1.0));
			float2 lerpResult23 = lerp( i.uv_texcoord , temp_output_20_0 , timeA16);
			float2 temp_cast_0 = (_TilingFlow).xx;
			float2 uv_TexCoord15 = i.uv_texcoord * temp_cast_0;
			float2 Diff21 = uv_TexCoord15;
			float2 flowA27 = ( lerpResult23 + Diff21 );
			float temp_output_1_0_g2 = (temp_output_3_0*1.0 + 0.5);
			float timeB13 = -(0.0 + (( ( temp_output_1_0_g2 - floor( ( temp_output_1_0_g2 + 0.5 ) ) ) * 2 ) - -1.0) * (1.0 - 0.0) / (0.0 - -1.0));
			float2 lerpResult22 = lerp( i.uv_texcoord , temp_output_20_0 , timeB13);
			float2 flowB28 = ( lerpResult22 + Diff21 );
			float4 lerpResult34 = lerp( tex2D( _CloudMap, flowA27 ) , tex2D( _CloudMap, flowB28 ) , float4( 0,0,0,0 ));
			float4 Cloud35 = lerpResult34;
			o.Albedo = Cloud35.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
2058;34;1541;958;1545.07;895.2509;1;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;2;-3873.58,-208.6435;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-3978.729,-39.10745;Inherit;False;Property;_SpeedFlowmap;Speed Flow map;2;0;Create;True;0;0;0;False;0;False;0.05;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-3708.029,-115.4064;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;4;-3423.828,220.4807;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;6;-3220.961,221.5905;Inherit;True;Sawtooth Wave;-1;;2;289adb816c3ac6d489f255fc3caf5016;0;1;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;5;-3225.963,-116.0674;Inherit;True;Sawtooth Wave;-1;;3;289adb816c3ac6d489f255fc3caf5016;0;1;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;8;-3010.956,-117.0025;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;7;-3005.954,220.6555;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-3349.394,-504.1698;Inherit;True;Property;_Flowmapcloud;Flowmap cloud;0;0;Create;True;0;0;0;False;0;False;-1;9129cd476c9dc9845a4bbd2f5a001672;9129cd476c9dc9845a4bbd2f5a001672;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;9;-2817.613,-118.1454;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-3309.24,-1048.044;Inherit;False;Property;_TilingFlow;TilingFlow;3;0;Create;True;0;0;0;False;0;False;0;0;-1;-1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;10;-2812.612,219.5125;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-2963.712,-834.7768;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;14;-3017.659,-505.3809;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-3002.647,-1064.686;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;-2670.461,-124.0435;Float;True;timeA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;-2665.459,213.6146;Float;True;timeB;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;-2721.647,-1071.686;Float;False;Diff;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-2696.205,-770.7958;Inherit;False;16;timeA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-2706.913,-433.8074;Inherit;False;13;timeB;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;20;-2716.907,-676.3376;Inherit;False;Overlay;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;23;-2482.557,-835.8598;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-2491.157,-633.1608;Inherit;False;21;Diff;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;22;-2471.602,-475.9279;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-2255.701,-480.2299;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-2300.271,-790.5588;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;28;-2096.084,-488.2719;Float;True;flowB;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-2091.09,-796.6638;Float;True;flowA;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-1654.526,-861.3508;Inherit;True;27;flowA;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;31;-1664.51,-640.4428;Inherit;True;Property;_CloudMap;CloudMap;1;0;Create;True;0;0;0;False;0;False;040b5d14c0963fd47981e050448e69c3;040b5d14c0963fd47981e050448e69c3;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;29;-1643.753,-419.1269;Inherit;True;28;flowB;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;32;-1388.292,-533.6259;Inherit;True;Global;TextureSample0;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;33;-1396.36,-751.0458;Inherit;True;Global;CloudMap002;CloudMap002;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;34;-1075.645,-645.3157;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-890.8303,-651.0798;Float;True;Cloud;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-618.7979,-647.3379;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;wow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;3;1;1;0
WireConnection;4;0;3;0
WireConnection;6;1;4;0
WireConnection;5;1;3;0
WireConnection;8;0;5;0
WireConnection;7;0;6;0
WireConnection;9;0;8;0
WireConnection;10;0;7;0
WireConnection;14;0;12;0
WireConnection;15;0;11;0
WireConnection;16;0;9;0
WireConnection;13;0;10;0
WireConnection;21;0;15;0
WireConnection;20;0;17;0
WireConnection;20;1;14;0
WireConnection;23;0;17;0
WireConnection;23;1;20;0
WireConnection;23;2;18;0
WireConnection;22;0;17;0
WireConnection;22;1;20;0
WireConnection;22;2;19;0
WireConnection;25;0;22;0
WireConnection;25;1;24;0
WireConnection;26;0;23;0
WireConnection;26;1;24;0
WireConnection;28;0;25;0
WireConnection;27;0;26;0
WireConnection;32;0;31;0
WireConnection;32;1;29;0
WireConnection;33;0;31;0
WireConnection;33;1;30;0
WireConnection;34;0;33;0
WireConnection;34;1;32;0
WireConnection;35;0;34;0
WireConnection;0;0;35;0
ASEEND*/
//CHKSM=C5D5C03FEAF51544A05E4317A3303CE82B6B349A