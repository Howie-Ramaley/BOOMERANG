using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Camera gameCamera;

    [SerializeField] private int followDuration;

    private int followTime;

    private Vector2 previousStartPosition;

    private Vector2 startPosition;

    private Vector2 target;

    private Vector2 previousTarget;

    private string targetID;

    private Vector2 topLeft;

    private Vector2 bottomRight;

    private float cameraZoom;
    
    private float cameraDefaultSize;

    private float startZoom;

    private float targetZoom;

    private bool followingPlayer;

    private Vector2 shakeOffset;

    private float shakeIntensity;

    private int shakeDuration;

    private int shakeTime;

    // Start is called before the first frame update
    void Start()
    {
        previousStartPosition = transform.position;
        startPosition = transform.position;
        followTime = 0;
        targetID = "";
        float width = 19.2F;
        float height = 10.8F;
        Vector2 tl = transform.parent.Find("Top Left Camera Boundary").position;
        topLeft = new Vector2(tl.x + (width / 2), tl.y - (height / 2));
        Vector2 br = transform.parent.Find("Bottom Right Camera Boundary").position;
        bottomRight = new Vector2(br.x - (width / 2), br.y + (height / 2));
        cameraDefaultSize = gameCamera.orthographicSize;
        startZoom = 1.0F;
        targetZoom = 1.0F;
        cameraZoom = 1.0F;
        shakeOffset = Vector2.zero;
        shakeIntensity = 0;
        shakeDuration = 0;
        shakeTime = 0;
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
            if(shakeTime > 0)
            {
                shakeTime++;
                if(shakeTime > shakeDuration)
                {
                    shakeTime = 0;
                    shakeDuration = 0;
                    shakeIntensity = 0;
                    shakeOffset = Vector2.zero;
                }
                else
                {
                    float st = ((float)shakeDuration / (float)shakeTime);
                    shakeOffset = new Vector2(Random.Range(-shakeIntensity * st, shakeIntensity * st), Random.Range(-shakeIntensity * st, shakeIntensity * st));
                }
            }

            //Debug.Log("followTime: " + followTime + ", followDuration: " + followDuration);

            followTime++;
            if(followTime > followDuration)
                followTime = followDuration;
            float t = ((float)followTime / (float)followDuration);
            
            Vector2 targ = target;
            if(previousTarget != null)
                targ = Vector2.Lerp(previousTarget, target, t);
            Vector2 start = startPosition;
            //if(previousStartPosition != null)
            //    start = Vector2.Lerp(previousStartPosition, startPosition, t);
            
            Vector2 lerp = clamp(Vector2.Lerp(startPosition, targ, t));
            //Vector2 lerp = clamp(GameObject.FindGameObjectWithTag("Player").transform.position);
            
            transform.position = new Vector3(lerp.x + shakeOffset.x, lerp.y + shakeOffset.y, -10);
            
            cameraZoom = Mathf.Lerp(startZoom, targetZoom, t);
            gameCamera.orthographicSize = cameraDefaultSize * cameraZoom;

            //Debug.Log(startPosition.z + ", " + target.z + ", " + cameraZoom);
            //Debug.Log(t);
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

    public void setTarget(float x, float y, float zoom, string id)
    {
        previousTarget = target;
        target = new Vector2(x, y);
        targetZoom = zoom;
        if(targetID != id)
        {
            previousStartPosition = startPosition;
            startPosition = transform.position;
            startZoom = cameraZoom;
            followTime = 0;
            targetID = id;
            if(id.Length >= 6 && id.Substring(0, 6) == "player")
                followingPlayer = true;
            else
                followingPlayer = false;
        }
    }

    public void setZoom(float zoom, string id)
    {
        targetZoom = zoom;
        if(targetID != id)
        {
            startZoom = cameraZoom;
            followTime = 0;
            targetID = id;
            if(id.Length >= 6 && id.Substring(0, 6) == "player")
                followingPlayer = true;
            else
                followingPlayer = false;
        }
    }

    public void setShake(float intensity, int time)
    {
        shakeTime = 1;
        shakeDuration = time;
        shakeIntensity = intensity;
    }

    public void setDuration(int d)
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
    public float getCameraZoom()
    {
        return cameraZoom;
    }
    public float getTargetCameraZoom()
    {
        return targetZoom;
    }
}
