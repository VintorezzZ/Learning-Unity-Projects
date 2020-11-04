using Unity.Entities;

[GenerateAuthoringComponent]
public struct ShipData : IComponentData
{
    public float speed;
    public float rotationSpeed;
    public int currentWP;
    public bool approach;
    public Entity bulletPrefab;
    public Entity explosionPrefab;
}
