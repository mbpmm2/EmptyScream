using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICurrentItem : MonoBehaviour
{
    public Sprite blankIcon;
    public Image icon;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInventory.OnNewStackableItem += ChangeIcon;
        PlayerInventory.OnNoNewStackableItem += DeleteIcon;
        ItemCore.OnStackableItemUse += UpdateText;
        PlayerInventory.OnAmmoAdded += UpdateText;
    }

    public void UpdateText(string newText)
    {
        text.text = "x" + newText;
    }

    public void ChangeIcon(Sprite newIcon, string newText)
    {
        icon.sprite = newIcon;
        text.text = "x" + newText;
    }

    public void DeleteIcon(int index)
    {
        icon.sprite = blankIcon;
        text.text = "";
    }

    private void OnDestroy()
    {
        PlayerInventory.OnNewStackableItem -= ChangeIcon;
        PlayerInventory.OnNoNewStackableItem -= DeleteIcon;
        ItemCore.OnStackableItemUse -= UpdateText;
        PlayerInventory.OnAmmoAdded -= UpdateText;
    }
}
