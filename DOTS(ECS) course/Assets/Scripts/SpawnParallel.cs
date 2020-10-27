using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;

public class SpawnParallel : MonoBehaviour
{
    public GameObject sheepPrefab;
    private Transform[] allSheepTransforms;

    struct MoveJob : IJobParallelForTransform
    {
        public void Execute(int index, TransformAccess transform)
        {
            transform.position += 0.1f * (transform.rotation * new Vector3(0, 0, 1));
            if (transform.position.z > 50)
                transform.position = new Vector3(transform.position.x, 0, -50);
        }
    }

    private MoveJob _moveJob;
    private JobHandle _moveHandle;
    private TransformAccessArray _transforms;
    const int numSheep = 15000;
    void Start()
    {
        allSheepTransforms = new Transform[numSheep];
        for (int i = 0; i < numSheep; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            GameObject sheep = Instantiate(sheepPrefab, pos, Quaternion.identity);
            allSheepTransforms[i] = sheep.transform;
        }
        _transforms = new TransformAccessArray(allSheepTransforms);
    }

    private void Update()
    {
       _moveJob = new MoveJob();
       _moveHandle = _moveJob.Schedule(_transforms);
    }

    private void LateUpdate()
    {
        _moveHandle.Complete();
    }

    private void OnDestroy()
    {
        _transforms.Dispose();
    }
}
