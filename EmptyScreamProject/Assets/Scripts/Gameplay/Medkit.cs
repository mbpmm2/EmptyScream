using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : ItemCore
{
    //public int amountLeft;
    public int healthPointsToGive;

    public bool canHeal;
    private Animator animator;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        canHeal = true;
        animator = GetComponent<Animator>();
        amountText = "" + amountLeft;
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
            canHeal = false;
            amountLeft--;
            animator.SetTrigger("Use");
            amountText = "" + amountLeft;

            if (OnStackableItemUse != null)
            {
                OnStackableItemUse(amountText);
            }

            Debug.Log("using medkit");
        }
        else
        {
            Debug.Log("can NOT use medkit");
        }
        
    }

    private void StartHeal()
    {
        player.HealPlayer(healthPointsToGive);
        canHeal = true;
    }
}
