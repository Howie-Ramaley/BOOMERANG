using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{

    private GameObject player;

    [SerializeField] private int fadeRange;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x) <= fadeRange && GetComponent<TextMesh>().color.a <= 0.01f)
        {
            StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<TextMesh>()));
        }/*else if (Mathf.Abs(transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x) > fadeRange && GetComponent<TextMesh>().color.a >= 0.99f)
        {
            StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<TextMesh>()));
        }*/
    }

    public IEnumerator FadeTextToFullAlpha(float t, TextMesh i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
 
    public IEnumerator FadeTextToZeroAlpha(float t, TextMesh i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
