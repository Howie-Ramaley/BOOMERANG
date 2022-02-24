using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField]private int damage;
    [SerializeField]private bool ignoresIFrames;
    private bool playerCollide;
    private bool hurts;

    // Start is called before the first frame update
    void Start()
    {
        playerCollide = false;
        hurts = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerCollide && hurts)
        {
            GameObject player;
            player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
                player.GetComponent<PlayerHealth>().hurt(damage, ignoresIFrames);
            else
                playerCollide = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerCollide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerCollide = false;
        }
    }

    public void setHurts(bool h) {hurts = h;}
    public bool gethurts() {return hurts;}
}
