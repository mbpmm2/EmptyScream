using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public delegate void OnTriggerAction(GameObject GO);
    public OnTriggerAction OnEnter;
    public OnTriggerAction OnExit;


    private void OnCollisionEnter(Collision collision)
    {
        if(OnEnter != null)
        {
            OnEnter(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (OnExit != null)
        {
            OnExit(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnEnter != null)
        {
            OnEnter(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (OnExit != null)
        {
            OnExit(other.gameObject);
        }
    }
}
