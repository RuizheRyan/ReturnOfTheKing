Shader "Unlit/2DLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass    //compute per-pixel distance
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            float4 ComputeDistancesPS(float2 TexCoord : TEXCOORD0) : COLOR0
            {
                float4 color = tex2D(_MainTex, TexCoord);
                //compute distance from center
                float distance = color.a > 0.3f ? length(TexCoord - 0.5f) : 1.0f;
                //save it to the Red channel
                //distance *= _MainTex_TexelSize.x;
                return float4(distance,0,0,1);
            }

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
                float4 col = ComputeDistancesPS(1-i.uv);
                //col = DistortPS(i.uv);
                //Texture = col;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }        
        
        Pass    //Find colest distance
        {
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            float4 HorizontalReductionPS(float2 TexCoord : TEXCOORD0) : COLOR0
            {
                float2 color = tex2D(_MainTex, TexCoord);
                float2 colorR = tex2D(_MainTex, TexCoord - float2(_MainTex_TexelSize.x, 0));
                float2 result = min(color,colorR);
                //return float4(result,0,1);

                return float4(result,0,1);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float4 col = HorizontalReductionPS(i.uv);
                //col = ComputeDistancesPS(i.uv);
                //Texture = col;
                // apply fog
                return col;
            }
            ENDCG
        }

        Pass    //Distort UV
        {
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float4 DistortPS(float2 TexCoord : TEXCOORD0) : COLOR0
            {
                //translate u and v into [-1 , 1] domain
                float u0 = TexCoord.x * 2 - 1;
                float v0 = TexCoord.y * 2 - 1;

                //then, as u0 approaches 0 (the center), v should also approach 0
                v0 = v0 * abs(u0);
                //convert back from [-1,1] domain to [0,1] domain
                v0 = (v0 + 1) / 2;
                //we now have the coordinates for reading from the initial image
                float2 newCoords = float2(TexCoord.x, v0);

                //read for both horizontal and vertical direction and store them in separate channels

                float horizontal = tex2D(_MainTex, newCoords).r;
                float vertical = tex2D(_MainTex, newCoords.yx).r;
                return float4(horizontal,vertical ,0,1);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
                col = DistortPS(i.uv);
                //col = ComputeDistancesPS(i.uv);
                //Texture = col;
                // apply fog
                return col;
            }
            ENDCG
        }
        Pass    //out-put shadow result
        {
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            float GetShadowDistanceH(float2 TexCoord)
            {
                float u = TexCoord.x;
                float v = TexCoord.y;

                u = abs(u - 0.5f) * 2;
                v = v * 2 - 1;
                float v0 = v / u;
                v0 = (v0 + 1) / 2;

                float2 newCoords = float2(TexCoord.x, v0);
                //horizontal info was stored in the Red component
                return tex2D(_MainTex, newCoords).r;
            }

            float GetShadowDistanceV(float2 TexCoord)
            {
                float u = TexCoord.y;
                float v = TexCoord.x;

                u = abs(u - 0.5f) * 2;
                v = v * 2 - 1;
                float v0 = v / u;
                v0 = (v0 + 1) / 2;

                float2 newCoords = float2(TexCoord.y, v0);
                //vertical info was stored in the Green component
                return tex2D(_MainTex, newCoords).g;
            }

            float4 DrawShadowsPS(float2 TexCoord: TEXCOORD0) : COLOR0
            {
                //apply a bias
                float2 biasedTexCoord = (TexCoord - 0.5) * 0.999 + 0.5;
                //distance -= 0;

                // distance of this pixel from the center
                float distance = length(biasedTexCoord - 0.5f);
                distance *= _MainTex_TexelSize.z;


                //distance stored in the shadow map
                float shadowMapDistance;
                //coords in [-1,1]
                float nY = 2.0f * (TexCoord.y - 0.5f);
                float nX = 2.0f * (TexCoord.x - 0.5f);


                //we use these to determine which quadrant we are in
                if (abs(nY) < abs(nX))
                {
                    shadowMapDistance = GetShadowDistanceH(TexCoord);
                }
                else
                {
                    shadowMapDistance = GetShadowDistanceV(TexCoord);
                }

                //if distance to this pixel is lower than distance from shadowMap,
                //then we are not in shadow
                float light = distance < shadowMapDistance ? 1 : 0;
                float4 result = light;
                //result.b = length(TexCoord - 0.5f);
                result.a = 1;
                return result;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float4 col = DrawShadowsPS(i.uv);
                //col = ComputeDistancesPS(i.uv);
                //Texture = col;
                // apply fog
                return col;
            }
            ENDCG
        }
        Pass    //AA
        {
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;            
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float4 col;
                for (int k = -1; k < 2; ++k) {
                    for (int j = -1; j < 2; ++j) {
                        float2 newCoords;
                        newCoords.x = i.uv.x + _MainTex_TexelSize.x * k;
                        newCoords.y = i.uv.y + _MainTex_TexelSize.y * j;
                        col += tex2D(_MainTex, newCoords);
                    }
                }
                col /= 9;
                return col;
            }
                ENDCG
        }
    }
}
