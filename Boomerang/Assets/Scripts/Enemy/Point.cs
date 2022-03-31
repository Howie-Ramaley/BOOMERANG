using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject == transform.parent)
        {
            IStunnable enemy = collider.gameObject.GetComponent<IStunnable>();
            
            if (name == "LeftWall") {
                enemy.bump(45);
            } else enemy.bump(-45);
            
        }
    }
}
