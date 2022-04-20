using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Camera gameCamera;
    [SerializeField] private int followDuration;

    //Camera following
    private int followTime;
    private Vector2 startPosition;
    private Vector2 target;
    private Vector2 previousTarget;
    private string targetID;

    //Camera boundaries
    private Vector2 tlPoint;
    private Vector2 topLeftBoundary;
    private Vector2 brPoint;
    private Vector2 bottomRightBoundary;

    //Camera zoom
    private float cameraZoom;
    private float cameraDefaultSize;
    private float startZoom;
    private float targetZoom;

    //See if we're following the player, if we are let them update the camera, else we update the camera
    private bool followingPlayer;

    //Camera shake
    private Vector2 shakeOffset;
    private float shakeIntensity;
    private int shakeDuration;
    private int shakeTime;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        followTime = 0;
        targetID = "";
        float width = 19.2F * cameraZoom;
        float height = 10.8F * cameraZoom;
        tlPoint = transform.parent.Find("Top Left Camera Boundary").position;
        topLeftBoundary = new Vector2(tlPoint.x + (width / 2), tlPoint.y - (height / 2));
        brPoint = transform.parent.Find("Bottom Right Camera Boundary").position;
        bottomRightBoundary = new Vector2(brPoint.x - (width / 2), brPoint.y + (height / 2));
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
            //randomly decide camera shake offset, reset values if camera shake is over
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

            //keeping track of time
            followTime++;
            if(followTime > followDuration)
                followTime = followDuration;
            float t = ((float)followTime / (float)followDuration);
            
            //lerp from startZoom to targetZoom and update camera's zoom
            cameraZoom = Mathf.Lerp(startZoom, targetZoom, t);
            gameCamera.orthographicSize = cameraDefaultSize * cameraZoom;

            //update camera's boundaries to reflect new cameraZoom
            float width = 19.2F * cameraZoom;
            float height = 10.8F * cameraZoom;
            topLeftBoundary = new Vector2(tlPoint.x + (width / 2), tlPoint.y - (height / 2));
            bottomRightBoundary = new Vector2(brPoint.x - (width / 2), brPoint.y + (height / 2));

            //extra lerping to make camera following smoother
            Vector2 targ = target;
            if(previousTarget != null)
                targ = Vector2.Lerp(previousTarget, target, t);
            //lerp from startPosition to target and make sure camera does not escape level boundaries
            Vector2 lerp = clamp(Vector2.Lerp(startPosition, targ, t));
            
            //update camera position with camera shake offset
            transform.position = new Vector3(lerp.x + shakeOffset.x, lerp.y + shakeOffset.y, -10);
        }
    }

    private Vector2 clamp(Vector2 v)
    {
        float xmin = topLeftBoundary.x;
        float xmax = bottomRightBoundary.x;
        float ymin = bottomRightBoundary.y;
        float ymax = topLeftBoundary.y - 0.75f;
        return new Vector2(Mathf.Clamp(v.x, xmin, xmax), Mathf.Clamp(v.y, ymin, ymax));
    }

    public void setTarget(float x, float y, float zoom, string id)
    {
        previousTarget = target;
        target = new Vector2(x, y);
        targetZoom = zoom;
        if(targetID != id)
        {
            startPosition = transform.position;
            startZoom = cameraZoom;
            followTime = 0;
            targetID = id;
            if(id.Length >= 6 && id.Substring(0, 6) == "player")
                followingPlayer = true;
            else
                followingPlayer = false;
        }
        //Debug.Log("targetZoom: " + targetZoom + " startZoom: " + startZoom);
    }

    public void setZoom(float zoom, string id)
    {
        previousTarget = target;
        targetZoom = zoom;
        if(targetID != id)
        {
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
