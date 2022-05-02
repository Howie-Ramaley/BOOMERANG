using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
    private LevelTransitioner levelTransitioner;
    private bool activated;

    void Start()
    {
        levelTransitioner = GameObject.FindGameObjectWithTag("LevelTransitioner").GetComponent<LevelTransitioner>();
        activated = false;
    }

    public void returnToMenu()
    {
        if(levelTransitioner == null)
            levelTransitioner = GameObject.FindGameObjectWithTag("LevelTransitioner").GetComponent<LevelTransitioner>();
        if(levelTransitioner != null && !activated)
        {
            activated = true;
            GlobalVars.setDeaths(0);
            GlobalVars.setTime(0);
            levelTransitioner.changeLevel("MainMenu");
        }
        else
            Debug.LogError("Couldn't find level transitioner");
    }
}
