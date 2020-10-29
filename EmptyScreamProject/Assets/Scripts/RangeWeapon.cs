using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


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
    //public ParticleSystem muzzleFlash;
    public GameObject impact;
    public GameObject impactTarget;
    public Texture2D[] decals;
    public Color color;
    public LayerMask mask;
    [Range(0f, 5f)]
    public float size;
    public GameObject weaponModel;

    [Header("Material Settings")]
    public MeshRenderer mesh;
    public Color normalColor;
    public float normalIntensity = 60.016924f;
    public Color noAmmoColor;
    public float noAmmoIntensity = 160.016924f;
    public Color lowAmmoColor;
    public float lowAmmoIntensity = 100.016924f;
    public Color lastBulletColor;
    public float lastBulletIntensity = 100.016924f;

    //private Animator animator;
    private TraumaInducer shakeThing;
    private float nextFire;
    private bool doOnce2;

    private void Start()
    {
        lerp = GetComponent<AnimationLerp>();
        clipBullets = clipMaxBullets;
        shakeThing = gameObject.GetComponent<TraumaInducer>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        mesh.material.SetColor("_EmissiveColor", normalColor * normalIntensity);
        playerController = GameManager.Get().playerGO.GetComponent<Player>().fpsController;
        player = GameManager.Get().playerGO.GetComponent<Player>();
        // mesh.material.SetColor("_EmissionColor", normalColor * 6.016924f);
        FirstPersonController.OnFPSJumpStart += JumpAnimationStart;
        FirstPersonController.OnFPSJumpEnd += JumpAnimationEnd;

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

            if (/*clipBullets <= 0 ||*/ (Input.GetKeyDown(KeyCode.R) && clipBullets<clipMaxBullets))
            {
                if (amountLeft > 0)
                {
                    if (!doOnce2)
                    {
                        animator.Play("Reload", -1, 0f);
                        doOnce2 = true;
                    }
                    StartCoroutine(Reload());
                }
                else
                {
                    animator.Play("ShootEmpty", -1, 0f);
                }
                
                return;
            }

            if (type == WeaponType.Automatic)
            {
                if (Input.GetMouseButton(0) && Time.time >= nextFire && clipBullets > 0)
                {
                    if (clipBullets==3)
                    {
                        AkSoundEngine.PostEvent("nail_gun_low_ammo", gameObject);
                    }
                    if (clipBullets == 1)
                    {
                        AkSoundEngine.PostEvent("nail_gun_last_bullet", gameObject);
                    }
                    nextFire = Time.time + 1f / fireRate;
                    Shoot();
                }
                else if (Input.GetMouseButtonDown(0) && clipBullets <= 0)
                {
                    animator.Play("ShootEmpty", -1, 0f);
                }
            }
            else if (type == WeaponType.SemiAutomatic)
            {
                if (Input.GetMouseButtonDown(0) && clipBullets > 0)
                {
                    if (clipBullets == 4)
                    {
                        AkSoundEngine.PostEvent("nail_gun_low_ammo", gameObject);
                        mesh.material.SetColor("_EmissiveColor", lowAmmoColor * lowAmmoIntensity);
                    }
                    if (clipBullets == 2)
                    {
                        AkSoundEngine.PostEvent("nail_gun_last_bullet", gameObject);
                        mesh.material.SetColor("_EmissiveColor", lastBulletColor * lastBulletIntensity);
                    }
                    Shoot();
                }
                else if (Input.GetMouseButtonDown(0) && clipBullets <= 0)
                {
                    animator.Play("ShootEmpty", -1, 0f);
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
        AkSoundEngine.PostEvent("nail_gun_shoot", gameObject);
        clipBullets--;

        if(clipBullets <= 0)
        {
            mesh.material.SetColor("_EmissiveColor", noAmmoColor * noAmmoIntensity);
        }

        //muzzleFlash.Play();
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position + (cam.transform.forward * 0.5f), cam.transform.forward,out hit, range, mask, QueryTriggerInteraction.Ignore))
        {

            GameObject impactGO;
            Target target = hit.transform.GetComponentInParent<Target>();

            if (hit.rigidbody!=null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            if (target!=null)
            {
                if (target.health >= 0)
                {
                    AkSoundEngine.PostEvent("Hit_E_Nails", gameObject);
                }
                target.TakeDamage(damage);
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
                AkSoundEngine.PostEvent("Nail_hit_wall", gameObject);
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
        AkSoundEngine.PostEvent("nail_gun_reload", gameObject);

        yield return new WaitForSeconds(reloadTime);

        

        if (amountLeft > 0)
        {
            mesh.material.SetColor("_EmissiveColor", normalColor * normalIntensity);

            if (amountLeft<clipMaxBullets)
            {
                int aux2= (clipMaxBullets - clipBullets);
                clipBullets += aux2;
                amountLeft -= aux2;

                if (amountLeft<0)
                {
                    clipBullets += amountLeft;
                    amountLeft = 0;
                }
                doOnce2 = false;
            }
            else
            {
                if (clipBullets < clipMaxBullets && clipBullets > 0)
                {
                    int aux = clipMaxBullets - clipBullets;
                    amountLeft -= aux;
                    clipBullets = clipMaxBullets;
                    doOnce2 = false;
                }
                else
                {
                    amountLeft -= clipMaxBullets;
                    clipBullets = clipMaxBullets;
                    doOnce2 = false;
                }
            }
        }
        
        isReloading = false;
    }

    private void OnDestroy()
    {
        FirstPersonController.OnFPSJumpStart -= JumpAnimationStart;
        FirstPersonController.OnFPSJumpEnd -= JumpAnimationEnd;
    }
}
