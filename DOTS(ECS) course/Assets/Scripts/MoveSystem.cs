using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Rendering;

public class MoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = Entities.WithName("MoveSystem").ForEach((ref Translation position, ref Rotation rotation) =>
            {
                position.Value += 0.1f * math.forward(rotation.Value);
                if (position.Value.z > 50)
                    position.Value.z = -50;
            })
            .Schedule(inputDeps);
        return jobHandle;
    }
}
