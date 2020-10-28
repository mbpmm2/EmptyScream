using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Syringe : ItemCore
{
    public delegate void OnSyringeAction(ItemType type);
    public static OnSyringeAction OnSyringeEmpty;
    public static OnSyringeAction OnSyringeUse;

    [Header("Syringe Config"),Space]
    public bool canInject = true;
    public float defenseTime;
    [Range(0, 100)]
    public float shieldPercentage;
    public Color liquidColor;
    public Color emptyColor;
    
    public MeshRenderer[] mesh;

    // Start is called before the first frame update
    void Start()
    {
        lerp = GetComponent<AnimationLerp>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerController = GameManager.Get().playerGO.GetComponent<Player>().fpsController;
        PlayerInventory.OnSyringePickedUp += SetLiquidFull;
        //animator = GetComponent<Animator>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        //animator = transform.GetChild(0).GetComponent<Animator>();
        player = GameManager.Get().playerGO.GetComponent<Player>();
        ItemAnimation.OnImmune += StartImmunity;
        amountText = "" + amountLeft;
        SetLiquidFull(ItemType.AllItems);
        FirstPersonController.OnFPSJumpStart += JumpAnimationStart;
        FirstPersonController.OnFPSJumpEnd += JumpAnimationEnd;
        canInject = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canUse)
        {
            if(canInject)
            {
                if (Input.GetButtonDown("Fire1") && amountLeft > 0)
                {
                    UseSyringe();
                }
            }
            
        }

    }

    private void UseSyringe()
    {
        if (!player.isDefenseActive)
        {
            AkSoundEngine.PostEvent("Needle_use", gameObject);
            canInject = false;
            player.isDoingAction = true;

            animator.SetBool("stopMovementAnimation", true);
            animator.SetTrigger("Use");
            Debug.Log("using syringe");
        }
        else
        {
            Debug.Log("can NOT use syringe");
        }

    }

    public void StartImmunity()
    {
        amountLeft--;
        amountText = "" + amountLeft;

        if (amountLeft <= 0)
        {
            Invoke("SetLiquidEmpty", 1f);

            if (OnSyringeEmpty != null)
            {
                OnSyringeEmpty(ItemType.Syringe);
            }
        }

        if (OnStackableItemUse != null)
        {
            OnStackableItemUse(amountText);
        }

        if(OnSyringeUse != null)
        {
            OnSyringeUse(ItemType.Syringe);
        }

        animator.SetBool("stopMovementAnimation", false);
        player.SetDefenseTimer(defenseTime, shieldPercentage);
        canInject = true;
        player.isDoingAction = false;
    }

    public void SetLiquidFull(ItemType type)
    {
        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].material.SetColor("_BaseColor", liquidColor);
        }       
    }

    public void SetLiquidEmpty()
    {
        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].material.SetColor("_BaseColor", emptyColor);
        }
    }

    private void OnDestroy()
    {
        PlayerInventory.OnSyringePickedUp -= SetLiquidFull;
        ItemAnimation.OnImmune -= StartImmunity;
        FirstPersonController.OnFPSJumpStart -= JumpAnimationStart;
        FirstPersonController.OnFPSJumpEnd -= JumpAnimationEnd;
    }
}
