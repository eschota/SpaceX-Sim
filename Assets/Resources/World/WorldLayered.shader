// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WorldLayered"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_MaskTex("MaskTex", 2D) = "white" {}
		_PatternC("PatternC", 2D) = "white" {}
		_PatternA("PatternA", 2D) = "white" {}
		_Patternb("Patternb", 2D) = "white" {}
		_HologramDistort("Hologram Distort", 2D) = "white" {}
		_Tiling1("Tiling", Vector) = (1,1,0,0)
		_ColorTint("Color Tint", Color) = (0.8867924,0.2300641,0.2300641,0)
		_PowerColor("PowerColor", Float) = 2.43
		_ColoArBG("ColoArBG", Color) = (0.009745464,0.02830189,0.02592902,0)
		_ColorBrBG("ColorBrBG", Color) = (0.007386964,0.01533531,0.01886791,0)
		_speed("speed", Float) = 0.2
		_PowerScanline("PowerScanline", Float) = 0.73
		_smoothness("smoothness", Float) = 0
		_Metallic("Metallic", Float) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
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
		uniform sampler2D _PatternC;
		uniform float4 _PatternC_ST;
		uniform float _Metallic;
		uniform float _smoothness;
		uniform sampler2D _HologramDistort;
		uniform float2 _Tiling1;
		uniform float _speed;
		uniform float _PowerScanline;


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
			float4 temp_cast_3 = (60.0).xxxx;
			return temp_cast_3;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float mulTime51 = _Time.y * 0.1;
			float2 temp_cast_0 = (mulTime51).xx;
			float dotResult4_g1 = dot( temp_cast_0 , float2( 12.9898,78.233 ) );
			float lerpResult10_g1 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g1 ) * 43758.55 ) ));
			float temp_output_53_0 = lerpResult10_g1;
			float2 temp_cast_1 = (( ase_screenPosNorm.y * temp_output_53_0 )).xx;
			float simplePerlin2D58 = snoise( temp_cast_1*25.0 );
			float lerpResult59 = lerp( simplePerlin2D58 , 0.0 , ( temp_output_53_0 < 0.95 ? 1.0 : 0.8 ));
			float temp_output_60_0 = ( lerpResult59 * 0.01 );
			float3 temp_cast_2 = (temp_output_60_0).xxx;
			v.vertex.xyz += temp_cast_2;
			v.vertex.w = 1;
		}

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
			float4 temp_output_20_0 = ( ( ( _ColorTint * lerpResult5 ) + lerpResult14 + lerpResult34 ) * _PowerColor );
			o.Albedo = temp_output_20_0.rgb;
			float2 uv_PatternC = i.uv_texcoord * _PatternC_ST.xy + _PatternC_ST.zw;
			o.Emission = ( temp_output_20_0 + tex2D( _PatternC, uv_PatternC ) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _smoothness;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 temp_cast_2 = (( _Time.y * _speed )).xx;
			float2 uv_TexCoord45 = i.uv_texcoord * _Tiling1 + temp_cast_2;
			float4 color68 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 lerpResult70 = lerp( ( 1.0 - tex2D( _HologramDistort, ( ase_screenPosNorm + float4( uv_TexCoord45, 0.0 , 0.0 ) ).xy ) ) , color68 , _PowerScanline);
			float mulTime51 = _Time.y * 0.1;
			float2 temp_cast_5 = (mulTime51).xx;
			float dotResult4_g1 = dot( temp_cast_5 , float2( 12.9898,78.233 ) );
			float lerpResult10_g1 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g1 ) * 43758.55 ) ));
			float temp_output_53_0 = lerpResult10_g1;
			float2 temp_cast_6 = (( ase_screenPosNorm.y * temp_output_53_0 )).xx;
			float simplePerlin2D58 = snoise( temp_cast_6*25.0 );
			float lerpResult59 = lerp( simplePerlin2D58 , 0.0 , ( temp_output_53_0 < 0.95 ? 1.0 : 0.8 ));
			float temp_output_60_0 = ( lerpResult59 * 0.01 );
			o.Alpha = ( lerpResult70 + temp_output_60_0 ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
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
				float4 screenPos : TEXCOORD3;
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
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.screenPos = IN.screenPos;
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
36;157;1424;714;-1482.61;752.6858;1;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;21;-373.2709,281.211;Inherit;True;Property;_PatternA;PatternA;3;0;Create;True;0;0;0;False;0;False;55e1aa774c84c84428a10b54249515b5;106d49ec43601e44f87600f3128cc257;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;3;-361.4531,67.04396;Inherit;True;Property;_MaskTex;MaskTex;1;0;Create;True;0;0;0;False;0;False;55e1aa774c84c84428a10b54249515b5;55e1aa774c84c84428a10b54249515b5;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;4;-126.7652,66.42065;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;40;121.4491,1407.571;Inherit;False;1544.499;519.1493;Comment;12;50;49;48;47;46;45;44;43;42;41;68;71;Opacity;0.4056491,1,0.08962262,1;0;0
Node;AmplifyShaderEditor.SamplerNode;22;-138.5827,280.5877;Inherit;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;23;170.3609,284.3247;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;7;170.1949,70.17067;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;39;-40.10543,1995.322;Inherit;False;1694.231;791.6746;Comment;11;61;60;59;58;57;56;55;54;53;52;51;Distort;0.7872605,0.02678891,0.8113208,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;41;182.2321,1808.097;Inherit;False;Property;_speed;speed;11;0;Create;True;0;0;0;False;0;False;0.2;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;42;183.3789,1635.908;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;32;-374.0763,491.1074;Inherit;True;Property;_Patternb;Patternb;4;0;Create;True;0;0;0;False;0;False;55e1aa774c84c84428a10b54249515b5;8ff7c8146fd10a24189196fa88e129fd;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;400.8467,1786.525;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;44;395.8174,1635.841;Inherit;False;Property;_Tiling1;Tiling;6;0;Create;True;0;0;0;False;0;False;1,1;1,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;1;-124.912,-391.0245;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;ba7b6ead7212fd14bb35b2db3aaed84a;c886c23c800bfc045bff214c5b33090d;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleAddOpNode;25;335.3609,186.3247;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;51;9.894587,2246.546;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;593.6688,1739.333;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenPosInputsNode;52;448.9538,2045.321;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;28;575.4777,21.49746;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;46;568.818,1469.841;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;107.1757,-392.0478;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;30;-139.388,490.4842;Inherit;True;Property;_TextureSample3;Texture Sample 2;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;53;198.7305,2246.969;Inherit;False;Random Range;-1;;1;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;682.8852,2345.494;Inherit;False;Constant;_ScaleNoise;ScaleNoise;11;0;Create;True;0;0;0;False;0;False;25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;13;922.0705,-341.1317;Inherit;False;Property;_ColoArBG;ColoArBG;9;0;Create;True;0;0;0;False;0;False;0.009745464,0.02830189,0.02592902,0;0.01628694,0.05660379,0.05660379,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;47;821.8181,1649.841;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;15;924.8355,-152.4157;Inherit;False;Property;_ColorBrBG;ColorBrBG;10;0;Create;True;0;0;0;False;0;False;0.007386964,0.01533531,0.01886791,0;0.007386964,0.01533531,0.01886791,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;33;343.0579,494.9724;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;6;621.2894,-561.4408;Inherit;False;Property;_ColorTint;Color Tint;7;0;Create;True;0;0;0;False;0;False;0.8867924,0.2300641,0.2300641,0;0.8604428,0.8862745,0.2313725,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;48;791.8423,1459.705;Inherit;True;Property;_HologramDistort;Hologram Distort;5;0;Create;True;0;0;0;False;0;False;d288b8940e5ac6e42990d89cafefce73;d288b8940e5ac6e42990d89cafefce73;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;5;603.4874,-387.8874;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;29;987.2434,22.34895;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;728.0638,2219.55;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;49;1107.968,1457.571;Inherit;True;Property;_TextureSample5;Texture Sample 0;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;14;1204.191,-261.4683;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;34;922.7624,457.9661;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;58;926.9702,2214.848;Inherit;True;Simplex2D;False;False;2;0;FLOAT2;1,1;False;1;FLOAT;3.54;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;57;425.7097,2491.903;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0.95;False;2;FLOAT;1;False;3;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;924.192,-555.9411;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;50;1430.98,1461.553;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;36;-379.9517,710.0151;Inherit;True;Property;_PatternC;PatternC;2;0;Create;True;0;0;0;False;0;False;55e1aa774c84c84428a10b54249515b5;4e63fdc0b1432be448a9c5d80c05c822;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;11;1522.163,-335.2117;Inherit;False;Property;_PowerColor;PowerColor;8;0;Create;True;0;0;0;False;0;False;2.43;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;1243.345,2670.996;Inherit;False;Constant;_PowerDistort;PowerDistort;11;0;Create;True;0;0;0;False;0;False;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;68;1387.145,1677.763;Inherit;False;Constant;_Color0;Color 0;12;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;59;1185.253,2444.92;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;1476.613,-553.4661;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;71;1523.145,1876.763;Inherit;False;Property;_PowerScanline;PowerScanline;12;0;Create;True;0;0;0;False;0;False;0.73;0.73;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;1712.91,-553.0773;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;37;-145.2638,712.3918;Inherit;True;Property;_TextureSample4;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;1492.126,2445.483;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;70;1762.145,1448.763;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;72;2205.61,-627.6858;Inherit;False;Property;_smoothness;smoothness;13;0;Create;True;0;0;0;False;0;False;0;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;2157.818,-74.28091;Inherit;False;Constant;_teselation;teselation;12;0;Create;True;0;0;0;False;0;False;60;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;31;169.5556,494.2212;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;1989.331,-362.9056;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;1992.357,1460.14;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Compare;55;414.8947,2240.546;Inherit;True;2;4;0;FLOAT;0;False;1;FLOAT;0.98;False;2;FLOAT;1;False;3;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;2209.61,-708.6858;Inherit;False;Property;_Metallic;Metallic;14;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2454.125,-555.5369;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;WorldLayered;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;0
WireConnection;22;0;21;0
WireConnection;23;0;22;0
WireConnection;7;0;4;0
WireConnection;43;0;42;0
WireConnection;43;1;41;0
WireConnection;25;0;7;0
WireConnection;25;1;23;0
WireConnection;45;0;44;0
WireConnection;45;1;43;0
WireConnection;28;0;25;0
WireConnection;28;2;7;0
WireConnection;2;0;1;0
WireConnection;30;0;32;0
WireConnection;53;1;51;0
WireConnection;47;0;46;0
WireConnection;47;1;45;0
WireConnection;33;0;30;0
WireConnection;33;2;4;0
WireConnection;5;0;2;0
WireConnection;5;2;7;0
WireConnection;29;0;28;0
WireConnection;54;0;52;2
WireConnection;54;1;53;0
WireConnection;49;0;48;0
WireConnection;49;1;47;0
WireConnection;14;0;13;0
WireConnection;14;1;15;0
WireConnection;14;2;29;0
WireConnection;34;1;6;0
WireConnection;34;2;33;0
WireConnection;58;0;54;0
WireConnection;58;1;56;0
WireConnection;57;0;53;0
WireConnection;8;0;6;0
WireConnection;8;1;5;0
WireConnection;50;0;49;0
WireConnection;59;0;58;0
WireConnection;59;2;57;0
WireConnection;18;0;8;0
WireConnection;18;1;14;0
WireConnection;18;2;34;0
WireConnection;20;0;18;0
WireConnection;20;1;11;0
WireConnection;37;0;36;0
WireConnection;60;0;59;0
WireConnection;60;1;61;0
WireConnection;70;0;50;0
WireConnection;70;1;68;0
WireConnection;70;2;71;0
WireConnection;31;0;30;0
WireConnection;38;0;20;0
WireConnection;38;1;37;0
WireConnection;62;0;70;0
WireConnection;62;1;60;0
WireConnection;55;0;53;0
WireConnection;0;0;20;0
WireConnection;0;2;38;0
WireConnection;0;3;73;0
WireConnection;0;4;72;0
WireConnection;0;9;62;0
WireConnection;0;11;60;0
WireConnection;0;14;63;0
ASEEND*/
//CHKSM=B082EBB9BE9DC078E7263FE0E9385B6592D5A10E