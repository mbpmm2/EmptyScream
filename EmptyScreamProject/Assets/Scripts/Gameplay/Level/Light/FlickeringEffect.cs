using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringEffect : MonoBehaviour
{
    public GameObject lightGameObject;
    public GameObject model;
    public Color lightColor;
    public float lightIntensity;
    public float minTime;
    public float maxTime;

    private Light lightEntity;
    private Renderer modelMesh;
    private Material modelMaterial;
    private float flickeringTime;
    private float flickerFinalTime;
    private float appearLightTime;
    private float appearLightTimer;
    private bool isLightOff;

    // Start is called before the first frame update
    void Start()
    {
        appearLightTime = 0.1f;
        if(model)
        {
            modelMesh = model.GetComponent<Renderer>();
        }
        
        lightEntity = lightGameObject.GetComponent<Light>();
        if(modelMesh)
        {
            modelMaterial = modelMesh.material;
        }
        
        setRandomTime();
        lightEntity.color = lightColor;
        if(modelMaterial)
        {
            modelMaterial.SetColor("_EmissionColor", lightColor * lightIntensity * 10);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isLightOff)
        {
            appearLightTimer += Time.deltaTime;
            if(appearLightTimer >= appearLightTime)
            {
                lightEntity.enabled = true;
                if(modelMaterial)
                {
                    modelMaterial.SetColor("_EmissionColor", lightColor * lightIntensity * 10);
                }
                lightEntity.color = lightColor;
                isLightOff = false;
                appearLightTimer = 0;
            }
        }
        else
        {
            flickeringTime += Time.deltaTime;

            if (flickeringTime >= flickerFinalTime)
            {
                lightEntity.enabled = false;
                if (modelMaterial)
                {
                    modelMaterial.SetColor("_EmissionColor", Color.black * 0);
                }
                    
                setRandomTime();
                flickeringTime = 0;
                isLightOff = true;
            }
        }
    }

    private void setRandomTime()
    {
        flickerFinalTime = Random.Range(minTime,maxTime);
    }
}
