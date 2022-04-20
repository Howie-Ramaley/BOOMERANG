using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    private GameObject player;
    private TextMesh text;
    [SerializeField] private float fadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMesh>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (text.color.a < 1.0f && GetComponentInChildren<BasicTrigger>().getPlayerCollide())
        {
            //Debug.Log("fade in");
            FadeIn();
        }
        
        if (GetComponent<TextMesh>().color.a > 0.0f && !GetComponentInChildren<BasicTrigger>().getPlayerCollide())
        {
            //Debug.Log("fade out");
            FadeOut();
        }
    }

    private void FadeIn()
    {
        //text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (fadeSpeed / 50f));
        if(text.color.a > 1f)
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
    }
 
    private void FadeOut()
    {
        //text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (fadeSpeed / 50f));
        if(text.color.a < 0f)
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
    }
}
