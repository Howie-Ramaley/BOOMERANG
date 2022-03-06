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

    //Reference to player
    private GameObject player;

    //
    private int throwKeyPressedFrames;

    //
    private int throwKeyHeldFrames;

    //
    private enum Direction{up, left, down, right, upleft, upright, downleft, downright, none}
    private Direction throwDir;

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
        if(readyToThrow)
        {
            Direction dir = Direction.none;
            bool twoKeys = false;
            if(Input.GetKeyDown(KeyCode.I))
                dir = Direction.up;
            else if(Input.GetKeyDown(KeyCode.J))
                dir = Direction.left;
            else if(Input.GetKeyDown(KeyCode.K))
                dir = Direction.down;
            else if(Input.GetKeyDown(KeyCode.L))
                dir = Direction.right;
            if(dir != Direction.none && throwKeyPressedFrames == 0)
            {
                twoKeys = true;
                Direction nextDir = Direction.none;
                if(Input.GetKeyDown(KeyCode.I) && dir != Direction.up)
                    nextDir = Direction.up;
                else if(Input.GetKeyDown(KeyCode.J) && dir != Direction.left)
                    nextDir = Direction.left;
                else if(Input.GetKeyDown(KeyCode.K) && dir != Direction.down)
                    nextDir = Direction.down;
                else if(Input.GetKeyDown(KeyCode.L) && dir != Direction.right)
                    nextDir = Direction.right;
                else
                    twoKeys = false;
                if(twoKeys)
                {
                    throwKeyPressedFrames = 1;
                    throwDir = dir;
                    throwDir = getDiagonal(nextDir);
                }
            }
            if(dir != Direction.none && !twoKeys)
            {
                if(throwKeyPressedFrames == 0)
                {
                    throwKeyPressedFrames = 1;
                    throwDir = dir;
                }
                else if(throwDir == Direction.up || throwDir == Direction.down || throwDir == Direction.left || throwDir == Direction.right)
                    throwDir = getDiagonal(dir);
            }
        }
        else
        {
            if(stuck)
            {
                if(Input.GetKeyDown(KeyCode.I))
                    returnBoomerang(Direction.up);
                else if(Input.GetKeyDown(KeyCode.J))
                    returnBoomerang(Direction.left);
                else if(Input.GetKeyDown(KeyCode.K))
                    returnBoomerang(Direction.down);
                else if(Input.GetKeyDown(KeyCode.L))
                    returnBoomerang(Direction.right);
            }
        }
    }

    void FixedUpdate()
    {
        if(readyToThrow && throwKeyPressedFrames > 0 && (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.L)))
        {
            throwKeyHeldFrames++;
            if(throwKeyPressedFrames > 0 && throwKeyPressedFrames < throwBufferTime)
                throwKeyPressedFrames = diagonalInputBufferTime - 1;
            if(throwKeyHeldFrames > superThrowHoldTime)
                GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerSprite").gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if(throwKeyPressedFrames > diagonalInputBufferTime && readyToThrow)
        {
            //Throw boomerang
            Color color = Color.white;
            int health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().getHealth();
            if(health == 2)
                color = new Color(1, 0.75f, 0.75f);
            else if(health == 1)
                color = new Color(1, 0.2f, 0.2f);
            GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerSprite").gameObject.GetComponent<SpriteRenderer>().color = color;
            if(throwKeyHeldFrames > superThrowHoldTime)
                superThrow = true;
            throwKeyHeldFrames = 0;
            throwKeyPressedFrames = 0;
            framesSinceThrown = 1;
            readyToThrow = false;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
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
        }

        if(returning)
        {
            float dist = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2));
            dist *= 50;
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
            if(throwKeyPressedFrames > diagonalInputBufferTime + throwBufferTime)
                throwKeyPressedFrames = 0;
        }

        if(framesSinceThrown > 0)
        {
            framesSinceThrown++;
            if(framesSinceThrown > autoReturnTime)
            {
                //auto return
                framesSinceThrown = 0;
                returnBoomerang(oppositeDirection(throwDir));
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
        if(collider != null)
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
                        returnBoomerang(oppositeDirection(throwDir));
                        hitList.Add(collider.gameObject);
                    }
                }
            }
            else if(((1 << collider.gameObject.layer) & groundLayer) != 0)
            {
                if(superThrow)
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
                }
                else
                {
                    returnBoomerang(oppositeDirection(throwDir));
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
            }
        }
    }

    private void returnBoomerang(Direction dir)
    {
        returning = true;
        stuck = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private Direction oppositeDirection(Direction dir)
    {
        if(dir == Direction.up)
            return Direction.down;
        else if(dir == Direction.left)
            return Direction.right;
        else if(dir == Direction.down)
            return Direction.up;
        else
            return Direction.left;
    }

    private Direction getDiagonal(Direction dir)
    {
        if(dir == throwDir)
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
}