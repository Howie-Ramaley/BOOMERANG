using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;

    private Vector2 startPosition;

    private Vector2 target;

    private float followProgress;

    [SerializeField] private float followDuration;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        startPosition = transform.position;
        followProgress = 0;
    }

    //Set the camera's position to follow the player's position
    //LateUpdate is called once per frame after all update functions have been called
    void LateUpdate()
    {
        //todo:
        //give camera set target points based on player's position and actions that it will follow

        if(player != null)
        {
            target = new Vector2(player.transform.position.x, player.transform.position.y);
        }
        else
        {
            player = GameObject.Find("Player");
        }

        if(target != null)
        {
            float t = followProgress / followDuration;
            
            Vector2 lerp = Vector2.Lerp(startPosition, target, t);
            transform.position = new Vector3(lerp.x, lerp.y, -10);

            followProgress++;
            if(followProgress > followDuration)
                followProgress = followDuration;
        }
    }

    public void setTarget(float x, float y)
    {
        target = new Vector2(x, y);
        startPosition = transform.position;
        followProgress = 0;
    }

    public void setDuration(float d)
    {
        followDuration = d;
    }
}
