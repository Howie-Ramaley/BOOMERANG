using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseGroundCheck : MonoBehaviour
{
    //Layer with all ground objects
    [SerializeField] private LayerMask groundLayer;

    //Tells if the player grounded or not
    private bool grounded;
    
    //Amount that FeetCheck's position is offset from the player
    private float offset;

    //Starting y position relative to the player
    private float starty;

    //
    private int collide;

    //Player's movement script
    private PlayerMovement playerMovement;

    void Start()
    {
        grounded = false;
        offset = 0;
        playerMovement = GetComponentInParent<PlayerMovement>();
        starty = transform.position.y - playerMovement.gameObject.transform.position.y;
        collide = 0;

        //Don't collide with player
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GetComponentInParent<BoxCollider2D>(), true);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), transform.parent.GetComponentInChildren<CapsuleCollider2D>(), true);
    }

    void LateUpdate()
    {
        //Set position to be at the bottom of the player and a little farther depending on how fast the player is falling down
        offset = Mathf.Abs(Mathf.Min(playerMovement.getRigidbody().velocity.y / 30, 0));
        transform.position = new Vector3(transform.position.x, transform.parent.position.y + starty - offset, transform.position.z);

        if(grounded && collide > 1)
            grounded = false;
        collide++;
    }

    //Helps code in OnCollisionStay2D happen quicker, triggers at the start of collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionStay2D(collision);
    }

    //Triggers when FeetCheck collides with another collider
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision != null)
        {
            //if a collider is in FeetCheck and is in the groundLayer
            if(collision != null && (((1 << collision.gameObject.layer) & groundLayer) != 0))
            {
                //bool none = true;
                for(int i = 0; i < collision.contactCount; i++)
                {
                    float normaly = collision.GetContact(i).normal.y;
                    //if the surface being collided with is not vertical (or almost vertical) then set grounded to true
                    if(normaly < -playerMovement.getSlip() || normaly > playerMovement.getSlip())
                    {
                        grounded = true;
                        //none = false;
                    }
                    //Debug.Log("normaly: " + normaly);
                }
                /*if(none)
                    grounded = false;*/
            }
            collide = 0;
        }
        else
        {
            grounded = false;
        }
    }

    //When collisions with FeetCheck stop, set grounded to false
    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }

    //Getters and setters
    public bool getGrounded()
    {
        return grounded;
    }
    public void setGrounded(bool g)
    {
        grounded = g;
    }
    public float getOffset()
    {
        return offset;
    }
}