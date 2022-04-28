using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangPickup : MonoBehaviour
{
    [SerializeField]private GameObject boomerang;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(boomerang != null)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                boomerang.GetComponent<Boomerang>().enabled = true;
                boomerang.GetComponent<Animator>().enabled = true;
                boomerang.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}
