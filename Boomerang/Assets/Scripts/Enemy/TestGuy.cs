using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGuy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public bool stun()
    {
        Debug.Log("STUN");
        return true;
    }
}
