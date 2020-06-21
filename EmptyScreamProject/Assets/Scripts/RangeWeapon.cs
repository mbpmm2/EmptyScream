using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeWeapon : ItemCore
{
    public enum WeaponType
    {
        Automatic,
        SemiAutomatic,
        AllTypes
    }

    public WeaponType type;

    public float damage;
    public float range;
    public float fireRate;
    public float impactForce;

    //public int bullets;
    public int clipMaxBullets;
    public int clipBullets;
    public float reloadTime;
    public bool isReloading = false;

    public Text bulletsDisplay;
    public Text clipBulletsDisplay;
    //public Image crosshair;

    public Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject impact;
    public GameObject impactTarget;
    public GameObject weaponModel;

    private Animator animator;
    private TraumaInducer shakeThing;
    private float nextFire;
    private bool doOnce;

    private void Start()
    {
        clipBullets = clipMaxBullets;
        shakeThing = gameObject.GetComponent<TraumaInducer>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        isReloading = false; // Check this.
        crosshair.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if(crosshair)
        {
            crosshair.gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        bulletsDisplay.text = amountLeft.ToString();
        clipBulletsDisplay.text = clipBullets.ToString();

        if (canUse)
        {
            if (isReloading)
            {
                return;
            }

            if (clipBullets <= 0)
            {
                if (amountLeft > 0)
                {
                    if (!doOnce)
                    {
                        animator.Play("Reload", -1, 0f);
                        doOnce = true;
                    }

                }
                StartCoroutine(Reload());
                return;
            }

            if (type == WeaponType.Automatic)
            {
                if (Input.GetButton("Fire1") && Time.time >= nextFire && clipBullets > 0)
                {
                    nextFire = Time.time + 1f / fireRate;
                    Shoot();
                }
            }
            else if (type == WeaponType.SemiAutomatic)
            {
                if (Input.GetButtonDown("Fire1") && clipBullets > 0)
                {
                    Shoot();
                }
            }
        }
    }

    

    public void Shoot()
    {

        StartCoroutine(shakeThing.Shake());
        //animator.SetTrigger("Shoot");
        animator.Play("Shoot", -1, 0f);
        //implementar sonido
        //AkSoundEngine.PostEvent("shoot", gameObject);
        clipBullets--;
        muzzleFlash.Play();
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position + (cam.transform.forward * 0.5f), cam.transform.forward,out hit, range))
        {
            Debug.Log(hit.transform.name);

            GameObject impactGO;
            Target target = hit.transform.GetComponentInParent<Target>();

            if (hit.rigidbody!=null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            if (target!=null)
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
            Destroy(impactGO, 15f);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading");

        yield return new WaitForSeconds(reloadTime);

        if (amountLeft > 0)
        {
            amountLeft -= clipMaxBullets;
            clipBullets = clipMaxBullets;
            doOnce = false;
            //animator.Play("Shoot", -1, 1f);
        }
        
        isReloading = false;
    }
}
