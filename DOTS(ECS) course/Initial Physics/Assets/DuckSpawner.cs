using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class DuckSpawner : MonoBehaviour
{
    EntityManager manager;
    public GameObject duckPrefab;
    const int numDucks = 5000;
    BlobAssetStore store;

    // Start is called before the first frame update
    void Start()
    {
        store = new BlobAssetStore();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(duckPrefab, settings);

        for (int i = 0; i < numDucks; i++)
        {
            var instance = manager.Instantiate(prefab);
            float x = UnityEngine.Random.Range(-200, 200);
            float y = UnityEngine.Random.Range(50, 200);
            float z = UnityEngine.Random.Range(-200, 200);
            manager.SetComponentData(instance, new Translation { Value = new float3(x,y,z) });
        }

    }

    void OnDestroy()
    {
        store.Dispose();
    }

}
