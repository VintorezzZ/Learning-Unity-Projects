using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    public Transform[] waypoints;
    public float3[] wps;
    public float3[] gunLocations;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        wps = new float3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            wps[i] = waypoints[i].position;
        }
       
    }
}
