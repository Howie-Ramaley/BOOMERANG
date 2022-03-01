using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tallboy : MonoBehaviour, IStunnable
{
    [SerializeField] private float speed;
    private Vector2 xy1 = new Vector2();
    private Vector2 xy2 = new Vector2();
    private bool approachingPoint2;

    private bool stunned;

    // Start is called before the first frame update
    void Start()
    {
        xy1.y = transform.parent.Find("Point1").transform.position.y;

        xy2.x = transform.parent.Find("Point2").transform.position.x;
        xy2.y = transform.parent.Find("Point2").transform.position.y;

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
            transform.localScale = new Vector3(2f, 0.5f, 1f);
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }
        else
        {
            stunned = false;
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<PlayerHit>().setHurts(true);
            GetComponent<SpriteRenderer>().color = Color.red;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
