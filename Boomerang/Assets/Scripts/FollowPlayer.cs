using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    //Set the camera's position to follow the player's position
    //LateUpdate is called once per frame after all update functions have been called
    void LateUpdate()
    {
        //todo:
        //give camera set target points based on player's position and actions that it will follow
        //linear interpolation

        if(player != null)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        }
        else
        {
            player = GameObject.Find("Player");
        }
    }
}
