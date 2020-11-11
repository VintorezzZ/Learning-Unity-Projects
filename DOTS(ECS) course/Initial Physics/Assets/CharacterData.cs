using Unity.Entities;

[GenerateAuthoringComponent]
public struct CharacterData : IComponentData
{
    public float speed;
    public Entity bulletPrefab;
}
