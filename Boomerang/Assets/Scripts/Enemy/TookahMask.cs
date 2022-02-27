using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TookahMask : MonoBehaviour, IStunnable
{
    [SerializeField] private float speed;
    //private float x1;
    //private float x2;
    private Vector2 xy1 = new Vector2();
    private Vector2 xy2 = new Vector2();
    private bool goingRight;
    private bool goingUp;
    private bool approachingPoint2;

    private bool stunned;

    // Start is called before the first frame update
    void Start()
    {
        //x1 = transform.parent.Find("Point1").transform.position.x;
        //x2 = transform.parent.Find("Point2").transform.position.x;
        xy1.x = transform.parent.Find("Point1").transform.position.x;
        xy1.y = transform.parent.Find("Point1").transform.position.y;

        xy2.x = transform.parent.Find("Point2").transform.position.x;
        xy2.y = transform.parent.Find("Point2").transform.position.y;

        

        goingRight = false;
        goingUp = false;
        stunned = false;
        approachingPoint2 = true;
    }

    void FixedUpdate()
    {
        if(!stunned)
        {
            float dist;
            float angle;
            float velx;
            float vely;
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
            transform.position = new Vector3(transform.position.x + velx, transform.position.y + vely, transform.position.z);
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
