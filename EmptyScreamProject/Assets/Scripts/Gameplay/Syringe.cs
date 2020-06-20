using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : ItemCore
{
    

    public int amountLeft;
    public float immunityTime;

   // public bool canInject;
    private Animator animator;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        //Player.OnImmunityStop += StopImmunity;
        //canInject = true;
        animator = GetComponent<Animator>();
        amountText = "" + amountLeft;
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
            amountLeft--;
            animator.SetTrigger("Use");
            amountText = "" + amountLeft;

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

    /*private void StopImmunity()
    {
        canInject = true;
    }*/

    private void OnDestroy()
    {
        //Player.OnImmunityStop -= StopImmunity;
    }
}
