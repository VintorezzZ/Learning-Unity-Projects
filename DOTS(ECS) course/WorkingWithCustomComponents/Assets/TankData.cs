using Unity.Entities;

[GenerateAuthoringComponent]
public struct TankData : IComponentData
{
    public float speed;
    public float rotationSpeed;
    public int currentWP;
}
