using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitioner : MonoBehaviour
{
    [SerializeField] private bool fadeIn;
    private int fadeOutTime;
    private int fadeOutFrames;
    private int fadeInTime;
    private int fadeInFrames;
    private Image white;
    private Image title;
    private string scene;

    // Start is called before the first frame update
    void Start()
    {
        white = transform.Find("White").GetComponent<Image>();
        title = transform.Find("Title").GetComponent<Image>();
        if(fadeIn)
        {
            fadeInTime = 60;
            white.enabled = true;
            title.enabled = true;
        }
        else
            fadeInTime = 0;
        fadeOutTime = 20;
        fadeOutFrames = 0;
        fadeInFrames = 1;
        scene = "";
    }

    void FixedUpdate()
    {
        if(white == null)
            white = transform.Find("White").GetComponent<Image>();
        if(title == null)
            title = transform.Find("Title").GetComponent<Image>();
        if(fadeInFrames > 0)
        {
            white.enabled = true;
            title.enabled = true;
            int timeSplit = fadeInTime / 2;
            white.color = new Color(white.color.r, white.color.g, white.color.b, 1 - ((float)fadeInFrames / (float)timeSplit));
            if(fadeInFrames > timeSplit)
                title.color = new Color(title.color.r, title.color.g, title.color.b, 1 - ((float)(fadeInFrames - timeSplit) / (float)(fadeInTime - timeSplit)));
            fadeInFrames++;
            if(fadeInFrames > fadeInTime)
            {
                fadeInFrames = 0;
                white.enabled = false;
                title.enabled = false;
            }
        }
        if(fadeOutFrames > 0)
        {
            white.enabled = true;
            title.enabled = true;
            white.color = new Color(white.color.r, white.color.g, white.color.b, (float)fadeOutFrames / (float)fadeOutTime);
            title.color = new Color(title.color.r, title.color.g, title.color.b, (float)fadeOutFrames / (float)fadeOutTime);
            fadeOutFrames++;
            if(fadeOutFrames > fadeOutTime)
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
    public void changeLevel(string s)
    {
        scene = s;
        fadeOutFrames = 1;
    }
}
