using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public delegate void OnInteractableAction();
    public OnInteractableAction OnInteract;

    public bool setDelayBetweenPress;
    public bool canInteract;
    public float delayBetweenPress;
    private float delayTimer;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(setDelayBetweenPress)
        {
            if (!canInteract)
            {
                delayTimer += Time.deltaTime;

                if (delayTimer >= delayBetweenPress)
                {
                    canInteract = true;
                    delayTimer = 0;
                }
            }
        }

    }

    public void Interact()
    {
        if(OnInteract != null)
        {
            OnInteract();
        }
    }
}
