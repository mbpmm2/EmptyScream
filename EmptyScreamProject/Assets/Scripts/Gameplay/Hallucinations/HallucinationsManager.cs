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
            if(i != lastIndex && (int)events[i].tier <= (currentPlayer.sanityCurrentTier-1))
            {
                indexesToUse.Add(i);
                
            }
        }

        indexesToUse.Sort();

        for (int i = 0; i < indexesToUse.Count; i++)
        {
            Debug.Log("Testttt: " + indexesToUse[i]);
        }

        int newIndex = Random.Range(indexesToUse[0], indexesToUse[indexesToUse.Count-1]+1);
        Debug.Log("first number is : " + indexesToUse[0]);
        Debug.Log("last number is : " + (indexesToUse[indexesToUse.Count - 1]));
        Debug.Log("Choosen number is : " + newIndex);

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

    private void ActivateTimer()
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
    }
}
