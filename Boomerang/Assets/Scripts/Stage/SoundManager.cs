using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    static AudioSource audioSrc;

    public static AudioClip playerJump, playerLand, playerHit, playerRoll;

    public static AudioClip boomerangThrow, boomerangDirtStuck, boomerangSpongeStuck, boomerangCatch;

    public static AudioClip enemyHit, enemyRoar, enemyWake, enemyTeleport, enemyLob, enemyProjectile;

    public static AudioClip bubblePop;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        playerJump = Resources.Load<AudioClip>("playerJump");
        boomerangThrow = Resources.Load<AudioClip>("boomerangThrow");

        /*
        playerLand = Resources.Load<AudioClip>("playerLand");
        playerHit = Resources.Load<AudioClip>("playerHit");
        playerRoll = Resources.Load<AudioClip>("playerRoll");
        
        boomerangDirtStuck = Resources.Load<AudioClip>("boomerangDirtStuck");
        boomerangSpongeStuck = Resources.Load<AudioClip>("boomerangSpongeStuck");
        boomerangCatch = Resources.Load<AudioClip>("boomerangCatch");
        
        enemyHit = Resources.Load<AudioClip>("enemyHit");
        enemyRoar = Resources.Load<AudioClip>("enemyRoar");
        enemyWake = Resources.Load<AudioClip>("enemyWake");
        enemyTeleport = Resources.Load<AudioClip>("enemyTeleport");
        enemyLob = Resources.Load<AudioClip>("enemyLob");
        enemyProjectile = Resources.Load<AudioClip>("enemyProjectile");
        
        bubblePop = Resources.Load<AudioClip>("bubblePop");
        */
    }

    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "jump":
                audioSrc.PlayOneShot(playerJump);
                break;
            case "throw":
                audioSrc.PlayOneShot(boomerangThrow);
                break;
            /*
            case "land":
                audioSrc.PlayOneShot(playerLand);
                break;
            */
        }

    }
}