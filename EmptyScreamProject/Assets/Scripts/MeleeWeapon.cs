using System.Collections;
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
    public GameObject impactTarget;
    public Texture2D[] decals;
    public LayerMask mask;
    public Color color;
    [Range(0f, 5f)]
    public float size;

    public GameObject model;
    public bool hitTarget;

    void Start()
    {
        lerp = GetComponent<AnimationLerp>();
        originalDamage = damage;
        animator = transform.GetChild(0).GetComponent<Animator>();
        player = GameManager.Get().playerGO.GetComponent<Player>();
        playerController = GameManager.Get().playerGO.GetComponent<Player>().fpsController;
        ItemAnimation.OnHitImpact += HitImpact;
        ItemAnimation.OnHitEnd += OnAnimationEnd;
        MeleeAnimations.OnBlock += Block;
        MeleeAnimations.OnCanUse += CanUse;
        MeleeAnimations.OnDisableBlock += DisableBlock;
        MeleeAnimations.OnAnimationEnd += OnAnimationEnd;
        Player.OnPlayerBlockDamage += ExecuteAnimation;
        FirstPersonController.OnFPSJumpStart += JumpAnimationStart;
        FirstPersonController.OnFPSJumpEnd += JumpAnimationEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if(canUse)
        {
            if (Input.GetButtonDown("Fire1") && !isInAnimation && !isBlocking)
            {
                isInAnimation = true;
                player.isDoingAction = true;
                canUse = false;
                CheckTarget();
                Hit();
            }

            if (Input.GetMouseButton(1) || Input.GetMouseButtonDown(1) && !isInAnimation && !isBlocking)
            {
                isInAnimation = true;
                player.isDoingAction = true;
                canUse = false;
                animator.SetBool("Block", true);
                animator.SetBool("stopMovementAnimation", true);
                lerp.canChange = true;
                lerp.timer = 0;
                lerp.lerpOnce = true;
                lerp.canLerp = false;
            }
        }

        if (Input.GetMouseButtonUp(1) || !Input.GetMouseButton(1))
        {
            if (isBlocking)
            {
                player.isDoingAction = false;
                canUse = true;
                animator.SetBool("Block", false);
                animator.SetBool("stopMovementAnimation", false);
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
            animator.SetBool("stopMovementAnimation", true);
           // animator.SetBool("isHit", true);
            //animator.SetTrigger("HitTarget");
            animator.Play("Hit", -1, 0f);
        }
        else
        {
            animator.SetBool("stopMovementAnimation", true);
            //animator.SetBool("isHit", true);
            //animator.SetTrigger("Hit");
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
        isInAnimation = false;
        player.isDoingAction = false;
    }

    public void HitImpact()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position + (cam.transform.forward * 0.2f), cam.transform.forward, out hit, range,mask/*,QueryTriggerInteraction.Ignore*/))
        {
            GameObject impactGO = null;
            Target target = hit.transform.GetComponentInParent<Target>();
            EnemyController enemy = hit.transform.GetComponentInParent<EnemyController>();

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

                if (hit.transform.tag == "KO" && enemy.currentState != EnemyController.States.Follow && enemy.currentState != EnemyController.States.Hit)
                {
                    Debug.Log("Le pego en la nuca");
                    target.InstantStun();
                }
                else
                {
                    target.TakeMeleeDamage(damage);
                }
                int rand = Random.Range(0, decals.Length);
                impactGO = Instantiate(impactTarget, hit.point, Quaternion.LookRotation(hit.normal));

                // SKINNED DECALS
                SkinnedMeshRenderer[] r = hit.collider.transform.root.GetComponentsInChildren<SkinnedMeshRenderer>();
                for (int i = 0; i < r.Length; i++)
                {
                    if (r[i] != null)
                    {
                        PaintDecal.instance.RenderDecal(r[i], decals[rand], hit.point + hit.normal * 0.25f, Quaternion.FromToRotation(Vector3.forward, -hit.normal), color, size);
                    }
                }
            }
            else
            {
                if (hit.transform.tag != "KO")
                {
                    AkSoundEngine.PostEvent("Wrench_attack", gameObject);
                    impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }


            impactGO.transform.SetParent(hit.transform);
            impactGO.transform.position += (impactGO.transform.forward * -0.0001f);
            Destroy(impactGO, 15f);
        }
    }
    public void OnAnimationEnd()
    {
        //isInAnimation = false;
        //canUse = true;
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
        FirstPersonController.OnFPSJumpStart -= JumpAnimationStart;
        FirstPersonController.OnFPSJumpEnd -= JumpAnimationEnd;
    }
}
