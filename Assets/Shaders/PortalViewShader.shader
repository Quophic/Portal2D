Shader "Custom/PortalViewShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 srcPos : TEXCOORD1;
                float2 tuv : TEXCOORD2;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _PortalTex;
            float4 _Plane1;
            float4 _Plane2;
            float4 _Plane3;
            float4 _Offset;
            float2 _TMcol0;
            float2 _TMcol1;

            float2 transfer(float2 i){
                return float2(i.x * _TMcol0.x + i.y * _TMcol1.x , i.x * _TMcol0.y + i.y * _TMcol1.y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.srcPos = ComputeScreenPos(o.vertex);
                o.tuv = o.uv - _Offset.zw;
                o.tuv = o.tuv - _Offset.xy;
                o.tuv = transfer(o.tuv);
                o.tuv = o.tuv + _Offset.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 src = tex2D(_MainTex, i.uv);
                fixed4 pv = tex2D(_PortalTex, i.tuv);
                fixed2 dir = i.srcPos - _Plane1.xy;
                fixed mask = step(0, dot(dir, _Plane1.zw));
                dir = i.srcPos - _Plane2.xy;
                fixed mask2 = step(0, dot(dir, _Plane2.zw));
                dir = i.srcPos - _Plane3.xy;
                fixed mask3 = step(0, dot(dir, _Plane3.zw));
                mask = mask * mask2 * mask3;
                return  src * (1 - mask) + pv * mask;
            }
            ENDCG
        }
    }
}
