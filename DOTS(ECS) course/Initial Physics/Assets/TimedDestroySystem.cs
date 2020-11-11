using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

public class TimedDestroySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        Entities.WithoutBurst().WithStructuralChanges()
            .WithName("TimedDestroySystem")
            .ForEach((Entity entity, ref LifetimeData lifetimeData) =>
            {
                lifetimeData.lifeLeft -= deltaTime;
                if (lifetimeData.lifeLeft <= 0f)
                    EntityManager.DestroyEntity(entity);
            })
            .Run();

        return inputDeps;
    }
}