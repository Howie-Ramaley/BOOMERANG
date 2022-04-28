using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private LevelTransitioner levelTransitioner;

    void Start()
    {
        levelTransitioner = GameObject.FindGameObjectWithTag("LevelTransitioner").GetComponent<LevelTransitioner>();
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene("GreatGroves");
        if(levelTransitioner == null)
            levelTransitioner = GameObject.FindGameObjectWithTag("LevelTransitioner").GetComponent<LevelTransitioner>();
        if(levelTransitioner != null)
            levelTransitioner.changeLevel("GreatGroves");
        else
            Debug.LogError("Couldn't find level transitioner");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
