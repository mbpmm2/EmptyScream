using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowsEvent : HallucinationEvent
{
    [Header("Event-Specific Configuration")]
    public GameObject shadowModel;
    public Transform[] shadowsLocations;

    public GameObject[] currentShadows;
    // Start is called before the first frame update
    void Start()
    {
        OnHallucinationStart += SpawnShadows;
        OnHallucinationEnd += DeleteShadows;
        //isActive = true;
        
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    private void SpawnShadows()
    {
        currentShadows = new GameObject[shadowsLocations.Length];
        for (int i = 0; i < shadowsLocations.Length; i++)
        {
            GameObject newShadow = Instantiate(shadowModel, shadowsLocations[i].transform.position, shadowsLocations[i].transform.rotation);
            currentShadows[i] = newShadow;
        }
    }

    private void DeleteShadows()
    {
        for (int i = 0; i < currentShadows.Length; i++)
        {
            Destroy(currentShadows[i]);
        }

        currentShadows = null;
    }

    private void OnDestroy()
    {
        this.OnHallucinationStart -= SpawnShadows;
        this.OnHallucinationEnd -= DeleteShadows;
    }
}
