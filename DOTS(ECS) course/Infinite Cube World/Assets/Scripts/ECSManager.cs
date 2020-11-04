using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
    EntityManager manager;
    public GameObject sandPrefab;
    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public GameObject rockPrefab;
    public GameObject snowPrefab;
    const int worldHalfSize = 75;

    [Range(0.1f, 10f)]
    public float strength = 1f;

    [Range(0.01f, 1f)]
    public float scale = 0.1f;

    private void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(sandPrefab, settings);

        for (int z = -worldHalfSize; z <= worldHalfSize; z++)
        {
            for (int x = -worldHalfSize; x <= worldHalfSize; x++)
            {
                var position = new Vector3(x, 0, z);
                Entity instance = manager.Instantiate(prefab);
                manager.SetComponentData(instance, new Translation { Value = position });
            }
        }
    }

    private void Update()
    {
        GameDataManager.strength = strength;
        GameDataManager.scale = scale;
    }
}
