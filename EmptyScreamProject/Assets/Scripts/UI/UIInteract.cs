using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInteract : MonoBehaviour
{
    public enum UIPickupType
    {
        defaultPickup,
        lockedPickup,
        unlockedPickup,
        maxTypes
    }

    public Sprite defaultIcon;
    public Sprite lockedIcon;
    public Sprite unlockedIcon;
    public Sprite blankIcon;
    public UIPickupType currentType;

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        Player.OnInteractAvailable += ChangeImage;
        Player.OnInteractUnavailable += ChangeImage;
        Player.OnInteractNull += ChangeImage;
        PlayerInventory.OnItemAvailable += ChangeImage;
        PlayerInventory.OnItemNull += ChangeImage;
        image.sprite = blankIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeImage(UIPickupType type)
    {
        if(currentType != type)
        {
            switch (type)
            {
                case UIPickupType.defaultPickup:
                    image.sprite = defaultIcon;
                    break;
                case UIPickupType.lockedPickup:
                    image.sprite = lockedIcon;
                    break;
                case UIPickupType.unlockedPickup:
                    image.sprite = unlockedIcon;
                    break;
                case UIPickupType.maxTypes:
                    image.sprite = blankIcon;
                    break;
                default:
                    break;
            }

            currentType = type;
        }
    }

    private void OnDestroy()
    {
        Player.OnInteractAvailable -= ChangeImage;
        Player.OnInteractUnavailable -= ChangeImage;
        Player.OnInteractNull -= ChangeImage;
        PlayerInventory.OnItemAvailable -= ChangeImage;
        PlayerInventory.OnItemNull -= ChangeImage;
    }
}
