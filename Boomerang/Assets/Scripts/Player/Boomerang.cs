using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    //
    [SerializeField] private int throwBufferTime;
    
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
    private enum Direction{up, left, down, right}
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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        throwKeyPressedFrames = 0;
        readyToThrow = true;
        velx = 0;
        vely = 0;
        framesSinceThrown = 0;
        returning = false;
        hitList = new List<GameObject>();

        //Start out invisible and doesn't trigger stun states
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void Update()
    {
        if(readyToThrow)
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                throwDir = Direction.up;
                throwKeyPressedFrames = 1;
            }
            else if(Input.GetKeyDown(KeyCode.J))
            {
                throwDir = Direction.left;
                throwKeyPressedFrames = 1;
            }
            else if(Input.GetKeyDown(KeyCode.K))
            {
                throwDir = Direction.down;
                throwKeyPressedFrames = 1;
            }
            else if(Input.GetKeyDown(KeyCode.L))
            {
                throwDir = Direction.right;
                throwKeyPressedFrames = 1;
            }
        }
        else
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

    void FixedUpdate()
    {
        if(throwKeyPressedFrames > 0 && readyToThrow)
        {
            //Throw boomerang
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
            if(throwKeyPressedFrames > throwBufferTime)
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
            if(collider.gameObject.tag == "Enemy")
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
                        enemy.stun();
                        returnBoomerang(oppositeDirection(throwDir));
                        hitList.Add(collider.gameObject);
                    }
                }
            }

            if(returning && collider.gameObject.tag == "Player")
            {
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
}