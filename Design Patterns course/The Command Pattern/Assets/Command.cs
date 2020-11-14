using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public abstract void Execute(Animator animator, bool forward);
}

public class MoveFroward : Command
{
    public override void Execute(Animator animator, bool forward)
    {
        if(forward)
            animator.SetTrigger("isWalking");
        else
            animator.SetTrigger("isWalkingR");
    }
}
public class PerformJump : Command
{
    public override void Execute(Animator animator, bool forward)
    {
        if (forward)
            animator.SetTrigger("isJumping");
        else
            animator.SetTrigger("isJumpingR");
    }
}

public class PerformPunch : Command
{
    public override void Execute(Animator animator, bool forward)
    {
        if (forward)
            animator.SetTrigger("isPunching");
        else
            animator.SetTrigger("isPunchingR");
    }
}

public class PerformKick : Command
{
    public override void Execute(Animator animator, bool forward)
    {
        if (forward)
            animator.SetTrigger("isKicking");
        else
            animator.SetTrigger("isKickingR");
    }
}

public class DoNothing : Command
{
    public override void Execute(Animator animator, bool forward)
    {
        
    }
}
