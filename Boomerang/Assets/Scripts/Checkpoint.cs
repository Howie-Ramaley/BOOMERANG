using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private int framesSinceLastCollide;
    private bool playerCollide;
    private PlayerMovement player;
    private Animator animator;
    private bool lit;

    // Start is called before the first frame update
    void Start()
    {
        framesSinceLastCollide = 0;
        playerCollide = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        lit = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        framesSinceLastCollide++;
        if(framesSinceLastCollide >= 2)
            playerCollide = false;

        if(playerCollide)
        {
            player.setCheckpoint(transform.position.x, transform.position.y, !lit);
            lit = true;
            animator.SetBool("lit", lit);
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
            playerCollide = true;
            framesSinceLastCollide = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerCollide = false;
        }
    }

    public bool getLit()
    {
        return lit;
    }
}
