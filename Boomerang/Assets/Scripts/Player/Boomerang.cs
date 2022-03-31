using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    //
    [SerializeField] private int throwBufferTime;

    //
    [SerializeField] private int diagonalInputBufferTime;
    
    //
    [SerializeField] private int superThrowHoldTime;

    //
    [SerializeField] private float throwSpeed;

    //
    [SerializeField] private int autoReturnTime;

    //
    [SerializeField] private int returnSpeed;

    //
    [SerializeField] private float stickThreshold;

    //
    [SerializeField] private float throwCooldownLength;
    private float throwCooldown;

    //Reference to player
    private GameObject player;

    //
    private int throwKeyPressedFrames;

    //
    private int throwKeyHeldFrames;

    //
    private Vector2 throwAngle;

    //
    private bool readyToThrow;

    //
    private bool returning;

    //
    private float velx;
    private float vely;

    //
    private int framesSinceThrown;

    //
    private List<GameObject> hitList;

    //
    private bool superThrow;

    //
    private bool stuck;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        throwKeyPressedFrames = 0;
        throwKeyHeldFrames = 0;
        readyToThrow = true;
        velx = 0;
        vely = 0;
        framesSinceThrown = 0;
        returning = false;
        hitList = new List<GameObject>();
        superThrow = false;

        //Start out invisible and doesn't trigger stun states
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void Update()
    {
        float horStick = Input.GetAxis("RightHorizontal");
        float vertStick = Input.GetAxis("RightVertical");
        if(readyToThrow)
        {
            if(stickIsTilted())
            {
                throwAngle = new Vector2(horStick, vertStick);
                if(throwKeyPressedFrames == 0)
                    throwKeyPressedFrames = 1;
            }
            else
            {
                Vector2 inAngle = Vector2.zero;
                if(Input.GetKeyDown(KeyCode.I))
                    inAngle.y += 1F;
                if(Input.GetKeyDown(KeyCode.J))
                    inAngle.x -= 1F;
                if(Input.GetKeyDown(KeyCode.K))
                    inAngle.y -= 1F;
                if(Input.GetKeyDown(KeyCode.L))
                    inAngle.x += 1F;
                if(inAngle.x > 0.01F || inAngle.x < -0.01F || inAngle.y > 0.01F || inAngle.y < -0.01F)
                {
                    if(throwKeyPressedFrames <= diagonalInputBufferTime)
                    {
                        if(throwKeyPressedFrames == 0)
                            throwKeyPressedFrames = 1;
                        if(throwAngle.x < 0.01F && throwAngle.x > -0.01F)
                            throwAngle.x = inAngle.x;
                        if(throwAngle.y < 0.01F && throwAngle.y > -0.01F)
                            throwAngle.y = inAngle.y;
                    }
                    else
                    {
                        if(inAngle.x > 0.01F || inAngle.x < -0.01F)
                            throwAngle.x = inAngle.x;
                        if(inAngle.y > 0.01F || inAngle.y < -0.01F)
                            throwAngle.y = inAngle.y;
                    }
                }
            }
        }
        else
        {
            if(stuck)
            {
                if(stickIsTilted() || Input.GetButton("Callback"))
                    returnBoomerang();
                else
                {
                    if(Input.GetKeyDown(KeyCode.I))
                        returnBoomerang();
                    else if(Input.GetKeyDown(KeyCode.J))
                        returnBoomerang();
                    else if(Input.GetKeyDown(KeyCode.K))
                        returnBoomerang();
                    else if(Input.GetKeyDown(KeyCode.L))
                        returnBoomerang();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (throwCooldown > 0) 
        {
            throwCooldown -= Time.deltaTime;
            //Debug.Log("Throw cooldown is: " + throwCooldown);
        }
        if(readyToThrow && throwKeyPressedFrames > 0 && (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.L) || stickIsTilted()))
        {
            throwKeyHeldFrames++;
            if(throwKeyHeldFrames > superThrowHoldTime)
                GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerSprite").gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if(readyToThrow && throwKeyPressedFrames > diagonalInputBufferTime  && throwCooldown <= 0)
        {
            //Debug.Log("THROW");
            //Throw boomerang
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().healthDisplayUpdate();
            if(throwKeyHeldFrames > superThrowHoldTime)
                superThrow = true;
            throwKeyHeldFrames = 0;
            throwKeyPressedFrames = 0;
            framesSinceThrown = 1;
            readyToThrow = false;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            /*
            if(throwDir == Direction.right)
                velx = throwSpeed;
            else if(throwDir == Direction.left)
                velx = -throwSpeed;
            else if(throwDir == Direction.up)
                vely = throwSpeed;
            else if(throwDir == Direction.down)
                vely = -throwSpeed;
            else if(throwDir == Direction.upleft)
            {
                velx = throwSpeed * Mathf.Sin(315F * Mathf.PI / 180F);
                vely = throwSpeed * Mathf.Cos(315F * Mathf.PI / 180F);
            }
            else if(throwDir == Direction.upright)
            {
                velx = throwSpeed * Mathf.Sin(45F * Mathf.PI / 180F);
                vely = throwSpeed * Mathf.Cos(45F * Mathf.PI / 180F);
            }
            else if(throwDir == Direction.downleft)
            {
                velx = throwSpeed * Mathf.Sin(225F * Mathf.PI / 180F);
                vely = throwSpeed * Mathf.Cos(225F * Mathf.PI / 180F);
            }
            else if(throwDir == Direction.downright)
            {
                velx = throwSpeed * Mathf.Sin(135F * Mathf.PI / 180F);
                vely = throwSpeed * Mathf.Cos(135F * Mathf.PI / 180F);
            }
            else if(throwDir == Direction.custom)
            {
                velx = throwAngle.x * throwSpeed;
                vely = throwAngle.y * throwSpeed;
            }
            */
            float dist = Mathf.Sqrt(Mathf.Pow(throwAngle.x, 2) + Mathf.Pow(throwAngle.y, 2));
            throwAngle *= 1.0F / dist;
            velx = throwAngle.x * throwSpeed;
            vely = throwAngle.y * throwSpeed;
        }

        if(returning)
        {
            float dist = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2));
            dist *= 50; //velx and vely are in units per 1/50 of a second and dist is in units, so we must multiply dist by 50 to make them comparable
            if(Mathf.Abs(dist) > returnSpeed)
                dist = returnSpeed;
            float angle = -Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) + Mathf.PI / 2;
            velx = dist * Mathf.Sin(angle);
            vely = dist * Mathf.Cos(angle);
        }
        else if(readyToThrow)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

        if(throwKeyPressedFrames > 0)
        {
            throwKeyPressedFrames++;
            if(throwKeyPressedFrames > diagonalInputBufferTime + throwBufferTime && throwKeyHeldFrames == 0 && !(Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.L) || stickIsTilted()))
                throwKeyPressedFrames = 0;
        }

        if(framesSinceThrown > 0)
        {
            framesSinceThrown++;
            if(framesSinceThrown > autoReturnTime)
            {
                //auto return
                framesSinceThrown = 0;
                returnBoomerang();
            }
        }

        transform.position = new Vector3(transform.position.x + velx / 50, transform.position.y + vely / 50, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnTriggerStay2D(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        //Debug.Log("Trigger");

        //if a collider is in the boomerang
        if(collider != null && !(collider.isTrigger && collider.gameObject.tag != "Enemy"))
        {   
            if(collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Fruit")
            {
                IStunnable enemy = collider.gameObject.GetComponent<IStunnable>();
                if(enemy != null)
                {
                    bool found = false;
                    for(int i = 0; i < hitList.Count && !found; i++)
                    {
                        if(hitList[i] == collider.gameObject)
                            found = true;
                    }
                    if(!found)
                    {
                        if(superThrow)
                            enemy.stun();
                        else
                        {
                            float angle = -Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) + Mathf.PI / 2;
                            if(!returning)
                                angle -= Mathf.PI;
                            enemy.bump(angle);
                        }
                        returnBoomerang();
                        hitList.Add(collider.gameObject);
                    }
                }
            }
            else if(((1 << collider.gameObject.layer) & groundLayer) != 0 && collider.gameObject.tag != "BG Platform")
            {
                if(superThrow && collider.gameObject.tag == "Dirt" && !returning)
                {
                    superThrow = false;
                    returning = false;
                    throwKeyPressedFrames = 0;
                    framesSinceThrown = 0;
                    GetComponent<BoxCollider2D>().isTrigger = false;
                    stuck = true;
                    velx = 0;
                    vely = 0;
                    hitList.RemoveRange(0, hitList.Count);
                    GetComponent<Animator>().enabled = false;
                }
                else
                {
                    returnBoomerang();
                    hitList.Add(collider.gameObject);
                }
            }
            else if(returning && collider.gameObject.tag == "Player")
            {
                superThrow = false;
                readyToThrow = true;
                returning = false;
                throwKeyPressedFrames = 0;
                framesSinceThrown = 0;
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                velx = 0;
                vely = 0;
                hitList.RemoveRange(0, hitList.Count);
                throwCooldown = throwCooldownLength;
            }
        }
    }

    private bool stickIsTilted()
    {
        float horStick = Input.GetAxis("RightHorizontal");
        float vertStick = Input.GetAxis("RightVertical");
        float dist = Mathf.Sqrt(Mathf.Pow(horStick, 2) + Mathf.Pow(vertStick, 2));
        if(dist >= stickThreshold)
            return true;
        else
            return false;
    }

    private void returnBoomerang()
    {
        throwAngle = Vector2.zero;
        GetComponent<Animator>().enabled = true;
        returning = true;
        stuck = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    /*UNUSED
    private Direction oppositeDirection(Direction dir)
    {
        if(dir == Direction.up)
            return Direction.down;
        else if(dir == Direction.left)
            return Direction.right;
        else if(dir == Direction.down)
            return Direction.up;
        else if(dir == Direction.right)
            return Direction.left;
        else if(dir == Direction.upleft)
            return Direction.downright;
        else if(dir == Direction.upright)
            return Direction.downleft;
        else if(dir == Direction.downleft)
            return Direction.upright;
        else if(dir == Direction.downright)
            return Direction.upleft;
        else
            return Direction.custom;
    }
    private Direction getDiagonal(Direction dir)
    {
        if(throwDir == Direction.custom || dir == Direction.custom || throwDir == Direction.none || dir == Direction.none)
        {
            Debug.Log("getDiagonal() error, throwDir or dir is custom or none");
            return throwDir;
        }
        else if(throwDir == Direction.upleft || throwDir == Direction.upright || throwDir == Direction.downleft || throwDir == Direction.downright)
        {
            Debug.Log("getDiagonal() error, throwDir is already a diagonal");
            return throwDir;
        }
        else if(dir == Direction.upleft || dir == Direction.upright || dir == Direction.downleft || dir == Direction.downright)
        {
            Debug.Log("getDiagonal() invalid parameter, received diagonal but only accepts nondiagonals");
            return throwDir;
        }
        else if(dir == throwDir)
            return dir;
        else if(dir == oppositeDirection(throwDir))
            return dir;
        else if(dir == Direction.up)
        {
            if(throwDir == Direction.left)
                return Direction.upleft;
            else
                return Direction.upright;
        }
        else if(dir == Direction.left)
        {
            if(throwDir == Direction.up)
                return Direction.upleft;
            else
                return Direction.downleft;
        }
        else if(dir == Direction.down)
        {
            if(throwDir == Direction.left)
                return Direction.downleft;
            else
                return Direction.downright;
        }
        else
        {
            if(throwDir == Direction.up)
                return Direction.upright;
            else
                return Direction.downright;
        }
    }
    private Direction removeDiagonal(Direction dir)
    {
        if(throwDir == Direction.custom || dir == Direction.custom || throwDir == Direction.none || dir == Direction.none)
        {
            Debug.Log("removeDiagonal() error, throwDir or dir is custom or none");
            return throwDir;
        }
        else if(throwDir != Direction.upleft && throwDir != Direction.upright && throwDir != Direction.downleft && throwDir != Direction.downright)
        {
            Debug.Log("removeDiagonal() error, throwDir is not a diagonal");
            return throwDir;
        }
        else if(dir == Direction.upleft || dir == Direction.upright || dir == Direction.downleft || dir == Direction.downright)
        {
            Debug.Log("removeDiagonal() invalid parameter, received diagonal but only accepts nondiagonals");
            return throwDir;
        }
        else if(dir == Direction.up)
        {
            if(throwDir == Direction.upright)
                return Direction.right;
            else if(throwDir == Direction.upleft)
                return Direction.left;
            else
                return throwDir;
        }
        else if(dir == Direction.left)
        {
            if(throwDir == Direction.upleft)
                return Direction.up;
            else if(throwDir == Direction.downleft)
                return Direction.down;
            else
                return throwDir;
        }
        else if(dir == Direction.down)
        {
            if(throwDir == Direction.downright)
                return Direction.right;
            else if(throwDir == Direction.downleft)
                return Direction.left;
            else
                return throwDir;
        }
        else
        {
            if(throwDir == Direction.upright)
                return Direction.up;
            else if(throwDir == Direction.downright)
                return Direction.down;
            else
                return throwDir;
        }
    }
    */
}