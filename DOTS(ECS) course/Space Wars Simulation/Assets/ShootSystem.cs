using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

public class ShootSystem : JobComponentSystem
{

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        NativeArray<float3> gunPositions =
            new NativeArray<float3>(GameDataManager.instance.gunLocations, Allocator.TempJob);
        Entities.WithoutBurst().WithStructuralChanges()
            .ForEach((Entity entity, ref Translation translation,
                ref Rotation rotation,
                ref ShipData shipData) =>
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
                }
                
            })
            .Run();

        return inputDeps;
    }

}
