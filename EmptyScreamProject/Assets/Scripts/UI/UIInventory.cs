using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public delegate void OnUIAction();
    public static OnUIAction OnUIActive;

    public GameObject slotsParent;
    public PlayerInventory inventory;

    public Color activeSlotColor;
    public Color normalSlotColor;
    public Color activeButtonBackgroundColor;
    public Color normalButtonBackgroundColor;
    public Color activeButtonTextColor;
    public Color normalButtonTextColor;

    public UIItemSlot[] slots;
    private int lastIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        Syringe.OnSyringeEmpty += DeactivateIcon;
        Medkit.OnMedkitEmpty += DeactivateIcon;
        PlayerInventory.OnSyringePickedUp += ActivateIcon;
        PlayerInventory.OnMedkitPickedUp += ActivateIcon;
        inventory.OnInventoryChange += ActivateSlot;

        slots = new UIItemSlot[slotsParent.transform.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotsParent.transform.GetChild(i).gameObject.GetComponent<UIItemSlot>();
        }

        GetHotkeyKeys();
        ActivateSlot(0);
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    public void ActivateSlot(int index)
    {
        slots[lastIndex].background.color = normalSlotColor;
        slots[lastIndex].keyButton.image.color = normalButtonBackgroundColor;
        slots[lastIndex].keyButtonText.color = normalButtonTextColor;

        slots[index].background.color = activeSlotColor;
        slots[index].keyButton.image.color = activeButtonBackgroundColor;
        slots[index].keyButtonText.color = activeButtonTextColor;

        lastIndex = index;

        if(OnUIActive != null)
        {
            OnUIActive();
        }
        //slots[index].
    }

    public void GetHotkeyKeys()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            string text = inventory.hotkeyKeys[i].ToString();

            if (text[4].ToString() == "a")
            {
                slots[i].keyButtonText.text = text[5].ToString();
            }
            else
            {
                slots[i].keyButtonText.text = inventory.hotkeyKeys[i].ToString();
            }
            
        }
    }

    private void ActivateIcon(ItemCore.ItemType type)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == (int)type)
            {
                Debug.Log("modo messi");
                Color newColor = new Vector4(slots[i].icon.color.r, slots[i].icon.color.g, slots[i].icon.color.b, 1);
                slots[i].icon.color = newColor;
                i = slots.Length;
            }
        }
    }

    private void DeactivateIcon(ItemCore.ItemType type)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(i == (int)type)
            {
                Color newColor = new Vector4(slots[i].icon.color.r, slots[i].icon.color.g, slots[i].icon.color.b, 0);
                slots[i].icon.color = newColor;
                i = slots.Length;
            }
        }
    }

    private void OnDestroy()
    {
        Syringe.OnSyringeEmpty -= DeactivateIcon;
        Medkit.OnMedkitEmpty -= DeactivateIcon;
        PlayerInventory.OnSyringePickedUp -= ActivateIcon;
        PlayerInventory.OnMedkitPickedUp -= ActivateIcon;
        inventory.OnInventoryChange -= ActivateSlot;
    }
}
