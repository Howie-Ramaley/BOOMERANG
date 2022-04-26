using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoomerangUI : MonoBehaviour
{
    private PlayerHealth health;
    private Boomerang boomerang;
    private Image pip1;
    private Image pip2;
    private Image pip3;
    private Color healthColor;
    private Color boomerangColor;

    // Start is called before the first frame update
    void Start()
    {
        pip1 = transform.Find("HealthPip1").gameObject.GetComponent<Image>();
        pip2 = transform.Find("HealthPip2").gameObject.GetComponent<Image>();
        pip3 = transform.Find("HealthPip3").gameObject.GetComponent<Image>();
        healthColor = pip1.color;
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        boomerang = GameObject.FindGameObjectWithTag("Boomerang").GetComponent<Boomerang>();
        boomerangColor = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health != null)
        {
            int hp = health.getHealth();
            pip1.color = healthColor;
            pip2.color = healthColor;
            pip3.color = healthColor;
            if(hp < 3)
                pip1.color = Color.black;
            if(hp < 2)
                pip2.color = Color.black;
            if(hp < 1)
                pip3.color = Color.black;
        }
        else
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        if(boomerang != null)
        {
            Color col = Color.black;
            if(boomerang.getReadyToThrow())
                col = boomerangColor;
            GetComponent<Image>().color = col;
        }
        else
            boomerang = GameObject.FindGameObjectWithTag("Boomerang").GetComponent<Boomerang>();
    }
}
