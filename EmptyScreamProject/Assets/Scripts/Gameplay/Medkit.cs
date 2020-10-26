using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Medkit : ItemCore
{
    public delegate void OnMedkitAction(ItemType type);
    public static OnMedkitAction OnMedkitEmpty;

    public int healthPointsToGive;

    public bool canHeal;

    // Start is called before the first frame update
    void Start()
    {
        lerp = GetComponent<AnimationLerp>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        playerController = GameManager.Get().playerGO.GetComponent<Player>().fpsController;
        canHeal = true;
        //animator = GetComponent<Animator>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        //animator = transform.GetChild(0).GetComponent<Animator>();
        player = GameManager.Get().playerGO.GetComponent<Player>();
        ItemAnimation.OnHeal += StartHeal;
        amountText = "" + amountLeft;
        FirstPersonController.OnFPSJumpStart += JumpAnimationStart;
        FirstPersonController.OnFPSJumpEnd += JumpAnimationEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if(canUse)
        {
            if (canHeal)
            {
                if (Input.GetButtonDown("Fire1") && amountLeft > 0)
                {
                    UseMedkit();
                }
            }
        }
    }

    private void UseMedkit()
    {
        if(player.CanHeal())
        {
            AkSoundEngine.PostEvent("Bandages_use", gameObject);
            canHeal = false;
            player.isDoingAction = true;
            animator.SetBool("stopMovementAnimation", true);
            animator.SetTrigger("Use");
            Debug.Log("using medkit");
        }
        else
        {
            Debug.Log("can NOT use medkit");
        }
        
    }

    private void StartHeal()
    {
        amountLeft--;
        amountText = "" + amountLeft;

        if (amountLeft <= 0)
        {
            if (OnMedkitEmpty != null)
            {
                OnMedkitEmpty(ItemType.Bandages);
            }
        }

        if (OnStackableItemUse != null)
        {
            OnStackableItemUse(amountText);
        }

        animator.SetBool("stopMovementAnimation", false);
        player.HealPlayer(healthPointsToGive);
        canHeal = true;
        player.isDoingAction = false;
    }

    private void OnDestroy()
    {
        ItemAnimation.OnHeal -= StartHeal;
        FirstPersonController.OnFPSJumpStart -= JumpAnimationStart;
        FirstPersonController.OnFPSJumpEnd -= JumpAnimationEnd;
    }
}
