// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PlanetLit"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[ASEBegin]_Texture0("Texture 0", 2D) = "white" {}
		_ScaleFrenel("ScaleFrenel", Float) = 0.5
		_ColorFrenel("ColorFrenel", Color) = (0.9622642,0.7592671,0.3495016,0)
		_PowerFrenel("PowerFrenel", Float) = 0.5
		_Metallic("Metallic", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		[ASEEnd]_strenghtNormal("strenghtNormal", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		[HideInInspector] _RenderQueueType("Render Queue Type", Float) = 1
		[HideInInspector] [ToggleUI] _AddPrecomputedVelocity("Add Precomputed Velocity", Float) = 1
		[HideInInspector] _StencilRef("Stencil Ref", Int) = 0
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Int) = 6
		[HideInInspector] _StencilRefDepth("Stencil Ref Depth", Int) = 8
		[HideInInspector] _StencilWriteMaskDepth("Stencil Write Mask Depth", Int) = 8
		[HideInInspector] _StencilRefMV("Stencil Ref MV", Int) = 40
		[HideInInspector] _StencilWriteMaskMV("Stencil Write Mask MV", Int) = 40
		[HideInInspector] _StencilRefDistortionVec("Stencil Ref Distortion Vec", Int) = 4
		[HideInInspector] _StencilWriteMaskDistortionVec("Stencil Write Mask Distortion Vec", Int) = 4
		[HideInInspector] _StencilWriteMaskGBuffer("Stencil Write Mask GBuffer", Int) = 14
		[HideInInspector] _StencilRefGBuffer("Stencil Ref GBuffer", Int) = 10
		[HideInInspector] _ZTestGBuffer("ZTest GBuffer", Int) = 4
		[HideInInspector] [ToggleUI] _RequireSplitLighting("Require Split Lighting", Float) = 0
		[HideInInspector] [ToggleUI] _ReceivesSSR("Receives SSR", Float) = 1
		[HideInInspector] _SurfaceType("Surface Type", Float) = 0
		[HideInInspector] _BlendMode("Blend Mode", Float) = 0
		[HideInInspector] _SrcBlend("Src Blend", Float) = 1
		[HideInInspector] _DstBlend("Dst Blend", Float) = 0
		[HideInInspector] _AlphaSrcBlend("Alpha Src Blend", Float) = 1
		[HideInInspector] _AlphaDstBlend("Alpha Dst Blend", Float) = 0
		[HideInInspector] [ToggleUI] _ZWrite("ZWrite", Float) = 1
		[HideInInspector] [ToggleUI] _TransparentZWrite("Transparent ZWrite", Float) = 1
		[HideInInspector] _CullMode("Cull Mode", Float) = 2
		[HideInInspector] _TransparentSortPriority("Transparent Sort Priority", Int) = 0
		[HideInInspector] [ToggleUI] _EnableFogOnTransparent("Enable Fog On Transparent", Float) = 1
		[HideInInspector] _CullModeForward("Cull Mode Forward", Float) = 2
		[HideInInspector] [Enum(Front, 1, Back, 2)] _TransparentCullMode("Transparent Cull Mode", Float) = 2
		[HideInInspector] _ZTestDepthEqualForOpaque("ZTest Depth Equal For Opaque", Int) = 4
		[HideInInspector] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("ZTest Transparent", Float) = 4
		[HideInInspector] [ToggleUI] _TransparentBackfaceEnable("Transparent Backface Enable", Float) = 0
		[HideInInspector] [ToggleUI] _AlphaCutoffEnable("Alpha Cutoff Enable", Float) = 0
		[HideInInspector] [ToggleUI] _UseShadowThreshold("Use Shadow Threshold", Float) = 0
		[HideInInspector] [ToggleUI] _DoubleSidedEnable("Double Sided Enable", Float) = 0
		[HideInInspector] [Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Double Sided Normal Mode", Float) = 2
		[HideInInspector] _DoubleSidedConstants("DoubleSidedConstants", Vector) = (1,1,-1,0)
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Opaque" "Queue"="Geometry" }

		HLSLINCLUDE
		#pragma target 4.5
		#pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
		#pragma multi_compile_instancing
		#pragma instancing_options renderinglayer

		struct GlobalSurfaceDescription // GBuffer Forward META TransparentBackface
		{
			float3 Albedo;
			float3 Normal;
			float3 BentNormal;
			float3 Specular;
			float CoatMask;
			float Metallic;
			float3 Emission;
			float Smoothness;
			float Occlusion;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float AlphaClipThresholdDepthPrepass;
			float AlphaClipThresholdDepthPostpass;
			float SpecularAAScreenSpaceVariance;
			float SpecularAAThreshold;
			float SpecularOcclusion;
			float DepthOffset;
			//Refraction
			float RefractionIndex;
			float3 RefractionColor;
			float RefractionDistance;
			//SSS/Translucent
			float Thickness;
			float SubsurfaceMask;
			float DiffusionProfile;
			//Anisotropy
			float Anisotropy;
			float3 Tangent;
			//Iridescent
			float IridescenceMask;
			float IridescenceThickness;
			//BakedGI
			float3 BakedGI;
			float3 BakedBackGI;
		};

		struct AlphaSurfaceDescription // ShadowCaster
		{
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float DepthOffset;
		};

		struct SceneSurfaceDescription // SceneSelection
		{
			float Alpha;
			float AlphaClipThreshold;
			float DepthOffset;
		};

		struct PrePassSurfaceDescription // DepthPrePass
		{
			float Alpha;
			float AlphaClipThresholdDepthPrepass;
			float DepthOffset;
		};

		struct PostPassSurfaceDescription //DepthPostPass
		{
			float Alpha;
			float AlphaClipThresholdDepthPostpass;
			float DepthOffset;
		};

		struct SmoothSurfaceDescription // MotionVectors DepthOnly
		{
			float3 Normal;
			float Smoothness;
			float Alpha;
			float AlphaClipThreshold;
			float DepthOffset;
		};

		struct DistortionSurfaceDescription //Distortion
		{
			float Alpha;
			float2 Distortion;
			float DistortionBlur;
			float AlphaClipThreshold;
		};

		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlaneASE (float3 pos, float4 plane)
		{
			return dot (float4(pos,1.0f), plane);
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlaneASE(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlaneASE(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlaneASE(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlaneASE(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		ENDHLSL
		
		Pass
		{
			
			Name "GBuffer"
			Tags { "LightMode"="GBuffer" }

			Cull [_CullMode]
			ZTest [_ZTestGBuffer]

			Stencil
			{
				Ref [_StencilRefGBuffer]
				WriteMask [_StencilWriteMaskGBuffer]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			HLSLPROGRAM

			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 999999


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#if !defined(DEBUG_DISPLAY) && defined(_ALPHATEST_ON)
			#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
			#endif

			#define SHADERPASS SHADERPASS_GBUFFER
			#pragma multi_compile _ DEBUG_DISPLAY
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile _ LIGHT_LAYERS

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_WORLD_NORMAL


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float4 interp03 : TEXCOORD3;
				float4 interp04 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _Texture0_ST;
			float4 _Texture0_TexelSize;
			float4 _ColorFrenel;
			float _strenghtNormal;
			float _Metallic;
			float _ScaleFrenel;
			float _PowerFrenel;
			float _Smoothness;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Texture0;


			float3 CombineSamplesSharp128_g1( float S0, float S1, float S2, float Strength )
			{
				{
				    float3 va = float3( 0.13, 0, ( S1 - S0 ) * Strength );
				    float3 vb = float3( 0, 0.13, ( S2 - S0 ) * Strength );
				    return normalize( cross( va, vb ) );
				}
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.Albedo;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;
				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData( posInput, surfaceDescription.Alpha );
					ApplyDecalToSurfaceData( decalSurfaceData, surfaceData );
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				#ifdef _ASE_BAKEDGI
				builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				#if (SHADERPASS == SHADERPASS_DISTORTION)
				builtinData.distortion = surfaceDescription.Distortion;
				builtinData.distortionBlur = surfaceDescription.DistortionBlur;
				#else
				builtinData.distortion = float2(0.0, 0.0);
				builtinData.distortionBlur = 0.0;
				#endif

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				outputPackedVaryingsMeshToPS.ase_texcoord5.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord5.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;
				inputMesh.tangentOS =  inputMesh.tangentOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.interp03.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.interp04.xyzw = inputMesh.uv2;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput,
						OUTPUT_GBUFFER(outGBuffer)
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
						)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				float3 positionRWS = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;

				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.interp03.xyzw;
				input.texCoord2 = packedInput.interp04.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);
				SurfaceData surfaceData;
				BuiltinData builtinData;

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 uv_Texture0 = packedInput.ase_texcoord5.xy * _Texture0_ST.xy + _Texture0_ST.zw;
				float4 tex2DNode12 = tex2D( _Texture0, uv_Texture0 );
				
				float localCalculateUVsSharp110_g1 = ( 0.0 );
				float2 temp_output_85_0_g1 = uv_Texture0;
				float2 UV110_g1 = temp_output_85_0_g1;
				float4 TexelSize110_g1 = _Texture0_TexelSize;
				float2 UV0110_g1 = float2( 0,0 );
				float2 UV1110_g1 = float2( 0,0 );
				float2 UV2110_g1 = float2( 0,0 );
				{
				{
				    UV110_g1.y -= TexelSize110_g1.y * 0.5;
				    UV0110_g1 = UV110_g1;
				    UV1110_g1 = UV110_g1 + float2( TexelSize110_g1.x, 0 );
				    UV2110_g1 = UV110_g1 + float2( 0, TexelSize110_g1.y );
				}
				}
				float4 break134_g1 = tex2D( _Texture0, UV0110_g1 );
				float S0128_g1 = break134_g1.r;
				float4 break136_g1 = tex2D( _Texture0, UV1110_g1 );
				float S1128_g1 = break136_g1.r;
				float4 break138_g1 = tex2D( _Texture0, UV2110_g1 );
				float S2128_g1 = break138_g1.r;
				float temp_output_91_0_g1 = _strenghtNormal;
				float Strength128_g1 = temp_output_91_0_g1;
				float3 localCombineSamplesSharp128_g1 = CombineSamplesSharp128_g1( S0128_g1 , S1128_g1 , S2128_g1 , Strength128_g1 );
				
				float fresnelNdotV14 = dot( normalWS, V );
				float fresnelNode14 = ( 0.0 + _ScaleFrenel * pow( 1.0 - fresnelNdotV14, _PowerFrenel ) );
				
				surfaceDescription.Albedo = tex2DNode12.rgb;
				surfaceDescription.Normal = localCombineSamplesSharp128_g1;
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = _Metallic;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = ( _ColorFrenel * fresnelNode14 ).rgb;
				surfaceDescription.Smoothness = _Smoothness;
				surfaceDescription.Occlusion = 1;
				surfaceDescription.Alpha = tex2DNode12.a;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
				surfaceDescription.AlphaClipThresholdShadow = 0.5;
				#endif

				surfaceDescription.AlphaClipThresholdDepthPrepass = 0.5;
				surfaceDescription.AlphaClipThresholdDepthPostpass = 0.5;

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				#ifdef _ASE_DISTORTION
				surfaceDescription.Distortion = float2 ( 2, -1 );
				surfaceDescription.DistortionBlur = 1;
				#endif

				#ifdef _ASE_BAKEDGI
				surfaceDescription.BakedGI = 0;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = 0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				GetSurfaceAndBuiltinData( surfaceDescription, input, V, posInput, surfaceData, builtinData );
				ENCODE_INTO_GBUFFER( surfaceData, builtinData, posInput.positionSS, outGBuffer );
				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "META"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM

			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 999999


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_VERT_NORMAL


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _Texture0_ST;
			float4 _Texture0_TexelSize;
			float4 _ColorFrenel;
			float _strenghtNormal;
			float _Metallic;
			float _ScaleFrenel;
			float _PowerFrenel;
			float _Smoothness;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Texture0;


			float3 CombineSamplesSharp128_g1( float S0, float S1, float S2, float Strength )
			{
				{
				    float3 va = float3( 0.13, 0, ( S1 - S0 ) * Strength );
				    float3 vb = float3( 0, 0.13, ( S2 - S0 ) * Strength );
				    return normalize( cross( va, vb ) );
				}
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.Albedo;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;
				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData( posInput, surfaceDescription.Alpha );
					ApplyDecalToSurfaceData( decalSurfaceData, surfaceData );
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				builtinData.emissiveColor = surfaceDescription.Emission;

				#if (SHADERPASS == SHADERPASS_DISTORTION)
				builtinData.distortion = surfaceDescription.Distortion;
				builtinData.distortionBlur = surfaceDescription.DistortionBlur;
				#else
				builtinData.distortion = float2(0.0, 0.0);
				builtinData.distortionBlur = 0.0;
				#endif

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			CBUFFER_START(UnityMetaPass)
			bool4 unity_MetaVertexControl;
			bool4 unity_MetaFragmentControl;
			CBUFFER_END

			float unity_OneOverOutputBoost;
			float unity_MaxOutputValue;

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh  )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				outputPackedVaryingsMeshToPS.ase_texcoord1.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				outputPackedVaryingsMeshToPS.ase_texcoord2.xyz = ase_worldNormal;
				
				outputPackedVaryingsMeshToPS.ase_texcoord.xy = inputMesh.uv0.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord.zw = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord1.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord2.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;
				inputMesh.tangentOS =  inputMesh.tangentOS ;

				float2 uv = float2(0.0, 0.0);
				if (unity_MetaVertexControl.x)
				{
					uv = inputMesh.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				}
				else if (unity_MetaVertexControl.y)
				{
					uv = inputMesh.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				}

				outputPackedVaryingsMeshToPS.positionCS = float4(uv * 2.0 - 1.0, inputMesh.positionOS.z > 0 ? 1.0e-4 : 0.0, 1.0);
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv0 = v.uv0;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv0 = patch[0].uv0 * bary.x + patch[1].uv0 * bary.y + patch[2].uv0 * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			float4 Frag(PackedVaryingsMeshToPS packedInput  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = float3(1.0, 1.0, 1.0);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 uv_Texture0 = packedInput.ase_texcoord.xy * _Texture0_ST.xy + _Texture0_ST.zw;
				float4 tex2DNode12 = tex2D( _Texture0, uv_Texture0 );
				
				float localCalculateUVsSharp110_g1 = ( 0.0 );
				float2 temp_output_85_0_g1 = uv_Texture0;
				float2 UV110_g1 = temp_output_85_0_g1;
				float4 TexelSize110_g1 = _Texture0_TexelSize;
				float2 UV0110_g1 = float2( 0,0 );
				float2 UV1110_g1 = float2( 0,0 );
				float2 UV2110_g1 = float2( 0,0 );
				{
				{
				    UV110_g1.y -= TexelSize110_g1.y * 0.5;
				    UV0110_g1 = UV110_g1;
				    UV1110_g1 = UV110_g1 + float2( TexelSize110_g1.x, 0 );
				    UV2110_g1 = UV110_g1 + float2( 0, TexelSize110_g1.y );
				}
				}
				float4 break134_g1 = tex2D( _Texture0, UV0110_g1 );
				float S0128_g1 = break134_g1.r;
				float4 break136_g1 = tex2D( _Texture0, UV1110_g1 );
				float S1128_g1 = break136_g1.r;
				float4 break138_g1 = tex2D( _Texture0, UV2110_g1 );
				float S2128_g1 = break138_g1.r;
				float temp_output_91_0_g1 = _strenghtNormal;
				float Strength128_g1 = temp_output_91_0_g1;
				float3 localCombineSamplesSharp128_g1 = CombineSamplesSharp128_g1( S0128_g1 , S1128_g1 , S2128_g1 , Strength128_g1 );
				
				float3 ase_worldPos = packedInput.ase_texcoord1.xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = packedInput.ase_texcoord2.xyz;
				float fresnelNdotV14 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode14 = ( 0.0 + _ScaleFrenel * pow( 1.0 - fresnelNdotV14, _PowerFrenel ) );
				
				surfaceDescription.Albedo = tex2DNode12.rgb;
				surfaceDescription.Normal = localCombineSamplesSharp128_g1;
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = _Metallic;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = ( _ColorFrenel * fresnelNode14 ).rgb;
				surfaceDescription.Smoothness = _Smoothness;
				surfaceDescription.Occlusion = 1;
				surfaceDescription.Alpha = tex2DNode12.a;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);
				LightTransportData lightTransportData = GetLightTransportData(surfaceData, builtinData, bsdfData);

				float4 res = float4(0.0, 0.0, 0.0, 1.0);
				if (unity_MetaFragmentControl.x)
				{
					res.rgb = clamp(pow(abs(lightTransportData.diffuseColor), saturate(unity_OneOverOutputBoost)), 0, unity_MaxOutputValue);
				}

				if (unity_MetaFragmentControl.y)
				{
					res.rgb = lightTransportData.emissiveColor;
				}

				return res;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			Cull [_CullMode]
			ZWrite On
			ZClip [_ZClip]
			ZTest LEqual
			ColorMask 0

			HLSLPROGRAM

			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 999999


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_SHADOWS

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

			//#define USE_LEGACY_UNITY_MATRIX_VARIABLES

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _Texture0_ST;
			float4 _Texture0_TexelSize;
			float4 _ColorFrenel;
			float _strenghtNormal;
			float _Metallic;
			float _ScaleFrenel;
			float _PowerFrenel;
			float _Smoothness;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Texture0;


			
			void BuildSurfaceData(FragInputs fragInputs, inout AlphaSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData( posInput, surfaceDescription.Alpha );
					ApplyDecalToSurfaceData( decalSurfaceData, surfaceData );
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(AlphaSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				#ifdef _ALPHATEST_SHADOW_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow );
				#else
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				outputPackedVaryingsMeshToPS.ase_texcoord1.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord1.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif
			
			void Frag( PackedVaryingsMeshToPS packedInput
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target0
							#ifdef WRITE_MSAA_DEPTH
							, out float1 depthColor : SV_Target1
							#endif
						#elif defined(WRITE_MSAA_DEPTH)
						, out float4 outNormalBuffer : SV_Target0
						, out float1 depthColor : SV_Target1
						#elif defined(SCENESELECTIONPASS)
						, out float4 outColor : SV_Target0
						#endif
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.interp00.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				AlphaSurfaceDescription surfaceDescription = (AlphaSurfaceDescription)0;
				float2 uv_Texture0 = packedInput.ase_texcoord1.xy * _Texture0_ST.xy + _Texture0_ST.zw;
				float4 tex2DNode12 = tex2D( _Texture0, uv_Texture0 );
				
				surfaceDescription.Alpha = tex2DNode12.a;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
				surfaceDescription.AlphaClipThresholdShadow = 0.5;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }
			ColorMask 0

			HLSLPROGRAM

			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 999999


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENESELECTIONPASS
			#pragma editor_sync_compilation

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			int _ObjectId;
			int _PassValue;

			CBUFFER_START( UnityPerMaterial )
			float4 _Texture0_ST;
			float4 _Texture0_TexelSize;
			float4 _ColorFrenel;
			float _strenghtNormal;
			float _Metallic;
			float _ScaleFrenel;
			float _PowerFrenel;
			float _Smoothness;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _Texture0;


			
			void BuildSurfaceData(FragInputs fragInputs, inout SceneSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData( posInput, surfaceDescription.Alpha );
					ApplyDecalToSurfaceData( decalSurfaceData, surfaceData );
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SceneSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				outputPackedVaryingsMeshToPS.ase_texcoord1.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord1.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target0
							#ifdef WRITE_MSAA_DEPTH
							, out float1 depthColor : SV_Target1
							#endif
						#elif defined(WRITE_MSAA_DEPTH)
						, out float4 outNormalBuffer : SV_Target0
						, out float1 depthColor : SV_Target1
						#elif defined(SCENESELECTIONPASS)
						, out float4 outColor : SV_Target0
						#endif
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.interp00.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SceneSurfaceDescription surfaceDescription = (SceneSurfaceDescription)0;
				float2 uv_Texture0 = packedInput.ase_texcoord1.xy * _Texture0_ST.xy + _Texture0_ST.zw;
				float4 tex2DNode12 = tex2D( _Texture0, uv_Texture0 );
				
				surfaceDescription.Alpha = tex2DNode12.a;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				Ref [_StencilRefDepth]
				WriteMask [_StencilWriteMaskDepth]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			HLSLPROGRAM

			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 999999


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#pragma multi_compile _ WRITE_NORMAL_BUFFER
			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _Texture0_ST;
			float4 _Texture0_TexelSize;
			float4 _ColorFrenel;
			float _strenghtNormal;
			float _Metallic;
			float _ScaleFrenel;
			float _PowerFrenel;
			float _Smoothness;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Texture0;


			float3 CombineSamplesSharp128_g1( float S0, float S1, float S2, float Strength )
			{
				{
				    float3 va = float3( 0.13, 0, ( S1 - S0 ) * Strength );
				    float3 vb = float3( 0, 0.13, ( S2 - S0 ) * Strength );
				    return normalize( cross( va, vb ) );
				}
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData( posInput, surfaceDescription.Alpha );
					ApplyDecalToSurfaceData( decalSurfaceData, surfaceData );
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				outputPackedVaryingsMeshToPS.ase_texcoord3.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;
				inputMesh.tangentOS =  inputMesh.tangentOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				return outputPackedVaryingsMeshToPS;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target0
							#ifdef WRITE_MSAA_DEPTH
							, out float1 depthColor : SV_Target1
							#endif
						#elif defined(WRITE_MSAA_DEPTH)
						, out float4 outNormalBuffer : SV_Target0
						, out float1 depthColor : SV_Target1
						#elif defined(SCENESELECTIONPASS)
						, out float4 outColor : SV_Target0
						#endif
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
				float localCalculateUVsSharp110_g1 = ( 0.0 );
				float2 uv_Texture0 = packedInput.ase_texcoord3.xy * _Texture0_ST.xy + _Texture0_ST.zw;
				float2 temp_output_85_0_g1 = uv_Texture0;
				float2 UV110_g1 = temp_output_85_0_g1;
				float4 TexelSize110_g1 = _Texture0_TexelSize;
				float2 UV0110_g1 = float2( 0,0 );
				float2 UV1110_g1 = float2( 0,0 );
				float2 UV2110_g1 = float2( 0,0 );
				{
				{
				    UV110_g1.y -= TexelSize110_g1.y * 0.5;
				    UV0110_g1 = UV110_g1;
				    UV1110_g1 = UV110_g1 + float2( TexelSize110_g1.x, 0 );
				    UV2110_g1 = UV110_g1 + float2( 0, TexelSize110_g1.y );
				}
				}
				float4 break134_g1 = tex2D( _Texture0, UV0110_g1 );
				float S0128_g1 = break134_g1.r;
				float4 break136_g1 = tex2D( _Texture0, UV1110_g1 );
				float S1128_g1 = break136_g1.r;
				float4 break138_g1 = tex2D( _Texture0, UV2110_g1 );
				float S2128_g1 = break138_g1.r;
				float temp_output_91_0_g1 = _strenghtNormal;
				float Strength128_g1 = temp_output_91_0_g1;
				float3 localCombineSamplesSharp128_g1 = CombineSamplesSharp128_g1( S0128_g1 , S1128_g1 , S2128_g1 , Strength128_g1 );
				
				float4 tex2DNode12 = tex2D( _Texture0, uv_Texture0 );
				
				surfaceDescription.Normal = localCombineSamplesSharp128_g1;
				surfaceDescription.Smoothness = _Smoothness;
				surfaceDescription.Alpha = tex2DNode12.a;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.positionCS.z;
				#elif defined(SCENESELECTIONPASS)
				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Motion Vectors"
			Tags { "LightMode"="MotionVectors" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				Ref [_StencilRefMV]
				WriteMask [_StencilWriteMaskMV]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			HLSLPROGRAM

			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 999999


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#define SHADERPASS SHADERPASS_MOTION_VECTORS
			#pragma multi_compile _ WRITE_NORMAL_BUFFER
			#pragma multi_compile _ WRITE_MSAA_DEPTH

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"


			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif


			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					float3 precomputedVelocity : TEXCOORD5;
				#endif
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 vmeshPositionCS : SV_Position;
				float3 vmeshInterp00 : TEXCOORD0;
				float3 vpassInterpolators0 : TEXCOORD1; //interpolators0
				float3 vpassInterpolators1 : TEXCOORD2; //interpolators1
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _Texture0_ST;
			float4 _Texture0_TexelSize;
			float4 _ColorFrenel;
			float _strenghtNormal;
			float _Metallic;
			float _ScaleFrenel;
			float _PowerFrenel;
			float _Smoothness;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Texture0;


			float3 CombineSamplesSharp128_g1( float S0, float S1, float S2, float Strength )
			{
				{
				    float3 va = float3( 0.13, 0, ( S1 - S0 ) * Strength );
				    float3 vb = float3( 0, 0.13, ( S2 - S0 ) * Strength );
				    return normalize( cross( va, vb ) );
				}
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData( posInput, surfaceDescription.Alpha );
					ApplyDecalToSurfaceData( decalSurfaceData, surfaceData );
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			AttributesMesh ApplyMeshModification(AttributesMesh inputMesh, float3 timeParameters, inout PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS )
			{
				_TimeParameters.xyz = timeParameters;
				outputPackedVaryingsMeshToPS.ase_texcoord3.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS =  inputMesh.normalOS ;
				return inputMesh;
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh)
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS = (PackedVaryingsMeshToPS)0;
				AttributesMesh defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, outputPackedVaryingsMeshToPS);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);

				float3 VMESHpositionRWS = positionRWS;
				float4 VMESHpositionCS = TransformWorldToHClip(positionRWS);

				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(VMESHpositionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						AttributesMesh previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						PackedVaryingsMeshToPS test = (PackedVaryingsMeshToPS)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						//ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}

				outputPackedVaryingsMeshToPS.vmeshPositionCS = VMESHpositionCS;
				outputPackedVaryingsMeshToPS.vmeshInterp00.xyz = VMESHpositionRWS;

				outputPackedVaryingsMeshToPS.vpassInterpolators0 = float3(VPASSpositionCS.xyw);
				outputPackedVaryingsMeshToPS.vpassInterpolators1 = float3(VPASSpreviousPositionCS.xyw);
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float3 previousPositionOS : TEXCOORD4;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					float3 precomputedVelocity : TEXCOORD5;
				#endif
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.previousPositionOS = v.previousPositionOS;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
				o.precomputedVelocity = v.precomputedVelocity;
				#endif
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.previousPositionOS = patch[0].previousPositionOS * bary.x + patch[1].previousPositionOS * bary.y + patch[2].previousPositionOS * bary.z;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					o.precomputedVelocity = patch[0].precomputedVelocity * bary.x + patch[1].precomputedVelocity * bary.y + patch[2].precomputedVelocity * bary.z;
				#endif
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
				, out float4 outMotionVector : SV_Target0
				#ifdef WRITE_NORMAL_BUFFER
				, out float4 outNormalBuffer : SV_Target1
					#ifdef WRITE_MSAA_DEPTH
						, out float1 depthColor : SV_Target2
					#endif
				#elif defined(WRITE_MSAA_DEPTH)
				, out float4 outNormalBuffer : SV_Target1
				, out float1 depthColor : SV_Target2
				#endif

				#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : SV_Depth
				#endif
				
				)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.vmeshPositionCS;
				input.positionRWS = packedInput.vmeshInterp00.xyz;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SurfaceData surfaceData;
				BuiltinData builtinData;

				SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
				float localCalculateUVsSharp110_g1 = ( 0.0 );
				float2 uv_Texture0 = packedInput.ase_texcoord3.xy * _Texture0_ST.xy + _Texture0_ST.zw;
				float2 temp_output_85_0_g1 = uv_Texture0;
				float2 UV110_g1 = temp_output_85_0_g1;
				float4 TexelSize110_g1 = _Texture0_TexelSize;
				float2 UV0110_g1 = float2( 0,0 );
				float2 UV1110_g1 = float2( 0,0 );
				float2 UV2110_g1 = float2( 0,0 );
				{
				{
				    UV110_g1.y -= TexelSize110_g1.y * 0.5;
				    UV0110_g1 = UV110_g1;
				    UV1110_g1 = UV110_g1 + float2( TexelSize110_g1.x, 0 );
				    UV2110_g1 = UV110_g1 + float2( 0, TexelSize110_g1.y );
				}
				}
				float4 break134_g1 = tex2D( _Texture0, UV0110_g1 );
				float S0128_g1 = break134_g1.r;
				float4 break136_g1 = tex2D( _Texture0, UV1110_g1 );
				float S1128_g1 = break136_g1.r;
				float4 break138_g1 = tex2D( _Texture0, UV2110_g1 );
				float S2128_g1 = break138_g1.r;
				float temp_output_91_0_g1 = _strenghtNormal;
				float Strength128_g1 = temp_output_91_0_g1;
				float3 localCombineSamplesSharp128_g1 = CombineSamplesSharp128_g1( S0128_g1 , S1128_g1 , S2128_g1 , Strength128_g1 );
				
				float4 tex2DNode12 = tex2D( _Texture0, uv_Texture0 );
				
				surfaceDescription.Normal = localCombineSamplesSharp128_g1;
				surfaceDescription.Smoothness = _Smoothness;
				surfaceDescription.Alpha = tex2DNode12.a;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				GetSurfaceAndBuiltinData( surfaceDescription, input, V, posInput, surfaceData, builtinData );

				float4 VPASSpositionCS = float4(packedInput.vpassInterpolators0.xy, 0.0, packedInput.vpassInterpolators0.z);
				float4 VPASSpreviousPositionCS = float4(packedInput.vpassInterpolators1.xy, 0.0, packedInput.vpassInterpolators1.z);

				#ifdef _DEPTHOFFSET_ON
				VPASSpositionCS.w += builtinData.depthOffset;
				VPASSpreviousPositionCS.w += builtinData.depthOffset;
				#endif

				float2 motionVector = CalculateMotionVector( VPASSpositionCS, VPASSpreviousPositionCS );
				EncodeMotionVector( motionVector * 0.5, outMotionVector );

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if( forceNoMotion )
					outMotionVector = float4( 2.0, 0.0, 0.0, 0.0 );

				#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer( ConvertSurfaceDataToNormalData( surfaceData ), posInput.positionSS, outNormalBuffer );

				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.vmeshPositionCS.z;
				#endif
				#elif defined(WRITE_MSAA_DEPTH)
				outNormalBuffer = float4( 0.0, 0.0, 0.0, 1.0 );
				depthColor = packedInput.vmeshPositionCS.z;
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="Forward" }

			Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]
			Cull [_CullModeForward]
			ZTest [_ZTestDepthEqualForOpaque]
			ZWrite [_ZWrite]

			Stencil
			{
				Ref [_StencilRef]
				WriteMask [_StencilWriteMask]
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}


			ColorMask [_ColorMaskTransparentVel] 1

			HLSLPROGRAM

			#define _SPECULAR_OCCLUSION_FROM_AO 1
			#define ASE_SRP_VERSION 999999


			#pragma shader_feature _SURFACE_TYPE_TRANSPARENT
			#pragma shader_feature_local _DOUBLESIDED_ON
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ENABLE_FOG_ON_TRANSPARENT
			#pragma shader_feature_local _ALPHATEST_ON

			#if !defined(DEBUG_DISPLAY) && defined(_ALPHATEST_ON)
			#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
			#endif

			#define SHADERPASS SHADERPASS_FORWARD
			#pragma multi_compile _ DEBUG_DISPLAY
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
			#pragma multi_compile USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
			#pragma multi_compile SHADOW_LOW SHADOW_MEDIUM SHADOW_HIGH

			#pragma vertex Vert
			#pragma fragment Frag

			//#define UNITY_MATERIAL_LIT

			#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#ifdef DEBUG_DISPLAY
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#endif
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
			#define HAS_LIGHTLOOP
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_WORLD_NORMAL


			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
				#define ASE_NEED_CULLFACE 1
			#endif

			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 previousPositionOS : TEXCOORD4;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						float3 precomputedVelocity : TEXCOORD5;
					#endif
				#endif
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				float4 positionCS : SV_Position;
				float3 interp00 : TEXCOORD0;
				float3 interp01 : TEXCOORD1;
				float4 interp02 : TEXCOORD2;
				float4 interp03 : TEXCOORD3;
				float4 interp04 : TEXCOORD4;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 vpassPositionCS : TEXCOORD5;
					float3 vpassPreviousPositionCS : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			CBUFFER_START( UnityPerMaterial )
			float4 _Texture0_ST;
			float4 _Texture0_TexelSize;
			float4 _ColorFrenel;
			float _strenghtNormal;
			float _Metallic;
			float _ScaleFrenel;
			float _PowerFrenel;
			float _Smoothness;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
			float _SrcBlend;
			float _DstBlend;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Texture0;


			float3 CombineSamplesSharp128_g1( float S0, float S1, float S2, float Strength )
			{
				{
				    float3 va = float3( 0.13, 0, ( S1 - S0 ) * Strength );
				    float3 vb = float3( 0, 0.13, ( S2 - S0 ) * Strength );
				    return normalize( cross( va, vb ) );
				}
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;

				// surface data
				surfaceData.baseColor =					surfaceDescription.Albedo;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif
				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
				#ifdef _HAS_REFRACTION
				if( _EnableSSRefraction )
				{
					surfaceData.ior = surfaceDescription.RefractionIndex;
					surfaceData.transmittanceColor = surfaceDescription.RefractionColor;
					surfaceData.atDistance = surfaceDescription.RefractionDistance;

					surfaceData.transmittanceMask = ( 1.0 - surfaceDescription.Alpha );
					surfaceDescription.Alpha = 1.0;
				}
				else
				{
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
					surfaceDescription.Alpha = 1.0;
				}
				#else
				surfaceData.ior = 1.0;
				surfaceData.transmittanceColor = float3( 1.0, 1.0, 1.0 );
				surfaceData.atDistance = 1.0;
				surfaceData.transmittanceMask = 0.0;
				#endif


				// material features
				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif
				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
				#endif
				#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif
				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				// others
				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
				surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif
				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				// normals
				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;
				GetNormalWS( fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants );

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];

				bentNormalWS = surfaceData.normalWS;
				#ifdef ASE_BENT_NORMAL
				GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				surfaceData.tangentWS = normalize( fragInputs.tangentToWorld[ 0 ].xyz );
				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.tangentWS = TransformTangentToWorld( surfaceDescription.Tangent, fragInputs.tangentToWorld );
				#endif
				surfaceData.tangentWS = Orthonormalize( surfaceData.tangentWS, surfaceData.normalWS );

				// decals
				#if HAVE_DECALS
				if( _EnableDecals )
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData( posInput, surfaceDescription.Alpha );
					ApplyDecalToSurfaceData( decalSurfaceData, surfaceData );
				}
				#endif

				#if defined(_SPECULAR_OCCLUSION_CUSTOM)
				#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO( V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness( surfaceData.perceptualSmoothness ) );
				#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
				surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion( ClampNdotV( dot( surfaceData.normalWS, V ) ), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness( surfaceData.perceptualSmoothness ) );
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceData.perceptualSmoothness = GeometricNormalFiltering( surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[ 2 ], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold );
				#endif

				// debug
				#if defined(DEBUG_DISPLAY)
				if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				{
					surfaceData.metallic = 0;
				}
				ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
				#endif
			}

			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
				LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				ApplyDoubleSidedFlipOrMirror( fragInputs, doubleSidedConstants );

				#ifdef _ALPHATEST_ON
				DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _DEPTHOFFSET_ON
				builtinData.depthOffset = surfaceDescription.DepthOffset;
				ApplyDepthOffsetPositionInput( V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput );
				#endif

				float3 bentNormalWS;
				BuildSurfaceData( fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS );

				InitBuiltinData( posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[ 2 ], fragInputs.texCoord1, fragInputs.texCoord2, builtinData );

				#ifdef _ASE_BAKEDGI
				builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

				builtinData.emissiveColor = surfaceDescription.Emission;

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			AttributesMesh ApplyMeshModification(AttributesMesh inputMesh, float3 timeParameters, inout PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS )
			{
				_TimeParameters.xyz = timeParameters;
				outputPackedVaryingsMeshToPS.ase_texcoord7.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord7.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = inputMesh.tangentOS;
				return inputMesh;
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh)
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS = (PackedVaryingsMeshToPS)0;
				AttributesMesh defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, outputPackedVaryingsMeshToPS);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(positionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						AttributesMesh previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						PackedVaryingsMeshToPS test = (PackedVaryingsMeshToPS)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						//ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}
				#endif

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.interp03.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.interp04.xyzw = inputMesh.uv2;

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outputPackedVaryingsMeshToPS.vpassPositionCS = float3(VPASSpositionCS.xyw);
					outputPackedVaryingsMeshToPS.vpassPreviousPositionCS = float3(VPASSpreviousPositionCS.xyw);
				#endif
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 previousPositionOS : TEXCOORD4;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						float3 precomputedVelocity : TEXCOORD5;
					#endif
				#endif
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					o.previousPositionOS = v.previousPositionOS;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						o.precomputedVelocity = v.precomputedVelocity;
					#endif
				#endif
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					o.previousPositionOS = patch[0].previousPositionOS * bary.x + patch[1].previousPositionOS * bary.y + patch[2].previousPositionOS * bary.z;
					#if defined (_ADD_PRECOMPUTED_VELOCITY)
						o.precomputedVelocity = patch[0].precomputedVelocity * bary.x + patch[1].precomputedVelocity * bary.y + patch[2].precomputedVelocity * bary.z;
					#endif
				#endif
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag(PackedVaryingsMeshToPS packedInput,
					#ifdef OUTPUT_SPLIT_LIGHTING
						out float4 outColor : SV_Target0,
						out float4 outDiffuseLighting : SV_Target1,
						OUTPUT_SSSBUFFER(outSSSBuffer)
					#else
						out float4 outColor : SV_Target0
					#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
						, out float4 outMotionVec : SV_Target1
					#endif
					#endif
					#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
					#endif
					
						)
			{
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outMotionVec = float4(2.0, 0.0, 0.0, 0.0);
				#endif

				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				float3 positionRWS = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.interp03.xyzw;
				input.texCoord2 = packedInput.interp04.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				input.positionSS.xy = _OffScreenRendering > 0 ? (input.positionSS.xy * _OffScreenDownsampleFactor) : input.positionSS.xy;
				uint2 tileIndex = uint2(input.positionSS.xy) / GetTileSize ();

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz, tileIndex );

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 uv_Texture0 = packedInput.ase_texcoord7.xy * _Texture0_ST.xy + _Texture0_ST.zw;
				float4 tex2DNode12 = tex2D( _Texture0, uv_Texture0 );
				
				float localCalculateUVsSharp110_g1 = ( 0.0 );
				float2 temp_output_85_0_g1 = uv_Texture0;
				float2 UV110_g1 = temp_output_85_0_g1;
				float4 TexelSize110_g1 = _Texture0_TexelSize;
				float2 UV0110_g1 = float2( 0,0 );
				float2 UV1110_g1 = float2( 0,0 );
				float2 UV2110_g1 = float2( 0,0 );
				{
				{
				    UV110_g1.y -= TexelSize110_g1.y * 0.5;
				    UV0110_g1 = UV110_g1;
				    UV1110_g1 = UV110_g1 + float2( TexelSize110_g1.x, 0 );
				    UV2110_g1 = UV110_g1 + float2( 0, TexelSize110_g1.y );
				}
				}
				float4 break134_g1 = tex2D( _Texture0, UV0110_g1 );
				float S0128_g1 = break134_g1.r;
				float4 break136_g1 = tex2D( _Texture0, UV1110_g1 );
				float S1128_g1 = break136_g1.r;
				float4 break138_g1 = tex2D( _Texture0, UV2110_g1 );
				float S2128_g1 = break138_g1.r;
				float temp_output_91_0_g1 = _strenghtNormal;
				float Strength128_g1 = temp_output_91_0_g1;
				float3 localCombineSamplesSharp128_g1 = CombineSamplesSharp128_g1( S0128_g1 , S1128_g1 , S2128_g1 , Strength128_g1 );
				
				float fresnelNdotV14 = dot( normalWS, V );
				float fresnelNode14 = ( 0.0 + _ScaleFrenel * pow( 1.0 - fresnelNdotV14, _PowerFrenel ) );
				
				surfaceDescription.Albedo = tex2DNode12.rgb;
				surfaceDescription.Normal = localCombineSamplesSharp128_g1;
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = _Metallic;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = ( _ColorFrenel * fresnelNode14 ).rgb;
				surfaceDescription.Smoothness = _Smoothness;
				surfaceDescription.Occlusion = 1;
				surfaceDescription.Alpha = tex2DNode12.a;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = _AlphaCutoff;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				#ifdef _ASE_BAKEDGI
				surfaceDescription.BakedGI = 0;
				#endif
				#ifdef _ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = 0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);

				PreLightData preLightData = GetPreLightData(V, posInput, bsdfData);

				outColor = float4(0.0, 0.0, 0.0, 0.0);
				#ifdef DEBUG_DISPLAY
				#ifdef OUTPUT_SPLIT_LIGHTING
					outDiffuseLighting = 0;
					ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
				#endif

				bool viewMaterial = false;
				int bufferSize = int(_DebugViewMaterialArray[0]);
				if (bufferSize != 0)
				{
					bool needLinearToSRGB = false;
					float3 result = float3(1.0, 0.0, 1.0);

					for (int index = 1; index <= bufferSize; index++)
					{
						int indexMaterialProperty = int(_DebugViewMaterialArray[index]);

						if (indexMaterialProperty != 0)
						{
							viewMaterial = true;

							GetPropertiesDataDebug(indexMaterialProperty, result, needLinearToSRGB);
							GetVaryingsDataDebug(indexMaterialProperty, input, result, needLinearToSRGB);
							GetBuiltinDataDebug(indexMaterialProperty, builtinData, result, needLinearToSRGB);
							GetSurfaceDataDebug(indexMaterialProperty, surfaceData, result, needLinearToSRGB);
							GetBSDFDataDebug(indexMaterialProperty, bsdfData, result, needLinearToSRGB);
						}
					}

					if (!needLinearToSRGB)
						result = SRGBToLinear(max(0, result));

					outColor = float4(result, 1.0);
				}

				if (!viewMaterial)
				{
					if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_DIFFUSE_COLOR || _DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_SPECULAR_COLOR)
					{
						float3 result = float3(0.0, 0.0, 0.0);

						GetPBRValidatorDebug(surfaceData, result);

						outColor = float4(result, 1.0f);
					}
					else if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_TRANSPARENCY_OVERDRAW)
					{
						float4 result = _DebugTransparencyOverdrawWeight * float4(TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_A);
						outColor = result;
					}
					else
				#endif
					{
				#ifdef _SURFACE_TYPE_TRANSPARENT
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_TRANSPARENT;
				#else
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_OPAQUE;
				#endif
						float3 diffuseLighting;
						float3 specularLighting;

						LightLoop(V, posInput, preLightData, bsdfData, builtinData, featureFlags, diffuseLighting, specularLighting);

						diffuseLighting *= GetCurrentExposureMultiplier();
						specularLighting *= GetCurrentExposureMultiplier();

				#ifdef OUTPUT_SPLIT_LIGHTING
						if (_EnableSubsurfaceScattering != 0 && ShouldOutputSplitLighting(bsdfData))
						{
							outColor = float4(specularLighting, 1.0);
							outDiffuseLighting = float4(TagLightingForSSS(diffuseLighting), 1.0);
						}
						else
						{
							outColor = float4(diffuseLighting + specularLighting, 1.0);
							outDiffuseLighting = 0;
						}
						ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
				#else
						outColor = ApplyBlendMode(diffuseLighting, specularLighting, builtinData.opacity);
						outColor = EvaluateAtmosphericScattering(posInput, V, outColor);
				#endif

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
						float4 VPASSpositionCS = float4(packedInput.vpassPositionCS.xy, 0.0, packedInput.vpassPositionCS.z);
						float4 VPASSpreviousPositionCS = float4(packedInput.vpassPreviousPositionCS.xy, 0.0, packedInput.vpassPreviousPositionCS.z);

						bool forceNoMotion = any(unity_MotionVectorsParams.yw == 0.0);
						if (!forceNoMotion)
						{
							float2 motionVec = CalculateMotionVector(VPASSpositionCS, VPASSpreviousPositionCS);
							EncodeMotionVector(motionVec * 0.5, outMotionVec);
							outMotionVec.zw = 1.0;
						}
				#endif
					}

				#ifdef DEBUG_DISPLAY
				}
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}
			ENDHLSL
		}
		
	}
	CustomEditor "UnityEditor.Rendering.HighDefinition.HDLitGUI"
	
	
}
/*ASEBEGIN
Version=18712
48;160;1541;844;1950.958;387.9543;1.807034;True;True
Node;AmplifyShaderEditor.WorldNormalVector;18;-707.4348,546.358;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;30;-682.0974,688.4426;Inherit;False;Property;_ScaleFrenel;ScaleFrenel;1;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-691.0974,773.4426;Inherit;False;Property;_PowerFrenel;PowerFrenel;3;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-428.0974,469.4427;Inherit;False;Property;_ColorFrenel;ColorFrenel;2;0;Create;True;0;0;0;False;0;False;0.9622642,0.7592671,0.3495016,0;0.9622642,0.7592671,0.3495016,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-545.7187,294.9586;Inherit;False;Property;_strenghtNormal;strenghtNormal;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;14;-504.1745,643.4915;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;11;-963.2985,-308.1005;Inherit;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;34;-1175.93,-15.26281;Inherit;True;Property;_Texture1;Texture 1;6;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.FunctionNode;36;-319.1301,169.6372;Inherit;False;Normal From Texture;-1;;1;9728ee98a55193249b513caf9a0f1676;13,149,0,147,0,143,0,141,0,139,0,151,0,137,0,153,0,159,0,157,0,155,0,135,0,108,0;4;87;SAMPLER2D;0;False;85;FLOAT2;0,0;False;74;SAMPLERSTATE;0;False;91;FLOAT;1.5;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-181.0974,607.4426;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;35;-926.93,-7.262811;Inherit;True;Property;_TextureSample1;Texture Sample 1;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-39.39849,-305.2005;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;25.7699,170.3372;Inherit;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;52.91443,84.03166;Inherit;False;Property;_Metallic;Metallic;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;0,0;Float;False;False;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;META;0;1;META;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;6;0,0;Float;False;False;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;Distortion;0;6;Distortion;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;True;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;True;1;False;-1;1;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;True;0;True;-10;255;False;-1;255;True;-11;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;False;True;1;LightMode=DistortionVectors;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;10;0,0;Float;False;False;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;Forward;0;10;Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;True;1;0;True;-19;0;True;-20;1;0;True;-21;0;True;-22;False;False;False;False;False;False;False;False;True;0;True;-28;False;True;True;True;True;True;0;True;-44;False;False;True;True;0;True;-4;255;False;-1;255;True;-5;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;0;True;-23;True;0;True;-30;False;True;1;LightMode=Forward;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;8;0,0;Float;False;False;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentDepthPrepass;0;8;TransparentDepthPrepass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;0;True;-25;True;False;False;False;False;0;False;-1;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=TransparentDepthPrepass;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;0,0;Float;False;False;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;DepthOnly;0;4;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;True;0;True;-25;False;False;False;False;True;True;0;True;-6;255;False;-1;255;True;-7;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;5;0,0;Float;False;False;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;Motion Vectors;0;5;Motion Vectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;True;0;True;-25;False;False;False;False;True;True;0;True;-8;255;False;-1;255;True;-9;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;False;False;True;1;LightMode=MotionVectors;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;9;0,0;Float;False;False;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentDepthPostpass;0;9;TransparentDepthPostpass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;0;True;-25;True;False;False;False;False;0;False;-1;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=TransparentDepthPostpass;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;0,0;Float;False;False;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;True;0;True;-25;True;False;False;False;False;0;False;-1;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;SceneSelectionPass;0;3;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;7;0,0;Float;False;False;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentBackface;0;7;TransparentBackface;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;True;1;0;True;-19;0;True;-20;1;0;True;-21;0;True;-22;False;False;False;False;False;False;False;False;True;1;False;-1;False;True;True;True;True;True;0;True;-44;False;False;False;True;0;True;-23;True;0;True;-31;False;True;1;LightMode=TransparentBackface;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;269,15;Float;False;True;-1;2;UnityEditor.Rendering.HighDefinition.HDLitGUI;0;2;PlanetLit;53b46d85872c5b24c8f4f0a1c3fe4c87;True;GBuffer;0;0;GBuffer;35;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;True;0;True;-25;False;False;False;False;True;True;0;True;-13;255;False;-1;255;True;-12;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;0;True;-14;False;True;1;LightMode=GBuffer;False;0;;0;0;Standard;41;Surface Type;0;  Rendering Pass;1;  Refraction Model;0;    Blending Mode;0;    Blend Preserves Specular;1;  Receive Fog;1;  Back Then Front Rendering;0;  Transparent Depth Prepass;0;  Transparent Depth Postpass;0;  Transparent Writes Motion Vector;0;  Distortion;0;    Distortion Mode;0;    Distortion Depth Test;1;  ZWrite;0;  Z Test;4;Double-Sided;0;Alpha Clipping;0;  Use Shadow Threshold;0;Material Type,InvertActionOnDeselection;0;  Energy Conserving Specular;1;  Transmission;1;Receive Decals;1;Receives SSR;1;Motion Vectors;1;  Add Precomputed Velocity;0;Specular AA;0;Specular Occlusion Mode;1;Override Baked GI;0;Depth Offset;0;DOTS Instancing;0;LOD CrossFade;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Vertex Position;1;0;11;True;True;True;True;True;True;False;False;False;False;True;False;;False;0
WireConnection;14;0;18;0
WireConnection;14;2;30;0
WireConnection;14;3;31;0
WireConnection;36;87;11;0
WireConnection;36;91;37;0
WireConnection;28;0;29;0
WireConnection;28;1;14;0
WireConnection;35;0;34;0
WireConnection;12;0;11;0
WireConnection;0;0;12;0
WireConnection;0;1;36;40
WireConnection;0;4;32;0
WireConnection;0;6;28;0
WireConnection;0;7;33;0
WireConnection;0;9;12;4
ASEEND*/
//CHKSM=8B4119364F7B0EBF57A42FAF902C053DC2A3FF60