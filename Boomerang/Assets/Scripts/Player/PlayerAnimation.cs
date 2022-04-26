using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //Animator
    [SerializeField] private Animator animator;

    //
    [SerializeField] private float offsetY;

    //How fast the roll is in the procedural roll animation
    [SerializeField] int rollInc;
    //Progress in degrees of player's roll
    private int rollProgress;


    public enum AnimationState{idle, roll, run, jump, fall, fallingLand, land, rollJump, LENGTH};
    private AnimationState animState;
    private List<AnimationState> animQueue;
    private int animMinLength;
    private int animWait;

    //
    private float xScale;
    //
    private float yScale;
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
        animMinLength = 0;
        animWait = 1;
        animQueue = new List<AnimationState>();
        animState = AnimationState.idle;
        rollProgress = 0;
        xScale = transform.parent.localScale.x;
        yScale = transform.parent.localScale.y;
        boxCollider = transform.parent.GetComponent<BoxCollider2D>();
        headTransform = transform.parent.Find("HeadCheck").transform;
        feetTransform = transform.parent.Find("FeetCheck").transform;
        capsuleTransform = transform.parent.Find("CapsuleCollider").transform;
        playerMovement = transform.parent.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        xScale = transform.parent.localScale.x;
        yScale = transform.parent.localScale.y;
    }

    void FixedUpdate()
    {
        if(animState == AnimationState.roll || animState == AnimationState.rollJump)
        {
            if(playerMovement.getVelx() > playerMovement.getSpeed() || playerMovement.getVelx() < -playerMovement.getSpeed() || rollProgress != 0)
            {
                rollProgress -= rollInc;
                if(rollProgress >= 360)
                    rollProgress = 0;
                else if(rollProgress < 0)
                    rollProgress = 360 - rollInc;
                transform.eulerAngles = new Vector3(0, 0, rollProgress);
                
                if(animState == AnimationState.roll)
                    animWait--;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                animWait = animMinLength + 1;
            }
        }
        else if(rollProgress != 0)
        {
            Debug.LogError("RollProgress was not 0 when it should've been.");
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if(animWait > 0)
            animWait++;
        if(animWait > animMinLength)
        {
            animWait = 0;
            if(animState == AnimationState.rollJump)
                roll();
        }
        int loop = 0;
        while(animWait == 0 && animQueue.Count > 0 && loop < 100)
        {
            //Debug.Log("Pass " + loop + ", queued: " + animQueue[0] + ", playing: " + animState);
            setAnimation(animQueue[0]);
            animQueue.RemoveAt(0);
            loop++;
        }
        if(loop >= 100)
        {
            Debug.LogError("INFINITE LOOP...");
        }
    }


    private bool canPlay()
    {
        List<AnimationState> none = new List<AnimationState>();
        return canPlay(none);
    }
    private bool canPlay(bool all)
    {
        List<AnimationState> states = new List<AnimationState>();
        for(int i = 0; i < (int)AnimationState.LENGTH; i++)
            states.Add((AnimationState)i);
        return canPlay(states);
    }
    private bool canPlay(List<AnimationState> interrupt)
    {
        if(animWait == 0)
            return true;
        for(int i = 0; i < interrupt.Count; i++)
        {
            if(interrupt[i] == animState)
                return true;
        }
        return false;
    }

    public void setAnimation(AnimationState state)
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
    }

    public void idle()
    {
        if(animState == AnimationState.run || animState == AnimationState.land || ((animState == AnimationState.roll || animState == AnimationState.rollJump) && rollProgress == 0))
        {
            if(canPlay())
            {
                //Reset player's colliders, checks, and sprite back to normal from roll
                headTransform.position = new Vector3(headTransform.position.x, transform.parent.position.y + 0.755F * yScale, headTransform.position.z);
                transform.position = new Vector3(transform.parent.position.x + (0.06f) * xScale, transform.parent.position.y + (0.14F + offsetY) * yScale, transform.position.z);
                
                boxCollider.size = new Vector2(1, 1.125F);
                boxCollider.offset = new Vector2(0, -0.1875F);
                capsuleTransform.gameObject.SetActive(true);
                //boxCollider.isTrigger = false;
                //capsuleTransform.position = new Vector3(capsuleTransform.position.x, transform.parent.position.y + 0.375F * yScale, capsuleTransform.position.z);
                //capsuleTransform.localScale = new Vector3(capsuleTransform.localScale.x, 0.75F, capsuleTransform.localScale.z);
                
                animWait = 0;
                animMinLength = -1;
                animState = AnimationState.idle;
                animator.SetInteger("animState", (int)animState);
            }
            else if(!animQueue.Contains(AnimationState.idle))
                animQueue.Add(AnimationState.idle);
        }
    }

    public void roll()
    {
        if(animState != AnimationState.roll)
        {
            List<AnimationState> states = new List<AnimationState>();
            for(int i = 0; i < (int)AnimationState.LENGTH; i++)
            {
                if(i != (int)AnimationState.rollJump)
                    states.Add((AnimationState)i);
            }
            if(canPlay(states))
            {
                //Adjust player's colliders, checks, and sprite to be ball-sized
                headTransform.position = new Vector3(headTransform.position.x, transform.parent.position.y + 0.255F * yScale, headTransform.position.z);
                transform.position = new Vector3(transform.position.x, transform.parent.position.y + (-0.25F + offsetY) * yScale, transform.position.z);
                
                boxCollider.size = new Vector2(1, 1);
                boxCollider.offset = new Vector2(0, -0.25F);
                capsuleTransform.gameObject.SetActive(false);
                //boxCollider.isTrigger = true;
                //capsuleTransform.position = new Vector3(capsuleTransform.position.x, transform.parent.position.y - 0.25F * yScale, capsuleTransform.position.z);
                //capsuleTransform.localScale = new Vector3(capsuleTransform.localScale.x, 1, capsuleTransform.localScale.z);
                
                animWait = 2;
                animMinLength = 2;
                animState = AnimationState.roll;
                animator.SetInteger("animState", (int)animState);
            }
            else if(!animQueue.Contains(AnimationState.roll))
                animQueue.Add(AnimationState.roll);
        }
    }

    public void run()
    {
        if(animState != AnimationState.run && (animState == AnimationState.idle || animState == AnimationState.land))
        {
            if(canPlay(new List<AnimationState>{AnimationState.land}))
            {
                transform.position = new Vector3(transform.position.x, transform.parent.position.y + (0.16F + offsetY) * yScale, transform.position.z);
                animWait = 1;
                animMinLength = 10;
                animState = AnimationState.run;
                animator.SetInteger("animState", (int)animState);
            }
            else if(!animQueue.Contains(AnimationState.run))
                animQueue.Add(AnimationState.run);
        }
    }

    public void jump()
    {
        if(animState != AnimationState.jump && animState != AnimationState.roll && animState != AnimationState.rollJump)
        {
            if(canPlay(new List<AnimationState>{AnimationState.fallingLand, AnimationState.land, AnimationState.run}))
            {
                transform.position = new Vector3(transform.position.x, transform.parent.position.y + (offsetY) * yScale, transform.position.z);
                animWait = 1;
                animMinLength = 15;
                animState = AnimationState.jump;
                animator.SetInteger("animState", (int)animState);
            }
            else if(!animQueue.Contains(AnimationState.jump))
                animQueue.Add(AnimationState.jump);
        }
    }

    public void fall()
    {
        if(animState == AnimationState.run || animState == AnimationState.jump || animState == AnimationState.idle)
        {
            if(canPlay())
            {
                transform.position = new Vector3(transform.position.x, transform.parent.position.y + (offsetY) * yScale, transform.position.z);
                animWait = 0;
                animMinLength = -1;
                animState = AnimationState.fall;
                animator.SetInteger("animState", (int)animState);
            }
            else if(!animQueue.Contains(AnimationState.fall))
                animQueue.Add(AnimationState.fall);
        }
    }

    public void fallingLand()
    {
        if(animState != AnimationState.fallingLand && animState != AnimationState.land && animState != AnimationState.roll && animState != AnimationState.rollJump && animState != AnimationState.run)
        {
            if(canPlay())
            {
                transform.position = new Vector3(transform.position.x, transform.parent.position.y + (0.3F + offsetY) * yScale, transform.position.z);
                animWait = 0;
                animMinLength = -1;
                animState = AnimationState.fallingLand;
                animator.SetInteger("animState", (int)animState);
            }
            else if(!animQueue.Contains(AnimationState.fallingLand))
                animQueue.Add(AnimationState.fallingLand);
        }
    }
    public void rollJump()
    {
        if(animState != AnimationState.rollJump)
        {
            if(canPlay(true))
            {
                transform.position = new Vector3(transform.position.x, transform.parent.position.y + (0.3F + offsetY) * yScale, transform.position.z);
                animWait = 1;
                animMinLength = 5;
                animState = AnimationState.rollJump;
                animator.SetInteger("animState", (int)animState);
            }
            else if(!animQueue.Contains(AnimationState.rollJump))
            {
                animQueue.Add(AnimationState.rollJump);
                Debug.LogError("This shouldn't happen either lawl");
            }
        }
    }
    public void land()
    {
        if(animState == AnimationState.fall || animState == AnimationState.fallingLand)
        {
            if(canPlay())
            {
                transform.position = new Vector3(transform.position.x, transform.parent.position.y + (0.3F + offsetY) * yScale, transform.position.z);
                animWait = 1;
                animMinLength = 10;
                animState = AnimationState.land;
                animator.SetInteger("animState", (int)animState);
            }
            else if(!animQueue.Contains(AnimationState.land))
                animQueue.Add(AnimationState.land);
        }
    }

    //Getters and setters
    public AnimationState getAnimState()
    {
        return animState;
    }

    public bool isRolling()
    {
        return (rollProgress != 0);
    }
}
