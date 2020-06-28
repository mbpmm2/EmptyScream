using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Door : Interactable
{
    public enum DoorType
    {
        defaultDoor,
        automaticDoor,
        singleUseDoor,
        maxDoors
    }

    public DoorType doorType;

    [Header("Single use Door settings")]
    public GameObject door;
    public GameObject[] pickupsColliders;

    public bool isOpen;
    public bool isLocked;
    private Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if(doorType != DoorType.automaticDoor)
        {
            isOpen = true;
        }
        
        canInteract = true;
        //isLocked = false;
        animator = GetComponent<Animator>();
        OnInteract += InteractDoor;
        if(animator)
        {
            animator.SetBool("Close", false);
            animator.SetBool("Open", false);
        }

        for (int i = 0; i < pickupsColliders.Length; i++)
        {
            pickupsColliders[i].SetActive(false);
        }

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void InteractDoor()
    {
        if(canInteract)
        {
            if(!isLocked)
            {
                if(doorType != DoorType.singleUseDoor)
                {
                    isOpen = !isOpen;

                    if (isOpen)
                    {
                        animator.SetBool("Close", true);
                        animator.SetBool("Open", false);
                        canInteract = false;

                        // Close
                    }
                    else
                    {
                        animator.SetBool("Open", true);
                        animator.SetBool("Close", false);
                        canInteract = false;

                        // Open
                    }
                }
                else
                {
                    if(door.activeSelf)
                    {
                        for (int i = 0; i < pickupsColliders.Length; i++)
                        {
                            pickupsColliders[i].SetActive(true);
                        }
                        door.SetActive(false);
                    }
                    
                }

                
            }
            
        }

    }

    public void EnableInteract()
    {
        canInteract = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(doorType == DoorType.automaticDoor)
        {
            switch (other.tag)
            {
                case "Player":
                    {
                        if(!isOpen)
                        {
                            animator.SetBool("Open", true);
                            animator.SetBool("Close", false);
                            canInteract = false;
                            isOpen = true;
                        }
                        
                    }
                    break;
                case "enemy":
                    {
                        if (!isOpen)
                        {
                            animator.SetBool("Open", true);
                            animator.SetBool("Close", false);
                            canInteract = false;
                            isOpen = true;
                        }
                        
                    }
                    break;
                default:
                    break;
            }
        }
        
    }

    public bool CheckInteract()
    {
        if(!isLocked)
        {
            return true;
        }

        return false;
        //return -1;
    }

    private void OnTriggerExit(Collider other)
    {
        if (doorType == DoorType.automaticDoor)
        {
            switch (other.tag)
            {
                case "Player":
                    {
                        if(isOpen)
                        {
                            animator.SetBool("Close", true);
                            animator.SetBool("Open", false);
                            canInteract = false;
                            isOpen = false;
                        }
                        
                    }
                    break;
                case "enemy":
                    {
                        if (isOpen)
                        {
                            animator.SetBool("Close", true);
                            animator.SetBool("Open", false);
                            canInteract = false;
                            isOpen = false;
                        }
                            
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        OnInteract -= InteractDoor;
    }
}
