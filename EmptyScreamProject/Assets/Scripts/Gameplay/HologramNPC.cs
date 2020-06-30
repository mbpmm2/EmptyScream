using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramNPC : MonoBehaviour
{
    public GameObject hologramNPC;

    // Start is called before the first frame update
    void Start()
    {
        if(hologramNPC)
        {
            hologramNPC.SetActive(false);
        }
        
    }

    public void Activate()
    {
        if (hologramNPC)
        {
            hologramNPC.SetActive(true);
        }
        
    }


    public void Deactivate()
    {
        if (hologramNPC)
        {
            hologramNPC.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Deactivate();
        }
    }
}
