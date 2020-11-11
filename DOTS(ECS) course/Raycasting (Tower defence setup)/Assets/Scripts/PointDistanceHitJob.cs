using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Jobs;
using Unity.Transforms;

public class PointDistanceHitJob : MonoBehaviour
{
    public float distance = 10.0f;
    public bool CollectAllHits = false;
    public bool DrawSurfaceNormals = true;
    NativeList<DistanceHit> DistanceHits;
    PointDistanceInput PointDistanceInput;

    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepWorld;

    Entity closestEntity;
    Vector3 lockedOnto;

    public AudioSource splat;
    public AudioSource fire;
    public ParticleSystem shoot;
    public GameObject blood;
    BlobAssetStore store;

    public struct PointDistanceJob : IJob
    {
        public PointDistanceInput PointDistanceInput;
        public NativeList<DistanceHit> DistanceHits;
        public bool CollectAllHits;
        [ReadOnly] public PhysicsWorld world;

        public void Execute()
        {
            if (CollectAllHits)
            {
                world.CalculateDistance(PointDistanceInput, ref DistanceHits);
            }
            else if (world.CalculateDistance(PointDistanceInput, out DistanceHit hit))
            {
                DistanceHits.Add(hit);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DistanceHits = new NativeList<DistanceHit>(Allocator.Persistent);
        buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();
        stepWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<StepPhysicsWorld>();
        shoot.Stop();
        store = new BlobAssetStore();
    }

    void OnDestroy()
    {
        if (DistanceHits.IsCreated)
        {
            DistanceHits.Dispose();
        }
        store.Dispose();
    }

    void LateUpdate()
    {
        stepWorld.GetOutputDependency().Complete();
        float3 origin = transform.position;

        DistanceHits.Clear();

        PointDistanceInput = new PointDistanceInput
        {
            Position = origin,
            MaxDistance = distance,
            Filter = CollisionFilter.Default
        };

        JobHandle pdjHandle = new PointDistanceJob
        {
            PointDistanceInput = PointDistanceInput,
            DistanceHits = DistanceHits,
            CollectAllHits = CollectAllHits,
            world = buildPhysicsWorld.PhysicsWorld,
        }.Schedule();

        pdjHandle.Complete();

        if (!GameDataManager.instance.manager.Exists(closestEntity))
        {
            float closestDistance = Mathf.Infinity;
            lockedOnto = Vector3.zero;
            foreach (DistanceHit hit in DistanceHits.ToArray())
            {
                Assert.IsTrue(hit.RigidBodyIndex >= 0 && hit.RigidBodyIndex < buildPhysicsWorld.PhysicsWorld.NumBodies);
                Assert.IsTrue(math.abs(math.lengthsq(hit.SurfaceNormal) - 1.0f) < 0.01f);

                var entity = buildPhysicsWorld.PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;
                bool hasComponent = GameDataManager.instance.manager.HasComponent<ZombieData>(entity);
                if (closestDistance > hit.Distance && hasComponent)
                {
                    closestDistance = hit.Distance;
                    closestEntity = entity;
                    lockedOnto = GameDataManager.instance.manager.GetComponentData<Translation>(entity).Value;
                    Invoke("DestroyClosest", 2);
                    fire.Play();
                    shoot.Play();
                }

            }
        }
        else
           lockedOnto = GameDataManager.instance.manager.GetComponentData<Translation>(closestEntity).Value;

        this.transform.LookAt(lockedOnto);

    }

    void DestroyClosest()
    {
        bool hasComponent = GameDataManager.instance.manager.HasComponent<ZombieData>(closestEntity);
        if (hasComponent)
        {
            var position = GameDataManager.instance.manager.GetComponentData<Translation>(closestEntity).Value;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
            var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(blood, settings);

            for (int i = 0; i < 100; i++)
            {
                position += (float3)UnityEngine.Random.insideUnitSphere * 0.1f;
                var splat = GameDataManager.instance.manager.Instantiate(prefab);
                GameDataManager.instance.manager.SetComponentData<Translation>(splat, new Translation { Value = position });
            }

            GameDataManager.instance.manager.DestroyEntity(closestEntity);
            splat.Play();
            fire.Stop();
            shoot.Stop();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (DistanceHits.IsCreated)
        {
            foreach (DistanceHit hit in DistanceHits.ToArray())
            {
                Assert.IsTrue(hit.RigidBodyIndex >= 0 && hit.RigidBodyIndex < buildPhysicsWorld.PhysicsWorld.NumBodies);
                Assert.IsTrue(math.abs(math.lengthsq(hit.SurfaceNormal) - 1.0f) < 0.01f);

                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(this.transform.position, hit.Position - (float3) this.transform.position);
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
