// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/PhongShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_BlendFct("Blend Factor", Float) = 0.5
		_PointLightColor("Point Light Color", Color) = (0, 0, 0)
		_PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		LOD 100
		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		// 2.) This matches the "forward base" of the LightMode tag to ensure the shader compiles
		// properly for the forward bass pass. As with the LightMode tag, for any additional lights
		// this would be changed from _fwdbase to _fwdadd.
#pragma multi_compile_fwdbase

		// 3.) Reference the Unity library that includes all the lighting shadow macros
#include "AutoLight.cginc"
#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
	uniform float _BlendFct;
	uniform float3 _PointLightColor;
	uniform float3 _PointLightPosition;


	struct vertIn {
		float4 vertex : POSITION;
		float4 normal : NORMAL;
		float4 color : COLOR;
		float2 uv : TEXCOORD2;
	};

	struct vertOut {
		float4 pos : SV_POSITION;
		float4 worldVertex : TEXCOORD0;
		float3 worldNormal : TEXCOORD1;
		float4 color : COLOR;
		float2 uv : TEXCOORD2;

		// 4.) The LIGHTING_COORDS macro (defined in AutoLight.cginc) defines the parameters needed to sample 
		// the shadow map. The (0,1) specifies which unused TEXCOORD semantics to hold the sampled values - 
		// As I'm not using any texcoords in this shader, I can use TEXCOORD0 and TEXCOORD1 for the shadow 
		// sampling. If I was already using TEXCOORD for UV coordinates, say, I could specify
		// LIGHTING_COORDS(1,2) instead to use TEXCOORD1 and TEXCOORD2.
		LIGHTING_COORDS(3, 4)
	};

	// Implementation of the vertex shader
	vertOut vert(vertIn v) {
		vertOut o;


		// Convert Vertex position and corresponding normal into world coords
		// Note that we have to multiply the normal by the transposed inverse of the world 
		// transformation matrix (for cases where we have non-uniform scaling; we also don't
		// care about the "fourth" dimension, because translations don't affect the normal) 
		o.worldVertex = mul(unity_WorldToObject, v.vertex);
		o.worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

		// Transform vertex in world coordinates to camera coordinates
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.color = v.color;
		o.uv = v.uv;

		TRANSFER_VERTEX_TO_FRAGMENT(o);
		return o;
	}

	// Implementation of the fragment shader
	fixed4 frag(vertOut v) : SV_Target{
		float3 worldNormal = v.worldNormal;
		float4 worldVertex = v.worldVertex;

		// Calculate ambient RGB intensities
		float Ka = 1;
		float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

		// Calculate diffuse RBG reflections
		// (when calculating the reflected ray in our specular component)
		float fAtt = 1;
		float Kd = 0.7;
		float3 L = normalize(_PointLightPosition - worldVertex.xyz);
		float LdotN = dot(L, normalize(worldNormal));
		float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);

		// Calculate specular reflections
		float Ks = 1;
		float specN = 5; // Values>>1 give tighter highlights
		float3 V = normalize(_WorldSpaceCameraPos - worldVertex.xyz);
		float3 R = reflect(-L, normalize(worldNormal));
		float3 RdotV = dot(R, V);
		float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(RdotV), specN);

		// Combine Phong illumination model components
		float4 finalColor = float4(amb.rgb + dif.rgb + spe.rgb, v.color.a);
		// Blend texture and colour together

		// 6.) The LIGHT_ATTENUATION samples the shadowmap (using the coordinates calculated by TRANSFER_VERTEX_TO_FRAGMENT
		// and stored in the structure defined by LIGHTING_COORDS), and returns the value as a float.
		float attenuation = LIGHT_ATTENUATION(v);

		fixed4 col = (tex2D(_MainTex, v.uv) * _BlendFct + finalColor * _BlendFct * attenuation);
		return col;
	}
		ENDCG
	}
	}
		Fallback "VertexLit"
}