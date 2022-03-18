//Create > Shader > Image Effect Shader
Shader "Custom/PostEffectShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            //Runs once for every vertice
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            //Runs once for every pixel
            fixed4 frag (v2f IN) : SV_Target
            {
                /*
                //If we are running at 1920x1080... that's over two million pixels!
                //How fast or slow does this end up being?
                float a = 2.18;
                float b = 3.17;
                float c = a * b;
                float d = pow(a, c);
                //Much, much faster than updating on CPU
                //Use GPU for simple tasks that must be repeated a lot of times very quickly
                //Use CPU for long complicated tasks
                */

                fixed4 col = tex2D( _MainTex, IN.uv + float2( 0, sin(IN.vertex.x / 100 + _Time[1] / 2) ) / 10 );

                // just invert the colors
                //col.rgb = 1 - col.rgb;
                
                //Make everything red
                //col.r = 1;

                return col;
            }
            ENDCG
        }
    }
}
