using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;

public class LandscapeSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float strength = GameDataManager.strength;
        float scale = GameDataManager.scale;
        var jobHandle = Entities
            .WithName("LandscapeSystem")
            .ForEach((ref Translation translation, ref BlockData blockData) =>
            {
                var vertex = translation.Value;
                var perlin1 = Mathf.PerlinNoise(vertex.x * scale, vertex.z * scale) * strength;
                var perlin2 = Mathf.PerlinNoise(vertex.x * scale * 0.2f, vertex.z * scale * 0.2f) * strength * 3;
                var perlin3 = Mathf.PerlinNoise(vertex.x * scale * 0.6f, vertex.z * scale * 0.6f) * strength * 5;
                var height = perlin1 + perlin2 + perlin3;

                translation.Value = new Vector3(vertex.x, height, vertex.z);

            })
            .Schedule(inputDeps);

        jobHandle.Complete();

        return inputDeps;
    }
}
