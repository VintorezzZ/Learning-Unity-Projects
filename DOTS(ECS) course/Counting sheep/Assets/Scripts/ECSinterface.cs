using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;


public class ECSinterface : MonoBehaviour
{
    World world;
    private EntityManager entityManager;
    public Text sheepCount;
    public Text tankCount;
    public GameObject tankPrefab;
    public GameObject palmPrefab;
    private void Start()
    {
        world = World.DefaultGameObjectInjectionWorld;
        entityManager = world.GetExistingSystem<MoveSystem>().EntityManager;
        print("All Entities: " + world.GetExistingSystem<MoveSystem>().EntityManager.GetAllEntities().Length);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-20, 20), 0, UnityEngine.Random.Range(-20, 20));
            Instantiate(tankPrefab, pos, quaternion.identity);
        }
        if (Input.GetKey(KeyCode.P))
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-50, 50), 0, UnityEngine.Random.Range(-50, 50));
            var settings = GameObjectConversionSettings.FromWorld(world, null);
            var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(palmPrefab, settings);
            var instance = entityManager.Instantiate(prefab);
            Vector3 position = transform.TransformPoint(new float3(pos.x, 0, pos.z));
            entityManager.SetComponentData(instance, new Translation {Value = position});
            entityManager.SetComponentData(instance, new Rotation {Value = new quaternion(0, 0, 0, 0)});
        }
    }

    public void CountSheep()
    {
        EntityQuery entityQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<SheepData>());
        sheepCount.text = entityQuery.CalculateEntityCount().ToString();
    }
    
    public void CountTanks()
    {
        EntityQuery entityQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<TankData>());
        tankCount.text = entityQuery.CalculateEntityCount().ToString();
    }
}
