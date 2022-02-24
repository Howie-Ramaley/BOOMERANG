using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum AnimationState{idle, roll, run, jump, fall, fallingLand, land};

    private AnimationState animState;

    //Animator
    [SerializeField] private Animator animator;

    //How fast the roll is in the procedural roll animation
    [SerializeField] int rollInc = 15;

    //Progress in degrees of player's roll
    private int rollProgress;

    //BoxCollider2D of player
    private BoxCollider2D boxCollider;

    //Transform of HeadCheck
    private Transform headTransform;

    //Transform of FeetCheck
    private Transform feetTransform;

    //Transform of CapsuleCollider
    private Transform capsuleTransform;

    //Player's movement script
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        animState = AnimationState.idle;
        boxCollider = transform.parent.GetComponent<BoxCollider2D>();
        headTransform = transform.parent.Find("HeadCheck").transform;
        feetTransform = transform.parent.Find("FeetCheck").transform;
        capsuleTransform = transform.parent.Find("CapsuleCollider").transform;
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
        rollProgress = 0;
    }

    void FixedUpdate()
    {
        if(playerMovement.getVelx() > playerMovement.getSpeed() || playerMovement.getVelx() < -playerMovement.getSpeed() || rollProgress != 0)
        {
            rollProgress -= rollInc;
            if(rollProgress >= 360)
                rollProgress = 0;
            else if(rollProgress < 0)
                rollProgress = 360 - rollInc;
            transform.eulerAngles = new Vector3(0, 0, rollProgress);
            //GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if(animState == AnimationState.roll || animState == AnimationState.run)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            //GetComponent<SpriteRenderer>().color = Color.white;
            idle();
        }
    }

    public void idle()
    {
        if(animState != AnimationState.idle)
        {
            //Reset player's colliders, checks, and sprite back to normal
            animState = AnimationState.idle;
            animator.SetInteger("animState", 0);
            boxCollider.isTrigger = false;
            float yScale = transform.parent.localScale.y;
            headTransform.position = new Vector3(headTransform.position.x, transform.parent.position.y + 0.755F * yScale, headTransform.position.z);
            transform.position = new Vector3(transform.position.x, transform.parent.position.y + 0.14F, transform.position.z);
            capsuleTransform.position = new Vector3(capsuleTransform.position.x, transform.parent.position.y + 0.375F * yScale, capsuleTransform.position.z);
            capsuleTransform.localScale = new Vector3(capsuleTransform.localScale.x, 0.75F, capsuleTransform.localScale.z);
        }
    }

    public void roll()
    {
        if(animState != AnimationState.roll)
        {
            //Adjust player's colliders, checks, and sprite to be ball-sized
            animState = AnimationState.roll;
            animator.SetInteger("animState", 1);
            boxCollider.isTrigger = true;
            float yScale = transform.parent.localScale.y;
            headTransform.position = new Vector3(headTransform.position.x, transform.parent.position.y + 0.255F * yScale, headTransform.position.z);
            transform.position = new Vector3(transform.position.x, transform.parent.position.y - 0.25F * yScale, transform.position.z);
            capsuleTransform.position = new Vector3(capsuleTransform.position.x, transform.parent.position.y - 0.25F * yScale, capsuleTransform.position.z);
            capsuleTransform.localScale = new Vector3(capsuleTransform.localScale.x, 1, capsuleTransform.localScale.z);
        }
    }

    public void run()
    {
        if(animState == AnimationState.roll)
            idle();
        animState = AnimationState.run;
        animator.SetInteger("animState", 2);
        transform.position = new Vector3(transform.position.x, transform.parent.position.y + 0.16F, transform.position.z);
    }

    public void jump()
    {
        animState = AnimationState.jump;
        animator.SetInteger("animState", 3);
    }

    public void fall()
    {
        animState = AnimationState.fall;
        animator.SetInteger("animState", 4);
    }

    public void fallingLand()
    {
        animState = AnimationState.fallingLand;
        animator.SetInteger("animState", 5);
    }

    public void land()
    {
        animState = AnimationState.land;
        animator.SetInteger("animState", 6);
    }

    //Getters and setters
    public AnimationState getAnimState()
    {
        return animState;
    }
}
