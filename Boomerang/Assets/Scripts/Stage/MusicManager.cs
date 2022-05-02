using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{

    static AudioSource audioSrc;

    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        changeVolume(GlobalVars.getMusicVolume());
    }

    void OnEnable()
    {
        volumeSlider.onValueChanged.AddListener(delegate
        {
        changeVolume(volumeSlider.value);
        GlobalVars.setMusicVolume(volumeSlider.value);
        });
    }

    void changeVolume(float sliderValue)
    {
        audioSrc.volume = sliderValue;
    }

    void OnDisable()
    {
        volumeSlider.onValueChanged.RemoveAllListeners();
    }
}
