using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;

public class MoveSystem : JobComponentSystem
{

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        int nextWP = UnityEngine.Random.Range(0, GameDataManager.instance.wps.Length);

        NativeArray<float3> waypointPositions = new NativeArray<float3>(GameDataManager.instance.wps, Allocator.TempJob);
        var jobHandle = Entities
               .WithName("MoveSystem")
               .ForEach((ref PhysicsVelocity physics, ref PhysicsMass mass, ref Translation position, 
                         ref Rotation rotation, ref ZombieData zombieData) =>
               {
                   float distance = math.distance(position.Value, waypointPositions[zombieData.currentWP]);
                   if (distance < 5)
                   {
                       zombieData.currentWP = nextWP;
                       //if (zombieData.currentWP >= waypointPositions.Length)
                       //   zombieData.currentWP = 0;
                   }

                   float3 heading;
                   heading = waypointPositions[zombieData.currentWP] - position.Value;

                   quaternion targetDirection = quaternion.LookRotation(heading, math.up());
                   rotation.Value = math.slerp(rotation.Value, targetDirection, deltaTime * zombieData.rotationSpeed);
                   physics.Linear = deltaTime * zombieData.speed * math.forward(rotation.Value);

                   mass.InverseInertia[0] = 0;
                  //mass.InverseInertia[1] = 0;
                   mass.InverseInertia[2] = 0;

               })
               .Schedule(inputDeps);

        jobHandle.Complete();
        waypointPositions.Dispose(jobHandle);

        return jobHandle;
    }

}
