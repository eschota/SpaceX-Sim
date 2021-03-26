// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Politic"
{
	Properties
	{
		_geo_map_complete("geo_map_complete", 2D) = "white" {}
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM



#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
		//only defining to not throw compilation error over Unity 5.5
		#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
			};

			uniform sampler2D _geo_map_complete;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				float3 vertexValue =  float3(0,0,0) ;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				float4 color13 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
				float2 uv016 = i.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_21_0 = round( ( 111.0 * tex2D( _geo_map_complete, uv016 ).r ) );
				float4 color2 = IsGammaSpace() ? float4(0.2156863,0,0,1) : float4(0.03820438,0,0,1);
				float ifLocalVar8 = 0;
				if( temp_output_21_0 == round( ( 111.0 * color2.r ) ) )
				ifLocalVar8 = 11.0;
				
				
				finalColor = ( color13 * ifLocalVar8 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16900
80.44444;108;1372;830;800.7599;556.7225;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-567.0259,-389.0686;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-618.2756,-644.5571;Float;True;Property;_geo_map_complete;geo_map_complete;0;0;Create;True;0;0;False;0;31c49db35f816bc45be51c4b47ec2be2;31c49db35f816bc45be51c4b47ec2be2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-578.9789,-192.3007;Float;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;111;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-598.632,-61.71535;Float;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;0.2156863,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-204.15,-604.5355;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-267.5679,-203.347;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;21;-24.96973,-595.3999;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;22;-44.96328,-188.112;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-501.0689,262.842;Float;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;11;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;8;252.5286,-224.4304;Float;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;13;58.90257,-790.6269;Float;False;Constant;_Color1;Color 1;1;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;464.5344,-492.3922;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;179.2568,-403.2441;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;705.6188,-396.5013;Half;False;True;2;Half;ASEMaterialInspector;0;1;Politic;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;1;1;16;0
WireConnection;18;0;39;0
WireConnection;18;1;1;1
WireConnection;20;0;39;0
WireConnection;20;1;2;1
WireConnection;21;0;18;0
WireConnection;22;0;20;0
WireConnection;8;0;21;0
WireConnection;8;1;22;0
WireConnection;8;3;7;0
WireConnection;14;0;13;0
WireConnection;14;1;8;0
WireConnection;45;0;21;0
WireConnection;0;0;14;0
ASEEND*/
//CHKSM=73A9486372EA4FD14A2290B95A65D51A7EDAFFA7