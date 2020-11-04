
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
    EntityManager manager;
    public GameObject shipPrefab;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    const int numShips = 100;

    // Start is called before the first frame update
    void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shipPrefab, settings);
        var bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);
        var explosion = GameObjectConversionUtility.ConvertGameObjectHierarchy(explosionPrefab, settings);

        List<GameObject> bulletSpawnPoints = new List<GameObject>();
        Transform ship = shipPrefab.transform;
        foreach (Transform gun in ship)
        {
            if (gun.CompareTag("BulletSpawnPoint"))
                bulletSpawnPoints.Add(gun.gameObject);
        }

        GameDataManager.instance.gunLocations = new float3[bulletSpawnPoints.Count];
        for (int i = 0; i < bulletSpawnPoints.Count; i++)
        {
            GameDataManager.instance.gunLocations[i] = bulletSpawnPoints[i].transform
                .TransformPoint(bulletSpawnPoints[i].transform.position);
        }
        
        for (int i = 0; i < numShips; i++)
        {
            var instance = manager.Instantiate(prefab);
            float x = UnityEngine.Random.Range(-300, 300);
            float y = UnityEngine.Random.Range(-300, 300);
            float z = UnityEngine.Random.Range(-300, 300);
            var position = transform.TransformPoint(new float3(x, y, z));
            manager.SetComponentData(instance, new Translation { Value = position });

            var q = Quaternion.Euler(new Vector3(0, 45, 0));
            manager.SetComponentData(instance, new Rotation { Value = new quaternion(q.x,q.y,q.z,q.w) });

            int closestWP = 0;
            float distance = Mathf.Infinity;
            for (int j = 0; j < GameDataManager.instance.wps.Length; j++)
            {
                if (Vector3.Distance(GameDataManager.instance.wps[j], position) < distance)
                {
                    closestWP = j;
                    distance = Vector3.Distance(GameDataManager.instance.wps[j], position);
                }
            }

            manager.SetComponentData(instance, new ShipData { speed = UnityEngine.Random.Range(5, 20),
                rotationSpeed = UnityEngine.Random.Range(3, 5),
                currentWP = closestWP,
                bulletPrefab = bulletEntityPrefab,
                explosionPrefab = explosion
            });
        }

    }
}
