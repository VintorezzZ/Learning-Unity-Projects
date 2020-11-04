using Unity.Entities;

[GenerateAuthoringComponent]
public struct BulletData : IComponentData
{
    public int waypoint;
    public Entity explosionPrefab;
}
