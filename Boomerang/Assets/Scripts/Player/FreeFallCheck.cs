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

    // Start is called before the first frame update
    void Start()
    {
        groundList = new List<Collision2D>();
        framesSinceLastCollide = 0;
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GetComponentInParent<BoxCollider2D>(), true);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), transform.parent.GetComponentInChildren<CapsuleCollider2D>(), true);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), transform.parent.Find("FeetCheck").GetComponent<BoxCollider2D>(), true);
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
        Debug.Log(noGround + " approaching:" + approachingGround + " list:" + groundList.Count);
        if(groundList.Count > 0)
            Debug.Log(groundList[0].gameObject.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionStay2D(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision != null && (((1 << collision.gameObject.layer) & groundLayer) != 0))
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
