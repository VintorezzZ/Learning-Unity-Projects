using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

public class MoveBulletSystem : JobComponentSystem
{

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime; 
        var jobHandle = Entities
               .WithName("MoveBulletSystem")
               .ForEach((ref Translation position, ref Rotation rotation, ref BulletData bulletData) =>
               {
                   position.Value += deltaTime * 100f * math.forward(rotation.Value);
               })
               .Schedule(inputDeps);

        jobHandle.Complete();

        Entities.WithoutBurst().WithStructuralChanges()
            .ForEach((Entity entity, 
            ref Translation translation,
            ref Rotation rotation, 
            ref BulletData bulletData, 
            ref LifeTimeData lifeTimeData) =>
            {
                float distanceToTarget = math.length(GameDataManager.instance.wps[bulletData.waypoint] - translation.Value);
                if (distanceToTarget < 27)
                {
                    lifeTimeData.lifeLeft = 0;

                    if (UnityEngine.Random.Range(0, 1000) <= 50)
                    {
                        var instance = EntityManager.Instantiate(bulletData.explosionPrefab);
                        EntityManager.SetComponentData(instance, new Translation { Value = translation.Value });
                        EntityManager.SetComponentData(instance, new Rotation { Value = rotation.Value });
                        EntityManager.SetComponentData(instance, new LifeTimeData { lifeLeft = 0.5f });
                    }
                   
                }

            }).Run();

        return jobHandle;
    }

}
