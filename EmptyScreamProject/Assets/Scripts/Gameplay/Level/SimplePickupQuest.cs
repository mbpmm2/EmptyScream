using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePickupQuest : MonoBehaviour
{
    public bool isOn;
    public int itemsToPickup;
    public GameObject barricade;
    public Light questLight;
    public Color enableColor;
    public Color disableColor;

    public int currentAmount;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInventory.OnItemPickedUp += CheckQuest;

        if (questLight)
        {
            questLight.color = disableColor;
        }
            
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

        if(questLight)
        {
            questLight.color = enableColor;
        }
        
        barricade.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerInventory.OnItemPickedUp -= CheckQuest;
    }
}
