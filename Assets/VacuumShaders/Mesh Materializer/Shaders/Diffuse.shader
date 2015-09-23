// VacuumShaders 2015
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/Mesh Materializer/Diffuse/Opaque"
{
	Properties 
	{
		[KeywordEnum(Original, Gamma, Linear)] _ColorSpace ("Color Space", Float) = 0.0

		
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		

		[Toggle(V_MM_EMISSION_ON)] _UseEmission ("Use Emission", Float) = 0
		[CanBeHidden] _Emission("Emission", float) = 1
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows addshadow vertex:vert
		
		#pragma shader_feature _COLORSPACE_ORIGINAL _COLORSPACE_GAMMA _COLORSPACE_LINEAR
		#pragma shader_feature V_MM_EMISSION_ON

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		fixed4 _Color;
		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		half _Emission;

		struct Input 
		{
			float2 uv_MainTex;

			float4 vColor;
		};		 
		
		void vert (inout appdata_full v, out Input o) 
		{
            UNITY_INITIALIZE_OUTPUT(Input,o);
            
			o.vColor = v.color;
			#ifdef _COLORSPACE_GAMMA
				o.vColor.rgb = pow(o.vColor.rgb, 0.454545);
			#elif _COLORSPACE_LINEAR
				o.vColor.rgb = pow(o.vColor.rgb, 2.2);
			#endif
        }

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = IN.vColor * tex2D(_MainTex, IN.uv_MainTex) * _Color;			
			o.Albedo = c.rgb;

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			#ifdef V_MM_EMISSION_ON
				o.Emission = _Emission * c.rgb * c.a;
			#endif
		}
		ENDCG
	} 

	FallBack "VacuumShaders/Mesh Materializer/Legacy Shaders/Unlit/Opaque"
}
