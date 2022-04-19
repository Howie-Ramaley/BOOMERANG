Shader "Custom/ScreenEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        //Added
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _VignetteDamper ("Vignette Damper", Float) = 50
        _TimeDamper ("Time Damper", Float) = 1
        _OscillationAmplifier ("Oscillation Amplifier", Float) = 100
        _DistortionAmplifier ("Distortion Amplifier", Float) = 100
        _DistortionSpreader ("Distortion Spreader", Float) = 100
        _DistortionTimeDamper ("Distortion Time Damper", Float) = 5
        _AngerLevel ("AngerLevel", int) = 0
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            //Added
            sampler2D _NoiseTex;
            float _VignetteDamper;
            float _TimeDamper;
            float _OscillationAmplifier;
            float _DistortionAmplifier;
            float _DistortionSpreader;
            float _DistortionTimeDamper;
            int _AngerLevel;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                _AngerLevel = 1;

                if(_AngerLevel > 0)
                {
                    if(_AngerLevel == 1)
                        _VignetteDamper = 200;
                    else if(_AngerLevel == 2)
                        _VignetteDamper = 150;
                    else if(_AngerLevel == 3)
                        _VignetteDamper = 100;

                    _VignetteDamper = 300;

                    float2 offset = float2(
                        tex2D( _NoiseTex, float2( (i.uv.y * 100) / _DistortionSpreader, _Time[1] / _DistortionTimeDamper ) ).r, //Displacement in the x direction
                        tex2D( _NoiseTex, float2( _Time[1] / _DistortionTimeDamper, (i.uv.x * 100) / _DistortionSpreader) ).r  //Displacement in the y direction
                    );
                    //float xdist = (abs(_ScreenParams.x / 2 - i.vertex.x) - _VignetteDamper + sin(_Time[1] / _TimeDamper) * _OscillationAmplifier - offset * _DistortionAmplifier) / _ScreenParams.x;
                    //float ydist = (abs(_ScreenParams.y / 2 - i.vertex.y) - _VignetteDamper + sin(_Time[1] / _TimeDamper) * _OscillationAmplifier - offset * _DistortionAmplifier) / _ScreenParams.y;
                    //float dist = (xdist + ydist);
                    float sx = _ScreenParams.x / 2;
                    float sy = _ScreenParams.y / 2;
                    float vx = i.vertex.x;
                    float vy = i.vertex.y;
                    float dist = (sqrt(pow(sx - vx, 2) + pow(sy - vy, 2)) / 10000.0) + abs(sin(_Time[1] / _TimeDamper / 5.0) / 5.0);
                    if(dist < 0)
                        dist = 0;
                    col.r += dist;
                    col.gb -= dist;
                }

                // just invert the colors
                //col.rgb = 1 - col.rgb;
                
                return col;
            }
            ENDCG
        }
    }
}
