// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WorldLayered"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_MaskTex("MaskTex", 2D) = "white" {}
		_PatternA("PatternA", 2D) = "white" {}
		_Patternb("Patternb", 2D) = "white" {}
		_ColorTint("Color Tint", Color) = (0.8867924,0.2300641,0.2300641,0)
		_PowerColor("PowerColor", Float) = 2.43
		_ColoArBG("ColoArBG", Color) = (0.009745464,0.02830189,0.02592902,0)
		_ColorBrBG("ColorBrBG", Color) = (0.007386964,0.01533531,0.01886791,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _ColorTint;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform sampler2D _MaskTex;
		uniform float4 _MaskTex_ST;
		uniform float4 _ColoArBG;
		uniform float4 _ColorBrBG;
		uniform sampler2D _PatternA;
		uniform float4 _PatternA_ST;
		uniform sampler2D _Patternb;
		uniform float4 _Patternb_ST;
		uniform float _PowerColor;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 uv_MaskTex = i.uv_texcoord * _MaskTex_ST.xy + _MaskTex_ST.zw;
			float4 tex2DNode4 = tex2D( _MaskTex, uv_MaskTex );
			float4 temp_output_7_0 = ( 1.0 - tex2DNode4 );
			float4 lerpResult5 = lerp( tex2D( _MainTex, uv_MainTex ) , float4( 0,0,0,0 ) , temp_output_7_0);
			float2 uv_PatternA = i.uv_texcoord * _PatternA_ST.xy + _PatternA_ST.zw;
			float4 lerpResult28 = lerp( ( temp_output_7_0 + ( 1.0 - tex2D( _PatternA, uv_PatternA ) ) ) , float4( 0,0,0,0 ) , temp_output_7_0);
			float4 lerpResult14 = lerp( _ColoArBG , _ColorBrBG , ( 1.0 - lerpResult28 ));
			float2 uv_Patternb = i.uv_texcoord * _Patternb_ST.xy + _Patternb_ST.zw;
			float4 tex2DNode30 = tex2D( _Patternb, uv_Patternb );
			float4 lerpResult33 = lerp( tex2DNode30 , float4( 0,0,0,0 ) , tex2DNode4);
			float4 lerpResult34 = lerp( float4( 0,0,0,0 ) , _ColorTint , lerpResult33);
			o.Emission = ( ( ( _ColorTint * lerpResult5 ) + lerpResult14 + lerpResult34 ) * _PowerColor ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
83;186;1424;714;-567.5364;532.6786;1.127721;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;3;-361.4531,67.04396;Inherit;True;Property;_MaskTex;MaskTex;1;0;Create;True;0;0;0;False;0;False;55e1aa774c84c84428a10b54249515b5;55e1aa774c84c84428a10b54249515b5;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;21;-373.2709,281.211;Inherit;True;Property;_PatternA;PatternA;2;0;Create;True;0;0;0;False;0;False;55e1aa774c84c84428a10b54249515b5;00ab95dd645be8b499b179701a38a53d;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;4;-126.7652,66.42065;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-138.5827,280.5877;Inherit;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;23;170.3609,284.3247;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;7;170.1949,70.17067;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;335.3609,186.3247;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-124.912,-391.0245;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;ba7b6ead7212fd14bb35b2db3aaed84a;ba7b6ead7212fd14bb35b2db3aaed84a;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;32;-374.0763,491.1074;Inherit;True;Property;_Patternb;Patternb;3;0;Create;True;0;0;0;False;0;False;55e1aa774c84c84428a10b54249515b5;8ff7c8146fd10a24189196fa88e129fd;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;28;575.4777,21.49746;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;107.1757,-392.0478;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;30;-139.388,490.4842;Inherit;True;Property;_TextureSample3;Texture Sample 2;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;33;343.0579,494.9724;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;15;924.8355,-152.4157;Inherit;False;Property;_ColorBrBG;ColorBrBG;7;0;Create;True;0;0;0;False;0;False;0.007386964,0.01533531,0.01886791,0;0.007386964,0.01533531,0.01886791,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;5;603.4874,-387.8874;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;29;987.2434,22.34895;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;6;621.2894,-561.4408;Inherit;False;Property;_ColorTint;Color Tint;4;0;Create;True;0;0;0;False;0;False;0.8867924,0.2300641,0.2300641,0;1,0.04160944,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;13;922.0705,-341.1317;Inherit;False;Property;_ColoArBG;ColoArBG;6;0;Create;True;0;0;0;False;0;False;0.009745464,0.02830189,0.02592902,0;0.01628694,0.05660379,0.05660379,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;924.192,-555.9411;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;14;1204.191,-261.4683;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;34;922.7624,457.9661;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;1522.163,-335.2117;Inherit;False;Property;_PowerColor;PowerColor;5;0;Create;True;0;0;0;False;0;False;2.43;2.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;1476.613,-553.4661;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;1712.91,-552.0773;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;31;169.5556,494.2212;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1927.219,-597.4766;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;WorldLayered;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;0
WireConnection;22;0;21;0
WireConnection;23;0;22;0
WireConnection;7;0;4;0
WireConnection;25;0;7;0
WireConnection;25;1;23;0
WireConnection;28;0;25;0
WireConnection;28;2;7;0
WireConnection;2;0;1;0
WireConnection;30;0;32;0
WireConnection;33;0;30;0
WireConnection;33;2;4;0
WireConnection;5;0;2;0
WireConnection;5;2;7;0
WireConnection;29;0;28;0
WireConnection;8;0;6;0
WireConnection;8;1;5;0
WireConnection;14;0;13;0
WireConnection;14;1;15;0
WireConnection;14;2;29;0
WireConnection;34;1;6;0
WireConnection;34;2;33;0
WireConnection;18;0;8;0
WireConnection;18;1;14;0
WireConnection;18;2;34;0
WireConnection;20;0;18;0
WireConnection;20;1;11;0
WireConnection;31;0;30;0
WireConnection;0;2;20;0
ASEEND*/
//CHKSM=AEA87F484BF44B4722BBEFB07DC0B5B3A5288C69