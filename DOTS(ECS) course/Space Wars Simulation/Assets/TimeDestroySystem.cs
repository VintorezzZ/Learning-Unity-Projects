using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

[UpdateAfter(typeof(MoveBulletSystem))]
public class TimeDestroySystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem buffer;
    protected override void OnCreate()
    {
        buffer = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dt = Time.DeltaTime;
        Entities.WithoutBurst().WithStructuralChanges()
            .ForEach((Entity entity, ref LifeTimeData lifetimeData) =>
            {
                lifetimeData.lifeLeft -= dt;
                if (lifetimeData.lifeLeft <= 0f)
                    EntityManager.DestroyEntity(entity);
            })
            .Run(); 
  
        return inputDeps;
    }
}
