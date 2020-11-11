using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ZombieData : IComponentData
{
    public float speed;
    public float rotationSpeed;
    public int currentWP;
}
