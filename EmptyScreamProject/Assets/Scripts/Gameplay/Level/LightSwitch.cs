using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public bool isOn;
    public Color onColor;
    public float onIntensity;
    public Color offColor;
    //public float offIntensity;

    public MeshRenderer[] lightMaps;
    public GameObject lightParent;
    public GameObject[] lights;


    // Start is called before the first frame update
    void Start()
    {
        if(lightParent)
        {
            if (lightParent.transform.childCount > 0)
            {
                lights = new GameObject[lightParent.transform.childCount];

                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i] = lightParent.transform.GetChild(i).gameObject;
                }
            }
        }
        
        

        TurnOff();
    }

    public void TurnOn()
    {
        isOn = true;
        CheckLights();
    }

    public void TurnOff()
    {
        isOn = false;
        CheckLights();
    }

    public void Switch()
    {
        isOn = !isOn;
        CheckLights();
    }

    public void CheckLights()
    {
        if(isOn)
        {
            AkSoundEngine.PostEvent("Lights_on", this.gameObject);
            if (lightParent)
            {
                if (lightParent.transform.childCount > 0)
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(true);
                    }
                }
            }
                
            
            if(lightMaps.Length > 0)
            {
                for (int i = 0; i < lightMaps.Length; i++)
                {
                    lightMaps[i].material.SetColor("_EmissiveColor", onColor * onIntensity);
                }
            }

            
        }
        else
        {
            AkSoundEngine.PostEvent("Lights_off", this.gameObject);
            if (lightParent)
            {
                if (lightParent.transform.childCount > 0)
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(false);
                    }
                }
            }
                

            if (lightMaps.Length > 0)
            {
                for (int i = 0; i < lightMaps.Length; i++)
                {
                    lightMaps[i].material.SetColor("_EmissiveColor", offColor);
                }
            }

        }
    }
}
