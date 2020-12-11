// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Antonio/Ocean"
{
	Properties
	{
		_WaveSpeed("Wave Speed", Float) = 1
		_WaveDirection("Wave Direction", Vector) = (1,0,0,0)
		_WaveStreach("Wave Streach", Vector) = (0.23,0.01,0,0)
		_WaveTile("Wave Tile", Float) = 1
		_Tessellation("Tessellation", Float) = 1
		_TessellationMin("Tessellation Min", Float) = 0
		_TessellationMax("Tessellation Max", Float) = 150
		_WaveHeight("Wave Height", Float) = 1
		_Smoothness("Smoothness", Float) = 1
		_Metallic("Metallic", Float) = 1
		_TopColor("Top Color", Color) = (0,0,0,0)
		_WaterColor("Water Color", Color) = (0,0,0,0)
		_EdgeDistance("Edge Distance", Float) = 0
		_EdgePower("Edge Power", Range( 0 , 1)) = 0
		_NormalMap("Normal Map", 2D) = "white" {}
		_NormalTile("Normal Tile", Float) = 0
		_NormalDirectionA("Normal Direction A", Vector) = (1,0,0,0)
		_NormalDirectionB("Normal Direction B", Vector) = (1,0,0,0)
		_NormalStrength("Normal Strength", Range( 0 , 1)) = 1
		_NormalSpeed("Normal Speed", Float) = 0
		_RefractAmount("Refract Amount", Float) = 0
		_SeaFoam("Sea Foam", 2D) = "white" {}
		_FoamTile("Foam Tile", Float) = 1
		_SeaFoamTile("Sea Foam Tile", Float) = 1
		_FoamMask("Foam Mask", Float) = 1
		_Depth("Depth", Float) = 0
		_Wave("Wave", Float) = 0
		_Opacity("Opacity", Float) = 0.6
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		AlphaToMask On
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#endif//ASE Sampling Macros

		#pragma surface surf Standard alpha:fade keepalpha vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform float _WaveHeight;
		uniform float _WaveSpeed;
		uniform float2 _WaveDirection;
		uniform float2 _WaveStreach;
		uniform float _WaveTile;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_NormalMap);
		uniform float2 _NormalDirectionA;
		uniform float _NormalSpeed;
		uniform float _NormalTile;
		SamplerState sampler_NormalMap;
		uniform float _NormalStrength;
		uniform float2 _NormalDirectionB;
		uniform float4 _WaterColor;
		uniform float4 _TopColor;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_SeaFoam);
		uniform float _SeaFoamTile;
		SamplerState sampler_SeaFoam;
		uniform float _FoamMask;
		uniform float _Wave;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _RefractAmount;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth;
		uniform float _EdgeDistance;
		uniform float _FoamTile;
		uniform float _EdgePower;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _Opacity;
		uniform float _TessellationMin;
		uniform float _TessellationMax;
		uniform float _Tessellation;


		float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod3D289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


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


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessellationMin,_TessellationMax,_Tessellation);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float temp_output_7_0 = ( _Time.y * _WaveSpeed );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult10 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceTile11 = appendResult10;
			float4 WaveTileUV21 = ( ( WorldSpaceTile11 * float4( _WaveStreach, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner3 = ( temp_output_7_0 * _WaveDirection + WaveTileUV21.xy);
			float simplePerlin3D1 = snoise( float3( panner3 ,  0.0 ) );
			float2 panner23 = ( temp_output_7_0 * _WaveDirection + ( WaveTileUV21 * float4( 0.1,0.1,0,0 ) ).xy);
			float simplePerlin3D24 = snoise( float3( panner23 ,  0.0 )*0.89 );
			float temp_output_26_0 = ( simplePerlin3D1 + simplePerlin3D24 );
			float3 WaveHeight31 = ( ( float3(0,1,0) * _WaveHeight ) * temp_output_26_0 );
			v.vertex.xyz += WaveHeight31;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 appendResult10 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 WorldSpaceTile11 = appendResult10;
			float4 temp_output_64_0 = ( ( WorldSpaceTile11 / 2.0 ) * _NormalTile );
			float2 panner68 = ( 1.0 * _Time.y * ( _NormalDirectionA * _NormalSpeed ) + temp_output_64_0.xy);
			float2 panner69 = ( 1.0 * _Time.y * ( _NormalDirectionB * ( _NormalSpeed * 3.0 ) ) + ( temp_output_64_0 * ( _NormalTile * 5.0 ) ).xy);
			float3 Normals83 = BlendNormals( UnpackScaleNormal( SAMPLE_TEXTURE2D( _NormalMap, sampler_NormalMap, panner68 ), _NormalStrength ) , UnpackScaleNormal( SAMPLE_TEXTURE2D( _NormalMap, sampler_NormalMap, panner69 ), _NormalStrength ) );
			o.Normal = Normals83;
			float2 panner109 = ( 1.0 * _Time.y * float2( 0.1,0 ) + ( WorldSpaceTile11 * _FoamMask ).xy);
			float simplePerlin2D108 = snoise( panner109 );
			float4 clampResult115 = clamp( ( SAMPLE_TEXTURE2D( _SeaFoam, sampler_SeaFoam, ( ( WorldSpaceTile11 / 1.0 ) * _SeaFoamTile ).xy ) * simplePerlin2D108 ) , float4( 0,0,0,0 ) , float4( 0.3584906,0.3584906,0.3584906,0 ) );
			float4 SeaFoam105 = clampResult115;
			float temp_output_7_0 = ( _Time.y * _WaveSpeed );
			float4 WaveTileUV21 = ( ( WorldSpaceTile11 * float4( _WaveStreach, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner3 = ( temp_output_7_0 * _WaveDirection + WaveTileUV21.xy);
			float simplePerlin3D1 = snoise( float3( panner3 ,  0.0 ) );
			float2 panner23 = ( temp_output_7_0 * _WaveDirection + ( WaveTileUV21 * float4( 0.1,0.1,0,0 ) ).xy);
			float simplePerlin3D24 = snoise( float3( panner23 ,  0.0 )*0.89 );
			float temp_output_26_0 = ( simplePerlin3D1 + simplePerlin3D24 );
			float WavePattern43 = temp_output_26_0;
			float clampResult44 = clamp( WavePattern43 , 0.0 , _Wave );
			float4 lerpResult41 = lerp( _WaterColor , ( _TopColor + SeaFoam105 ) , clampResult44);
			float4 Albedo45 = lerpResult41;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor123 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( float3( (ase_grabScreenPosNorm).xy ,  0.0 ) + ( _RefractAmount * Normals83 ) ).xy);
			float4 clampResult124 = clamp( screenColor123 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 Refraction122 = clampResult124;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth127 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth127 = abs( ( screenDepth127 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth ) );
			float clampResult129 = clamp( ( 1.0 - distanceDepth127 ) , 0.0 , 1.0 );
			float Depth130 = clampResult129;
			float4 lerpResult132 = lerp( Albedo45 , Refraction122 , Depth130);
			o.Albedo = lerpResult132.rgb;
			float screenDepth52 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth52 = abs( ( screenDepth52 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _EdgeDistance ) );
			float4 clampResult59 = clamp( ( ( ( 1.0 - distanceDepth52 ) + ( SAMPLE_TEXTURE2D( _SeaFoam, sampler_SeaFoam, ( ( WorldSpaceTile11 / 0.0 ) * _FoamTile ).xy ) * 0.08 ) ) * _EdgePower ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 Edge57 = clampResult59;
			o.Emission = Edge57.rgb;
			float clampResult140 = clamp( _Metallic , 0.0 , 1.0 );
			o.Metallic = clampResult140;
			o.Smoothness = _Smoothness;
			o.Alpha = _Opacity;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
2004;76;1779;983;1123.357;-148.0391;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;12;-4241.172,1752.243;Inherit;False;601.3202;241.845;World Space UVs;3;11;10;9;World Space UV;0.09518514,0.8773585,0.1057607,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;9;-4210.891,1799.209;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;10;-4014.681,1837.766;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;85;-1159.32,-1110.565;Inherit;False;2612.922;651.7131;Normals;21;78;63;65;64;66;71;76;79;77;67;80;68;69;82;61;37;62;83;81;86;87;Normals;0.4781845,0.1058824,0.9921569,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;34;-2775.193,-416.7419;Inherit;False;1562.204;646.331;Wave UV Height;11;31;30;20;19;29;15;16;17;14;13;21;Wave UV Height;0.8679245,0.5923784,0.1432894,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-3867.583,1832.162;Float;False;WorldSpaceTile;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-1104.021,-740.6026;Float;False;Constant;_Float0;Float 0;18;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;63;-1137.462,-1039.537;Inherit;True;11;WorldSpaceTile;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;13;-2750.457,-366.7418;Inherit;False;11;WorldSpaceTile;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;15;-2720.484,-282.3557;Float;False;Property;_WaveStreach;Wave Streach;2;0;Create;True;0;0;0;False;0;False;0.23,0.01;0.2,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;65;-1122.502,-663.9985;Float;False;Property;_NormalTile;Normal Tile;15;0;Create;True;0;0;0;False;0;False;0;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2518.924,-144.9189;Float;False;Property;_WaveTile;Wave Tile;3;0;Create;True;0;0;0;False;0;False;1;0.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-2499.505,-363.1238;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-517.6398,-786.1267;Float;False;Property;_NormalSpeed;Normal Speed;19;0;Create;True;0;0;0;False;0;False;0;0.005;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;86;-897.1701,-1035.643;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-2346.764,-284.8369;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-753.8947,-1034.626;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-861.9949,-658.494;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;76;-531.8517,-621.0526;Float;False;Property;_NormalDirectionB;Normal Direction B;17;0;Create;True;0;0;0;False;0;False;1,0;0.6,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;71;-523.6986,-976.5924;Float;False;Property;_NormalDirectionA;Normal Direction A;16;0;Create;True;0;0;0;False;0;False;1,0;-0.62,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-171.0042,-598.0812;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;-3466.382,2196.316;Float;False;Constant;_Float2;Float 2;19;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;99;-3560.586,2411.333;Inherit;False;11;WorldSpaceTile;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-3497.136,2613.454;Float;False;Property;_FoamMask;Foam Mask;24;0;Create;True;0;0;0;False;0;False;1;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;26.61678,-622.6931;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;-2082.593,-289.3815;Float;False;WaveTileUV;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;35;-2780.361,305.1432;Inherit;False;1566.101;634.1342;Wave Pattern;13;43;26;24;1;3;23;28;7;5;22;27;8;6;Wave Pattern;0.8207547,0.1509879,0.8070652,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-176.3676,-1015.729;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-652.2911,-685.1642;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;103;-3322.478,2416.879;Float;False;Property;_SeaFoamTile;Sea Foam Tile;23;0;Create;True;0;0;0;False;0;False;1;1.62;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-24.93391,-842.7637;Float;False;Property;_NormalStrength;Normal Strength;18;0;Create;True;0;0;0;False;0;False;1;0.692;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-2729.929,640.7314;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;27;-2729.186,827.8341;Inherit;False;21;WaveTileUV;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2726.129,736.7316;Float;False;Property;_WaveSpeed;Wave Speed;0;0;Create;True;0;0;0;False;0;False;1;0.21;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;101;-3277.836,2171.714;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;97;-3579.862,1456.699;Inherit;False;1515.377;574.3428;Foam;10;95;141;89;142;91;93;92;90;94;88;Foam;0,1,0.8573189,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;61;293.3904,-897.2667;Float;True;Property;_NormalMap;Normal Map;14;0;Create;True;0;0;0;False;0;False;None;3e77c538df295e84280aedc8012dd868;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;68;297.0901,-1042.502;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;-3278.734,2591.355;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;69;305.8344,-684.2529;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;37;548.5143,-1060.565;Inherit;True;Property;_Texture;Texture;7;0;Create;True;0;0;0;False;0;False;-1;a648a6ea78e587a4292b1668391f3776;70b136ca3ad6a3741aa08f7ae1d770aa;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;22;-2730.361,355.1432;Inherit;False;21;WaveTileUV;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-3087.273,2285.746;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-2483.329,663.7313;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;90;-3529.862,1745.888;Inherit;False;11;WorldSpaceTile;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;109;-3065.535,2592.654;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;88;-3226.16,1584.007;Float;True;Property;_SeaFoam;Sea Foam;21;0;Create;True;0;0;0;False;0;False;None;e1690315a338a394dacf9a5111b8a8f1;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;62;561.457,-716.7289;Inherit;True;Property;_TextureSample0;Texture Sample 0;8;0;Create;True;0;0;0;False;0;False;-1;a648a6ea78e587a4292b1668391f3776;a648a6ea78e587a4292b1668391f3776;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;94;-3470.735,1853.232;Float;False;Constant;_Float1;Float 1;19;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;5;-2730.118,486.2226;Float;False;Property;_WaveDirection;Wave Direction;1;0;Create;True;0;0;0;False;0;False;1,0;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-2490.009,831.9982;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.1,0.1,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-3295.508,1906.541;Float;False;Property;_FoamTile;Foam Tile;22;0;Create;True;0;0;0;False;0;False;1;4.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;93;-3269.709,1783.31;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BlendNormalsNode;81;897.231,-889.5634;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;3;-2269.101,362.061;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;23;-2282.05,616.5175;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;98;-2884.32,2260.593;Inherit;True;Property;_TextureSample2;Texture Sample 2;19;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;108;-2760.034,2586.155;Inherit;False;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;60;-3174.357,1069.641;Inherit;False;2015.075;341.1382;Edge;8;57;59;55;54;56;52;53;96;Edge;0,0.6430383,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-3124.357,1135.911;Float;False;Property;_EdgeDistance;Edge Distance;12;0;Create;True;0;0;0;False;0;False;0;3.68;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;83;1212.002,-896.8685;Float;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;24;-1995.051,608.5175;Inherit;False;Simplex3D;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.89;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-3094.146,1835.342;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-2440.684,2406.649;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-1996.061,355.4044;Inherit;False;Simplex3D;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;126;-545.8386,-1797.847;Inherit;False;1540.6;418.1692;Refraction;9;118;116;119;117;121;123;120;124;122;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;142;-2766.741,1918.533;Inherit;False;Constant;_Float3;Float 3;25;0;Create;True;0;0;0;False;0;False;0.08;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;115;-2241.784,2405.351;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.3584906,0.3584906,0.3584906,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-495.8386,-1590.186;Float;False;Property;_RefractAmount;Refract Amount;20;0;Create;True;0;0;0;False;0;False;0;0.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-1747.266,473.9714;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-477.8386,-1508.186;Inherit;False;83;Normals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GrabScreenPosition;116;-315.1832,-1746.794;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;89;-2875.769,1693.122;Inherit;True;Property;_TextureSample1;Texture Sample 1;18;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;52;-2866.098,1119.641;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;105;-2043.435,2399.359;Float;True;SeaFoam;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;47;-3090.528,-1516.431;Inherit;False;1627.764;1005.206;Albedo;9;45;41;38;44;39;42;106;107;144;Albedo;0.8962264,0.3001513,0.3001513,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;117;-67.44591,-1747.847;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;128;-568.45,-2198.561;Float;False;Property;_Depth;Depth;25;0;Create;True;0;0;0;False;0;False;0;15.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;-220.8386,-1557.186;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-2565.142,1817.733;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;54;-2592.777,1128.603;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;43;-1483.032,473.8027;Float;False;WavePattern;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;144;-2824.297,-753.7881;Inherit;False;Property;_Wave;Wave;26;0;Create;True;0;0;0;False;0;False;0;0.96;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;127;-377.2369,-2216.438;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;42;-2873.804,-859.4695;Inherit;False;43;WavePattern;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;121;193.1614,-1583.186;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;107;-2857.293,-946.218;Inherit;False;105;SeaFoam;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;95;-2289.286,1796.599;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;39;-2914.396,-1211.423;Float;False;Property;_TopColor;Top Color;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.009077947,0.5261022,0.6415094,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;19;-2088.06,-211.2157;Float;False;Constant;_WaveUp;Wave Up;5;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;38;-2919.365,-1390.62;Float;False;Property;_WaterColor;Water Color;11;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0.4404063,0.5943396,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;56;-2013.451,1237.396;Float;False;Property;_EdgePower;Edge Power;13;0;Create;True;0;0;0;False;0;False;0;0.69;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2076.45,44.65912;Float;False;Property;_WaveHeight;Wave Height;7;0;Create;True;0;0;0;False;0;False;1;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;123;350.9729,-1588.678;Float;False;Global;_GrabScreen0;Grab Screen 0;21;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;44;-2610.852,-852.8226;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;96;-2262.367,1165.533;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;106;-2584.377,-1099.522;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;135;-74.78345,-2218.055;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;124;586.9729,-1584.678;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;41;-2361.14,-1125.045;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;129;173.6501,-2218.261;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-1882.547,-81.68002;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-1709.451,1125.396;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;122;753.1614,-1590.186;Float;False;Refraction;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1673.977,-77.38268;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;59;-1549.451,1125.396;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-2035.285,-1129.176;Float;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;130;354.6501,-2221.261;Float;False;Depth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;134;-584.1165,354.0823;Inherit;False;130;Depth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;139;-595.927,1003.286;Float;False;Property;_TessellationMax;Tessellation Max;6;0;Create;True;0;0;0;False;0;False;150;120;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-593.927,925.2864;Float;False;Property;_TessellationMin;Tessellation Min;5;0;Create;True;0;0;0;False;0;False;0;80;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-1373.451,1125.396;Float;False;Edge;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-1486.172,-88.81378;Float;True;WaveHeight;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;-608.922,237.4408;Inherit;False;122;Refraction;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-525.9271,475.2864;Float;False;Property;_Metallic;Metallic;9;0;Create;True;0;0;0;False;0;False;1;0.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-599.0818,125.6308;Inherit;False;45;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-518.598,834.6873;Float;False;Property;_Tessellation;Tessellation;4;0;Create;True;0;0;0;False;0;False;1;120;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-169.9798,510.5104;Float;False;Property;_Smoothness;Smoothness;8;0;Create;True;0;0;0;False;0;False;1;0.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;132;-263.8573,165.7548;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;-296.1706,673.6831;Inherit;False;31;WaveHeight;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;145;-331.5349,573.7797;Inherit;False;Property;_Opacity;Opacity;27;0;Create;True;0;0;0;False;0;False;0.6;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;143;-722.6755,540.6711;Inherit;False;122;Refraction;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;-257.7194,309.9417;Inherit;False;83;Normals;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;140;-305.927,453.2864;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceBasedTessNode;137;-329.9271,880.2864;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-252.9527,389.5078;Inherit;False;57;Edge;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;34.51595,358.2055;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Antonio/Ocean;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;9;1
WireConnection;10;1;9;3
WireConnection;11;0;10;0
WireConnection;14;0;13;0
WireConnection;14;1;15;0
WireConnection;86;0;63;0
WireConnection;86;1;87;0
WireConnection;16;0;14;0
WireConnection;16;1;17;0
WireConnection;64;0;86;0
WireConnection;64;1;65;0
WireConnection;66;0;65;0
WireConnection;79;0;78;0
WireConnection;80;0;76;0
WireConnection;80;1;79;0
WireConnection;21;0;16;0
WireConnection;77;0;71;0
WireConnection;77;1;78;0
WireConnection;67;0;64;0
WireConnection;67;1;66;0
WireConnection;101;0;99;0
WireConnection;101;1;100;0
WireConnection;68;0;64;0
WireConnection;68;2;77;0
WireConnection;110;0;99;0
WireConnection;110;1;111;0
WireConnection;69;0;67;0
WireConnection;69;2;80;0
WireConnection;37;0;61;0
WireConnection;37;1;68;0
WireConnection;37;5;82;0
WireConnection;102;0;101;0
WireConnection;102;1;103;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;109;0;110;0
WireConnection;62;0;61;0
WireConnection;62;1;69;0
WireConnection;62;5;82;0
WireConnection;28;0;27;0
WireConnection;93;0;90;0
WireConnection;93;1;94;0
WireConnection;81;0;37;0
WireConnection;81;1;62;0
WireConnection;3;0;22;0
WireConnection;3;2;5;0
WireConnection;3;1;7;0
WireConnection;23;0;28;0
WireConnection;23;2;5;0
WireConnection;23;1;7;0
WireConnection;98;0;88;0
WireConnection;98;1;102;0
WireConnection;108;0;109;0
WireConnection;83;0;81;0
WireConnection;24;0;23;0
WireConnection;91;0;93;0
WireConnection;91;1;92;0
WireConnection;114;0;98;0
WireConnection;114;1;108;0
WireConnection;1;0;3;0
WireConnection;115;0;114;0
WireConnection;26;0;1;0
WireConnection;26;1;24;0
WireConnection;89;0;88;0
WireConnection;89;1;91;0
WireConnection;52;0;53;0
WireConnection;105;0;115;0
WireConnection;117;0;116;0
WireConnection;119;0;118;0
WireConnection;119;1;120;0
WireConnection;141;0;89;0
WireConnection;141;1;142;0
WireConnection;54;0;52;0
WireConnection;43;0;26;0
WireConnection;127;0;128;0
WireConnection;121;0;117;0
WireConnection;121;1;119;0
WireConnection;95;0;54;0
WireConnection;95;1;141;0
WireConnection;123;0;121;0
WireConnection;44;0;42;0
WireConnection;44;2;144;0
WireConnection;96;0;95;0
WireConnection;106;0;39;0
WireConnection;106;1;107;0
WireConnection;135;0;127;0
WireConnection;124;0;123;0
WireConnection;41;0;38;0
WireConnection;41;1;106;0
WireConnection;41;2;44;0
WireConnection;129;0;135;0
WireConnection;20;0;19;0
WireConnection;20;1;29;0
WireConnection;55;0;96;0
WireConnection;55;1;56;0
WireConnection;122;0;124;0
WireConnection;30;0;20;0
WireConnection;30;1;26;0
WireConnection;59;0;55;0
WireConnection;45;0;41;0
WireConnection;130;0;129;0
WireConnection;57;0;59;0
WireConnection;31;0;30;0
WireConnection;132;0;46;0
WireConnection;132;1;133;0
WireConnection;132;2;134;0
WireConnection;140;0;136;0
WireConnection;137;0;18;0
WireConnection;137;1;138;0
WireConnection;137;2;139;0
WireConnection;0;0;132;0
WireConnection;0;1;84;0
WireConnection;0;2;58;0
WireConnection;0;3;140;0
WireConnection;0;4;36;0
WireConnection;0;9;145;0
WireConnection;0;11;32;0
WireConnection;0;14;137;0
ASEEND*/
//CHKSM=499AA3AE221558835A7742EB050D86192D8AEE0A