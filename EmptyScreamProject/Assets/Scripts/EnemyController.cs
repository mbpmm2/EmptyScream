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
    private Animator animator;

    public GameObject bloodSplat;
    public Transform bloodSplatPosition;
    public bool canFollow;
    Transform target;
    public NavMeshAgent agent;
    public float timer;
    public float deathTime;
    public float healthPoints;
    public float damage;
    public float distance;

    private bool doOnce;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        SetRigidbodyState(true);
        SetColliderState(true);
        GetComponent<Animator>().enabled = true;

        target = GameManager.Get().playerGO.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameManager.Get().playerGO.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, target.position);

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
                break;
            default:
                break;
        }

        if (distance <= agent.stoppingDistance && currentState!=States.Dead)
        {
            Attack();
            FaceTarget();
            doOnce = true;
        }
        else if (currentState != States.Dead)
        {
            if (doOnce)
            {
                doOnce = false;
                ChangeState(States.Follow);
            }
            
        }
    }

    private void Attack()
    {
        ChangeState(States.Hit);
    }

    public void DoDamage()
    {
        if (distance <= agent.stoppingDistance)
        {
            player.ReceiveDamage(damage);
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
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
                animator.SetBool("Idle", true);
                animator.SetBool("Follow", false);
                break;
            case States.Follow:
                animator.SetBool("Stun", false);
                animator.SetBool("Idle", false);
                animator.SetBool("Follow", true);
                animator.SetBool("Hit", false);
                agent.SetDestination(target.position);
                break;
            case States.Hit:
                animator.SetBool("Stun", false);
                animator.SetBool("Idle", false);
                animator.SetBool("Follow", false);
                animator.SetBool("Hit", true);
                break;
            case States.Stunned:
                break;
            case States.Dead:
                break;
            default:
                break;
        }
    }

    void SetRigidbodyState(bool state)
    {

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        //GetComponent<Rigidbody>().isKinematic = !state;

    }


    void SetColliderState(bool state)
    {

        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        //GetComponent<Collider>().enabled = !state;

    }

    public void Die()
    {
        agent.isStopped = true;
        GetComponent<Animator>().enabled = false;
        SetRigidbodyState(false);
        SetColliderState(true);
        ChangeState(States.Dead);
        //agent.enabled = false;
        //agent.Stop();

        Invoke("CreateBlood", 3.0f);

        if (gameObject != null)
        {
            Destroy(gameObject, deathTime);
        }
    }

    private void CreateBlood()
    {
        if (bloodSplat)
        {
            GameObject newBlood = Instantiate(bloodSplat, bloodSplatPosition.position, bloodSplat.transform.rotation);
            newBlood.transform.position = newBlood.transform.position + newBlood.transform.forward * -1.3f;
        }
    }

}