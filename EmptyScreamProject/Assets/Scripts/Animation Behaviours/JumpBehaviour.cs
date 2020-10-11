using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ItemCore item = animator.gameObject.transform.parent.GetComponent<ItemCore>();

        item.lastRunState = !item.lastRunState;
        item.isInAnimation = true;
        item.canUse = false;
        item.lerp.canChange = true;
        item.lerp.timer = 0;
        item.lerp.lerpOnce = true;
        item.lerp.canLerp = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ItemCore item = animator.gameObject.transform.parent.GetComponent<ItemCore>();

        if (item.canUse || item.isInAnimation)
        {
            if (item.lastRunState != item.playerController.isRunning)
            {
                if (item.playerController.isRunning)
                {
                    animator.SetBool("stopMovementAnimation", true);
                }
                else
                {
                    animator.SetBool("stopMovementAnimation", false);
                    item.doOnce = true;
                }

                animator.SetBool("isRunning", item.playerController.isRunning);
                item.lastRunState = item.playerController.isRunning;
                item.runTriggerActivated = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ItemCore item = animator.gameObject.transform.parent.GetComponent<ItemCore>();

        item.canUse = true;
        
        item.isInAnimation = false;
        item.lerp.canChange = false;
        item.lerp.lerpOnce = false;
        item.lerp.canLerp = true;
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
