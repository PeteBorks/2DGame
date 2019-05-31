using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBehaviour : StateMachineBehaviour
{
    Zorg z;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        z = animator.GetComponent<Zorg>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (z.vectorDistance.magnitude > 15f)
        {
            z.StopWalk();
            animator.SetTrigger("dash");
        }
        else if (z.vectorDistance.magnitude > 3f && Random.Range(0,100) < 2)
        {
            z.StopWalk();
            z.RandomBehaviour();
        }
        else if(z.vectorDistance.magnitude < 1.5f)
        {
            z.StopWalk();
            animator.SetTrigger("punch");
            
        }
        else
            z.Walk();
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

}
