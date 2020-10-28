using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager_tank : MonoBehaviour
{
    EntityManager manager;
    public GameObject tankPrefab;
    const int numTanks = 500;

    // Start is called before the first frame update
    void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(tankPrefab, settings);

        for (int i = 0; i < numTanks; i++)
        {
            var instance = manager.Instantiate(prefab);
            float x = UnityEngine.Random.Range(-100, 100);
            float z = UnityEngine.Random.Range(-100, 100);
            var position = transform.TransformPoint(new float3(x, 0, z));
            manager.SetComponentData(instance, new Translation { Value = position });

            var q = Quaternion.Euler(new Vector3(0, 45, 0));
            manager.SetComponentData(instance, new Rotation {Value = new quaternion(q.x, q.y, q.z, q.w)});
        }

    }
}
