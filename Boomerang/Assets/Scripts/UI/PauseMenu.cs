using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;
    public static bool GameIsPaused = false;
    
    public GameObject PauseMenuUI;

    private LevelTransitioner levelTransitioner;
    private bool goingToMenu;

    void Start()
    {
        levelTransitioner = GameObject.FindGameObjectWithTag("LevelTransitioner").GetComponent<LevelTransitioner>();
        goingToMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(levelTransitioner == null)
            levelTransitioner = GameObject.FindGameObjectWithTag("LevelTransitioner").GetComponent<LevelTransitioner>();

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Pause"))
        {
            if(GameIsPaused)
            {
                Resume();
            } else 
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        if(levelTransitioner == null)
            levelTransitioner = GameObject.FindGameObjectWithTag("LevelTransitioner").GetComponent<LevelTransitioner>();
        if(levelTransitioner != null && !goingToMenu)
        {
            goingToMenu = true;
            Time.timeScale = 1f;
            levelTransitioner.changeLevel("MainMenu");
        }
        else
            Debug.LogError("Couldn't find level transitioner");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
