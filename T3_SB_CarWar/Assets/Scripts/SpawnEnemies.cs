using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public static SpawnEnemies instance;

    public List<Transform> spawnPool;
    public List<GameObject> enemiesList;
    //public List<GameObject> deadList;
    public GameObject enemies;
    private float timer = 20;


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
        _SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) 
        {
            timer = 20;
            _SpawnEnemies();
        }

        GameManager.instance.UpdateTimer(timer);
    }

    void _SpawnEnemies()
    {
        for (int i = 0; i < spawnPool.Count; i++)
        {
            Instantiate(enemiesList[i], spawnPool[i].position, Quaternion.identity);

        }
    }


}
