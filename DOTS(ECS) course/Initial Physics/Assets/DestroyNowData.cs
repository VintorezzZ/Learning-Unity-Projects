using Unity.Entities;

[GenerateAuthoringComponent]
public struct DestroyNowData : IComponentData
{
    public bool shouldDestroy;
}
