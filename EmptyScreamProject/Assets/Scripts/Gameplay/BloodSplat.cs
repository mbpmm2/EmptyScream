using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BloodSplat : MonoBehaviour
{
    public bool isParent;
    public float stayTime;
    public float sizeSpeedMultiplier;
    public float desiredSize;

    private bool canIncreaseSize;
    private float stayTimer;
    private float sizeTimer;
    private DecalProjector[] decals;

    // Start is called before the first frame update
    void Start()
    {
        
        if(isParent)
        {
            decals = new DecalProjector[2];
            decals[1] = transform.GetChild(0).GetComponent<DecalProjector>();
        }
        else
        {
            decals = new DecalProjector[1];
        }

        decals[0] = GetComponent<DecalProjector>();

        for (int i = 0; i < decals.Length; i++)
        {
            Vector3 newSize = new Vector3(0, 0, decals[i].size.z);
            decals[i].size = newSize;
        }
        //decal = GetComponent<DecalProjector>();
        canIncreaseSize = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(canIncreaseSize)
        {
            sizeTimer += Time.deltaTime * sizeSpeedMultiplier;

            for (int i = 0; i < decals.Length; i++)
            {
                Vector3 newSize = new Vector3(sizeTimer, sizeTimer, decals[i].size.z);
                decals[i].size = newSize;
            }
            

            if (sizeTimer >= desiredSize)
            {
                sizeTimer = 0;
                canIncreaseSize = false;
            }
        }

        stayTimer += Time.deltaTime;

        if(stayTimer >= stayTime)
        {
            Destroy(gameObject);
        }
    }
}
