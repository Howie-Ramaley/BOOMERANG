using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private float followDuration;

    private float followStartTime;

    private Vector2 startPosition;

    private Vector2 target;

    private string targetID;

    private Vector2 topLeft;

    private Vector2 bottomRight;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        followStartTime = 0;
        targetID = "";
        float width = 19.2F;
        float height = 10.8F;
        Vector2 tl = transform.parent.Find("Top Left Camera Boundary").position;
        topLeft = new Vector2(tl.x + (width / 2), tl.y - (height / 2));
        Vector2 br = transform.parent.Find("Bottom Right Camera Boundary").position;
        bottomRight = new Vector2(br.x - (width / 2), br.y + (height / 2));
    }

    //Set the camera's position to follow the player's position
    //LateUpdate is called once per frame after all update functions have been called
    void LateUpdate()
    {
        //todo:
        //give camera set target points based on player's position and actions that it will follow

        if(target != null)
        {
            float t = (Time.time - followStartTime) / followDuration;
            Vector2 lerp = clamp(Vector2.Lerp(startPosition, target, t));
            transform.position = new Vector3(lerp.x, lerp.y, -10);
        }
    }

    private Vector2 clamp(Vector2 v)
    {
        float xmin = topLeft.x;
        float xmax = bottomRight.x;
        float ymin = bottomRight.y;
        float ymax = topLeft.y;
        return new Vector2(Mathf.Clamp(v.x, xmin, xmax), Mathf.Clamp(v.y, ymin, ymax));
    }

    public void setTarget(float x, float y, string id)
    {
        target = new Vector2(x, y);
        if(targetID != id)
        {
            startPosition = transform.position;
            followStartTime = Time.time;
            targetID = id;
        }
    }

    public void setDuration(float d)
    {
        followDuration = d;
    }
}
