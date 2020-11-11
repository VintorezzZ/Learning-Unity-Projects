using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class BulletCollisionEventSystem : JobComponentSystem
{
    BuildPhysicsWorld physWorld;
    StepPhysicsWorld stepWorld;

    protected override void OnCreate()
    {
        physWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    struct CollisionEventJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<BulletData> BulletGroup;
        public ComponentDataFromEntity<PhysicsVelocity> PhysicsVelocityGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.Entities.EntityA;
            Entity entityB = collisionEvent.Entities.EntityB;

            bool isTargetA = PhysicsVelocityGroup.Exists(entityA);
            bool isTargetB = PhysicsVelocityGroup.Exists(entityB);

            bool isBulletA = BulletGroup.Exists(entityA);
            bool isBulletB = BulletGroup.Exists(entityB);

            if (isBulletA && isTargetB)
            {
                var velocityComponent = PhysicsVelocityGroup[entityB];
                velocityComponent.Linear = new float3(0, 1000, 0);
                PhysicsVelocityGroup[entityB] = velocityComponent;
            }

            if (isBulletB && isTargetA)
            {
                var velocityComponent = PhysicsVelocityGroup[entityA];
                velocityComponent.Linear = new float3(0, 1000, 0);
                PhysicsVelocityGroup[entityA] = velocityComponent;
            }

        }
    
    
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle jobHandle = new CollisionEventJob
        {
            BulletGroup = GetComponentDataFromEntity<BulletData>(),
            PhysicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>()
        }.Schedule(stepWorld.Simulation, ref physWorld.PhysicsWorld, inputDeps);

        jobHandle.Complete();
        return jobHandle;
    }
}
