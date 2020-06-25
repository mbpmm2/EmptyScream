using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public delegate void OnInventoryAction(int index);
    public delegate void OnInventoryAction2(Sprite newIcon, string amount);
    public delegate void OnInventoryAction3(string amount);
    public delegate void OnItemPickupAction(UIInteract.UIPickupType type);
    public OnInventoryAction OnInventoryChange;
    public static OnInventoryAction2 OnNewStackableItem;
    public static OnInventoryAction OnNoNewStackableItem;
    public static OnInventoryAction3 OnAmmoAdded;
    public static OnItemPickupAction OnItemAvailable;
    public static OnItemPickupAction OnItemNull;
    public static OnItemPickupAction OnSyringePickedUp;

    [Header("Inventory Config")]
    public KeyCode[] hotkeyKeys;
    public KeyCode pickupKey;
    public GameObject itemsParent;

    //[Header("Inventory Config")]
    private int newIndex = 0;
    public int lastIndex=0;
    public GameObject[] hotkeyItems;
    public ItemCore[] items;
    public Camera cam;
    public LayerMask rayCastLayer;
    public float rayDistance;
    private bool itemChangeFinished = true;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        ItemAnimation.OnHideEnd += DrawItem;
        ItemAnimation.OnDrawEnd += EnableItemChange;
        ItemAnimation.OnShootStart += DisableItemChange;
        ItemAnimation.OnReloadStart += DisableItemChange;
        ItemAnimation.OnShootEnd += EnableItemChange;
        ItemAnimation.OnReloadEnd += EnableItemChange;
        ItemAnimation.OnHitStart += DisableItemChange;
        ItemAnimation.OnHitEnd += EnableItemChange;

        hotkeyItems = new GameObject[itemsParent.transform.childCount];
        items = new ItemCore[itemsParent.transform.childCount];

        for (int i = 0; i < hotkeyItems.Length; i++)
        {
            hotkeyItems[i] = itemsParent.transform.GetChild(i).gameObject;
            items[i] = hotkeyItems[i].GetComponent<ItemCore>();

            for (int c = 0; c < hotkeyItems[i].transform.childCount; c++)
            {
                hotkeyItems[i].transform.GetChild(c).gameObject.SetActive(false);
            }

            items[i].DisableCrosshair();
            //hotkeyItems[i].SetActive(false);
        }

        for (int i = 0; i < hotkeyItems[0].transform.childCount; i++)
        {
            hotkeyItems[0].transform.GetChild(i).gameObject.SetActive(true);
        }

        //hotkeyItems[0].SetActive(true);
        items[0].EnableCrosshair();
        hotkeyItems[0].GetComponent<Animator>().SetTrigger("Draw");
        AkSoundEngine.PostEvent("select_nail_gun", gameObject);
        lastIndex = 0;
        //ActivateItem(0);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hotkeyKeys.Length; i++)
        {
            if(Input.GetKeyDown(hotkeyKeys[i]))
            {
                ActivateItem(i);
            }
        }

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayDistance, rayCastLayer))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * hit.distance, Color.yellow);

            //string layerHitted = LayerMask.LayerToName(hit.transform.gameObject.layer);

            switch (hit.transform.gameObject.tag)
            {
                case "pickup":
                    
                    Debug.DrawRay(cam.transform.position, cam.transform.forward * hit.distance, Color.green);

                    if(OnItemAvailable != null)
                    {
                        OnItemAvailable(UIInteract.UIPickupType.defaultPickup);
                    }

                    if (Input.GetKeyDown(pickupKey))
                    {
                        
                        CheckAmmoType(hit.transform.gameObject.GetComponent<ItemPickup>().itemInfo);
                        //Debug.Log("getting ammo");
                        //PickUpItem(hit.transform.gameObject);
                    }
                    break;
                case "interactable":
                    break;
                default:
                    if(OnItemNull != null)
                    {
                        OnItemNull(UIInteract.UIPickupType.maxTypes);
                    }
                    break;
            }
        }
        else
        {
            /*if (OnItemNull != null)
            {
                OnItemNull(UIInteract.UIPickupType.maxTypes);
            }*/
            Debug.DrawRay(cam.transform.position, cam.transform.forward * rayDistance, Color.white);
        }
    }

    private void ActivateItem(int index)
    {
        if(index != lastIndex)
        {
            if (itemChangeFinished)
            {
                if (items[lastIndex].canUse)
                {
                    items[lastIndex].canUse = false;

                    hotkeyItems[lastIndex].GetComponent<Animator>().SetTrigger("Hide");

                    switch (items[lastIndex].itType)
                    {
                        case ItemCore.ItemType.NailGun:
                            AkSoundEngine.PostEvent("baja_nail_gun", gameObject);
                            break;
                        case ItemCore.ItemType.Wrench:
                            AkSoundEngine.PostEvent("baja_wrench", gameObject);
                            break;
                        case ItemCore.ItemType.Bandages:
                            AkSoundEngine.PostEvent("baja_bandages", gameObject);
                            break;
                        case ItemCore.ItemType.Syringe:
                            AkSoundEngine.PostEvent("baja_jeringa", gameObject);
                            break;
                        case ItemCore.ItemType.AllItems:
                            break;
                    }

                    if (OnInventoryChange != null)
                    {
                        OnInventoryChange(index);
                    }

                    if (items[index].canStack)
                    {
                        if(OnNewStackableItem != null)
                        {
                            OnNewStackableItem(items[index].icon, items[index].amountText);
                        }
                    }
                    else
                    {
                        if (OnNoNewStackableItem != null)
                        {
                            OnNoNewStackableItem(-1);
                        }
                    }

                    newIndex = index;

                    itemChangeFinished = false;
                }

            }
        }
    }

    private void DrawItem()
    {
        for (int i = 0; i < hotkeyItems[lastIndex].transform.childCount; i++)
        {
            hotkeyItems[lastIndex].transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < hotkeyItems[newIndex].transform.childCount; i++)
        {
            hotkeyItems[newIndex].transform.GetChild(i).gameObject.SetActive(true);
        }

        if(items[newIndex].ammoType < ItemPickup.PickupType.syringe)
        {
            items[newIndex].gameObject.GetComponent<RangeWeapon>().isReloading = false;
        }

        items[lastIndex].DisableCrosshair();
        items[newIndex].EnableCrosshair();

        items[lastIndex].canUse = false;
        items[newIndex].canUse = true;


        /* hotkeyItems[lastIndex].SetActive(false);
         hotkeyItems[newIndex].SetActive(true);*/

        switch (items[newIndex].itType)
        {
            case ItemCore.ItemType.NailGun:
                AkSoundEngine.PostEvent("select_nail_gun", gameObject);
                break;
            case ItemCore.ItemType.Wrench:
                AkSoundEngine.PostEvent("select_wrench", gameObject);
                break;
            case ItemCore.ItemType.Bandages:
                AkSoundEngine.PostEvent("select_bandages", gameObject);
                break;
            case ItemCore.ItemType.Syringe:
                AkSoundEngine.PostEvent("select_jeringa", gameObject);
                break;
            case ItemCore.ItemType.AllItems:
                break;
        }

        hotkeyItems[newIndex].GetComponent<Animator>().SetTrigger("Draw");
        lastIndex = newIndex;
       // la  Debug.Log("Item Changed");
    }

    private void EnableItemChange()
    {
        itemChangeFinished = true;
       // hotkeyItems[lastIndex].GetComponent<ItemCore>().canUse = true;
        items[lastIndex].canUse = true;
    }

    private void DisableItemChange()
    {
        itemChangeFinished = false;
    }

    private void CheckAmmoType(ItemPickup.ItemInfo itemInfo)
    {
        bool hasFoundItem = false;

        for (int i = 0; i < items.Length; i++)
        {
            if(items[i].ammoType == itemInfo.type)
            {
                hasFoundItem = true;
                items[i].amountLeft += itemInfo.amount;
                items[i].amountText = "" + items[i].amountLeft;

                if (OnAmmoAdded != null)
                {
                    if(i == lastIndex)
                    {
                        if (items[i].ammoType >= ItemPickup.PickupType.syringe)
                        {
                            //items[newIndex].gameObject.GetComponent<RangeWeapon>().isReloading = false;
                            OnAmmoAdded(items[i].amountText);

                            if(items[i].ammoType == ItemPickup.PickupType.syringe)
                            {
                                if(OnSyringePickedUp != null)
                                {
                                    OnSyringePickedUp(UIInteract.UIPickupType.maxTypes);
                                }
                            }
                        }
                        
                    }
                    
                }

                Debug.Log("ammo added");
            }
        }

        if(hasFoundItem)
        {
            Destroy(itemInfo.owner);
        }
    }

    private void OnDestroy()
    {
        ItemAnimation.OnHideEnd -= DrawItem;
        ItemAnimation.OnDrawEnd -= EnableItemChange;
        ItemAnimation.OnShootStart -= DisableItemChange;
        ItemAnimation.OnReloadStart -= DisableItemChange;
        ItemAnimation.OnShootEnd -= EnableItemChange;
        ItemAnimation.OnReloadEnd -= EnableItemChange;
        ItemAnimation.OnHitStart -= DisableItemChange;
        ItemAnimation.OnHitEnd -= EnableItemChange;
    }
}
