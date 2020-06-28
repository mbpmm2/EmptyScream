using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickupQuest : MonoBehaviour
{
    public bool isOn;
    public int itemsToPickup;
    public GameObject barricade;

    public int currentAmount;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInventory.OnItemPickedUp += CheckQuest;
    }

    private void CheckQuest(ItemCore.ItemType type)
    {
        if(isOn)
        {
            currentAmount++;
            if (currentAmount >= itemsToPickup)
            {
                OpenBarricade();
            }
        }
        
    }

    private void OpenBarricade()
    {
        isOn = false;
        barricade.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerInventory.OnItemPickedUp -= CheckQuest;
    }
}
