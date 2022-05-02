using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalTimeAndDeaths : MonoBehaviour
{
    private TMPro.TextMeshProUGUI time;
    private TMPro.TextMeshProUGUI deaths;

    // Start is called before the first frame update
    void Start()
    {
        time = transform.Find("Time").GetComponent<TMPro.TextMeshProUGUI>();
        deaths = transform.Find("Deaths").GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(time != null)
        {
            time.text = "Time: " + GlobalVars.getTimeText();
        }
        else
            time = transform.Find("Time").GetComponent<TMPro.TextMeshProUGUI>();
        if(deaths != null)
        {
            deaths.text = "Deaths: " + GlobalVars.getDeaths();
        }
        else
            deaths = transform.Find("Deaths").GetComponent<TMPro.TextMeshProUGUI>();
    }
}
