using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public delegate void OnInteractableAction();
    public OnInteractableAction OnInteract;

    public bool canInteract;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void Interact()
    {
        if(OnInteract != null)
        {
            OnInteract();
        }
    }
}
