using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathScript : MonoBehaviour
{

    public TextMeshProUGUI deathCounter;

    private GameObject player;

    private int deaths;

    private int wait;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        wait = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player.GetComponent<PlayerHealth>().getDied() && wait == 0)
        {
            deaths += 1;
            wait = 2;
        }
        else if(wait > 0)
        {
            wait--;
        }
        deathCounter.text = "Deaths: " + deaths;
    }
}
