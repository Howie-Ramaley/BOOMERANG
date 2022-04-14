using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobGoblin : Enemy
{
    [SerializeField] private Animator animator;
    [SerializeField] private int startTeleportTime;
    [SerializeField] private int teleportingTime;
    [SerializeField] private int attackTime;
    [SerializeField] private float projectileSpeed;
    private int startTeleportFrames;
    private int teleportingFrames;
    private int attackFrames;
    private List<Vector2> points;
    private int atPointIndex;

    private bool frozen;

    private List<LobGob> lobGobs;
    

    // Start is called before the first frame update
    void Start()
    {
        startTeleportFrames = 1;
        teleportingFrames = 0;
        attackFrames = 0;
        int n = 1;
        points = new List<Vector2>();
        Transform pointTransform = transform.parent.Find("Point" + n);
        while(pointTransform != null)
        {
            Vector2 tempPoint = new Vector2();
            tempPoint.x = pointTransform.position.x;
            tempPoint.y = pointTransform.position.y;
            points.Add(tempPoint);
            n++;
            pointTransform = transform.parent.Find("Point" + n);
        }
        atPointIndex = 0;

        n = 1;
        lobGobs = new List<LobGob>();
        Transform lgTransform = transform.parent.Find("Projectile" + n);
        while(lgTransform != null)
        {
            LobGob lobGob = lgTransform.gameObject.GetComponent<LobGob>();
            lobGobs.Add(lobGob);
            lobGob.lobGoblin = this;
            n++;
            lgTransform = transform.parent.Find("Projectile" + n);
        }

        frozen = false;
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        
        //transform.position = new Vector3(transform.position.x + velx, transform.position.y + vely, transform.position.z);

        if(player.transform.position.x >= transform.position.x)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        else
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);

        if(teleportingFrames > 0 && !frozen)
        {
            teleportingFrames++;
            if(teleportingFrames > teleportingTime)
            {
                teleportingFrames = 0;
                atPointIndex++;
                if(atPointIndex >= points.Count)
                    atPointIndex = 0;
                transform.position = points[atPointIndex];
                GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    private void teleport()
    {
        teleportingFrames = 1;
        //start teleport animation
        GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    private void shoot()
    {
        int n = 0;
        LobGob shootyBangBang = lobGobs[n];
        while(shootyBangBang.getShot() && n < lobGobs.Count - 1)
        {
            n++;
            shootyBangBang = lobGobs[n];
        }
        
        if(!shootyBangBang.getShot())
            shootyBangBang.shoot();
    }
    
    public void wooYeahGetThatGoblinFucker()
    {
        base.stun();
        frozen = false;
    }

    override protected void patrol()
    {
        if(startTeleportFrames > 0 && !frozen)
        {
            startTeleportFrames++;
            if(startTeleportFrames > startTeleportTime * 2)
            {
                startTeleportFrames = 1;
                teleport();
            }
        }
    }

    override protected void aggroBehavior()
    {
        if(startTeleportFrames > 0 && !frozen)
        {
            startTeleportFrames++;
            if(startTeleportFrames > startTeleportTime)
            {
                startTeleportFrames = 0;
                teleport();
                attackFrames = 1;
            }
        }

        if(attackFrames > 0 && !frozen)
        {
            attackFrames++;
            if(attackFrames > attackTime)
            {
                attackFrames = 0;
                shoot();
                startTeleportFrames = 1;
            }
        }
    }

    override public bool stun()
    {
        if(stunned)
        {
            base.stun();
            startTeleportFrames = 1;
        }
        return false;
    }
    override public bool bump(float angle, float bumpStrength)
    {
        return false;
    }

    public float getProjectileSpeed()
    {
        return projectileSpeed;
    }

    public void freeze()
    {
        frozen = true;
        attackFrames = 0;
        teleportingFrames = 0;
        startTeleportFrames = 0;
    }
}