using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityEvent : MonoBehaviour
{
    public enum DepleteType
    {
        instant,
        gradual,
        maxTypes
    }

    public float sanityDepletedAmount;
    public float affectRate;
    public float affectRateTimer;
    public DepleteType type;

    public bool canDeplete;

    private Player currentPlayer;


    // Start is called before the first frame update
    void Start()
    {
        affectRateTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (type == DepleteType.gradual)
        {
            if (canDeplete)
            {
                affectRateTimer += Time.deltaTime;

                if (affectRateTimer >= affectRate)
                {
                    DepleteSanity();
                    affectRateTimer = 0;
                }
            }
        }
    }

    public void DepleteSanity()
    {
        currentPlayer.ChangeSanityValue(-sanityDepletedAmount);
        //currentPlayer.sanity -= sanityDepletedAmount;
    }

    public void SetPlayer(Player newPlayer)
    {
        currentPlayer = newPlayer;
    }
}
