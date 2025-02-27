Shader "Custom/SlimeShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}      // 基础贴图
        _Transparency ("Transparency", Range(0,1)) = 0.5         // 透明度控制参数
        _RimColor ("Rim Glow Color", Color) = (1.0, 1.0, 1.0, 1.0) // 边缘发光颜色
        _RimPower ("Rim Power", Range(0.1, 10.0)) = 2.0          // 边缘光强度（Fresnel幂指数）
        _NoiseTex ("Noise Texture (Alpha)", 2D) = "white" {}      // 噪声纹理，其Alpha通道用于液体扰动效果
        _NoiseScale ("Noise Scale", Float) = 1.0                 // 噪声贴图缩放系数
        _NoiseSpeed ("Noise Speed (X,Y)", Vector) = (0.2, 0.2, 0, 0) // 噪声UV流动速度
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

            // 声明属性对应的变量（_Time为内置变量，不需重新声明）
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _Transparency;
            fixed4 _RimColor;
            float _RimPower;
            float _NoiseScale;
            float4 _NoiseSpeed;

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
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                // 使用相同的UV供纹理采样，同时用于噪声扰动
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

                // 采样噪声纹理实现液体扰动效果，利用内置 _Time.y 控制动画
                float2 flowUV = i.uv + _NoiseSpeed.xy * _Time.y;
                fixed noiseVal = tex2D(_NoiseTex, flowUV).a;

                // 使用_MainTex贴图作为基础颜色
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed3 baseColor = texColor.rgb;
                // 如果希望让纹理受视角影响，可保留下列乘法；若希望保持原色，请注释此行
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
