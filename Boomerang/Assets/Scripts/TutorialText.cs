using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{

    private GameObject player;

    [SerializeField] private float fadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GetComponentInChildren<BasicTrigger>().getPlayerEnter())
        {
            StartCoroutine(FadeTextToFullAlpha(fadeSpeed, GetComponent<TextMesh>()));
        }
        
        if (GetComponentInChildren<BasicTrigger>().getPlayerExit())
        {
            StartCoroutine(FadeTextToZeroAlpha(fadeSpeed, GetComponent<TextMesh>()));
        }
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
