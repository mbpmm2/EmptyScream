using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public delegate void OnEnemyAction();
    public OnEnemyAction OnEnemyDeath;

    public enum States
    {
        Idle,
        Follow,
        Hit,
        Stunned,
        Dead,
        allStates
    }

    public States currentState;
    public States lastState;

    public bool canFollow;
    Transform target;
    NavMeshAgent agent;
    public float timer;
    public float deathTime;
    public float healthPoints;
    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.Get().playerGO.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.Idle:
                break;
            case States.Follow:
                MovementUpdate();
                break;
            case States.Hit:
                break;
            case States.Stunned:
                break;
            case States.Dead:
                DeathUpdate();
                break;
            default:
                break;
        }
    }

    private void DeathUpdate()
    {
        if (timer > deathTime)
        {
            //gameObject.SetActive(false);
            Destroy(this.gameObject);
            if (OnEnemyDeath != null)
            {
                OnEnemyDeath();
            }
        }
    }

    private void MovementUpdate()
    {
        agent.SetDestination(target.position);
    }

    public void ChangeState(States newState)
    {
        lastState = currentState;
        currentState = newState;

        //manage animation
        switch (currentState)
        {
            case States.Idle:
                break;
            case States.Follow:
                agent.SetDestination(target.position);
                break;
            case States.Hit:
                break;
            case States.Stunned:
                break;
            case States.Dead:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            ChangeState(States.Follow);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            ChangeState(States.Idle);
        }
    }
}