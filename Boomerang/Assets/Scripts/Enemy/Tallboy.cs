using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tallboy : Enemy
{
    [SerializeField] private Animator animator;
    private Vector2 xy1 = new Vector2();
    private Vector2 xy2 = new Vector2();
    private bool approachingPoint2;
    //private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        xy1.x = transform.parent.Find("Point1").transform.position.x;
        xy1.y = transform.parent.Find("Point1").transform.position.y;

        xy2.x = transform.parent.Find("Point2").transform.position.x;
        xy2.y = transform.parent.Find("Point2").transform.position.y;
        
        approachingPoint2 = true;

        //body = GetComponent<Rigidbody2D>();
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        //body.velocity = new Vector2(velx, vely);
        transform.position = new Vector3(transform.position.x + velx, transform.position.y + vely, transform.position.z);
        
        animator.SetFloat("velx", velx);

        if(velx > 0.01F)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        else if(velx < -0.01F)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
    }

    override protected void patrol()
    {
        float dist;
        float angle;
        if (approachingPoint2) {
            dist = Mathf.Sqrt(Mathf.Pow(xy2.x - transform.position.x, 2) + Mathf.Pow(xy2.y - transform.position.y, 2));
            angle = -Mathf.Atan2(xy2.y - transform.position.y, xy2.x - transform.position.x) + Mathf.PI / 2;
            velx = speed * Mathf.Sin(angle);
            vely = speed * Mathf.Cos(angle);
            if (dist < speed) {
                approachingPoint2 = false;
            }
        } else {
            dist = Mathf.Sqrt(Mathf.Pow(xy1.x - transform.position.x, 2) + Mathf.Pow(xy1.y - transform.position.y, 2));
            angle = -Mathf.Atan2(xy1.y - transform.position.y, xy1.x - transform.position.x) + Mathf.PI / 2;
            velx = speed * Mathf.Sin(angle);
            vely = speed * Mathf.Cos(angle);
                
            if (dist < speed) {
                approachingPoint2 = true;
            }
        }
    }

    override public void stun()
    {
        if(!stunned)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90F);
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0F);
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        base.stun();
    }
}
