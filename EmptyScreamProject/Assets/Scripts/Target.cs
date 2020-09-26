using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    public EnemyController enemyController;

    private void Start()
    {
        enemyController = GetComponentInChildren<EnemyController>();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health<=10 && health>0)
        {
            Stun();
        }
        if (health<=0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (enemyController.currentState != EnemyController.States.Dead)
        {
            enemyController.Die();
        }
    }

    public void Stun()
    {
        if (enemyController.currentState != EnemyController.States.Stunned)
        {
            enemyController.Stun();
        }
    }
}
