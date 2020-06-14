using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemyTest : MonoBehaviour
{
    public float damage;
    public float damageRate;
    public float damageRateTimer;
    public bool canDamage;

    private Player currentPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canDamage)
        {
            damageRateTimer += Time.deltaTime;

            if(damageRateTimer >= damageRate)
            {
                if(currentPlayer != null)
                {
                    currentPlayer.CauseDamage(damage);
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
            canDamage = true;
            damageRateTimer = 0;
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
