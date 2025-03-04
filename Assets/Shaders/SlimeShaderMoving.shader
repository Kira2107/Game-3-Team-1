Shader "Custom/SlimeShaderMoving"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}      
        _Transparency ("Transparency", Range(0,1)) = 0.5         
        _RimColor ("Rim Glow Color", Color) = (1.0, 1.0, 1.0, 1.0) 
        _RimPower ("Rim Power", Range(0.1, 10.0)) = 2.0          
        _NoiseTex ("Noise Texture (Alpha)", 2D) = "white" {}      
        _NoiseScale ("Noise Scale", Float) = 1.0                 
        _NoiseSpeed ("Noise Speed (X,Y)", Vector) = (0.2, 0.2, 0, 0)

        // 新增控制下半身扰动的参数
        _DistortAmount ("Distortion Amount", Float) = 0.1
        _DistortThreshold ("Distortion Threshold", Float) = 0.0
        _DistortTransition ("Distortion Transition", Range(0.01, 1.0)) = 0.1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _Transparency;
            fixed4 _RimColor;
            float _RimPower;
            float _NoiseScale;
            float4 _NoiseSpeed;

            // 新增属性变量
            float _DistortAmount;
            float _DistortThreshold;
            float _DistortTransition;

            struct a2v {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            v2f vert(a2v v)
            {
                v2f o;
                float4 modifiedVertex = v.vertex;
                
                // 判断是否为下半身并进行扰动
                float distortionBlend = saturate((_DistortThreshold - v.vertex.y) / _DistortTransition);
                if (distortionBlend > 0.0)
                {
                    float2 noiseUV = v.uv * _NoiseScale + _NoiseSpeed.xy * _Time.y;
                    // 使用 tex2Dlod 进行纹理采样，需要传入显式 LOD 参数（这里使用 0）
                    float noiseX = tex2Dlod(_NoiseTex, float4(noiseUV, 0, 0)).r;
                    float noiseZ = tex2Dlod(_NoiseTex, float4(noiseUV + float2(100.0, 100.0), 0, 0)).r;
                    float offsetX = (noiseX - 0.5) * _DistortAmount * distortionBlend;
                    float offsetZ = (noiseZ - 0.5) * _DistortAmount * distortionBlend;
                    modifiedVertex.x += offsetX;
                    modifiedVertex.z += offsetZ;
                }

                o.pos = UnityObjectToClipPos(modifiedVertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, modifiedVertex).xyz;
                o.uv = v.uv * _NoiseScale;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

                // 边缘光计算（Fresnel效果）
                float rimFactor = pow(1.0 - saturate(dot(normal, viewDir)), _RimPower);
                fixed3 rimLight = _RimColor.rgb * rimFactor;

                // 采样噪声纹理实现液体扰动效果
                float2 flowUV = i.uv + _NoiseSpeed.xy * _Time.y;
                fixed noiseVal = tex2D(_NoiseTex, flowUV).a;

                // 使用_MainTex贴图作为基础颜色
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed3 baseColor = texColor.rgb;
                baseColor *= saturate(dot(normal, viewDir));

                fixed3 finalColor = baseColor + rimLight;
                fixed finalAlpha = _Transparency * noiseVal;

                return fixed4(finalColor, finalAlpha);
            }
            ENDCG
        }
    }

    FallBack "Transparent/VertexLit"
}
