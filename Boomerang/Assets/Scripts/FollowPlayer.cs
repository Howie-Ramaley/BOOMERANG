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

    private bool followingPlayer;

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

    //Set the camera's position to follow target
    //LateUpdate is called once per frame after all update functions have been called
    void FixedUpdate()
    {
        if(!followingPlayer)
            manualCameraUpdate();
    }
    //Manually called by player to update camera at the proper time and prevent jitter
    public void manualCameraUpdate()
    {
        if(target != null)
        {
            float t = (Time.time - followStartTime) / followDuration;
            Vector2 lerp = clamp(Vector2.Lerp(startPosition, target, t));
            //Vector2 lerp = clamp(GameObject.FindGameObjectWithTag("Player").transform.position);
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
            if(id.Length >= 6 && id.Substring(0, 6) == "player")
                followingPlayer = true;
            else
                followingPlayer = false;
        }
    }

    public void setDuration(float d)
    {
        followDuration = d;
    }
    public void setTargetID(string id)
    {
        targetID = id;
    }
    public string getTargetID()
    {
        return targetID;
    }
}
