using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public delegate void OnInventoryAction(int index);
    public OnInventoryAction OnInventoryChange;

    [Header("Inventory Config")]
    public KeyCode[] hotkeyKeys;
    public GameObject itemsParent;

    //[Header("Inventory Config")]
    private int newIndex = 0;
    public int lastIndex=0;
    private GameObject[] hotkeyItems;
    private bool itemChangeFinished = true;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        ItemAnimation.OnHideEnd += DrawItem;
        ItemAnimation.OnDrawEnd += EnableItemChange;

        hotkeyItems = new GameObject[itemsParent.transform.childCount];

        for (int i = 0; i < hotkeyItems.Length; i++)
        {
            hotkeyItems[i] = itemsParent.transform.GetChild(i).gameObject;
            hotkeyItems[i].SetActive(false);
        }

        hotkeyItems[0].SetActive(true);
        hotkeyItems[0].GetComponent<Animator>().SetTrigger("Draw");
        lastIndex = 0;
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
            if(itemChangeFinished)
            {
                hotkeyItems[lastIndex].GetComponent<Animator>().SetTrigger("Hide");

                if (OnInventoryChange != null)
                {
                    OnInventoryChange(index);
                }

                newIndex = index;

                itemChangeFinished = false;
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
    }

    private void OnDestroy()
    {
        ItemAnimation.OnHideEnd -= DrawItem;
        ItemAnimation.OnDrawEnd -= EnableItemChange;
    }
}
