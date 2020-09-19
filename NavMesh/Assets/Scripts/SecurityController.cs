using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform[] allTargets;
    [SerializeField] private int target;
    [SerializeField] private float time;
    [SerializeField] private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        allTargets = GameObject.Find("VisitorsContainer").GetComponentsInChildren<Transform>();
        target = Random.Range(0, allTargets.Length);
    }

    // Update is called once per frame
    void Update()
    {


        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetBool("Check", true);
                     
            NextTarget();
        }
    }



    void NextTarget()
    {
        time += Time.deltaTime;
        int seconds = Random.Range(3, 6);
        if (time > seconds)
        {
            target = Random.Range(0, allTargets.Length);
            time = 0;

            animator.SetBool("Check", false);
            agent.destination = allTargets[target].position;
        }
    }
}
