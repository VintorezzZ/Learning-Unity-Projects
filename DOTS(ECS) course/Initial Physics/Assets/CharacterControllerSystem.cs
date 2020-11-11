using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

public class CharacterControllerSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        float inputY = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        float shooting = Input.GetAxis("Fire1");

        var jobHandle = Entities
            .WithName("CharacterControllerSystem")
            .ForEach((ref PhysicsVelocity physics, ref PhysicsMass mass, ref Translation position, ref Rotation rotation, ref CharacterData player) =>
            {
                if (inputZ == 0)
                    physics.Linear = float3.zero;
                else
                    physics.Linear += inputZ * deltaTime * player.speed * math.forward(rotation.Value);

                mass.InverseInertia[0] = 0;
                mass.InverseInertia[2] = 0;

                //physics.Angular += new float3(0, inputY * 0.1f, 0);
                rotation.Value = math.mul(math.normalize(rotation.Value), 
                            quaternion.AxisAngle(math.up(), deltaTime * inputY));
                    
            })
            .Schedule(inputDeps);

        jobHandle.Complete();


        Entities.WithoutBurst().WithStructuralChanges()
            .WithName("ShootControllerSystem")
            .ForEach((ref PhysicsVelocity physics, ref Translation position,
                        ref Rotation rotation, ref CharacterData player) =>
            {
                if (shooting > 0)
                {
                    var instance = EntityManager.Instantiate(player.bulletPrefab);
                    float3 offset = new float3(UnityEngine.Random.Range(-1, 2), 1, 1);
                    EntityManager.SetComponentData(instance, new Translation { 
                            Value = position.Value + math.mul(rotation.Value, offset) });
                    EntityManager.SetComponentData(instance, new Rotation { Value = rotation.Value });

                    float rspeed = UnityEngine.Random.Range(20, 150);
                    float3 reffect = new float3(UnityEngine.Random.Range(-500, 500),
                        UnityEngine.Random.Range(-500, 500),
                        UnityEngine.Random.Range(-500, 500));

                    EntityManager.SetComponentData(instance, new BulletData { speed = rspeed, collisionEffect = reffect });
                }
            })
            .Run();

        return jobHandle;
    }
}
