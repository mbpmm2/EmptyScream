using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : ItemCore
{
    public float immunityTime;
    public Color liquidColor;
    public Color emptyColor;

    private Animator animator;
    public MeshRenderer[] mesh;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInventory.OnSyringePickedUp += SetLiquidFull;
        //Player.OnImmunityStop += StopImmunity;
        //canInject = true;
        animator = GetComponent<Animator>();
        amountText = "" + amountLeft;
        SetLiquidFull(UIInteract.UIPickupType.maxTypes);
    }

    // Update is called once per frame
    void Update()
    {
        if (canUse)
        {
            if (Input.GetButtonDown("Fire1") && amountLeft > 0)
            {
                UseSyringe();
            }
        }

    }

    private void UseSyringe()
    {
        if (!player.isImmune)
        {
            //canInject = false;
            canUse = false;
            amountLeft--;
            animator.SetTrigger("Use");
            amountText = "" + amountLeft;

            if(amountLeft <= 0)
            {
                Invoke("SetLiquidEmpty", 1f);
            }

            if(OnStackableItemUse != null)
            {
                OnStackableItemUse(amountText);
            }

            Debug.Log("using syringe");
        }
        else
        {
            Debug.Log("can NOT use syringe");
        }

    }

    private void StartImmunity()
    {
        player.SetImmunityTimer(immunityTime);
    }

    public void SetLiquidFull(UIInteract.UIPickupType type)
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

    /*private void StopImmunity()
    {
        canInject = true;
    }*/

    private void OnDestroy()
    {
        PlayerInventory.OnSyringePickedUp -= SetLiquidFull;
    }
}
