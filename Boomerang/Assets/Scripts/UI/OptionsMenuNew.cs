using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuNew : MonoBehaviour
{
    [SerializeField]private bool isMainMenu;
    private bool previousSet;
    private TMPro.TMP_Dropdown difficulty;
    private Toggle showHUD;
    private Toggle showTimer;
    private Toggle showDeathCounter;
    private Slider music;
    private Slider sfx;
    private GameObject hud;
    private GameObject time;
    private GameObject deaths;
    private bool updatedOnce;
    // Start is called before the first frame update
    void Start()
    {
        previousSet = false;
        difficulty = transform.Find("Difficulty").GetComponent<TMPro.TMP_Dropdown>();
        showHUD = transform.Find("ShowHUD").GetComponent<Toggle>();
        showTimer = transform.Find("ShowTimer").GetComponent<Toggle>();
        showDeathCounter = transform.Find("ShowDeathCounter").GetComponent<Toggle>();
        music = transform.Find("MusicSlider").GetComponent<Slider>();
        sfx = transform.Find("SoundFXSlider").GetComponent<Slider>();
        if(!isMainMenu)
        {
            hud = transform.parent.transform.parent.Find("BoomerangUI").gameObject;
            time = transform.parent.transform.parent.Find("Timer").gameObject;
            deaths = transform.parent.transform.parent.Find("Death Counter").gameObject;
        }
        updatedOnce = false;
    }

    void Update()
    {
        bool allSet = true;

        if(difficulty == null)
        {
            difficulty = transform.Find("Difficulty").GetComponent<TMPro.TMP_Dropdown>();
            allSet = false;
            Debug.LogError("difficulty");
        }
        else if(previousSet)
            GlobalVars.setDifficulty(difficulty.value);
        
        if(showHUD == null)
        {
            showHUD = transform.Find("ShowHUD").GetComponent<Toggle>();
            allSet = false;
            Debug.LogError("showhud");
        }
        else if(previousSet)
            GlobalVars.setHudEnabled(showHUD.isOn);
        
        if(showTimer == null)
        {
            showTimer = transform.Find("ShowTimer").GetComponent<Toggle>();
            allSet = false;
            Debug.LogError("showtimer");
        }
        else if(previousSet)
            GlobalVars.setTimeEnabled(showTimer.isOn);
        
        if(showDeathCounter == null)
        {
            showDeathCounter = transform.Find("ShowDeathCounter").GetComponent<Toggle>();
            allSet = false;
            Debug.LogError("showdeathcounter");
        }
        else if(previousSet)
            GlobalVars.setDeathsEnabled(showDeathCounter.isOn);
        
        if(music == null)
        {
            music = transform.Find("MusicSlider").GetComponent<Slider>();
            allSet = false;
            Debug.LogError("musicslider");
        }
        else if(previousSet)
            GlobalVars.setMusicVolume(music.value);
        
        if(sfx == null)
        {
            sfx = transform.Find("SoundFXSlider").GetComponent<Slider>();
            allSet = false;
            Debug.LogError("soundfxslider");
        }
        else if(previousSet)
            GlobalVars.setSoundVolume(sfx.value);

        if(!previousSet && allSet)
        {
            difficulty.value = GlobalVars.getDifficulty();
            showHUD.isOn = GlobalVars.getHudEnabled();
            showTimer.isOn = GlobalVars.getTimeEnabled();
            showDeathCounter.isOn = GlobalVars.getDeathsEnabled();
            music.value = GlobalVars.getMusicVolume();
            sfx.value = GlobalVars.getSoundVolume();
            previousSet = true;
        }

        if(!isMainMenu)
        {
            if(hud != null)
            {
                if(GlobalVars.getHudEnabled())
                    hud.SetActive(true);
                else
                    hud.SetActive(false);
            }
            else
                hud = transform.parent.transform.parent.Find("BoomerangUI").gameObject;
                
            if(time != null)
            {
                if(GlobalVars.getTimeEnabled())
                    time.SetActive(true);
                else
                    time.SetActive(false);
            }
            else
                time = transform.parent.transform.parent.Find("Timer").gameObject;
            
            if(deaths != null)
            {
                if(GlobalVars.getDeathsEnabled())
                    deaths.SetActive(true);
                else
                    deaths.SetActive(false);
            }
            else
                deaths = transform.parent.transform.parent.Find("Death Counter").gameObject;
            
            if(!updatedOnce)
            {
                updatedOnce = true;
                gameObject.SetActive(false);
            }
        }
    }
}
