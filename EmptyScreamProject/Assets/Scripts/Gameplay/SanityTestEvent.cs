using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityTestEvent : MonoBehaviour
{
    private SanityEvent sanityEvent;

    // Start is called before the first frame update
    void Start()
    {
        sanityEvent = GetComponent<SanityEvent>();
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            sanityEvent.SetPlayer(other.GetComponent<Player>());
            //sanityEvent.DepleteSanity();
            sanityEvent.canDeplete = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            sanityEvent.SetPlayer(null);
            sanityEvent.canDeplete = false;
        }
    }
}
