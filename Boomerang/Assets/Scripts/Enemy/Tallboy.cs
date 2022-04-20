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
    private Transform leftWall;
    private Transform rightWall;

    // Start is called before the first frame update
    void Start()
    {
        xy1.x = transform.parent.Find("Point1").transform.position.x;
        xy1.y = transform.parent.Find("Point1").transform.position.y;

        xy2.x = transform.parent.Find("Point2").transform.position.x;
        xy2.y = transform.parent.Find("Point2").transform.position.y;

        leftWall = transform.parent.Find("LeftWall");
        rightWall = transform.parent.Find("RightWall");
        
        approachingPoint2 = true;

        //body = GetComponent<Rigidbody2D>();
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        //body.velocity = new Vector2(velx, vely);
        transform.position = new Vector3(transform.position.x + velx, transform.position.y + vely, transform.position.z);

        if(transform.position.x < leftWall.position.x)
            transform.position = new Vector3(leftWall.position.x, transform.position.y, transform.position.z);
        else if(transform.position.x > rightWall.position.x)
            transform.position = new Vector3(rightWall.position.x, transform.position.y, transform.position.z);

        animator.SetFloat("velx", velx);

        if(velx > 0.01F)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        else if(velx < -0.01F)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);

        if(aggro)
            speed = 0.1F;
        else
            speed = 0.07F;
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

    override public bool stun()
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
        return base.stun();
    }
}
