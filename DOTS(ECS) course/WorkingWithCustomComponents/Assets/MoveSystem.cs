using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

public class MoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        NativeArray<float3> waypointPositions =
            new NativeArray<float3>(GameDataManager.instance.wps, Allocator.TempJob); //wps
        //float3 targetLocation = GameDataManager.instance.player.position;
        var jobHandle = Entities
               .WithName("MoveSystem")
               .ForEach((ref Translation position, ref Rotation rotation, ref TankData tankData) =>
               {
                   //float3 heading = targetLocation - position.Value;
                   float3 heading = waypointPositions[tankData.currentWP] - position.Value; //wps
                   heading.y = 0;
                   quaternion targetDirection = quaternion.LookRotation(heading, math.up());
                   rotation.Value = math.slerp(rotation.Value, targetDirection, deltaTime * tankData.rotationSpeed);
                   position.Value += deltaTime * tankData.speed * math.forward(rotation.Value);

                   if (math.distance(position.Value, waypointPositions[tankData.currentWP]) < 1)
                   {
                       tankData.currentWP++;
                       if (tankData.currentWP >= waypointPositions.Length)
                           tankData.currentWP = 0;
                   }
               })
               .Schedule(inputDeps);

        waypointPositions.Dispose(jobHandle); // wps
        return jobHandle;
    }

}
