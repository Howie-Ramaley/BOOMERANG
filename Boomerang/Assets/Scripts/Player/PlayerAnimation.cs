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
    [SerializeField] int rollInc;

    [SerializeField] int jumpTime;

    [SerializeField] int fallingLandTime;

    [SerializeField] int landTime;

    private int jumpFrames;

    private int landFrames;

    private int fallingLandFrames;

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
        jumpFrames = 0;
        landFrames = 0;
        fallingLandFrames = 0;
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
        else if(animState == AnimationState.roll)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            //GetComponent<SpriteRenderer>().color = Color.white;
            idle();
        }

        if(jumpFrames > 0)
        {
            jumpFrames++;
            if(jumpFrames > jumpTime)
            {
                jumpFrames = 0;
                fall();
                updateAnimator(false, false, false);
            }
        }

        if(landFrames > 0)
        {
            landFrames++;
            if(landFrames > landTime)
            {
                landFrames = 0;
                idle();
                updateAnimator(false, false, false);
            }
        }

        if(fallingLandFrames > 0)
        {
            fallingLandFrames++;
            if(fallingLandFrames > fallingLandTime)
            {
                fallingLandFrames = 0;
                updateAnimator(false, false, false);
            }
        }
    }

    /*public void change(AnimationState state)
    {
        if(state == AnimationState.idle)
            idle();
        else if(state == AnimationState.roll)
            roll();
        else if(state == AnimationState.run)
            run();
        else if(state == AnimationState.jump)
            jump();
        else if(state == AnimationState.fall)
            fall();
        else if(state == AnimationState.fallingLand)
            fallingLand();
        else if(state == AnimationState.land)
            land();
    }*/

    private void updateAnimator(bool interruptJump, bool interruptFallingLand, bool interruptLand)
    {
        if((landFrames == 0 || interruptLand) && (jumpFrames == 0 || interruptJump) && (fallingLandFrames == 0 || interruptFallingLand))
            animator.SetInteger("animState", (int)animState);
        else if(jumpFrames != 0)
            animator.SetInteger("animState", (int)AnimationState.jump);
        else if(fallingLandFrames != 0)
            animator.SetInteger("animState", (int)AnimationState.fallingLand);
        else if(landFrames != 0)
            animator.SetInteger("animState", (int)AnimationState.land);
    }

    public void idle()
    {
        if(animState == AnimationState.run || animState == AnimationState.land || (animState == AnimationState.roll && rollProgress == 0))
        {
            //Reset player's colliders, checks, and sprite back to normal
            animState = AnimationState.idle;
            updateAnimator(false, false, false);
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
            updateAnimator(true, true, true);
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
        if(animState != AnimationState.run && (animState == AnimationState.idle || animState == AnimationState.land))
        {
            animState = AnimationState.run;
            updateAnimator(false, false, true);
            transform.position = new Vector3(transform.position.x, transform.parent.position.y + 0.16F, transform.position.z);
        }
    }

    public void jump()
    {
        if(animState != AnimationState.jump && animState != AnimationState.roll)
        {
            animState = AnimationState.jump;
            updateAnimator(false, true, true);
            transform.position = new Vector3(transform.position.x, transform.parent.position.y, transform.position.z);
            jumpFrames = 1;
        }
    }

    public void fall()
    {
        if(animState == AnimationState.run || animState == AnimationState.jump)
        {
            animState = AnimationState.fall;
            updateAnimator(false, false, false);
            transform.position = new Vector3(transform.position.x, transform.parent.position.y, transform.position.z);
        }
    }

    public void fallingLand()
    {
        if(animState != AnimationState.fallingLand && animState != AnimationState.land && animState != AnimationState.roll)
        {
            animState = AnimationState.fallingLand;
            updateAnimator(false, false, false);
            transform.position = new Vector3(transform.position.x, transform.parent.position.y + 0.3F, transform.position.z);
            fallingLandFrames = 1;
        }
    }

    public void land()
    {
        if(animState == AnimationState.fall || animState == AnimationState.fallingLand)
        {
            animState = AnimationState.land;
            updateAnimator(false, false, false);
            transform.position = new Vector3(transform.position.x, transform.parent.position.y + 0.3F, transform.position.z);
            landFrames = 1;
        }
    }

    //Getters and setters
    public AnimationState getAnimState()
    {
        return animState;
    }
}
