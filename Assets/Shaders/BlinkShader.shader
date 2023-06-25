Shader"Custom/BlinkShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Cutoff", Range(0, 1)) = 0.5
        _ClosingSpeed ("Closing Speed", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        Pass
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

float _Cutoff;
float _ClosingSpeed;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f IN) : SV_Target
{
                // Calculate the cutoff value based on the closing speed
    float cutoffValue = _Cutoff + (_ClosingSpeed * _Time.y);

                // Clip the fragment if it's below the cutoff value
    clip(cutoffValue - IN.uv.y);

                // Output a black color
    return fixed4(0, 0, 0, 1);
}
            ENDCG
        }
    }
}
