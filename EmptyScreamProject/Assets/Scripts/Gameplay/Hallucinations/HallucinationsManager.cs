using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallucinationsManager : MonoBehaviour
{
    public HallucinationEvent[] events;

    public float hallucinationPeriodsSeconds;
    public bool canStartTimer;
    public float hallucinationPeriodTimer;

    public int lastIndex;
    public List<int> indexesToUse;
    public Player currentPlayer;
    // Start is called before the first frame update
    void Start()
    {
        canStartTimer = true;
        lastIndex = -1;

        for (int i = 0; i < events.Length; i++)
        {
            events[i].OnHallucinationEnd += ActivateTimer;
        }

        Player.OnPlayerChangeSanityTier += ActivateTimer2;
    }

    // Update is called once per frame
    void Update()
    {
        if(canStartTimer)
        {
            hallucinationPeriodTimer += Time.deltaTime;

            if(hallucinationPeriodTimer >= hallucinationPeriodsSeconds)
            {
                ActivateRandomEvent();
                hallucinationPeriodTimer = 0;
                canStartTimer = false;
            }
        }
    }

    public void ActivateRandomEvent()
    {
        for (int i = 0; i < events.Length; i++)
        {
            if(events.Length != 1)
            {
                if (i != lastIndex && (int)events[i].tier <= (currentPlayer.sanityCurrentTier - 1))
                {
                    indexesToUse.Add(i);
                }
            }
            else
            {
                if ((int)events[i].tier <= (currentPlayer.sanityCurrentTier - 1))
                {
                    indexesToUse.Add(i);
                }
            }
            
        }

        if(indexesToUse.Count > 0)
        {
            indexesToUse.Sort();

            int newIndex = -1;

            if(indexesToUse.Count != 1)
            {
                newIndex = Random.Range(indexesToUse[0], indexesToUse[indexesToUse.Count - 1] + 1);
            }
            else
            {
                newIndex = indexesToUse[0];
            }

            if (lastIndex >= 0)
            {
                if (!events[lastIndex].isActive)
                {
                    //canStartTimer = false;
                    events[newIndex].ActivateEvent();
                    lastIndex = newIndex;
                }
            }
            else
            {
                events[newIndex].ActivateEvent();
                lastIndex = newIndex;
            }

            indexesToUse.Clear();
        }

        

    }

    private void ActivateTimer()
    {
        canStartTimer = true;
        hallucinationPeriodTimer = 0;
    }

    private void ActivateTimer2(float ignoreThisValue)
    {
        canStartTimer = true;
        hallucinationPeriodTimer = 0;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < events.Length; i++)
        {
            events[i].OnHallucinationEnd -= ActivateTimer;
        }

        Player.OnPlayerChangeSanityTier -= ActivateTimer2;
    }
}
