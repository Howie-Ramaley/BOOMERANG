using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFallCheck : MonoBehaviour
{
    //Layer with all ground objects
    [SerializeField] private LayerMask groundLayer;
    private List<Collision2D> groundList;
    private bool noGround;
    private bool approachingGround;
    private int framesSinceLastCollide;
    private PreciseGroundCheck feetCheck;
    private BoxCollider2D boxCollider;
    private float starty;
    private float startyScale;

    // Start is called before the first frame update
    void Start()
    {
        groundList = new List<Collision2D>();
        framesSinceLastCollide = 0;
        boxCollider = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(boxCollider, GetComponentInParent<BoxCollider2D>(), true);
        Physics2D.IgnoreCollision(boxCollider, transform.parent.GetComponentInChildren<CapsuleCollider2D>(), true);
        feetCheck = transform.parent.Find("FeetCheck").GetComponent<PreciseGroundCheck>();
        Physics2D.IgnoreCollision(boxCollider, feetCheck.gameObject.GetComponent<BoxCollider2D>(), true);
        starty = transform.position.y - transform.parent.position.y;
        startyScale = transform.localScale.y;
    }

    void FixedUpdate()
    {
        if(groundList.Count > 0 && framesSinceLastCollide >= 2)
            groundList.RemoveRange(0, groundList.Count);
        framesSinceLastCollide++;

        if(groundList.Count == 0)
            noGround = true;
        else if(noGround)
            approachingGround = true;
        /*Debug.Log(noGround + " approaching:" + approachingGround + " list:" + groundList.Count);
        if(groundList.Count > 0)
            Debug.Log(groundList[0].gameObject.name);*/
    }

    void LateUpdate()
    {
        float offset = feetCheck.getOffset();
        transform.position = new Vector3(transform.position.x, transform.parent.position.y + starty - offset, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, startyScale - (offset * 2), transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionStay2D(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision != null && (((1 << collision.gameObject.layer) & groundLayer) != 0) && collision.gameObject.tag != "FeetCheck")
        {
            if(!groundList.Contains(collision))
                groundList.Add(collision);
            framesSinceLastCollide = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(groundList.Contains(collision))
            groundList.Remove(collision);
    }

    public bool isApproachingGround()
    {
        return approachingGround;
    }

    public void reset()
    {
        noGround = false;
        approachingGround = false;
    }
}
