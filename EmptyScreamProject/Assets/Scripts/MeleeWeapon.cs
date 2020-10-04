﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.Characters.FirstPerson;

public class MeleeWeapon : ItemCore
{
    public float damage;
    private float originalDamage;
    public float impactForce;
    public float range;
    public bool isBlocking;

    public Camera cam;
    public GameObject impact;
    public GameObject[] impactTarget;

    public GameObject model;
    //private Animator animator;
   // public bool animationEnded;

    public bool hitTarget;

    Player player;
    public FirstPersonController playerController;
    public bool lastWalkingState = false;
    public bool doOnce;

    void Start()
    {
        lerp = GetComponent<AnimationLerp>();
        originalDamage = damage;
        animator = transform.GetChild(0).GetComponent<Animator>();
        player = GameManager.Get().playerGO.GetComponent<Player>();
        playerController = GameManager.Get().playerGO.GetComponent<Player>().fpsController;
        Player.OnPlayerChangeSanityTier += UpdateDamage;
        ItemAnimation.OnHitImpact += HitImpact;
        ItemAnimation.OnHitEnd += OnAnimationEnd;
        MeleeAnimations.OnBlock += Block;
        MeleeAnimations.OnCanUse += CanUse;
        MeleeAnimations.OnDisableBlock += DisableBlock;
        MeleeAnimations.OnAnimationEnd += OnAnimationEnd;
        Player.OnPlayerBlockDamage += ExecuteAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        if(canUse)
        {
            if (Input.GetButtonDown("Fire1") && !isInAnimation && !isBlocking)
            {
                isInAnimation = true;
                CheckTarget();
                Hit();
            }

            if (playerController.m_IsWalking != lastWalkingState && !playerController.isStanding)
            {
                lastWalkingState = playerController.m_IsWalking;

                if(lastWalkingState)
                {
                    animator.SetBool("isWalking", lastWalkingState);
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isStanding", false);
                    isInAnimation = false;
                    doOnce = false;
                }
                else
                {
                    animator.SetBool("isWalking", lastWalkingState);
                    animator.SetBool("isRunning", true);
                    animator.SetBool("isStanding", false);
                    isInAnimation = true;
                    doOnce = false;
                }
            }

            if (Input.GetMouseButtonDown(1) && !isInAnimation && !isBlocking)
            {
                isInAnimation = true;
                canUse = false;
                animator.SetBool("Block", true);
                lerp.canChange = true;
                lerp.timer = 0;
                lerp.lerpOnce = true;
                lerp.canLerp = false;
            }
        }

        if(playerController.m_IsWalking != lastWalkingState) // STOPPED RUNNING
        {
            if (isInAnimation && !canUse)
            {
                lastWalkingState = playerController.m_IsWalking;
                if (lastWalkingState)
                {
                    animator.SetBool("isWalking", lastWalkingState);
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isStanding", false);
                    isInAnimation = false;
                    doOnce = false;
                }
            }
        }
        else if(playerController.isStanding) // STOPPED MOVING
        {
            if (!doOnce)
            {
                doOnce = true;
                isInAnimation = false;
                lastWalkingState = false;
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isStanding", true);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (isBlocking && !canUse)
            {
                canUse = true;
                animator.SetBool("Block", false);
                lerp.canChange = false;
                lerp.lerpOnce = false;
                lerp.canLerp = true;
            }
                
        }
    }

    public void Hit()
    {
        //AkSoundEngine.PostEvent("hit", gameObject);
        if (!hitTarget)
        {
            animator.Play("Hit", -1, 0f);
        }
        else
        {
            animator.Play("HitTarget", -1, 0f);
        }

    }

    public void Block()
    {
        //animationEnded = true;
        isBlocking = true;
        player.isBlocking = isBlocking;
    }

    public void DisableBlock()
    {
        isBlocking = false;
        player.isBlocking = isBlocking;
    }

    public void CanUse()
    {
        canUse = true;
    }

    public void HitImpact()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position + (cam.transform.forward * 0.2f), cam.transform.forward, out hit, range))
        {
            GameObject impactGO;
            Target target = hit.transform.GetComponentInParent<Target>();

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            if (target != null)
            {
                if (target.health >= 0)
                {
                    AkSoundEngine.PostEvent("Hit_E_Wrench", gameObject);
                }
                target.TakeMeleeDamage(damage);
                int rand = Random.Range(0, impactTarget.Length);
                impactGO = Instantiate(impactTarget[rand], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                AkSoundEngine.PostEvent("Wrench_attack", gameObject);
                impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            }

            impactGO.transform.SetParent(hit.transform);
            impactGO.transform.position += (impactGO.transform.forward * -0.0001f);
            Destroy(impactGO, 15f);
        }
    }
    public void OnAnimationEnd()
    {
        isInAnimation = false;
    }

    public void UpdateDamage(float test)
    {
        damage = originalDamage * player.damageMultiplier;
    }

    public void CheckTarget()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position + (cam.transform.forward * 0.2f), cam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponentInParent<Target>();

            if (target != null)
            {
                hitTarget = true;
            }

            hitTarget = true;
            /*

            
            else
            {
                hitTarget = false;
            }*/


        }
        else
        {
            hitTarget = false;
        }
    }

    public void ExecuteAnimation()
    {
        animator.Play("BlockDamage", -1, 0f);
    }

    private void OnDestroy()
    {
        ItemAnimation.OnHitImpact -= HitImpact;
        ItemAnimation.OnHitEnd -= OnAnimationEnd;
        MeleeAnimations.OnBlock -= Block;
        MeleeAnimations.OnCanUse -= CanUse;
        MeleeAnimations.OnDisableBlock -= DisableBlock;
        MeleeAnimations.OnAnimationEnd -= OnAnimationEnd;
        Player.OnPlayerBlockDamage -= ExecuteAnimation;
    }
}
