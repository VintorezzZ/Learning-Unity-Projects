using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public static SpawnEnemies instance;

    public List<Transform> spawnPool;
    public List<GameObject> enemiesList;
    public List<GameObject> deadList;
    public GameObject enemies;
    private float timer;
    public int deadCount;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        deadCount = 0;
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
