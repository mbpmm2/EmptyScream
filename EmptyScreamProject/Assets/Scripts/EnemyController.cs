using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public delegate void OnEnemyAction();
    public static OnEnemyAction OnEnemyDeath;

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

    Transform target;
    public NavMeshAgent agent;

    public float timer;
    public float deathTime;

    public float sanityChangeValue;
    public float healthPoints;
    public float damage;
    public float distance;
    public GameObject targetRig;

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
        AkSoundEngine.PostEvent("Attack_E", this.gameObject);
        if (distance <= agent.stoppingDistance)
        {
            player.ReceiveDamage(damage);
        }
    }

    public void PlayFootStep()
    {
        AkSoundEngine.PostEvent("Steps_E", this.gameObject);
    }

    public void PlayIdle()
    {
        AkSoundEngine.PostEvent("Enemy_Idle", this.gameObject);
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
            
        }
    }

    private void MovementUpdate()
    {
        agent.SetDestination(target.position);
        targetRig.transform.position = target.transform.position;
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
                
                animator.SetBool("Idle", false);
                animator.SetBool("Follow", true);
                animator.SetBool("Hit", false);
                agent.SetDestination(target.position);
                break;
            case States.Hit:
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
        AkSoundEngine.PostEvent("Death_E", this.gameObject);
        SetRigidbodyState(false);
        SetColliderState(true);
        agent.isStopped = true;
        GetComponent<Animator>().enabled = false;
        
        ChangeState(States.Dead);
        player.ChangeSanityValue(sanityChangeValue);
        //agent.enabled = false;
        //agent.Stop();

        Invoke("CreateBlood", 3.0f);

        if (gameObject != null)
        {
            Destroy(gameObject, deathTime);
        }

        if (OnEnemyDeath != null)
        {
            OnEnemyDeath();
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