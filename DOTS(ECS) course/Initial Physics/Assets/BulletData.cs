using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct BulletData : IComponentData
{
    public float speed;
    public float3 collisionEffect;
}
