using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    bool fadingIn;
    bool fadingOut;
    private GameObject player;

    [SerializeField] private float fadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fadingIn = false;
        fadingOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        TextMesh text = GetComponent<TextMesh>();
        if (text.color.a < 1.0f && (GetComponentInChildren<BasicTrigger>().getPlayerEnter() || fadingIn))
        {
            StartCoroutine(FadeTextToFullAlpha(fadeSpeed, text));
            fadingIn = true;
        }
        else
            fadingIn = false;
        
        if (GetComponent<TextMesh>().color.a > 0.0f && (GetComponentInChildren<BasicTrigger>().getPlayerExit() || fadingOut))
        {
            StartCoroutine(FadeTextToZeroAlpha(fadeSpeed, text));
            fadingOut = true;
        }
        else
            fadingOut = false;
    }

    public IEnumerator FadeTextToFullAlpha(float t, TextMesh i)
    {
        //i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
 
    public IEnumerator FadeTextToZeroAlpha(float t, TextMesh i)
    {
        //i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
