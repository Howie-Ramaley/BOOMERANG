using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public enum DifficultyOptions{easy, normal, hard};
    public static DifficultyOptions difficulty = DifficultyOptions.normal;
    public static float time = 0;
    public static string timeText = "";
    public static int deaths = 0;

    public static float musicVolume = 0.18f;
    public static float soundVolume = 0.72f;

    public static bool hudEnabled = true;
    public static bool timeEnabled = true;
    public static bool deathsEnabled = true;

    public static void setDifficulty(int o)
    {
        difficulty = (DifficultyOptions)o;
    }
    public static int getDifficulty()
    {
        return (int)difficulty;
    }

    public static void setTime(float t)
    {
        //Debug.Log("time set to " + t);
        time = t;
    }
    public static float getTime()
    {
        return time;
    }
    public static void setTimeText(string t)
    {
        timeText = t;
    }
    public static string getTimeText()
    {
        return timeText;
    }
    public static void setDeaths(int d)
    {
        deaths = d;
    }
    public static int getDeaths()
    {
        return deaths;
    }

    public static void setMusicVolume(float v)
    {
        musicVolume = v;
    }
    public static float getMusicVolume()
    {
        return musicVolume;
    }

    public static void setSoundVolume(float v)
    {
        soundVolume = v;
    }
    public static float getSoundVolume()
    {
        return soundVolume;
    }

    public static void setHudEnabled(bool h)
    {
        hudEnabled = h;
    }
    public static bool getHudEnabled()
    {
        return hudEnabled;
    }

    public static void setTimeEnabled(bool t)
    {
        timeEnabled = t;
    }
    public static bool getTimeEnabled()
    {
        return timeEnabled;
    }

    public static void setDeathsEnabled(bool d)
    {
        deathsEnabled = d;
    }
    public static bool getDeathsEnabled()
    {
        return deathsEnabled;
    }
}
