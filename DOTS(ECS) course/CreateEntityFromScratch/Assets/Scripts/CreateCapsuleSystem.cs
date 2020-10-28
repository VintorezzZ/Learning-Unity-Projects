using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateCapsuleSystem : JobComponentSystem
{
    protected override void OnCreate()
    {
        base.OnCreate();

        for (int i = 0; i < 100; i++)
        {
            var instance = EntityManager.CreateEntity(
                ComponentType.ReadOnly<LocalToWorld>(),
                ComponentType.ReadOnly<RenderBounds>(),
                ComponentType.ReadWrite<NonUniformScale>(),
                ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadWrite<Rotation>(),
                ComponentType.ReadOnly<RenderMesh>()
            );

            float3 position = new float3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            float scale = Random.Range(1, 30);

            EntityManager.SetComponentData(instance,
                new LocalToWorld
                {
                    Value = new float4x4(rotation: quaternion.identity,
                        translation: position)
                });

            EntityManager.SetComponentData(instance, new RenderBounds {Value = new AABB()});

            EntityManager.SetComponentData(instance,
                new Translation {Value = position});
            EntityManager.SetComponentData(instance,
                new Rotation {Value = new quaternion(0, 0, 0, 0)});
            EntityManager.SetComponentData(instance,
                new NonUniformScale {Value = scale});

            var rHolder = Resources.Load<GameObject>("ResourceHolder").GetComponent<ResourceHolder>();

            EntityManager.SetSharedComponentData(instance,
                new RenderMesh
                {
                    mesh = rHolder.theMesh,
                    material = rHolder.theMaterial
                });
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return inputDeps;
    }
}
