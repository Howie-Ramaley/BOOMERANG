using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private int health;
    [SerializeField]private int iFramesOnEnemyHit;
    private int iFrames;
    private int iFrameProgress;
    private int diedFrames;

    // Start is called before the first frame update
    void Start()
    {
        iFrameProgress = 0;
        iFrames = iFramesOnEnemyHit;
        diedFrames = 0;
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

        if(diedFrames > 0)
        {
            diedFrames++;
            if(diedFrames > 2)
                diedFrames = 0;
        }
    }

    public bool hurt(int damage, bool ignoreIFrames)
    {
        if (iFrameProgress == 0 || ignoreIFrames)
        {
            health -= damage;
            SoundManager.PlaySound("p_hit");
            startIFrames(false);

            //Debug.Log("HURT");
            healthDisplayUpdate();
            return true;
        }
        else
            return false;
    }

    public void healthDisplayUpdate()
    {
        GameObject player = gameObject;
        SpriteRenderer sprite = player.GetComponentInChildren<PlayerAnimation>().gameObject.GetComponent<SpriteRenderer>();
        if(health == 2)
        {
            sprite.color = new Color(0.88f, 0.44f, 0.44f);
        }
        else if(health == 1)
        {
            sprite.color = new Color(0.77f, 0.13f, 0.21f);
        }
        else if (health <= 0)
        {
            //player.SetActive(false);
            health = 3;
            sprite.color = new Color(1, 1, 1);
            player.GetComponent<PlayerMovement>().respawn();
            diedFrames = 1;
        }
        else
        {
            sprite.color = new Color(1, 1, 1);
        }
    }

    public bool getDied()
    {
        return diedFrames > 0;
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
