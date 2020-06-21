using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BloodSplat : MonoBehaviour
{
    public float stayTime;
    public float sizeSpeedMultiplier;
    public float desiredSize;

    private bool canIncreaseSize;
    private float stayTimer;
    private float sizeTimer;
    private DecalProjector decal;

    // Start is called before the first frame update
    void Start()
    {
        decal = GetComponent<DecalProjector>();
        canIncreaseSize = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(canIncreaseSize)
        {
            sizeTimer += Time.deltaTime * sizeSpeedMultiplier;

            Vector3 newSize = new Vector3(sizeTimer,sizeTimer,decal.size.z);

            decal.size = newSize;

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
