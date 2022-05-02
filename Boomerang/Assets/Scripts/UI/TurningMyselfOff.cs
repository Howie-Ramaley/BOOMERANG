using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningMyselfOff : MonoBehaviour
{
    private int wait;
    // Start is called before the first frame update
    void Start()
    {
        wait = 1;
    }

    void FixedUpdate()
    {
        if(wait > 0)
        {
            wait++;
            if(wait >= 2)
            {
                wait = 0;
                gameObject.SetActive(false);
            }
        }
    }
}
