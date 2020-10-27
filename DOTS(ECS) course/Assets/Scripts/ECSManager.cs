using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

public class ECSManager : MonoBehaviour
{
    private EntityManager entityManager;
    public GameObject sheepPrefab;
    private const int numSheep = 15000;
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(sheepPrefab, settings);

        for (int i = 0; i < numSheep; i++)
        {
            var instance = entityManager.Instantiate(prefab);
            var position = transform.TransformPoint(new float3(Random.Range(-50, 50), 0, Random.Range(-50, 50)));
            entityManager.SetComponentData(instance, new Translation {Value = position});
            entityManager.SetComponentData(instance, new Rotation {Value = new quaternion(0, 0, 0, 0)});
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
