using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public delegate void OnInventoryAction(int index);
    public delegate void OnInventoryAction2(Sprite newIcon, string amount);
    public OnInventoryAction OnInventoryChange;
    public static OnInventoryAction2 OnNewStackableItem;
    public static OnInventoryAction OnNoNewStackableItem;

    [Header("Inventory Config")]
    public KeyCode[] hotkeyKeys;
    public GameObject itemsParent;

    //[Header("Inventory Config")]
    private int newIndex = 0;
    public int lastIndex=0;
    public GameObject[] hotkeyItems;
    public ItemCore[] items;
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
            hotkeyItems[i].SetActive(false);
        }

        hotkeyItems[0].SetActive(true);
        hotkeyItems[0].GetComponent<Animator>().SetTrigger("Draw");
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
        hotkeyItems[lastIndex].SetActive(false);
        hotkeyItems[newIndex].SetActive(true);
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
