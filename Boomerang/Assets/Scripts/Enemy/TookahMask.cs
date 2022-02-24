using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TookahMask : MonoBehaviour, IStunnable
{
    [SerializeField] private float speed;
    private float x1;
    private float x2;
    private bool goingRight;

    private bool stunned;

    // Start is called before the first frame update
    void Start()
    {
        x1 = transform.parent.Find("Point1").transform.position.x;
        x2 = transform.parent.Find("Point2").transform.position.x;
        goingRight = false;
        stunned = false;
    }

    void FixedUpdate()
    {
        if(!stunned)
        {
            transform.position = new Vector3(transform.position.x + (goingRight ? speed : -speed), transform.position.y, transform.position.z);
            if(transform.position.x < x1)
                goingRight = true;
            else if(transform.position.x > x2)
                goingRight = false;
        }
    }

    public void stun()
    {
        if(!stunned)
        {
            stunned = true;
            GetComponent<BoxCollider2D>().isTrigger = false;
            GetComponent<PlayerHit>().setHurts(false);
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
        }
        else
        {
            stunned = false;
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<PlayerHit>().setHurts(true);
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
