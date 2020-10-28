using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

public class MoveSystem_tank : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        float speed = 1f;
        float rotationSpeed = 0.5f;
        float3 targetLocation = new float3(0, 0, 0);
        var jobHandle = Entities
               .WithName("MoveSystem_tank")
               .ForEach((ref Translation position, ref Rotation rotation, ref TankData tankData) =>
               {
                   float3 heading = targetLocation - position.Value;
                   heading.y = 0;
                   Quaternion targetDirection = quaternion.LookRotation(heading, math.up());
                   rotation.Value = math.slerp(rotation.Value, targetDirection, deltaTime * rotationSpeed);
                   position.Value +=
                       speed * deltaTime *
                       math.forward(rotation
                           .Value); //(targetLocation - position.Value); //math.forward(rotation.Value);
               })
               .Schedule(inputDeps);

        return jobHandle;
    }

}
