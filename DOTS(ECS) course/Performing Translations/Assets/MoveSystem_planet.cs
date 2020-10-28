using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

public class MoveSystem_planet : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        float speed = 10.0f;
        float3 targetLocation = new float3(0, 0, 0);
        var jobHandle = Entities
               .WithName("MoveSystem_planet")
               .ForEach((ref Translation position, ref Rotation rotation) =>
               {
                   float3 pivot = targetLocation;
                   float rotSpeed = deltaTime * speed * 1 / math.distance(position.Value, pivot);
                   position.Value = math.mul(quaternion.AxisAngle(new float3(0,1,0), rotSpeed),
                                            position.Value - pivot) + pivot;
               })
               .Schedule(inputDeps);

        return jobHandle;
    }

}
