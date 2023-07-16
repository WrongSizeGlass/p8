Shader "Shader/Toon"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.5, 0.5, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0,0,0,1)
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.6,0.6,0.6,1)
		_Glossiness("Glossiness", Float) = 32
		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.8
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
	}
	SubShader
	{
		
		Pass
		{
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
				SHADOW_COORDS(2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				TRANSFER_SHADOW(o)
				return o;
			}
			
			float4 _Color;
			float4 _AmbientColor;
			float _Glossiness;
			float4 _SpecularColor;
			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;

			float4 frag (v2f i) : SV_Target
			{
				float3 viewDir = normalize(i.viewDir);
				float4 sample = tex2D(_MainTex, i.uv);
				float3 normal = normalize(i.worldNormal);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector); //Dot product normals
				float NdotL = dot(_WorldSpaceLightPos0, normal); //Dot product of light direction based on world space
				float shadow = SHADOW_ATTENUATION(i); //macro that returns a value between 0 and 1, where 0 is no shadow and 1 is fully shadow
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow); //multiply with dot protuct light direction in world space
				float4 light = lightIntensity * _LightColor0; //then multiplied by the final output
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);//using smoothstep to "toonify" the reflection
				float4 specular = specularIntensitySmooth * _SpecularColor;//and then multiplied by the final output
				float4 rimDot = 1 - dot(viewDir, normal);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold); //Controls how far the rim of the surfaces will go out, and using pow function to scale the rim
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;//then multiply by the final output
				return _Color * sample * (_AmbientColor + light + specular + rimDot + rim);
			}
			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}