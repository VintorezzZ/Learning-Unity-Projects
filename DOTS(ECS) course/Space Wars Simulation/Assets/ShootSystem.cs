using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ShootSystem : JobComponentSystem
{

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        NativeArray<float3> gunPositions = new NativeArray<float3>(GameDataManager.instance.gunLocations, Allocator.TempJob);
        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity, ref Translation translation, ref Rotation rotation, ref ShipData shipData) =>
            {
                float3 directionToTarget = GameDataManager.instance.wps[shipData.currentWP] - translation.Value;
                float angleToTarget = math.acos(
                    math.dot(math.forward(rotation.Value), directionToTarget) /
                    (math.length(math.forward(rotation.Value)) *
                     math.length(directionToTarget)));

                if (angleToTarget < math.radians(10) && math.length(directionToTarget) < 100)
                {
                    foreach (float3 gunPos in gunPositions)
                    {
                        var instance = EntityManager.Instantiate(shipData.bulletPrefab);
                        EntityManager.SetComponentData(instance, new Translation
                        {
                            Value = translation.Value + math.mul(rotation.Value, gunPos)
                        });
                        EntityManager.SetComponentData(instance, new Rotation
                        {
                            Value = rotation.Value
                        });
                        EntityManager.SetComponentData(instance, new LifeTimeData
                        {
                            lifeLeft = 1
                        });
                        EntityManager.SetComponentData(instance, new BulletData
                        {
                            waypoint = shipData.currentWP,
                            explosionPrefab = shipData.explosionPrefab
                        });
                    }
                }
            })
            .Run();

        return inputDeps;
    }

}
