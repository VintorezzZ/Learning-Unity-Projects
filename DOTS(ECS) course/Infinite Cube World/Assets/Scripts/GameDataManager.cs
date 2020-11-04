using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public static class GameDataManager
{
    public static float scale1;
    public static float strength1;

    public static float scale2;
    public static float strength2;

    public static float scale3;
    public static float strength3;

    public static Entity sand;
    public static Entity dirt;
    public static Entity grass;
    public static Entity rock;
    public static Entity snow;

    public static float sandLevel = 2f;
    public static float dirtLevel = 4f;
    public static float grassLevel = 6f;
    public static float rockLevel = 8f;
    public static float snowLevel = 10f;

    public static bool changeData = false;
}
