using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;

public class LandscapeSystem : JobComponentSystem
{
    EntityQuery blockQuery;

    protected override void OnCreate()
    {
        blockQuery = GetEntityQuery(typeof(BlockData));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float strength1 = GameDataManager.strength1;
        float scale1 = GameDataManager.scale1;

        float strength2 = GameDataManager.strength2;
        float scale2 = GameDataManager.scale2;

        float strength3 = GameDataManager.strength3;
        float scale3 = GameDataManager.scale3;

        float3 offset = GameDataManager.playerPosition;

        var jobHandle = Entities
            .WithName("LandscapeSystem")
            .ForEach((ref Translation translation, ref BlockData blockData) =>
            {
                var vertex = blockData.initialPosition + offset;
                var perlin1 = Mathf.PerlinNoise(vertex.x * scale1, vertex.z * scale1) * strength1;
                var perlin2 = Mathf.PerlinNoise(vertex.x * scale2, vertex.z * scale2) * strength2;
                var perlin3 = Mathf.PerlinNoise(vertex.x * scale3, vertex.z * scale3) * strength3;
                var height = perlin1 + perlin2 + perlin3;

                translation.Value = new Vector3(vertex.x, height, vertex.z);

            })
            .Schedule(inputDeps);

        jobHandle.Complete();

        if (GameDataManager.changeData)
        {
            using (var blockEntities = blockQuery.ToEntityArray(Allocator.TempJob))
            {
                foreach (var entity in blockEntities)
                {
                    float height = EntityManager.GetComponentData<Translation>(entity).Value.y;

                    Entity block;
                    if (height <= GameDataManager.sandLevel)
                        block = GameDataManager.sand;
                    else if (height <= GameDataManager.dirtLevel)
                        block = GameDataManager.dirt;
                    else if (height <= GameDataManager.grassLevel)
                        block = GameDataManager.grass;
                    else if (height <= GameDataManager.rockLevel)
                        block = GameDataManager.rock;
                    else
                        block = GameDataManager.snow;

                    RenderMesh colourRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(block);
                    var entityRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(entity);
                    entityRenderMesh.material = colourRenderMesh.material;
                    EntityManager.SetSharedComponentData(entity, entityRenderMesh);
                }
            }
            GameDataManager.changeData = false;
        }
        return inputDeps;
    }
}
