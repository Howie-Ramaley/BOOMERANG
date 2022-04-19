using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Makes your script run in edit mode
//OnRenderImage will be called, as well as Update, OnStart, and possibly others
[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour
{
    public Material matDrunk;
    public Material matScreenEffect;
    public bool drunk = true;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //src = source, dest = destination
        //src is the full rendered scene that you would normally send directly to the monitor
        //We are intercepting this so we can do a bit more work, before passing it on
        
        //PRETENDING TO DO IMAGE EFFECT IN CPU
        //(don't do this it's terrible)
        /*
        Color[] pixels = new Color[1920 * 1080];
        for(int x = 0; x < 1920; x++)
        {
            for(int y = 0; y < 1080; y++)
            {
                pixels[x + y * 1080].r = Mathf.Pow(2.18f, 3.17f);
            }
        }
        //probably apply some kind of texture.SetPixels(pixels);
        */
        Material mat = matScreenEffect;
        if(drunk)
            mat = matDrunk;
        Graphics.Blit(src, dest, mat);
    }
}
