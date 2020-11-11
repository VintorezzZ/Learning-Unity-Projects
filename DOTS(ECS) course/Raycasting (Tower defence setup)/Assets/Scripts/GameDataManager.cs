using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    public EntityManager manager;
    public Transform[] waypoints;
    public float3[] wps;

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
