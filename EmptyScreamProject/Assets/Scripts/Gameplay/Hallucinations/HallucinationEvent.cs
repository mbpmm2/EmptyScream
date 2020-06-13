using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallucinationEvent : MonoBehaviour
{
    public delegate void OnHallucinationAction();
   // public delegate void OnHallucinationAction2(int index);
    public OnHallucinationAction OnHallucinationStart;
    public OnHallucinationAction OnHallucinationEnd;
    // public OnHallucinationAction2 OnHallucinationEndIndex;

    public enum eventTiers
    {
        Tier1,
        Tier2,
        Tier3,
        maxTiers
    }

    [Header("Main Configuration")]
    public float eventDuration;
    public bool isActive;
    public eventTiers tier;

    private float eventTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            eventTimer += Time.deltaTime;

            if(eventTimer >= eventDuration)
            {
                eventTimer = 0;
                DeactivateEvent();
            }
        }
    }

    public void ActivateEvent()
    {
        isActive = true;
        if(OnHallucinationStart != null)
        {
            OnHallucinationStart();
        }
    }

    public void DeactivateEvent()
    {
        isActive = false;

        if(OnHallucinationEnd != null)
        {
            OnHallucinationEnd();
        }

        /*if (OnHallucinationEndIndex != null)
        {
            OnHallucinationEndIndex();
        }*/
    }

    /*public bool IsActive()
    {
        return isActive;
    }*/
}
