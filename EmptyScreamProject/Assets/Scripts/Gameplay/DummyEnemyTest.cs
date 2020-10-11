using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemyTest : MonoBehaviour
{
    public float damage;
    public float damageRate;
    public float damageRateTimer;
    public bool canDamage;
    public bool damageOnce;
    private bool doOnce;

    private Player currentPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canDamage && !damageOnce)
        {
            damageRateTimer += Time.deltaTime;

            if(damageRateTimer >= damageRate)
            {
                if(currentPlayer != null)
                {
                    currentPlayer.ReceiveDamage(damage);
                }

                damageRateTimer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            currentPlayer = other.GetComponent<Player>();
            if(damageOnce)
            {
                if(!doOnce)
                {
                    if (currentPlayer != null)
                    {
                        currentPlayer.ReceiveDamage(damage);
                    }
                    doOnce = true;
                }
                
            }
            else
            {
                canDamage = true;
                damageRateTimer = 0;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            currentPlayer = null;
            canDamage = false;
            damageRateTimer = 0;
        }
    }
}
