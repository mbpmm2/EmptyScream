using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

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

    //public bool lastWalkingState = false;
    public FirstPersonController playerController;
    public bool lastRunState = false;
    public bool runTriggerActivated = false;
    public bool doOnce;
    public bool isInAnimation;
    public bool canUse;
    public bool canStack;
    public ItemPickup.PickupType ammoType;
    public int amountLeft;
    public ItemCore.ItemType itType;
    public Animator animator;
    public AnimationLerp lerp;

    [Header("Icons & Text")]
    public Image crosshair;
    public Sprite icon;
    public string amountText;

    public Player player;

    public void JumpAnimationStart()
    {
        if (canUse || isInAnimation)
        {
            isInAnimation = true;
            animator.Play("Jump", -1, 0f);
        }
    }

    public void JumpAnimationEnd()
    {
        if (isInAnimation || canUse)
        {
            if (canUse)
            {
                isInAnimation = false;
            }

            animator.Play("JumpEnd", -1, 0f);
        }
    }

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
