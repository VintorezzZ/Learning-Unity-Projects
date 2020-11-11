using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    const int numZombies = 500;
    BlobAssetStore store;

    // Start is called before the first frame update
    void Start()
    {
        store = new BlobAssetStore();
        GameDataManager.instance.manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(zombiePrefab, settings);

        for (int i = 0; i < numZombies; i++)
        {
            var instance = GameDataManager.instance.manager.Instantiate(prefab);
            float x = UnityEngine.Random.Range(-300, 300);
            float y = 0;
            float z = UnityEngine.Random.Range(-300, 300);
            var position = transform.TransformPoint(new float3(x, y, z));
            GameDataManager.instance.manager.SetComponentData(instance, new Translation { Value = position });

            //get closest planet and make that the target
            int closestWP = 0;
            float distance = Mathf.Infinity;
            for(int j = 0; j < GameDataManager.instance.wps.Length; j++)
            {
                if (Vector3.Distance(GameDataManager.instance.wps[j], position) < distance)
                {
                    closestWP = j;
                    distance = Vector3.Distance(GameDataManager.instance.wps[j], position);
                }
            }

            GameDataManager.instance.manager.SetComponentData(instance, new ZombieData { speed = UnityEngine.Random.Range(150, 200),
                rotationSpeed = UnityEngine.Random.Range(1, 2),
                currentWP = closestWP
            });

        }

    }

    void OnDestroy()
    {
        store.Dispose();
    }

}
