using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnemyDetection : MonoBehaviour
{
    public Door scriptPuerta;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            scriptPuerta.isInCombatRoom = true;
        }
    }
}
