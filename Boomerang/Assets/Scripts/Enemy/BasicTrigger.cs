using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrigger : MonoBehaviour
{
    private int framesSinceLastCollide;
    private bool playerCollide;

    // Start is called before the first frame update
    void Start()
    {
        framesSinceLastCollide = 0;
        playerCollide = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        framesSinceLastCollide++;
        if(framesSinceLastCollide >= 2)
            playerCollide = false;
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

    public bool getPlayerCollide()
    {
        return playerCollide;
    }
}