using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TookahMask : Enemy
{
    private Vector2 xy1 = new Vector2();
    private Vector2 xy2 = new Vector2();
    private bool approachingPoint2;

    // Start is called before the first frame update
    void Start()
    {
        xy1.x = transform.parent.Find("Point1").transform.position.x;
        xy1.y = transform.parent.Find("Point1").transform.position.y;

        xy2.x = transform.parent.Find("Point2").transform.position.x;
        xy2.y = transform.parent.Find("Point2").transform.position.y;

        approachingPoint2 = true;
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
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
}
