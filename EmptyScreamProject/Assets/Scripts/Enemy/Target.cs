using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    private float maxHealth;
    public float koHealth;
    public EnemyController enemyController;

    public int maxStuns = 3;
    public int stuns;

    private void Start()
    {
        enemyController = GetComponentInChildren<EnemyController>();
        maxHealth = health;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (enemyController.currentState!=EnemyController.States.Stunned && enemyController.currentState != EnemyController.States.Dead)
        {
            enemyController.ChangeState(EnemyController.States.Follow);
        }
        if (health<=0)
        {
            Die();
        }
    }

    public void TakeMeleeDamage(float damage)
    {
        health -= damage;
        if (enemyController.currentState != EnemyController.States.Stunned && enemyController.currentState != EnemyController.States.Dead)
        {
            enemyController.ChangeState(EnemyController.States.Follow);
        }
        
        if (health <= koHealth && health > 0)
        {
            Stun();
        }
        if (health <= 0)
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
        if (enemyController.currentState != EnemyController.States.Stunned && enemyController.currentState != EnemyController.States.Dead)
        {
            stuns++;
            if (stuns<maxStuns)
            {
                enemyController.Stun();
                health = maxHealth * 0.7f;
            }
            else
            {
                Die();
            }
        }
    }

    public void InstantStun()
    {
        if (enemyController.currentState != EnemyController.States.Stunned && enemyController.currentState != EnemyController.States.Dead && !enemyController.sight.playerInSight)
        {
            stuns++;
            if (stuns < maxStuns)
            {
                enemyController.Stun();
            }
            else
            {
                Die();
            }
        }
    }
}
