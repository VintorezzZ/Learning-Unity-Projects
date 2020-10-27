using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class LargeCubeSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = Entities
            .WithName("MoveSystem")
            .ForEach((ref Translation translation, ref Rotation rotation, ref LargeCubeData largeCubeData) =>
            {
                translation.Value -= 0.01f * math.up();
            })
            .Schedule(inputDeps);
        return jobHandle;
    }
    
}
