using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

public class MeleeWeapon : MonoBehaviour
{
    public float damage;
    public float impactForce;
    public float range;

    public Camera cam;
    public GameObject impact;

    public GameObject model;
    private Animator animator;
    private bool animationEnded;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !animationEnded)
        {
            animationEnded = true;
            Hit();
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

        if (Physics.Raycast(cam.transform.position + (cam.transform.forward * 0.5f), cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);


            Target target = hit.transform.GetComponent<Target>();

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            impactGO.transform.SetParent(hit.transform);
            impactGO.transform.position += (impactGO.transform.forward * -0.0001f);
            Destroy(impactGO, 5f);
        }
    }
    public void OnAnimationEnd()
    {
        animationEnded = false;
    }
}
