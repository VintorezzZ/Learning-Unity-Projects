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
                position.Value.y += 0.1f;
                if (position.Value.y > 50)
                    position.Value.y = 0;
            })
            .Schedule(inputDeps);
        return jobHandle;
    }
}
