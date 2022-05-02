using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public enum DifficultyOptions{easy, normal, hard};
    public static DifficultyOptions difficulty;
    public static float time;
    public static int deaths;

    public static float musicVolume = 0.18F;
    public static float soundVolume = 1F;

    public static bool hudEnabled;
    public static bool timeEnabled;
    public static bool deathsEnabled;

    public static void setDifficulty(DifficultyOptions o)
    {
        difficulty = o;
    }
    public static DifficultyOptions getDifficulty()
    {
        return difficulty;
    }

    public static void setTime(float t)
    {
        time = t;
    }
    public static float getTime()
    {
        return time;
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
