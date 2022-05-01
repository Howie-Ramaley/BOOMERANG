using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    string targetID;
    private FollowPlayer gameCamera;
    private int framesSinceLastCollide;
    private bool playerCollide;
    private Transform point;

    // Start is called before the first frame update
    void Start()
    {
        targetID = transform.gameObject.name;
        framesSinceLastCollide = 0;
        playerCollide = false;
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
        point = transform.Find("Point");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        framesSinceLastCollide++;
        if(framesSinceLastCollide >= 2)
            playerCollide = false;

        if(playerCollide)
            gameCamera.setTarget(point.position.x, point.position.y, targetID);
        else
            if(gameCamera.getTargetID() == targetID)
                gameCamera.setTargetID("");
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
}