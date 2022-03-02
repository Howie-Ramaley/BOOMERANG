using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IStunnable
{
    //Movement speed
    [SerializeField] protected float speed;

    //
    [SerializeField] protected float aggroRange;

    //
    [SerializeField] protected bool groundEnemy;

    //
    protected bool aggro;

    //
    protected bool stunned;

    //
    protected GameObject player;

    //
    protected float velx;
    protected float vely;

    // Start is called before the first frame update
    void Start()
    {
        aggro = false;
        stunned = false;
        velx = 0;
        vely = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void FixedUpdate()
    {
        if(player != null)
        {
            float dist = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2));
            if(dist < aggroRange)
                aggro = true;
            else
                aggro = false;
        }
        else
            player = GameObject.FindGameObjectWithTag("Player");

        if(!stunned && !aggro)
            patrol();
        else if(aggro && !stunned)
            aggroBehavior();

        transform.position = new Vector3(transform.position.x + velx, transform.position.y + vely, transform.position.z);
    }

    public virtual void stun()
    {
        if(!stunned)
        {
            velx = 0;
            vely = 0;
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

    protected virtual void patrol(){}

    protected virtual void aggroBehavior()
    {
        float dist = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2));
        float angle = -Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) + Mathf.PI / 2;
        velx = ((dist < speed) ? dist : speed) * Mathf.Sin(angle);
        if(!groundEnemy)
            vely = ((dist < speed) ? dist : speed) * Mathf.Cos(angle);
    }
}
