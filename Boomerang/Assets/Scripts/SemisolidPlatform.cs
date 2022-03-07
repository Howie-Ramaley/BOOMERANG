using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemisolidPlatform : MonoBehaviour
{
    private GameObject player;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            float playerHeight = 1.5F;
            if(player.GetComponentInChildren<PlayerAnimation>().getAnimState() == PlayerAnimation.AnimationState.roll)
                playerHeight = 1F;
            playerHeight *= player.transform.localScale.y;
            float playerWidth = 1F * player.transform.localScale.x;
            float pBottom = player.transform.position.y - (playerHeight / 2F);
            float pRight = player.transform.position.x + playerWidth / 2F;
            float pLeft = player.transform.position.x - playerWidth / 2F;
            float top = transform.position.y + transform.localScale.y / 2F;
            float bottom = transform.position.y - transform.localScale.y / 2F;
            if(pRight > transform.position.x - transform.localScale.x / 2 && pLeft < transform.position.x + transform.localScale.x / 2)
            {
                if(pBottom > bottom && pBottom < top)
                    player.transform.position = new Vector3(player.transform.position.x, top + 0.01F + (playerHeight / 2F), player.transform.position.z);
                if(pBottom >= top)
                    boxCollider.isTrigger = false;
                else
                    boxCollider.isTrigger = true;
            }
            else
                boxCollider.isTrigger = true;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
