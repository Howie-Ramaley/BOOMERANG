using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathScript : MonoBehaviour
{

    public TextMeshProUGUI deathCounter;

    private GameObject player;

    private int deaths;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player.GetComponent<PlayerHealth>().getHealth() <= 0)
        {
            deaths += 1;
        }
        deathCounter.text = "Deaths: " + deaths;
    }
}
