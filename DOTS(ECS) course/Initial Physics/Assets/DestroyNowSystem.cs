using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

public class DestroyNowSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.WithoutBurst().WithStructuralChanges()
            .WithName("DestroyNowSystem")
            .ForEach((Entity entity, ref DestroyNowData destroyNowData) =>
            {
                if (destroyNowData.shouldDestroy)
                    EntityManager.DestroyEntity(entity);
            })
            .Run();

        return inputDeps;
    }
}
