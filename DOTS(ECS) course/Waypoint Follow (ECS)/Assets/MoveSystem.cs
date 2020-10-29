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
        var jobHandle = Entities
            .WithName("MoveSystem")
            .ForEach((ref Translation position, ref Rotation rotation, ref ShipData shipData) =>
            {
                float3 heading = waypointPositions[shipData.currentWP] - position.Value; //wps
                //heading.y = 0;
                quaternion targetDirection = quaternion.LookRotation(heading, math.up());
                rotation.Value = math.slerp(rotation.Value, targetDirection, deltaTime * shipData.rotationSpeed);
                position.Value += deltaTime * shipData.speed * math.forward(rotation.Value);

                if (math.distance(position.Value, waypointPositions[shipData.currentWP]) < 50)
                {
                    shipData.currentWP++;
                    if (shipData.currentWP >= waypointPositions.Length)
                        shipData.currentWP = 0;
                }
            })
            .Schedule(inputDeps);

        waypointPositions.Dispose(jobHandle); // wps
        return jobHandle;
    }

}