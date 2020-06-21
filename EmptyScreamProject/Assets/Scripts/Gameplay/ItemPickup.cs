using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //public delegate void OnItemPickup(PickupType type, int amount);

    public enum PickupType
    {
        nails, // Ranged Weapons
        syringe, // Items
        band, // Items
        maxTypes
    }

    [System.Serializable]
    public struct ItemInfo
    {
        public PickupType type;
        public int amount;
        public GameObject owner;
    }

    public ItemInfo itemInfo;


    // Start is called before the first frame update
    void Start()
    {
        itemInfo.owner = gameObject;
    }

   /* // Update is called once per frame
    void Update()
    {
        
    }*/

    public ItemInfo PickUpItem()
    {
        return itemInfo;
    }
}
