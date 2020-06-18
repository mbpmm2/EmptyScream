using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

public class MeleeWeapon : ItemCore
{
    public float damage;
    public float impactForce;
    public float range;

    public Camera cam;
    public GameObject impact;
    public GameObject impactTarget;

    public GameObject model;
    private Animator animator;
    private bool animationEnded;

    public GameObject crosshair;
    Player player;
    // Start is called before the first frame update


    private void OnEnable()
    {
        crosshair.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (crosshair)
        {
            crosshair.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameManager.Get().playerGO.GetComponent<Player>();
        Player.OnPlayerChangeSanityTier += UpdateDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if(canUse)
        {
            if (Input.GetButtonDown("Fire1") && !animationEnded)
            {
                animationEnded = true;
                Hit();
            }
        }
        
    }

    public void Hit()
    {
        //AkSoundEngine.PostEvent("hit", gameObject);
        animator.Play("Hit", -1, 0f);

    }

    public void HitImpact()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position + (cam.transform.forward * 0.2f), cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            GameObject impactGO;
            Target target = hit.transform.GetComponentInParent<Target>();

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            if (target != null)
            {
                target.TakeDamage(damage);
                impactGO = Instantiate(impactTarget, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            }

            impactGO.transform.SetParent(hit.transform);
            impactGO.transform.position += (impactGO.transform.forward * -0.0001f);
            Destroy(impactGO, 5f);
        }
    }
    public void OnAnimationEnd()
    {
        animationEnded = false;
    }

    public void UpdateDamage(float test)
    {
        damage = damage * player.damageMultiplier;
    }
}
