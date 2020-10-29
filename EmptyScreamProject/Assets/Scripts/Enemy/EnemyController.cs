﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
        Wander,
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
    public float damage;
    public float distance;
    public GameObject targetRig;

    private bool doOnce;
    private bool doOnce2;
    private Player player;
    public GameObject parent;

    public float stunMaxTime;
    public float stunTimer;

    public RagdollHelper ragdoll;
    public SphereCollider detectionCollider;
    public SphereCollider instantKOCol;
    public Rigidbody instantKORB;
    public EnemySight sight;

    [Header("UI"), Space]
    public GameObject stunIcon;
    public Image fillBar;
    private float fillTimer;
    [Header("Surprise Settings")]
    public bool isSurpriseEnemy;
    public GameObject activator;
    // Start is called before the first frame update
    void Start()
    {
        SetRigidbodyState(true);
        SetColliderState(true);
        stunIcon.SetActive(false);
        GetComponent<Animator>().enabled = true;

        target = GameManager.Get().playerGO.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameManager.Get().playerGO.GetComponent<Player>();

        if (isSurpriseEnemy)
        {
            Invoke("Stun", 1.0f);
        }
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, target.position);

        //test ragdoll
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    ragdoll.ragdolled = true;
        //}

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    ragdoll.ragdolled = false;
        //}

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
                if (!isSurpriseEnemy)
                {
                    stunTimer += Time.deltaTime;
                    
                    if (fillBar)
                    {
                        fillTimer -= Time.deltaTime;
                        fillBar.fillAmount = fillTimer / stunMaxTime;
                    }

                    if (stunTimer > stunMaxTime)
                    {
                        ragdoll.ragdolled = false;
                        stunTimer = 0;
                        GetComponent<CapsuleCollider>().enabled = true;
                        stunIcon.SetActive(false);
                        //ChangeState(States.Idle);
                    }
                }
                else
                {
                    if (!activator || activator.GetComponent<Door>() && !activator.GetComponent<Door>().isOpen)
                    {
                        ragdoll.ragdolled = false;
                        isSurpriseEnemy = false;
                        //transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                        stunTimer = 0;
                        GetComponent<CapsuleCollider>().enabled = true;
                    }
                }
                break;
            case States.Wander:
                break;
            case States.Dead:
                break;
            default:
                break;
        }

        if (distance <= agent.stoppingDistance+0.5f && (currentState!=States.Dead && currentState != States.Stunned) && (sight.playerInSight || sight.playerHeard))
        {
            Attack();
            FaceTarget();
            doOnce = true;
        }
        else if (currentState != States.Dead && currentState != States.Stunned && lastState!=States.Stunned)
        {
            if (doOnce)
            {
                Debug.Log("ahre");
                doOnce = false;
                ChangeState(States.Follow);
            }
        }
    }

    private void Attack()
    {
        if (doOnce2)
        {
            ChangeState(States.Hit);
            doOnce2 = false;
        }
    }

    public void DoDamage()
    {
        //RaycastHit hit;

        //if (Physics.Raycast(transform.position + (transform.up*1.5f), transform.forward, out hit, 18))
        //{
        //    if (hit.transform.tag=="Player")
        //    {

        //    }
        //}
        //Debug.DrawRay(transform.position + (transform.up * 1.5f), transform.forward, Color.cyan,Time.deltaTime);
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

    public void FaceTarget()
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
            Destroy(parent.gameObject);
            
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

        if (currentState!=lastState)
        {
            doOnce2 = true;
        }
        //manage animation
        switch (currentState)
        {
            case States.Idle:
                animator.SetBool("Idle", true);
                animator.SetBool("Follow", false);
                animator.SetBool("Hit", false);
                //animator.SetBool("Wander", false);
                break;
            case States.Follow:
                animator.SetBool("Idle", false);
                animator.SetBool("Follow", true);
                animator.SetBool("Hit", false);
                //animator.SetBool("Wander", false);
                agent.SetDestination(target.position);
                break;
            case States.Hit:
                animator.SetBool("Idle", false);
                animator.SetBool("Follow", false);
                animator.SetBool("Hit", true);
                //animator.SetBool("Wander", true);
                break;
            case States.Stunned:
                animator.SetBool("Idle", false);
                animator.SetBool("Follow", false);
                animator.SetBool("Hit", false);
                //animator.SetBool("Wander", false);
                break;
            case States.Wander:
                animator.SetBool("Idle", false);
                animator.SetBool("Follow", false);
                animator.SetBool("Hit", false);
                //animator.SetBool("Wander", true);
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
        instantKORB.isKinematic = true;
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
        Debug.Log(currentState.ToString());

        AkSoundEngine.PostEvent("Death_E", this.gameObject);

        SetRigidbodyState(false);
        stunIcon.SetActive(false);
        SetColliderState(true);
        agent.isStopped = true;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Animator>().enabled = false;

        ChangeState(States.Dead);

        Invoke("CreateBlood", 3.0f);

        if (gameObject != null)
        {
            Destroy(parent.gameObject, deathTime);
        }

        if (OnEnemyDeath != null)
        {
            OnEnemyDeath();
        }
    }

    public void Stun()
    {
        if(currentState != States.Stunned && currentState != States.Dead)
        {
            Debug.Log(currentState.ToString());

            if (!isSurpriseEnemy)
            {
                AkSoundEngine.PostEvent("Hit_E_Wrench", this.gameObject);
            }

            SetRigidbodyState(false);
            SetColliderState(true);
            agent.isStopped = true;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Animator>().enabled = false;
            ragdoll.ragdolled = true;

            ChangeState(States.Stunned);
            if (!isSurpriseEnemy)
            {
                stunIcon.SetActive(true);
                fillTimer = stunMaxTime;
            }


            instantKORB.isKinematic = true;
        }
    }

    public void WakeUp()
    {
        stunTimer = 0;
        SetRigidbodyState(true);
        agent.isStopped = false;
        GetComponent<CapsuleCollider>().enabled = true;

        ragdoll.ragdolled = false;

        ChangeState(States.Idle);
        detectionCollider.enabled = true;
        instantKOCol.enabled = true;
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