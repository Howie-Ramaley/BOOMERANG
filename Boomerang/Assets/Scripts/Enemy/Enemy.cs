using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour, IStunnable
{
    //Movement speed
    [SerializeField] protected float speed;

    //
    [SerializeField] protected float aggroRange;

    //
    [SerializeField] protected bool territorial;

    //
    [SerializeField] protected bool groundEnemy;

    //
    [SerializeField] protected float bumpSpeed;
    
    //
    [SerializeField] protected bool tempStun;
    [SerializeField] protected float tempStunLength;
    
    //
    [SerializeField]protected AIPath astar;

    //
    private float stunTime;

    //
    protected bool bumped;

    //
    protected bool aggro;

    //
    protected bool stunned;

    //
    protected GameObject player;

    //
    protected float velx;
    protected float vely;

    protected AggroArea aggroArea;

    //
    [SerializeField] private float delayTimeLength;
    private float delayTime;

    // Start is called before the first frame update
    void Start()
    {
        aggro = false;
        stunned = false;
        velx = 0;
        vely = 0;
        delayTime = 0F;
        stunTime = 0F;
        player = GameObject.FindGameObjectWithTag("Player");
        bumped = false;
        astar = GetComponent<AIPath>();
        aggroArea = transform.parent.GetComponentInChildren<AggroArea>();
        AIDestinationSetter aids = GetComponent<AIDestinationSetter>();
        aids.target = player.transform;
    }

    protected virtual void FixedUpdate()
    {
        if (delayTime > 0) 
        {
            delayTime -= Time.deltaTime;
            //Debug.Log("Delay time is: " + delayTime);
        }

        if (stunTime > 0) 
        {
            stunTime -= Time.deltaTime;
            //Debug.Log("Stun time is: " + stunTime);
        }
        if(stunTime <= 0 && tempStun == true && stunned)
        {
            stun();
            /*stunned = false;
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<PlayerHit>().setHurts(true);
            GetComponent<SpriteRenderer>().color = Color.red;*/
        }

        if(player != null)
        {
            float dist = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2));
            bool areaCheck = false;
            if(aggroArea != null && aggroArea.getPlayerCollide())
                areaCheck = true;
            else if(aggroArea == null)
            {
                aggroArea = transform.parent.GetComponentInChildren<AggroArea>();
                areaCheck = true;
            }
            
            if(dist < aggroRange)
            {
                if(areaCheck)
                    aggro = true;
            }
            else if(!territorial)
                aggro = false;

            if(territorial && !areaCheck)
                aggro = false;
        }
        else
            player = GameObject.FindGameObjectWithTag("Player");

        if(!bumped)
        {
            if(!stunned && !aggro && delayTime <= 0)
            {
                patrol();
            }
            else if(aggro && !stunned && delayTime <= 0)
            {
                if(astar != null)
                {
                    velx = 0;
                    vely = 0;
                    astar.enabled = true;
                }
                else
                    aggroBehavior();
            }
        }
        else
        {
            bool xDone = false;
            float stop = 0.1F;
            if(velx > stop)
                velx -= stop;
            else if(velx < -stop)
                velx += stop;
            if(velx > -stop && velx < stop)
            {
                velx = 0;
                xDone = true;
            }
            if(vely > stop)
                vely -= stop;
            else if(vely < -stop)
                vely += stop;
            if(vely > -stop && vely < stop)
            {
                vely = 0;
                if(xDone)
                {
                    delayTime = delayTimeLength;
                    bumped = false;
                }
            }
        }

        if(astar != null && (bumped || !aggro || delayTime > 0 || stunned))
            astar.enabled = false;
    }

    public virtual void stun()
    {
        if(!stunned)
        {
            velx = 0;
            vely = 0;
            stunned = true;
            stunTime = tempStunLength;
            GetComponent<BoxCollider2D>().isTrigger = false;
            GetComponent<PlayerHit>().setHurts(false);
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
        }
        else if(tempStun == false || tempStunLength > 0)
        {
            stunned = false;
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<PlayerHit>().setHurts(true);
            GetComponent<SpriteRenderer>().color = Color.red;
            delayTime = delayTimeLength;
        }
    }

    public virtual void bump(float angle)
    {
        if(!stunned)
        {
            velx = bumpSpeed * Mathf.Sin(angle);
            if(!groundEnemy)
                vely = bumpSpeed * Mathf.Cos(angle);
            bumped = true;
        }
    }

    protected virtual void patrol(){}

    protected virtual void aggroBehavior()
    {
        if(astar != null)
            Debug.Log("FUUUUUUUUCK THIS SHOULDN'T HAPPEN TELL ELIJAH");
        float px = player.transform.position.x;
        float py = player.transform.position.y;
        if(!groundEnemy && Mathf.Abs(player.transform.position.x - transform.position.x) > 2F)
            py += 1F;
        float dist = Mathf.Sqrt(Mathf.Pow(px - transform.position.x, 2) + Mathf.Pow(py - transform.position.y, 2));
        float angle = -Mathf.Atan2(py - transform.position.y, px - transform.position.x) + Mathf.PI / 2;
        velx = ((dist < speed) ? dist : speed) * Mathf.Sin(angle);
        if(!groundEnemy)
            vely = ((dist < speed) ? dist : speed) * Mathf.Cos(angle);
    }
}
