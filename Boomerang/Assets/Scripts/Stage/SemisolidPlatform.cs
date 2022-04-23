using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemisolidPlatform : MonoBehaviour
{
    [SerializeField] private Transform player;
    private BoxCollider2D boxCollider;
    private PolygonCollider2D polyCollider;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        boxCollider = GetComponent<BoxCollider2D>();
        polyCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null && player.gameObject != null)
        {
            float playerHeight = 1.5F;
            //if(player.GetComponentInChildren<PlayerAnimation>().getAnimState() == PlayerAnimation.AnimationState.roll)
                //playerHeight = 1F;
            playerHeight *= player.localScale.y;
            float playerWidth = 1F * player.localScale.x;
            float pBottom = player.position.y - (playerHeight / 2F);
            float pRight = player.position.x + playerWidth / 2F;
            float pLeft = player.position.x - playerWidth / 2F;
            /*float top = transform.position.y + transform.localScale.y / 2F;
            float bottom = transform.position.y - transform.localScale.y / 2F;
            if(polyCollider == null)
            {*/
                float top = transform.position.y + (boxCollider.offset.y * transform.localScale.y) + transform.localScale.y * boxCollider.size.y / 2F;
                float bottom = transform.position.y + (boxCollider.offset.y * transform.localScale.y) - transform.localScale.y * boxCollider.size.y / 2F;
            //}
            if(!Input.GetKey(KeyCode.S) && Input.GetAxis("Vertical") > -0.8F && (pRight > transform.position.x - transform.localScale.x / 2 && pLeft < transform.position.x + transform.localScale.x / 2))
            {
                if(pBottom > bottom && pBottom < top)
                {
                    player.position = new Vector3(player.position.x, top + 0.01F + (playerHeight / 2F), player.position.z);
                    PlayerMovement playerMovement = player.gameObject.GetComponent<PlayerMovement>();
                    if(playerMovement.getVely() + playerMovement.getGravityVel() < 0)
                    {
                        playerMovement.setGravityVel(0);
                        playerMovement.setVely(0);
                        playerMovement.getRigidbody().velocity = new Vector2(playerMovement.getRigidbody().velocity.x, 0);
                    }
                }
                if(polyCollider == null)
                {
                    if(pBottom >= top)
                        boxCollider.isTrigger = false;
                    else
                        boxCollider.isTrigger = true;
                }
                else
                {
                    if(pBottom >= top)
                        polyCollider.isTrigger = false;
                    else
                        polyCollider.isTrigger = true;
                }
            }
            else
            {
                if(polyCollider == null)
                    boxCollider.isTrigger = true;
                else
                    polyCollider.isTrigger = true;
            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
