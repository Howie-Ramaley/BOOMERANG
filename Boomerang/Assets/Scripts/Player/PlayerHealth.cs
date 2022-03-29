using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private int health;
    [SerializeField]private int iFramesOnEnemyHit;
    private int iFrames;
    private int iFrameProgress;



    // Start is called before the first frame update
    void Start()
    {
        iFrameProgress = 0;
        iFrames = iFramesOnEnemyHit;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (iFrameProgress > 0)
        {
            iFrameProgress++;
            if (iFrameProgress > iFrames)
            {
                iFrameProgress = 0;
            }
        }
    }

    public void hurt(int damage, bool ignoreIFrames)
    {
        if (iFrameProgress == 0 || ignoreIFrames)
        {
            health -= damage;
            startIFrames(false);

            //Debug.Log("HURT");
            healthDisplayUpdate();
        }
    }

    public void healthDisplayUpdate()
    {
        GameObject player = gameObject;
        SpriteRenderer sprite = player.GetComponentInChildren<PlayerAnimation>().gameObject.GetComponent<SpriteRenderer>();
        if(health == 2)
        {
            sprite.color = new Color(1, 0.75f, 0.75f);
        }
        else if(health == 1)
        {
            sprite.color = new Color(1, 0.2f, 0.2f);
        }
        else if (health <= 0)
        {
            //player.SetActive(false);
            health = 3;
            sprite.color = new Color(1, 1, 1);
            player.GetComponent<PlayerMovement>().respawn();
        }
        else
        {
            sprite.color = new Color(1, 1, 1);
        }
    }

    public int getHealth()
    {
        return health;
    }
    public void setHealth(int h)
    {
        health = h;
        healthDisplayUpdate();
    }
    public int getIFrameProgress()
    {
        return iFrameProgress;
    }

    public void startIFrames(bool roll)
    {
        iFrameProgress = 1;
        if(!roll)
            iFrames = iFramesOnEnemyHit;
        else
            iFrames = 30;
    }
}
