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
            controller.ChangeState(EnemyController.States.Follow);
            controller.agent.isStopped = false;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            controller.ChangeState(EnemyController.States.Idle);
            controller.agent.isStopped = true;
        }
    }
}
