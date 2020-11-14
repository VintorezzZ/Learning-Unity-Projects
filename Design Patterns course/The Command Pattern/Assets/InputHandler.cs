using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GameObject actor;
    Animator anim;
    Command keyQ, keyW, keyE, keyP, keyK;
    List<Command> oldCommands = new List<Command>();

    Coroutine replayCoroutine;
    bool shouldStartReplay; 
    bool isReplaying;
    void Start()
    {
        keyQ = new PerformJump();
        keyP = new PerformPunch();
        keyK = new PerformKick();
        keyW = new MoveFroward(); 
        keyE = new DoNothing();
        anim = actor.GetComponent<Animator>();
        Camera.main.GetComponent<CameraFollow360>().player = actor.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReplaying)
            HandleInput();
        StartReplay();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            keyQ.Execute(anim, true);
            oldCommands.Add(keyQ);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            keyW.Execute(anim, true);
            oldCommands.Add(keyW);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            keyE.Execute(anim, true);
            oldCommands.Add(keyE);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            keyP.Execute(anim, true);
            oldCommands.Add(keyP);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            keyK.Execute(anim, true);
            oldCommands.Add(keyK);
        }

        if (Input.GetKeyDown(KeyCode.Space))        
            shouldStartReplay = true;

        if (Input.GetKeyDown(KeyCode.Z))
            UndoLastCommand();
    }

    private void StartReplay()
    {
        if (shouldStartReplay && oldCommands.Count > 0)
        {
            shouldStartReplay = false;
            if (replayCoroutine != null)
            {
                StopCoroutine(replayCoroutine);
            }
            replayCoroutine = StartCoroutine(ReplayCommands());
        }
    }


    void UndoLastCommand()
    {
        if (oldCommands.Count > 0)
        {
            Command c = oldCommands[oldCommands.Count - 1];
            c.Execute(anim, false);
            oldCommands.RemoveAt(oldCommands.Count - 1);
        } 
        
    }

    IEnumerator ReplayCommands()
    {
        isReplaying = true;

        for (int i = 0; i < oldCommands.Count; i++)
        {
            oldCommands[i].Execute(anim, true);
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
    }
}
