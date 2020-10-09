using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<Transform> spawnPool;
    public List<GameObject> enemiesList;
    public List<GameObject> deadList;
    public GameObject enemies;
    private float timer;

    void Start()
    {
        _SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 20f) 
        {
            timer = 0;
            _SpawnEnemies();
        }
    }

    void _SpawnEnemies()
    {
        for (int i = 0; i < spawnPool.Count; i++)
        {
            Instantiate(enemiesList[i], spawnPool[i].position, Quaternion.identity);

        }
    }


}
