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
    [SerializeField] private float projectileAmplitude;
    [SerializeField] private float projectileFrequency;
    [SerializeField] private float platformSpeed;
    private int startTeleportFrames;
    private int teleportingFrames;
    private int attackFrames;
    private List<Vector2> points;
    private int atPointIndex;
    private bool frozen;
    private List<LobGob> lobGobs;
    private Transform platformPoint1;
    private Transform platformPoint2;
    private Vector2 platformTarget;
    

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
            n++;
            lgTransform = transform.parent.Find("Projectile" + n);
        }

        platformPoint1 = transform.parent.Find("PlatformPoint1");
        platformPoint2 = transform.parent.Find("PlatformPoint2");

        frozen = false;
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        if(!stunned)
        {
            if(player.transform.position.x >= transform.position.x)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
            else
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
        else
        {
            float x = platformTarget.x;
            float y = platformTarget.y;
            float dist = Mathf.Sqrt(Mathf.Pow(x - transform.position.x, 2) + Mathf.Pow(y - transform.position.y, 2));
            float angle = -Mathf.Atan2(y - transform.position.y, x - transform.position.x) + Mathf.PI / 2;
            if (dist < platformSpeed)
            {
                if(platformTarget == (Vector2)platformPoint1.position)
                    platformTarget = platformPoint2.position;
                else
                    platformTarget = platformPoint1.position;
            }
            else
                dist = platformSpeed;
            velx = dist * Mathf.Sin(angle);
            vely = dist * Mathf.Cos(angle);
            transform.position = new Vector3(transform.position.x + velx, transform.position.y + vely, transform.position.z);
        }

        if(startTeleportFrames == 0 && teleportingFrames == 0 && attackFrames == 0)
        {
            if(aggro)
                attackFrames = 1;
            else
                startTeleportFrames = 1;
        }

        if(!frozen && !stunned)
        {
            if(startTeleportFrames > 0)
            {
                startTeleportFrames++;
                if((aggro && startTeleportFrames > startTeleportTime) || (!aggro && startTeleportFrames > startTeleportTime * 2))
                {
                    startTeleportFrames = 0;
                    teleport();
                }
            }
            else if(teleportingFrames > 0)
            {
                teleportingFrames++;
                if(teleportingFrames > teleportingTime)
                {
                    //actual teleport
                    teleportingFrames = 0;
                    atPointIndex++;
                    if(atPointIndex >= points.Count)
                        atPointIndex = 0;
                    platformPoint1.position = points[atPointIndex] + (Vector2)platformPoint1.position - (Vector2)transform.position;
                    platformPoint2.position = points[atPointIndex] + (Vector2)platformPoint2.position - (Vector2)transform.position;
                    //platformPoint1 = new Vector2(points[atPointIndex].x + platformPoint1.x - transform.position.x, points[atPointIndex].y + platformPoint1.y - transform.position.y);
                    //platformPoint2 = new Vector2(points[atPointIndex].x + platformPoint2.x - transform.position.x, points[atPointIndex].y + platformPoint2.y - transform.position.y);
                    transform.position = points[atPointIndex];
                    GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else if(attackFrames > 0)
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
        for(int i = 0; i < lobGobs.Count; i++)
            lobGobs[i].reset();
        platformTarget = platformPoint1.position;
    }

    override protected void patrol()
    {
    }

    override protected void aggroBehavior()
    {
    }

    override public bool stun()
    {
        if(stunned)
        {
            startTeleportFrames = 1;
            return base.stun();
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
    public float getProjectileAmplitude()
    {
        return projectileAmplitude;
    }
    public float getProjectileFrequency()
    {
        return projectileFrequency;
    }

    public void freeze()
    {
        frozen = true;
        attackFrames = 0;
        teleportingFrames = 0;
        startTeleportFrames = 0;
    }
}