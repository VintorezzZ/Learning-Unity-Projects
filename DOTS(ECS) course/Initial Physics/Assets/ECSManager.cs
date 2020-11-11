using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
    EntityManager manager;
    public GameObject playerPrefab;
    public GameObject bulletPrefab;
    BlobAssetStore store;

    // Start is called before the first frame update
    void Start()
    {
        store = new BlobAssetStore();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        var character = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, settings);
        var bullet = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);

        var playerCharacter = manager.Instantiate(character);
        manager.SetComponentData(playerCharacter, new Translation { Value = new float3(0, 2.2f, 0) });
        manager.SetComponentData(playerCharacter, new CharacterData { speed = 5, bulletPrefab = bullet });

    }

    void OnDestroy()
    {
        store.Dispose();
    }


}
