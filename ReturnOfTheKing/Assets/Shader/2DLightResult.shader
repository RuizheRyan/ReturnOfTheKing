Shader "Unlit/2DLightResult"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Angle("LightAngle", Range(0, 360)) = 360
        _Distance("Distance", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Stencil
            {
                Ref 4
                Comp Always
                Pass Replace
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Angle;
            float _Distance;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                if (col.r < 0.9) 
                {
                    discard;
                }
                float2 up;
                up.x = 0;
                up.y = 1;
                float degreeToRad = _Angle / 360 * 3.1415926;
                float2 uvCoord = normalize(0.5 - i.uv);
                //col.r = dot(uvCoord, up) / 2 + 0.5;
                //col.g = 0;
                //col.b = 0;
                if (acos(dot(uvCoord, up)) > degreeToRad)
                {
                    discard;
                }
                if (length(i.uv - 0.5) > _Distance / 2) {
                    discard;
                }
                return col;
            }
            ENDCG
        }
    }
}
