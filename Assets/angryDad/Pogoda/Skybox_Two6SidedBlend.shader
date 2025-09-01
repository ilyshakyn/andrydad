
Shader "Skybox/Two6SidedBlend_Builtin"
{
   Properties{
        // --- Набор A ---
        _FrontA ("Front [+Z] A", 2D) = "black" {}
        _BackA  ("Back  [-Z] A", 2D) = "black" {}
        _LeftA  ("Left  [+X] A", 2D) = "black" {}
        _RightA ("Right [-X] A", 2D) = "black" {}
        _UpA    ("Up    [+Y] A", 2D) = "black" {}
        _DownA  ("Down  [-Y] A", 2D) = "black" {}

        // --- Набор B ---
        _FrontB ("Front [+Z] B", 2D) = "black" {}
        _BackB  ("Back  [-Z] B", 2D) = "black" {}
        _LeftB  ("Left  [+X] B", 2D) = "black" {}
        _RightB ("Right [-X] B", 2D) = "black" {}
        _UpB    ("Up    [+Y] B", 2D) = "black" {}
        _DownB  ("Down  [-Y] B", 2D) = "black" {}

        _Exposure ("Exposure", Range(0,8)) = 1
        _Rotation ("Rotation Y (deg)", Range(0,360)) = 0
        _Blend    ("Blend A?B", Range(0,1)) = 0
        _Tint     ("Tint Color", Color) = (1,1,1,1)
    }

    SubShader{
        Tags{ "Queue"="Background" "RenderType"="Background" }
        Cull Off ZWrite Off

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _FrontA,_BackA,_LeftA,_RightA,_UpA,_DownA;
            sampler2D _FrontB,_BackB,_LeftB,_RightB,_UpB,_DownB;
            float _Exposure, _Blend, _Rotation;
            float4 _Tint;

            struct appdata { float4 vertex:POSITION; };
            struct v2f { float4 pos:SV_POSITION; float3 dirWS:TEXCOORD0; };

            float3 RotateY(float3 d, float deg){
                float r = radians(deg), s = sin(r), c = cos(r);
                return float3(c*d.x + s*d.z, d.y, -s*d.x + c*d.z);
            }

            // выбор стороны куба и UV как в 6-sided skybox
            void SampleSix(in sampler2D F,in sampler2D B,in sampler2D L,in sampler2D R,in sampler2D U,in sampler2D D,
                           float3 dir, out float4 col)
            {
                float3 a = abs(dir);
                float2 uv; float4 tex;
                if (a.x >= a.y && a.x >= a.z){
                    // ±X
                    if (dir.x > 0){ // +X (Left в инспекторе Skybox/6 Sided = +X)
                        uv = float2(-dir.z, dir.y) / a.x * 0.5 + 0.5;
                        tex = tex2D(L, uv);
                    } else {        // -X (Right = -X)
                        uv = float2(dir.z, dir.y) / a.x * 0.5 + 0.5;
                        tex = tex2D(R, uv);
                    }
                }
                else if (a.y >= a.x && a.y >= a.z){
                    // ±Y
                    if (dir.y > 0){ // +Y (Up)
                        uv = float2(dir.x, -dir.z) / a.y * 0.5 + 0.5;
                        tex = tex2D(U, uv);
                    } else {        // -Y (Down)
                        uv = float2(dir.x, dir.z) / a.y * 0.5 + 0.5;
                        tex = tex2D(D, uv);
                    }
                }
                else{
                    // ±Z
                    if (dir.z > 0){ // +Z (Front)
                        uv = float2(dir.x, dir.y) / a.z * 0.5 + 0.5;
                        tex = tex2D(F, uv);
                    } else {        // -Z (Back)
                        uv = float2(-dir.x, dir.y) / a.z * 0.5 + 0.5;
                        tex = tex2D(B, uv);
                    }
                }
                col = tex;
            }

            v2f vert(appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 dir = normalize(v.vertex.xyz);
                o.dirWS = RotateY(dir, _Rotation);
                return o;
            }

            fixed4 frag(v2f i):SV_Target{
                float3 d = normalize(i.dirWS);

                float4 colA, colB;
                SampleSix(_FrontA,_BackA,_LeftA,_RightA,_UpA,_DownA, d, colA);
                SampleSix(_FrontB,_BackB,_LeftB,_RightB,_UpB,_DownB, d, colB);

                fixed4 c = lerp(colA, colB, saturate(_Blend));
                c.rgb *= _Exposure;
                c *= _Tint;
                return c;
            }
            ENDCG
        }
    }
}