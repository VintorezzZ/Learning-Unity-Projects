using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Jobs;

public class RaycastHitJob : MonoBehaviour
{
    public float distance = 10.0f;
    public Vector3 direction = new Vector3(0, 0, 1);

    public bool CollectAllHits = false;
    public bool DrawSurfaceNormals = true;

    RaycastInput raycastInput;

    NativeList<Unity.Physics.RaycastHit> RaycastHits;
    NativeList<DistanceHit> DistanceHits;

    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepWorld;

    public struct RaycastJob : IJob
    {
        public RaycastInput rayInput;
        public NativeList<Unity.Physics.RaycastHit> RaycastHits;
        public bool CollectAllHits;
        [ReadOnly] public PhysicsWorld world;

        public void Execute()
        {
            if (CollectAllHits)
            {
                world.CastRay(rayInput, ref RaycastHits);
            }
            else if (world.CastRay(rayInput, out Unity.Physics.RaycastHit hit))
            {
                RaycastHits.Add(hit);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RaycastHits = new NativeList<Unity.Physics.RaycastHit>(Allocator.Persistent);
        DistanceHits = new NativeList<DistanceHit>(Allocator.Persistent);

        buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
        stepWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
    }

    void OnDestroy()
    {
        if (RaycastHits.IsCreated)
        {
            RaycastHits.Dispose();
        }
        if (DistanceHits.IsCreated)
        {
            DistanceHits.Dispose();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        stepWorld.GetOutputDependency().Complete();

        float3 origin = this.transform.position;
        float3 rdirection = (transform.rotation * direction) * distance;

        RaycastHits.Clear();
        DistanceHits.Clear();

        raycastInput = new RaycastInput
        {
            Start = origin,
            End = origin + rdirection,
            Filter = CollisionFilter.Default
        };

        JobHandle rayCastJobHandle = new RaycastJob
        {
            rayInput = raycastInput,
            RaycastHits = RaycastHits,
            CollectAllHits = CollectAllHits,
            world = buildPhysicsWorld.PhysicsWorld,
        }.Schedule();

        rayCastJobHandle.Complete();

        foreach (Unity.Physics.RaycastHit hit in RaycastHits.ToArray())
        {
            var entity = buildPhysicsWorld.PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;
            GameDataManager.instance.manager.DestroyEntity(entity);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(raycastInput.Start, raycastInput.End - raycastInput.Start);

        if (RaycastHits.IsCreated)
        {
            foreach (Unity.Physics.RaycastHit hit in RaycastHits.ToArray())
            {
                Assert.IsTrue(hit.RigidBodyIndex >= 0 && hit.RigidBodyIndex < buildPhysicsWorld.PhysicsWorld.NumBodies);
                Assert.IsTrue(math.abs(math.lengthsq(hit.SurfaceNormal) - 1.0f) < 0.01f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(raycastInput.Start, hit.Position - raycastInput.Start);
                Gizmos.DrawSphere(hit.Position, 0.02f);

                if (DrawSurfaceNormals)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(hit.Position, hit.SurfaceNormal);
                }
            }
        }
    }
}
