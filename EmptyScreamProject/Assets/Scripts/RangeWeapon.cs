using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    public float damage;
    public float range;
    public float fireRate;
    public float impactForce;

    public Camera cam;
    public ParticleSystem muzzleFlash;
    public GameObject impact;

    private float nextFire;

    void Update()
    {
        if (Input.GetButton("Fire1")&& Time.time>= nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    public void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, range))
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
            Destroy(impactGO, 2f);
        }
    }
}
