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
        playerLand = Resources.Load<AudioClip>("playerLand");
        playerHit = Resources.Load<AudioClip>("playerHit");
        playerRoll = Resources.Load<AudioClip>("playerRoll");
        
        boomerangThrow = Resources.Load<AudioClip>("boomerangThrow");
        boomerangDirtStuck = Resources.Load<AudioClip>("boomerangDirtStuck");
        //boomerangSpongeStuck = Resources.Load<AudioClip>("boomerangSpongeStuck");
        //boomerangCatch = Resources.Load<AudioClip>("boomerangCatch");
        
        enemyHit = Resources.Load<AudioClip>("enemyHit");
        //enemyRoar = Resources.Load<AudioClip>("enemyRoar");
        enemyWake = Resources.Load<AudioClip>("enemyWake");
        //enemyTeleport = Resources.Load<AudioClip>("enemyTeleport");
        //enemyLob = Resources.Load<AudioClip>("enemyLob");
        //enemyProjectile = Resources.Load<AudioClip>("enemyProjectile");
        
        //bubblePop = Resources.Load<AudioClip>("bubblePop");
    }

    public static void PlaySound(string clip)
    {
        audioSrc.volume = 1.0f;
        switch(clip)
        {
            case "jump":
                audioSrc.PlayOneShot(playerJump);
                break;
            case "land":
                audioSrc.volume = 0.5f;
                audioSrc.PlayOneShot(playerLand);
                break;
            case "p_hit":
                audioSrc.PlayOneShot(playerHit);
                break;
            case "roll":
                audioSrc.PlayOneShot(playerRoll);
                break;

            case "throw":
                audioSrc.PlayOneShot(boomerangThrow);
                break;
            case "d_stuck":
                audioSrc.PlayOneShot(boomerangDirtStuck);
                break;
            /*case "s_stuck":
                audioSrc.PlayOneShot(boomerangeSpongeStuck);
                break;
            case "catch":
                audioSrc.PlayOneShot(boomerangCatch);
                break;*/
            case "e_hit":
                audioSrc.PlayOneShot(enemyHit);
                break;
            /*case "roar":
                audioSrc.PlayOneShot(enemyRoar);
                break;*/
            case "wake":
                audioSrc.PlayOneShot(enemyWake);
                break;
            /*case "teleport":
                audioSrc.PlayOneShot(enemyTeleport);
                break;
            /*case "lob":
                audioSrc.PlayOneShot(enemyLob);
                break;
            /*case "projectile":
                audioSrc.PlayOneShot(enemyProjectile);
                break;
            /*case "pop":
                audioSrc.PlayOneShot(bubblePop);
                break;
            */
        }

    }
}
