using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public delegate void OnPlayerAction();
    public delegate void OnPlayerStatusAction(float amount);
    public delegate void OnPlayerStatusAction2();
    public delegate void OnPlayerStatusAction3(float amount, float maxAmount);
    public delegate void OnInteractionAction(UIInteract.UIPickupType type);
    public static OnPlayerStatusAction OnPlayerChangeHP;
    public static OnPlayerStatusAction2 OnPlayerHurt;
    public static OnPlayerStatusAction2 OnPlayerChangeHP2;
    public static OnPlayerStatusAction2 OnDefenseStart;
    public static OnPlayerStatusAction2 OnDefenseStop;
    public static OnInteractionAction OnInteractAvailable;
    public static OnInteractionAction OnInteractUnavailable;
    public static OnInteractionAction OnInteractNull;
    public static OnPlayerStatusAction3 OnDefenseTimerON;
    public static OnPlayerAction OnPlayerBlockDamage;

    [Header("Player Config")]
    public float maxHealth;
    public float walkSpeed;
    public float runSpeed;
    public float crouchMovSpeed;
    [Range(0,100)]
    public float damageAbsorbPercentage;
    private float bonusDamageAbsorbPercentage;
    private TraumaInducer screenShake;
    

    [Header("Player Current State")]
    public float health;
    //public float sanity;
    public float damageMultiplier;
    public float speedMultiplier;
    public bool isDefenseActive;
    public bool isBlocking;

    [Header("Interact Settings")]
    public KeyCode interactKey;
    public Camera cam;
    public LayerMask rayCastLayer;
    public float rayDistance;

    [Header("Private Variables")]
    public bool isDoingAction;
    public bool isBeingDamaged;
    public FirstPersonController fpsController;
    private float defenseTimer;
    private float defenseTime;
    private bool checkOnce;
    private GameObject lastGO;

    [Header("Death Settings")]
    public float restartTime;
    public GameObject ragdoll;
    public GameObject headParent;


    // Light effects
    [Header("Darkness effect config")]
    public bool isInLight;
    public List<GameObject> lightsOnPlayer;
    public float decreaseSanitySpeed;

    //[Header("Components Assigment")]
    //Player Inventory;

    // Start is called before the first frame update
    void Start()
    {
        isInLight = true;
        screenShake = GetComponent<TraumaInducer>();
        fpsController = GetComponent<FirstPersonController>();
        fpsController.m_WalkSpeed = walkSpeed;
        fpsController.m_RunSpeed = runSpeed;
        fpsController.crouchMovSpeed = crouchMovSpeed;
        lastGO = gameObject;

        isDefenseActive = false;

        health = maxHealth;
        if (OnPlayerChangeHP != null)
        {
            OnPlayerChangeHP(health);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDefenseActive)
        {
            defenseTimer += Time.deltaTime;

            if(OnDefenseTimerON != null)
            {
                OnDefenseTimerON(defenseTimer,defenseTime);
            }

            if (defenseTimer >= defenseTime)
            {
                StopDefense();
                defenseTimer = 0;
            }
        }

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayDistance, rayCastLayer))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.forward * hit.distance, Color.yellow);

            switch (hit.transform.tag)
            {
                case "interactable":
                    if(lastGO != hit.transform.gameObject)
                    {
                        lastGO = hit.transform.gameObject;
                        checkOnce = false;
                    }
                    Debug.DrawRay(cam.transform.position, cam.transform.forward * hit.distance, Color.green);

                    if(!checkOnce)
                    {
                        if(hit.transform.gameObject.GetComponent<Door>())
                        {
                            if (hit.transform.gameObject.GetComponent<Door>().CheckInteract())
                            {
                                if (OnInteractAvailable != null)
                                {
                                    OnInteractAvailable(UIInteract.UIPickupType.defaultPickup);
                                }
                            }
                            else
                            {
                                if (OnInteractUnavailable != null)
                                {
                                    OnInteractAvailable(UIInteract.UIPickupType.lockedPickup);
                                }
                            }
                        }
                        else if(hit.transform.gameObject.GetComponent<ButtonWorld>())
                        {
                            if (OnInteractAvailable != null)
                            {
                                OnInteractAvailable(UIInteract.UIPickupType.defaultPickup);
                            }
                        }
                        

                        checkOnce = true;
                    }

                    if (Input.GetKeyDown(interactKey))
                    {
                        hit.transform.gameObject.GetComponent<Interactable>().Interact();
                    }
                    break;
                case "pickup":
                    break;
                default:
                    if(OnInteractNull != null)
                    {
                        OnInteractNull(UIInteract.UIPickupType.maxTypes);
                    }

                    if (lastGO != gameObject)
                    {
                        lastGO = gameObject;
                        checkOnce = false;
                    }
                    break;
            }
        }
        else
        {
            if (OnInteractNull != null)
            {
                OnInteractNull(UIInteract.UIPickupType.maxTypes);
            }

            if (lastGO != gameObject)
            {
                lastGO = gameObject;
                checkOnce = false;
            }
            Debug.DrawRay(cam.transform.position, cam.transform.forward * rayDistance, Color.white);
        }
    }

    public void ReceiveDamage(float amount)
    {
        if(isBlocking)
        {
            health -= amount - ( amount * ((damageAbsorbPercentage + bonusDamageAbsorbPercentage) * 0.01f));
            if(OnPlayerBlockDamage != null)
            {
                OnPlayerBlockDamage();
            }
        }
        else
        {
            health -= amount - (amount * (bonusDamageAbsorbPercentage * 0.01f));
        }
        

        if(health <= 0)
        {
            Die();
            health = 0;
        }

        if(OnPlayerChangeHP != null)
        {
            OnPlayerChangeHP(health);
        }

        if(OnPlayerHurt != null)
        {
            OnPlayerHurt();
        }

        StartCoroutine(screenShake.Shake());
    }

    public void HealPlayer(int amount)
    {
        health += amount;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        if (OnPlayerChangeHP != null)
        {
            OnPlayerChangeHP(health);
        }

        if (OnPlayerChangeHP2 != null)
        {
            OnPlayerChangeHP2();
        }
    }

    public bool CanHeal()
    {
        if (health >= maxHealth)
        {
            return false;
        }

        return true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "enemy")
        {
            DummyEnemyTest enemy = other.gameObject.GetComponent<DummyEnemyTest>();
            isBeingDamaged = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            isBeingDamaged = false;
        }
    }

    public void SetDefenseTimer(float time, float bonusPercentage)
    {
        defenseTime = time;
        isDefenseActive = true;
        bonusDamageAbsorbPercentage = bonusPercentage;
        if (OnDefenseStart != null)
        {
            OnDefenseStart();
        }
    }

    private void StopDefense()
    {
        defenseTime = 0;
        isDefenseActive = false;
        bonusDamageAbsorbPercentage = 0;

        if (OnDefenseStop != null)
        {
            OnDefenseStop();
        }
    }

    private void Die()
    {
        GameManager.Get().restartTime = restartTime;
        GameManager.Get().restart = true;
        ragdoll.SetActive(true);
        cam.transform.SetParent(headParent.transform);
        GetComponent<FirstPersonController>().enabled = false;
        GetComponent<PlayerInventory>().itemsParent.SetActive(false);
        GetComponent<PlayerInventory>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        enabled = false;
    }
}
