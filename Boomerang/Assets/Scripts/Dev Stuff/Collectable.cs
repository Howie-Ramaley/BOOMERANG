using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private int wait;

    void Start()
    {
        wait = 0;
    }

    void FixedUpdate()
    {
        if(wait > 0)
        {
            wait++;
            if(wait >= 3)
                gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        wait = 1;
    }
}
