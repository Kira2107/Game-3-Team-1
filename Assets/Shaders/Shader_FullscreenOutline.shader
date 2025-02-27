Shader "Custom/FullscreenOutlineBuiltin"
{
    Properties
    {
        _Thickness("Thickness (in texel units)", Float) = 1.0
        _EdgeColor("Edge Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        // 将该效果放在 Overlay 队列中，确保后处理效果能覆盖在屏幕上
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        
        // GrabPass 用于抓取屏幕渲染结果
        GrabPass { }
        
        Pass
        {
            // 关闭深度测试与剔除，保持全屏绘制
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // GrabPass 采集的屏幕颜色
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize; // x = 1/width, y = 1/height

            // 摄像机深度纹理（确保摄像机开启了深度纹理）
            sampler2D _CameraDepthTexture;
            float4 _CameraDepthTexture_TexelSize;

            float _Thickness; // 控制采样偏移
            float4 _EdgeColor; // 描边颜色

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // 辅助函数：从 _CameraDepthTexture 中采样并转换为线性深度（0~1）
            float GetLinearDepth(float2 uv)
            {
                // 采样深度（深度纹理通常经过了编码）
                float depth = tex2D(_CameraDepthTexture, uv).r;
                return Linear01Depth(depth);
            }

            float4 frag(v2f i) : SV_Target
            {
                // 采样抓取的屏幕颜色
                float4 col = tex2D(_GrabTexture, i.uv);
                float centerDepth = GetLinearDepth(i.uv);

                // 设置一个检测阈值，阈值越低对深度变化越敏感
                float threshold = 0.01;

                // 根据深度纹理的像素尺寸计算偏移
                float2 offset = _CameraDepthTexture_TexelSize.xy * _Thickness;

                // 采样左右上下邻域的深度
                float depthLeft  = GetLinearDepth(i.uv + float2(-offset.x, 0));
                float depthRight = GetLinearDepth(i.uv + float2( offset.x, 0));
                float depthUp    = GetLinearDepth(i.uv + float2(0,  offset.y));
                float depthDown  = GetLinearDepth(i.uv + float2(0, -offset.y));

                // 计算与中心深度的最大差值
                float diff = max( max(abs(centerDepth - depthLeft), abs(centerDepth - depthRight)),
                                  max(abs(centerDepth - depthUp), abs(centerDepth - depthDown)) );

                // 如果深度差异超过阈值，则认为是边缘
                float edge = step(threshold, diff);

                // 将原始颜色和边缘颜色混合，edge==1时显示边缘色
                float4 result = lerp(col, _EdgeColor, edge);
                return result;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
