using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]private float force;
    [SerializeField]private int respawnTime;
    private PlayerMovement player;
    private int respawnFrames;
    private CircleCollider2D circCollider;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        respawnFrames = 0;
        circCollider = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(respawnFrames > 0)
        {
            respawnFrames++;
            if(respawnFrames > respawnTime)
            {
                respawnFrames = 0;
                circCollider.enabled = true;
                sprite.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnTriggerStay2D(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            player.launch(0F, force);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            player.launch(0F, force);
            circCollider.enabled = false;
            sprite.enabled = false;
            respawnFrames = 1;
        }
    }
}
