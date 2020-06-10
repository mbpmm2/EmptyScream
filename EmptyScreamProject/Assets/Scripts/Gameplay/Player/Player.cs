using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void OnPlayerStatusAction(float amount);
    public delegate void OnPlayerStatusAction2();
    public static OnPlayerStatusAction OnPlayerChangeHP;
    public static OnPlayerStatusAction2 OnPlayerHurt;

    [Header("Player Config")]
    public float maxHealth;
    public float maxSanity;

    [Header("Player Current State")]
    public float health;
    public float sanity;

    //public float damageSpeed;
    public bool isBeingDamaged;
    public float damage;

    //[Header("Components Assigment")]
    //Player Inventory;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        sanity = maxSanity;
        if (OnPlayerChangeHP != null)
        {
            OnPlayerChangeHP(health);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if(isBeingDamaged)
        {
            CauseDamage(damage);
        }*/
    }

    public void CauseDamage(float amount)
    {
        health -= amount;

        if(health <= 0)
        {
            health = 0;
        }

        if(OnPlayerChangeHP != null)
        {
            OnPlayerChangeHP(health);
        }

        if(OnPlayerHurt != null)
        {
            OnPlayerHurt();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "enemy")
        {
            DummyEnemyTest enemy = other.gameObject.GetComponent<DummyEnemyTest>();

            damage = enemy.damage;
            isBeingDamaged = true;

           // Debug.Log("gg ameo");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            isBeingDamaged = false;
        }
    }
}
