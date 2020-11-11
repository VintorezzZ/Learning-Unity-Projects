using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct BoxTriggerData : IComponentData
{
    public float3 triggerEffect;
}
