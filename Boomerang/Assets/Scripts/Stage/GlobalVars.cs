using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public enum DifficultyOptions{easy, normal, hard};
    public static DifficultyOptions difficulty;
    public static float time;
    public static int deaths;

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
}
