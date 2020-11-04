using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

public class ParticleSystem : JobComponentSystem
{

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime; 
        var jobHandle = Entities
               .WithName("ParticleSystem")
               .ForEach((ref NonUniformScale scale, ref ParticleData particleData) =>
               {
                   particleData.timeAlive += deltaTime;
                   scale.Value += particleData.timeAlive * 0.8f;
               })
               .Schedule(inputDeps);

        jobHandle.Complete();
               

        return jobHandle;
    }

}
