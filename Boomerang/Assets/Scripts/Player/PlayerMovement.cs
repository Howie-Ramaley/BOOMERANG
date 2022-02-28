using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player's personal reference of the layer where all ground objects are stored
    [SerializeField] private LayerMask groundLayer;

    //Walking speed of player
    [SerializeField] private float speed;

    //Jumping speed of player
    [SerializeField] private float jumpSpeed;

    //How much the player's velx is incremented until it reaches the value in the speed variable
    //Higher: Gets to top speed faster. Lower: Takes longer to get going, feels more sluggish/heavy
    [SerializeField] private float speedIncrement;
    
    //How much the player's velx is decremented until it reaches 0.
    //Higher: Stops on a dime. Lower: Takes longer to stop, feels like walking on ice
    [SerializeField] private float stopIncrement;

    //How much the player's velx is decremented until it reaches speed.
    [SerializeField] private float rollStopIncrement;

    //How many FixedUpdates can go by after the jump button is pressed and still allow a jump
    [SerializeField] private int jumpBufferTime;

    //How many FixedUpdates can go by after the roll button is pressed and still allow a roll
    [SerializeField] private int rollBufferTime;

    //How much gravity effects the player.
    [SerializeField] private float gravityScale;

    //How high gravityVel can get before being limited
    [SerializeField] private float gravityMax;

    //The lower this is, the more steep wall have to be to be considered walls when determining jumps and stopping gravity.
    [SerializeField] private float slip;

    //Amount of FixedUpdate's after leaving the ground where the player can still jump
    [SerializeField] private int coyoteTime;

    //How fast the roll is
    [SerializeField] private float rollSpeed;

    //How many FixedUpdates until the next roll can be done.
    [SerializeField] private int rollCooldownTime;

    //Amount of FixedUpdate's since leaving the ground (not including leaving the ground from jumps)
    //If this is less than coyoteTime, then allow a jump
    private int framesNotGrounded;

    //The amount of vertical velocity that is currently added to the character from gravity.
    //Goes up by (0.1962 * gravityScale) every FixedUpdate. (9.81 / 50 = 0.1962)
    private float gravityVel;

    //Amount of FixedUpdate's since the jump key was pressed
    //Incremented every FixedUpdate until surpassing jumpBufferTime, then is set back to 0 
    //If more than 0, allow a jump.
    private int jumpKeyPressedFrames;
    
    //Amount of FixedUpdate's since the roll key was pressed
    //Incremented every FixedUpdate until surpassing rollBufferTime, then is set back to 0 
    //If more than 0, allow a roll.
    private int rollKeyPressedFrames;
    
    //Gives player built in Unity physics
    private Rigidbody2D body;
    
    //Player's hitbox, tells the rigidbody where the player is solid
    //Used to help position boxcast
    private BoxCollider2D boxCollider;

    //Reference to PlayerAnimation script on PlayerSprite child object
    private PlayerAnimation animate;
    
    //Player's rigidbody's velocity is set to velx, vely every FixedUpdate
    private float velx;
    private float vely;

    //PreciseGroundCheck script attached to FeetCheck that tells when the player's gravity should be stopped.
    private PreciseGroundCheck preciseGroundCheck;

    //Player last used "run right" movement key: facingRight = true. Player last used "run left" movement key: facingRight = false.
    private bool facingRight;

    //How many FixedUpdates have gone by since the last roll. If more than rollCooldownTime, reset to 0.
    //Allow a roll if rollCooldownFrames is 0.
    private int rollCooldownFrames;

    private FollowPlayer gameCamera;


    // Start is called before the first frame update
    void Start()
    {
        //All variables with [SerializeFields] are set in Unity editor
        jumpKeyPressedFrames = 0;
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animate = GetComponentInChildren<PlayerAnimation>();
        velx = 0;
        vely = 0;
        gravityVel = 0;
        preciseGroundCheck = GetComponentInChildren<PreciseGroundCheck>();
        framesNotGrounded = coyoteTime;
        facingRight = true;
        rollCooldownFrames = 0;
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();

        //Don't rotate on collisions
        body.freezeRotation = true;
        //Don't collide with FeetCheck
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), transform.Find("FeetCheck").GetComponent<BoxCollider2D>(), true);
        Physics2D.IgnoreCollision(GetComponentInChildren<CapsuleCollider2D>(), transform.Find("FeetCheck").GetComponent<BoxCollider2D>(), true);
    }

    //Detects when the jump key and roll key is pressed
    //Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
            rollKeyPressedFrames = 1;

        if(Input.GetKeyDown(KeyCode.W))
            jumpKeyPressedFrames = 1;

        //Update camera to follow player
        string id = "player";
        if(velx > 0.01F)
            id += "Right";
        else if(velx < -0.01F)
            id += "Left";
        if(animate.getAnimState() == PlayerAnimation.AnimationState.roll)
            id += "Roll";
        if(animate.getAnimState() == PlayerAnimation.AnimationState.jump)
            id += "Jump";
        gameCamera.setTarget(transform.position.x + velx / 20, transform.position.y + vely / 40, id);
    }

    //Gets player input and makes player move accordingly
    //FixedUpdate is called at a fixed rate (50 times per sec), unnaffected by high or inconsistent frame rates
    void FixedUpdate()
    {
        //TO-DO:
        //fix getting stuck on small stuff rarely

        //Horizontal movement
        
        if(rollKeyPressedFrames > 0 && framesNotGrounded < coyoteTime && rollCooldownFrames == 0)
        {
            //Roll
            animate.roll();
            rollKeyPressedFrames = 0;
            rollCooldownFrames = 1;
            if(Input.GetKey(KeyCode.D))
                velx = rollSpeed + speed;
            else if(Input.GetKey(KeyCode.A))
                velx = -rollSpeed - speed;
            else 
            {
                if(facingRight)
                    velx = rollSpeed;
                else
                    velx = -rollSpeed;
            }
        }
        else if(Input.GetKey(KeyCode.D))
        {
            //Go right
            animate.run();
            facingRight = true;
            if(velx < speed)
            {
                velx += speedIncrement;
                if(velx > speed)
                velx = speed;
            }
            if(velx > speed)
            {
                if(isGrounded())
                    velx -= rollStopIncrement;
                else
                    velx -= rollStopIncrement;
                if(velx < speed)
                    velx = speed;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //Go left
            animate.run();
            facingRight = false;
            if(velx > -speed)
            {
                velx -= speedIncrement;
                if(velx < -speed)
                velx = -speed;
            }
            if(velx < -speed)
            {
                if(isGrounded())
                    velx += rollStopIncrement;
                else
                    velx += rollStopIncrement;
                if(velx > -speed)
                    velx = -speed;
            }
        }
        else
        {
            //if(isGrounded() && preciseGroundCheck.getOffset() < 0.01F)
                //animate.idle();
            //Slow down and come to a stop
            if(velx > speed)
            {
                if(isGrounded())
                    velx -= rollStopIncrement;
                else
                    velx -= rollStopIncrement;
            }
            else if(velx < -speed)
            {
                if(isGrounded())
                    velx += rollStopIncrement;
                else
                    velx += rollStopIncrement;
            }
            else
            {
                if(velx > 0)
                    velx -= stopIncrement;
                else if(velx < 0)
                    velx += stopIncrement;
                if(velx < stopIncrement && velx > -stopIncrement)
                    velx = 0;
            }
        }

        //flip if not facing right
        animate.transform.eulerAngles = new Vector3(animate.transform.eulerAngles.x, facingRight ? 0 : 180, animate.transform.eulerAngles.z);

        if(rollKeyPressedFrames > 0)
        {
            rollKeyPressedFrames++;
            if(rollKeyPressedFrames > rollBufferTime)
                rollKeyPressedFrames = 0;
        }
        if(rollCooldownFrames > 0)
        {
            rollCooldownFrames++;
            if(rollCooldownFrames > rollCooldownTime)
                rollCooldownFrames = 0;
        }

        //Vertical movement

        if(!isNearGround())
            framesNotGrounded++;
        else
            framesNotGrounded = 0;

        //if the jump key wasn't pressed too late and the player is grounded
        if(jumpKeyPressedFrames > 0 && framesNotGrounded < coyoteTime)
            Jump();
        //else if , the player is grounded, start halting the player's gravity
        else if(gravityVel >= vely && isNearGround())
        {
            //if FeetCheck is right under the player, set player's gravity to 0
            if(isGrounded())
            {
                animate.land();
                vely = 0;
                gravityVel = 0;
            }
            //else, FeetCheck is not right under the player, so slow the player's gravity instead
            //Necessary so that player reaches all the way to the ground without stopping, but without sliding down slopes
            else
            {
                animate.fallingLand();
                vely = 0;
                if(gravityVel > 0.1F)
                    gravityVel /= 10;
            }
        }
        //else, the player is in the air, so increment gravity
        else
        {
            animate.fall();
            gravityVel += (gravityScale * 0.1962F);
            if(gravityVel > gravityMax)
                gravityVel = gravityMax;
        }

        //Increment jumpKeyPressedFrames, reset to 0 if more than jumpBufferTime
        if(jumpKeyPressedFrames > 0)
        {
            jumpKeyPressedFrames++;
            if(jumpKeyPressedFrames > jumpBufferTime)
                jumpKeyPressedFrames = 0;
        }

        //Update player's velocity
        body.velocity = new Vector2(velx, vely - gravityVel);
    }

    //it's what you think it is
    private void Jump()
    {
        animate.jump();
        vely = jumpSpeed;
        gravityVel = 0;
        jumpKeyPressedFrames = 0;
        framesNotGrounded = coyoteTime;
    }

    //More precise ground check for telling when to stop gravity. Observes trigger status of child object with collision box
    private bool isNearGround()
    {
        return preciseGroundCheck.getGrounded();
    }

    public bool isGrounded()
    {
        return (preciseGroundCheck.getGrounded() && preciseGroundCheck.getOffset() < 0.01F);
    }

    /*UNUSED
    //Check space right under player in Ground layer for collision objects
    //if there's none and at least of their normal's y's are outside the slip range, then return true, else false
    //Used instead of isGrounded() for jumping because of a bug that doesn't let you jump when walking into walls
    private bool canJump()
    {
        //Debug to make outline of boxcast apper in scene view
        //float temp = boxCollider.size.x - 0.04F * Mathf.Abs(body.velocity.x);
        //float tempy = boxCollider.bounds.center.y - boxCollider.bounds.size.y / 2 + 0.05F;
        //Debug.Log(temp);
        //Vector2 c1 = new Vector2(boxCollider.bounds.center.x - temp / 2, tempy - 0.05F);
        //Vector2 c2 = new Vector2(boxCollider.bounds.center.x + temp / 2, tempy - 0.05F);
        //Vector2 c3 = new Vector2(boxCollider.bounds.center.x - temp / 2, tempy + 0.05F);
        //Vector2 c4 = new Vector2(boxCollider.bounds.center.x + temp / 2, tempy + 0.05F);
        //Debug.DrawLine(c1, c2, Color.red, 1, false);
        //Debug.DrawLine(c2, c4, Color.red, 1, false);
        //Debug.DrawLine(c4, c3, Color.red, 1, false);
        //Debug.DrawLine(c3, c1, Color.red, 1, false);
        
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.center.y - boxCollider.bounds.size.y / 2 + 0.075F),
            new Vector2(boxCollider.bounds.size.x, 0.15F),
            0,
            Vector2.down,
            0.15F,
            groundLayer);
        Collider2D ground = raycastHit.collider;
        if(ground != null)
        {
            List<ContactPoint2D> contacts = new List<ContactPoint2D>();
            ground.GetContacts(contacts);

            for(int i = 0; i < contacts.Count; i++)
            {
                float normaly = contacts[i].normal.y;
                if(normaly < -slip || normaly > slip)
                {
                    return true;
                }
            }
        }
        return false;
    }*/

    /*UNUSED
    //Check and see if there are any colliders in Ground layer inside of the player's collider
    private bool isInsideWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0, groundLayer);
        if(raycastHit.collider == null)
            return false;
        else
            return true;
    }*/

    //Getters and setters
    public float getVely() {return vely;}
    public void setVely(float v) {vely = v;}
    public float getVelx() {return velx;}
    public void setVelx(float v) {velx = v;}
    public float getGravityVel() {return gravityVel;}
    public void setGravityVel(float v) {gravityVel = v;}
    public Rigidbody2D getRigidbody() {return body;}
    public float getSlip() {return slip;}
    public void setSlip(float s) {slip = s;}
    public float getSpeed() {return speed;}
    public void setSpeed(float s) {speed = s;}
    public bool getFacingRight() {return facingRight;}
    public void setFacingRight(bool f) {facingRight = f;} 
}
