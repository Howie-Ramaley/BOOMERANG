using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobGob : MonoBehaviour
{
    private float speed;
    private Transform player;
    private float velx;
    private float vely;
    private bool shot;

    // Start is called before the first frame update
    void Start()
    {
        speed = transform.parent.GetComponentInChildren<LobGoblin>().getProjectileSpeed();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        velx = 0;
        vely = 0;
        shot = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shot)
            transform.position = new Vector3(transform.position.x + velx, transform.position.y + vely, transform.position.z);
        else
            GetComponent<SpriteRenderer>().enabled = false;
    }

    public void shoot()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        shot = true;
        GetComponent<SpriteRenderer>().enabled = true;
        Vector2 p = new Vector2(player.transform.position.x, player.transform.position.y);
        float angle = -Mathf.Atan2(p.y - transform.position.y, p.x - transform.position.x) + Mathf.PI / 2;
        velx = speed * Mathf.Sin(angle);
        vely = speed * Mathf.Cos(angle);
    }

    public bool getShot()
    {
        return shot;
    }
}
