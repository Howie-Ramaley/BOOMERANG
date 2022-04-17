using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobGob : MonoBehaviour, IStunnable
{
    private float amplitude;
    private float frequency;
    private float speed;
    private Transform player;
    private PlayerHealth playerHealth;
    private LobGoblin lobGoblin;
    private float angle;
    private int shotProgress;
    private Vector2 startPos;
    private bool reflected;
    private PlayerHit playerHit;

    // Start is called before the first frame update
    void Start()
    {
        lobGoblin = transform.parent.GetComponentInChildren<LobGoblin>();
        speed = lobGoblin.getProjectileSpeed();
        amplitude = lobGoblin.getProjectileAmplitude();
        frequency = lobGoblin.getProjectileFrequency();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        angle = 0;
        shotProgress = 0;
        startPos = transform.position;
        reflected = false;
        playerHit = GetComponent<PlayerHit>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(getShot() && !reflected)
        {
            float spd = speed;
            float amp = amplitude;
            float frq = frequency;
            if(shotProgress < 25)
            {
                spd = speed * shotProgress / 25f;
                //frq = frequency * shotProgress / 25f;
                amp = amplitude * shotProgress / 25f;
            }
            Vector2 baseDisp = new Vector2(
                spd * shotProgress,
                Mathf.Sin((shotProgress) * frq) * amp
            );
            float deg = -angle + Mathf.PI/2;
            Vector2 displace = new Vector2(
                Mathf.Cos(deg) * baseDisp.x - Mathf.Sin(deg) * baseDisp.y,
                Mathf.Sin(deg) * baseDisp.x + Mathf.Cos(deg) * baseDisp.y
            );
            transform.position = new Vector3(startPos.x + displace.x, startPos.y + displace.y, transform.position.z);
        }
        else if(getShot())
        {
            Vector2 displace = new Vector2(
                speed * 2 * Mathf.Sin(angle) * shotProgress,
                speed * 2 * Mathf.Cos(angle) * shotProgress
            );
            transform.position = new Vector3(startPos.x + displace.x, startPos.y + displace.y, transform.position.z);
        }
        else
            transform.position = lobGoblin.gameObject.transform.position;

        if(Mathf.Sqrt(Mathf.Pow(player.position.x - transform.position.x, 2) + Mathf.Pow(player.position.y - transform.position.y, 2)) >= 14)
            reset();

        if(playerHit.getHit() || playerHealth.getDied())
        {
            reset();
            playerHit.resetHit();
        }

        if(shotProgress > 0)
            shotProgress++;
    }

    public void shoot()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        speed = lobGoblin.getProjectileSpeed();
        amplitude = lobGoblin.getProjectileAmplitude();
        frequency = lobGoblin.getProjectileFrequency();
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<PlayerHit>().setHurts(true);
        Vector2 p = new Vector2(player.position.x, player.position.y);
        angle = -Mathf.Atan2(p.y - transform.position.y, p.x - transform.position.x) + Mathf.PI / 2;
        startPos = transform.position;
        shotProgress = 1;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnTriggerStay2D(collider);
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if(getShot() && reflected && collider.gameObject == lobGoblin.gameObject)
        {
            reset();
            lobGoblin.wooYeahGetThatGoblinFucker();
        }
    }

    public bool getShot()
    {
        return shotProgress > 0;
    }

    public void reset()
    {
        shotProgress = 0;
        reflected = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PlayerHit>().setHurts(false);
    }

    public bool stun()
    {
        if(getShot())
        {
            reflected = true;
            Vector2 lg = new Vector2(lobGoblin.gameObject.transform.position.x, lobGoblin.gameObject.transform.position.y);
            angle = -Mathf.Atan2(lg.y - transform.position.y, lg.x - transform.position.x) + Mathf.PI / 2;
            shotProgress = 1;
            startPos = transform.position;
            lobGoblin.gameObject.GetComponent<LobGoblin>().freeze();   
            return true;
        }
        else
            return false;
    }
    public bool bump(float angle, float bumpStrength)
    {
        return stun();
    }
}
