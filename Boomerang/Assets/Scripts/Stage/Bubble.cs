using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]private float force;
    [SerializeField]private int respawnTime;
    private PlayerMovement player;
    private int respawnFrames;
    private CircleCollider2D circCollider;
    private GameObject sprite;
    private Vector2 spriteOffset;
    private Vector2 spawnPoint;
    private Vector2 respawnOffset;
    private float respawnScale;
    private int frameCounter;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        respawnFrames = 0;
        circCollider = GetComponent<CircleCollider2D>();
        sprite = transform.Find("BubbleSprite").gameObject;
        spriteOffset = Vector2.zero;
        respawnOffset = Vector2.zero;
        spawnPoint = transform.Find("SpawnPoint").position - transform.position;
        respawnScale = 1;
        frameCounter = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        frameCounter++;

        if(respawnFrames > 0)
        {
            respawnFrames++;
            float t = (float)respawnFrames / (float)respawnTime;
            //Debug.Log(spawnPoint.x + ", " + spawnPoint.y);
            respawnOffset = Vector2.Lerp(spawnPoint, Vector2.zero, t);
            respawnScale = Mathf.Lerp(0, 1, t);
            if(respawnFrames > respawnTime)
            {
                respawnFrames = 0;
                circCollider.enabled = true;
                //sprite.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        spriteOffset.x = Mathf.Sin((float)frameCounter / 50F) / 10F;
        spriteOffset.y = Mathf.Sin((float)frameCounter / 20F) / 10F;
        Vector3 offset = new Vector3(spriteOffset.x + respawnOffset.x, spriteOffset.y + respawnOffset.y, 0);
        sprite.transform.position = transform.position + offset;
        sprite.transform.localScale = new Vector3(respawnScale, respawnScale, 1);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        OnTriggerStay2D(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(player != null)
                player.launch(0F, force);
            else
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
                player.launch(0F, force);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(player != null)
                player.launch(0F, force);
            else
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
                player.launch(0F, force);
            }
            SoundManager.PlaySound("pop");
            circCollider.enabled = false;
            //sprite.GetComponent<SpriteRenderer>().enabled = false;
            spriteOffset = Vector2.zero;
            respawnFrames = 1;
            frameCounter = 1;
        }
    }
}
