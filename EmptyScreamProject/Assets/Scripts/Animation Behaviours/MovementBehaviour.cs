using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ItemCore item = animator.gameObject.transform.parent.GetComponent<ItemCore>();
        Medkit medkit = animator.gameObject.transform.parent.GetComponent<Medkit>();
        Syringe syringe = animator.gameObject.transform.parent.GetComponent<Syringe>();
        //MeleeWeapon melee = animator.gameObject.transform.parent.GetComponent<MeleeWeapon>();

        // item.animationEnded = false;
        if (!item.runTriggerActivated)
        {
            //item.runTriggerActivated = false;
            item.animator.SetBool("stopMovementAnimation", false);
        }
        else
        {
            item.runTriggerActivated = false;
        }
        item.isInAnimation = false;
        item.lerp.canChange = true;
        item.lerp.timer = 0;
        item.lerp.lerpOnce = true;
        item.lerp.canLerp = false;
        item.canUse = true;
        item.isInAnimation = false;
        item.player.isDoingAction = false;

        if (medkit)
        {
            medkit.canHeal = true;
        }

        if (syringe)
        {
            syringe.canInject = true;
        }
            
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ItemCore item = animator.gameObject.transform.parent.GetComponent<ItemCore>();

        if (item.canUse || item.isInAnimation)
        {
            animator.SetFloat("WalkInputX", item.playerController.finalAxisValueX);
            animator.SetFloat("WalkInputY", item.playerController.finalAxisValueY);
            animator.SetBool("isCrouching", item.playerController.m_IsCrouching);


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

            }
        }

        if(item.playerController.finalAxisValueX != 0 || item.playerController.finalAxisValueY != 0)
        {
            // Animate Camera
            if(!item.doOnce)
            {
                //item.isInAnimation = false;
                item.lerp.canChange = true;
                item.lerp.timer = 0;
                item.lerp.lerpOnce = true;
                item.lerp.canLerp = false;
                item.doOnce = true;
            }
        }
        else
        {
            // De-Animate Camera
            //item.isInAnimation = false;
            if(!item.isInAnimation)
            {
                item.lerp.canChange = false;
                item.lerp.lerpOnce = false;
                item.lerp.canLerp = true;
                item.doOnce = false;
            }
            
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ItemCore item = animator.gameObject.transform.parent.GetComponent<ItemCore>();

       // item.canUse = true;

       /* item.isInAnimation = false;
        item.lerp.canChange = false;
        item.lerp.lerpOnce = false;
        item.lerp.canLerp = true;*/
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
