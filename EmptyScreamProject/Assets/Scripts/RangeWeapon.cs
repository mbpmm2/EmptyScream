using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeWeapon : MonoBehaviour
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

    public int bullets;
    public int clipMaxBullets;
    public int clipBullets;
    public float reloadTime;
    private bool isReloading = false;

    public Text bulletsDisplay;
    public Text clipBulletsDisplay;
    public Image crosshair;

    public Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject impact;
    public GameObject weaponModel;

    private TraumaInducer shakeThing;
    private float nextFire;

    private void Start()
    {
        clipBullets = clipMaxBullets;
        shakeThing = gameObject.GetComponent<TraumaInducer>();
    }

    private void OnEnable()
    {
        isReloading = false;
        crosshair.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        crosshair.gameObject.SetActive(false);
    }

    void Update()
    {
        bulletsDisplay.text = bullets.ToString();
        clipBulletsDisplay.text = clipBullets.ToString();

        if (isReloading)
        {
            return;
        }

        if (clipBullets<=0)
        {
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

    public void Shoot()
    {

        StartCoroutine(shakeThing.Shake());
        //implementar sonido
        //AkSoundEngine.PostEvent("shoot", gameObject);
        clipBullets--;
        muzzleFlash.Play();
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position + (cam.transform.forward * 0.5f), cam.transform.forward,out hit, range))
        {
            Debug.Log(hit.transform.name);
            

            Target target = hit.transform.GetComponent<Target>();

            if (hit.rigidbody!=null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            if (target!=null)
            {
                target.TakeDamage(damage);
            }

            GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 4f);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading");

        yield return new WaitForSeconds(reloadTime);

        if (bullets>0)
        {
            bullets -= clipMaxBullets;
            clipBullets = clipMaxBullets;
        }
        
        isReloading = false;
    }
}
