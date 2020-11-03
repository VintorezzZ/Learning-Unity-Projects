using System.Collections;
using System.Collections.Generic;
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
        
        return jobHandle;
    }

}
