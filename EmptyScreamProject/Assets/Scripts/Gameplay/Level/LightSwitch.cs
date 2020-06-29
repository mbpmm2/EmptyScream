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
        lights = new GameObject[lightParent.transform.childCount];

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i] = lightParent.transform.GetChild(i).gameObject;
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
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(true);
            }

            for (int i = 0; i < lightMaps.Length; i++)
            {
                lightMaps[i].material.SetColor("_EmissiveColor", onColor * onIntensity);
            }
        }
        else
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].SetActive(false);
            }

            for (int i = 0; i < lightMaps.Length; i++)
            {
                lightMaps[i].material.SetColor("_EmissiveColor", offColor);
            }
        }
    }
}
