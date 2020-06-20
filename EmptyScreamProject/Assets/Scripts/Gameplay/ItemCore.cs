using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCore : MonoBehaviour
{
    public delegate void OnItemAction(string amount);
    public static OnItemAction OnStackableItemUse;

    public bool canUse;
    public bool canStack;

    [Header("Icons & Text")]
    public Sprite icon;
    public string amountText;
}
