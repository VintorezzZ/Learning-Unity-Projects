using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<Transform> spawnPool;
    public List<GameObject> enemiesList;
    public GameObject enemies;
    public Transform center;

    void Start()
    {
        for (int i = 0; i < spawnPool.Count; i++)
        {
            Instantiate(enemiesList[i], spawnPool[i].position, Quaternion.identity);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
