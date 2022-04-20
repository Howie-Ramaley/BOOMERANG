using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrigger : MonoBehaviour
{
    private int framesSinceLastCollide;
    private bool playerCollide;

    private int framesSinceEnter;
    private bool playerEnter;

    private int framesSinceExit;
    private bool playerExit;

    // Start is called before the first frame update
    void Start()
    {
        framesSinceLastCollide = 0;
        playerCollide = false;

        framesSinceEnter = 0;
        playerEnter = false;

        framesSinceExit = 0;
        playerExit = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        framesSinceLastCollide++;
        if(framesSinceLastCollide >= 2)
            playerCollide = false;

        framesSinceEnter++;
        if(framesSinceEnter >= 2)
            playerEnter = false;

        framesSinceExit++;
        if(framesSinceExit >= 2)
            playerExit = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerEnter = true;
            framesSinceEnter = 0;
        }
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
            playerExit = true;
        }
    }

    public bool getPlayerCollide()
    {
        return playerCollide;
    }

    public bool getPlayerEnter()
    {
        return playerEnter;
    }

    public bool getPlayerExit()
    {
        return playerExit;
    }
}
