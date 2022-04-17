using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField]private int damage;
    [SerializeField]private bool ignoresIFrames;
    [SerializeField]private bool strongKnockback;
    private bool playerCollide;
    private bool hurts;
    private PlayerHealth pHealth;
    private int hitFrames;

    private float framesSinceLastCollide;

    // Start is called before the first frame update
    void Start()
    {
        playerCollide = false;
        hurts = true;
        framesSinceLastCollide = 0;
        pHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        hitFrames = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerCollide && framesSinceLastCollide >= 2)
            playerCollide = false;
        framesSinceLastCollide++;

        if (playerCollide && hurts)
        {
            if(pHealth != null)
            {
                float speed = 16F;
                bool isRolling = pHealth.gameObject.GetComponentInChildren<PlayerAnimation>().getAnimState() == PlayerAnimation.AnimationState.roll;
                if(strongKnockback && (pHealth.getIFrameProgress() == 0 || (pHealth.getIFrameProgress() > 10 && !isRolling)))
                    pHealth.gameObject.GetComponent<PlayerMovement>().knockback(speed);
                bool hit = pHealth.hurt(damage, ignoresIFrames);
                if(hit)
                    hitFrames = 1;
            }
            else
            {
                playerCollide = false;
                pHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            }
        }

        if(hitFrames > 0)
        {
            hitFrames++;
            if(hitFrames > 2)
                hitFrames = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnTriggerStay2D(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerCollide = true;
            framesSinceLastCollide = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerCollide = false;
        }
    }

    public void setHurts(bool h) {hurts = h;}
    public bool gethurts() {return hurts;}

    public bool getHit()
    {
        return hitFrames > 0;
    }
    public void resetHit()
    {
        hitFrames = 0;
    }
}
