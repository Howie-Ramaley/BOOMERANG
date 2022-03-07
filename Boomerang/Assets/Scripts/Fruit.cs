using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour, IStunnable
{

    protected bool stunned;
    protected float velx;
    protected float vely;
    //protected Vector2 startPosition;
    Rigidbody2D _rigidbody;

    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        //_rigidbody.gravityScale = 0;
        //velx = 0;
        //vely = 0;
        //startPosition = transform.position;
        stunned = true;
    }

    public virtual void bump(float angle)
    {
        stun();
    }

    public virtual void stun()
    {
        if(stunned)
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            _rigidbody.freezeRotation = true;
            stunned = false;
        }
    }
     
}
