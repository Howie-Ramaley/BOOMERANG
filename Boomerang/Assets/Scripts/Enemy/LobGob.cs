using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobGob : MonoBehaviour, IStunnable
{
    private float speed;
    private Transform player;
    public LobGoblin lobGoblin;
    private float velx;
    private float vely;
    private bool shot;
    private bool reflected;
    private bool hitPlayer;

    // Start is called before the first frame update
    void Start()
    {
        speed = transform.parent.GetComponentInChildren<LobGoblin>().getProjectileSpeed();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        velx = 0;
        vely = 0;
        shot = false;
        reflected = false;
        hitPlayer = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(hitPlayer)
        {
            shot = false;
            hitPlayer = false;
        }

        if(shot && !reflected)
        {
            transform.position = new Vector3(transform.position.x + velx, transform.position.y + vely, transform.position.z);
        }
        else if(shot)
            transform.position = new Vector3(transform.position.x + velx, transform.position.y + vely, transform.position.z);
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<PlayerHit>().setHurts(false);
            transform.position = lobGoblin.gameObject.transform.position;
        }

        if(Mathf.Sqrt(Mathf.Pow(player.position.x - transform.position.x, 2) + Mathf.Pow(player.position.y - transform.position.y, 2)) >= 14)
            shot = false;
    }

    public void shoot()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        shot = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<PlayerHit>().setHurts(true);
        Vector2 p = new Vector2(player.position.x, player.position.y);
        float angle = -Mathf.Atan2(p.y - transform.position.y, p.x - transform.position.x) + Mathf.PI / 2;
        velx = speed * Mathf.Sin(angle);
        vely = speed * Mathf.Cos(angle);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnTriggerStay2D(collider);
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if(shot && reflected && collider.gameObject == lobGoblin.gameObject)
        {
            reflected = false;
            shot = false;
            lobGoblin.wooYeahGetThatGoblinFucker();
        }
        else if(shot && collider.gameObject.tag == "Player")
        {
            if(player.gameObject.GetComponent<PlayerHealth>().getIFrameProgress() == 0)
                hitPlayer = true;
        }
    }

    public bool getShot()
    {
        return shot;
    }

    public bool stun()
    {
        reflected = true;
        Vector2 lg = new Vector2(lobGoblin.gameObject.transform.position.x, lobGoblin.gameObject.transform.position.y);
        float angle = -Mathf.Atan2(lg.y - transform.position.y, lg.x - transform.position.x) + Mathf.PI / 2;
        velx = speed * 2 * Mathf.Sin(angle);
        vely = speed * 2 * Mathf.Cos(angle);
        lobGoblin.gameObject.GetComponent<LobGoblin>().freeze();   
        return true;
    }
    public bool bump(float angle, float bumpStrength)
    {
        return stun();
    }
}
