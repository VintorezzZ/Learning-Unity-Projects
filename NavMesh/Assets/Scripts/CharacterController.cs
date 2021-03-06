﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
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
        allTargets = GameObject.Find("List_targets").GetComponentsInChildren<Transform>();
        target = Random.Range(0, allTargets.Length);
    }

    // Update is called once per frame
    void Update()
    {
        

        if (agent.remainingDistance <= agent.stoppingDistance )
        {
            animator.SetBool("Walk", false);

            if (allTargets[target].name == "Dance pool")
            {
                                
                animator.SetBool("Dance", true);
                
            }
            else
            {
                
                animator.SetBool("Walk", false);
                
            }
                                  

            NextTarget();
        }
    }



    void NextTarget()
    {
        time += Time.deltaTime;
        int seconds = Random.Range(5, 15);
        if (time > seconds)
        {
            target = Random.Range(0, allTargets.Length);
            time = 0;

            animator.SetBool("Dance", false);
            animator.SetBool("Walk", true);
            agent.destination = allTargets[target].position;
        }
    }


}


