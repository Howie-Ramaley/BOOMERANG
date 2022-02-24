using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBumpCheck : MonoBehaviour
{
    //Layer with all ground objects
    [SerializeField] private LayerMask groundLayer;

    //Triggers when collider intersects HeadCheck
    private void OnTriggerStay2D(Collider2D collider)
    {
        var playerMovement = GetComponentInParent<PlayerMovement>() as PlayerMovement;
        //if player is moving up
        if(playerMovement.getVely() - playerMovement.getGravityVel() > 0)
        {
            //if a collider is in the HeadCheck and is in the groundLayer
            if(collider != null && (((1 << collider.gameObject.layer) & groundLayer) != 0))
            {
                Rigidbody2D otherBody = collider.gameObject.GetComponent<Rigidbody2D>();
                //if the collider's object has a rigidbody, transfer velocity with respect to each of their masses
                if(otherBody != null)
                {
                    float vx = playerMovement.getVelx();
                    float vy = playerMovement.getVely();
                    playerMovement.setVelx(vx * playerMovement.getRigidbody().mass / (playerMovement.getRigidbody().mass + otherBody.mass));
                    playerMovement.setVely(vy * playerMovement.getRigidbody().mass / (playerMovement.getRigidbody().mass + otherBody.mass));
                    otherBody.velocity = new Vector2(
                        otherBody.velocity.x + vx * otherBody.mass / (playerMovement.getRigidbody().mass + otherBody.mass),
                        otherBody.velocity.y + vy * otherBody.mass / (playerMovement.getRigidbody().mass + otherBody.mass));
                }
                //else just set player's vely and gravityVel to 0
                else
                {
                    playerMovement.setVely(0);
                    playerMovement.setGravityVel(0);
                }
            }
        }
    }
}
