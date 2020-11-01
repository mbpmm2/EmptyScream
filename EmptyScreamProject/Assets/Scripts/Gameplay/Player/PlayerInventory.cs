using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerInventory : MonoBehaviour
{
    public delegate void OnInventoryAction(int index);
    public delegate void OnInventoryAction2(Sprite newIcon, string amount);
    public delegate void OnInventoryAction3(string amount);
    public delegate void OnItemPickupAction(UIInteract.UIPickupType type);
    public delegate void OnItemPickupAction2(ItemCore.ItemType type);
    public OnInventoryAction OnInventoryChange;
    public static OnInventoryAction2 OnNewStackableItem;
    public static OnInventoryAction OnNoNewStackableItem;
    public static OnInventoryAction3 OnAmmoAdded;
    public static OnItemPickupAction OnItemAvailable;
    public static OnItemPickupAction OnItemNull;
    public static OnItemPickupAction2 OnItemPickedUp;
    public static OnItemPickupAction2 OnSyringePickedUp;
    public static OnItemPickupAction2 OnMedkitPickedUp;

    [Header("Inventory Config")]
    public KeyCode[] hotkeyKeys;
    public KeyCode pickupKey;
    public GameObject itemsParent;

    //[Header("Inventory Config")]
    private int newIndex = 0;
    private FirstPersonController playerController;
    private Player player;
    public int lastIndex=0;
    public GameObject[] hotkeyItems;
    public ItemCore[] items;
    public Camera cam;
    public LayerMask rayCastLayer;
    public float rayDistance;
    public bool itemChangeFinished = true;

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
            
            if (hotkeyItems[i].GetComponent<ItemCore>())
            {
                items[i] = hotkeyItems[i].GetComponent<ItemCore>();
            }
            else
            {
                items[i] = hotkeyItems[i].GetComponentInChildren<ItemCore>();
            }

            for (int c = 0; c < hotkeyItems[i].transform.childCount; c++)
            {
                hotkeyItems[i].transform.GetChild(c).gameObject.SetActive(false);
            }

            items[i].DisableCrosshair();
            //hotkeyItems[i].SetActive(false);
        }

        Invoke("test", 0.5f);
        playerController = GameManager.Get().playerGO.GetComponent<FirstPersonController>();
        player = GameManager.Get().playerGO.GetComponent<Player>();
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
                        AkSoundEngine.PostEvent("Grab_item", gameObject);
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
            //Debug.Log("messi");

            if(!playerController.isRunning && !playerController.m_Jumping && !player.isDoingAction)
            {
                if (itemChangeFinished)
                {
                    if (items[lastIndex].canUse && !items[lastIndex].isInAnimation)
                    {

                        items[lastIndex].canUse = false;
                        //items[lastIndex].doOnce = false;
                        items[lastIndex].lastRunState = !items[lastIndex].lastRunState;
                        items[lastIndex].animator.SetTrigger("Hide");
                        items[lastIndex].animator.SetBool("stopMovementAnimation", true);
                        //hotkeyItems[lastIndex].GetComponent<Animator>().SetTrigger("Hide");


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
                            if (OnNewStackableItem != null)
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

        //hotkeyItems[newIndex].GetComponent<Animator>().SetTrigger("Draw");
        items[newIndex].animator.SetTrigger("Draw");
       /* if (items[lastIndex].lerp)
        {
            Debug.Log("TRUE");
            items[lastIndex].lerp.canChange = true;
        }*/
        
        lastIndex = newIndex;
       // la  Debug.Log("Item Changed");
    }

    private void EnableItemChange()
    {
        //Debug.Log("FALSE");

        itemChangeFinished = true;
       // hotkeyItems[lastIndex].GetComponent<ItemCore>().canUse = true;
        items[lastIndex].canUse = true;
        if(items[lastIndex].lerp)
        {
            items[lastIndex].lerp.canChange = false;
            items[lastIndex].lerp.canLerp = true;
            items[lastIndex].lerp.lerpOnce = false;

            /*if (items[lastIndex].itType == ItemCore.ItemType.NailGun)
            {
                items[lastIndex].lerp.canChange = false;
                items[lastIndex].lerp.canLerp = true;
            }*/
        }

        
       /* if (lerp)
        {
            lerp.canChange = false;
            lerp.canLerp = true;
        }*/
    }

    private void DisableItemChange()
    {
        itemChangeFinished = false;
        if(items[lastIndex].lerp)
        {
            items[lastIndex].lerp.canChange = true;
            items[lastIndex].lerp.lerpOnce = true;
            items[lastIndex].lerp.timer = 0;
        }
        
    }

    private void CheckAmmoType(ItemPickup.ItemInfo itemInfo)
    {
        bool hasFoundItem = false;

        for (int i = 0; i < items.Length; i++)
        {
            if(items[i].ammoType == itemInfo.type)
            {
                hasFoundItem = true;

                if (items[i].amountLeft == 0)
                {
                    hotkeyItems[i].transform.GetChild(0).GetComponent<Animator>().Play("Reload", -1, 0f);

                    //items[lastIndex].animator.SetTrigger("Hide");
                }

                items[i].amountLeft += itemInfo.amount;
                items[i].amountText = "" + items[i].amountLeft;

                if (items[i].ammoType == ItemPickup.PickupType.syringe)
                {
                    if (OnSyringePickedUp != null)
                    {
                        OnSyringePickedUp(ItemCore.ItemType.Syringe);
                    }
                }
                else if (items[i].ammoType == ItemPickup.PickupType.band)
                {
                    if (OnMedkitPickedUp != null)
                    {
                        OnMedkitPickedUp(ItemCore.ItemType.Bandages);
                    }
                }

                if (OnAmmoAdded != null)
                {
                    if(i == lastIndex)
                    {
                        if (items[i].ammoType >= ItemPickup.PickupType.syringe)
                        {
                            //items[newIndex].gameObject.GetComponent<RangeWeapon>().isReloading = false;
                            OnAmmoAdded(items[i].amountText);

                        }
                        
                    }
                    
                }

                Debug.Log("ammo added");
            }
        }

        if(hasFoundItem)
        {
            if(OnItemPickedUp != null)
            {
                OnItemPickedUp(ItemCore.ItemType.AllItems);
            }
            Destroy(itemInfo.owner);
        }
    }

    private void test()
    {
        for (int i = 0; i < hotkeyItems[0].transform.childCount; i++)
        {
            hotkeyItems[0].transform.GetChild(i).gameObject.SetActive(true);
        }

        items[0].EnableCrosshair();
        items[0].animator.SetTrigger("Draw");
        AkSoundEngine.PostEvent("select_nail_gun", gameObject);
        lastIndex = 0;
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
