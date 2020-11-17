using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRayCast : MonoBehaviour
{
    [SerializeField] private int maxDistance = 10;

    private NavMeshAgent agent;
    [SerializeField] private Transform[] allTargets;
    [SerializeField] private float time;
    [SerializeField] private int target = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {

        CastRay();


        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            NextTarget();
        }
    }



    void NextTarget()
    {
        time += Time.deltaTime;
        int seconds = Random.Range(1, 3);
        if (time > seconds)
        {
            time = 0;

            target++;

            if (target >= allTargets.Length)
                target = 0;   

            agent.destination = allTargets[target].position;
        }
    }


    void CastRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red, 1);

        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            if (hitInfo.transform.tag == "Player")
            {
                Debug.Log("Player detected!");
            }
        }

    }
}

