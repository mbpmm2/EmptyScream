using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public EnemyController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if(controller.currentState != EnemyController.States.Dead && controller.currentState != EnemyController.States.Stunned)
            {
                controller.ChangeState(EnemyController.States.Follow);
                controller.agent.isStopped = false;
            }
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (controller.currentState != EnemyController.States.Dead && controller.currentState != EnemyController.States.Stunned)
            {
                controller.ChangeState(EnemyController.States.Idle);
                controller.agent.isStopped = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.name == "Player")
        //{
        //    if (controller.currentState != EnemyController.States.Dead && controller.currentState != EnemyController.States.Stunned)
        //    {
        //        controller.ChangeState(EnemyController.States.Follow);
        //        controller.agent.isStopped = true;
        //    }
        //}
    }
}
