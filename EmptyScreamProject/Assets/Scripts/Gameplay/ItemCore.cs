using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCore : MonoBehaviour
{
    public enum ItemType
    {
        NailGun,
        Wrench,
        Bandages,
        Syringe,
        AllItems
    }

    public delegate void OnItemAction(string amount);
    public static OnItemAction OnStackableItemUse;

    public bool canUse;
    public bool canStack;
    public ItemPickup.PickupType ammoType;
    public int amountLeft;
    public ItemCore.ItemType itType;
    public Animator animator;

    [Header("Icons & Text")]
    public Image crosshair;
    public Sprite icon;
    public string amountText;

    public void EnableCrosshair()
    {
        if(crosshair)
        {
            crosshair.gameObject.SetActive(true);
        }
    }

    public void DisableCrosshair()
    {
        if (crosshair)
        {
            crosshair.gameObject.SetActive(false);
        }
    }
}
