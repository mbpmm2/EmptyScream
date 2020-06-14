using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfectedScreen : MonoBehaviour
{
    public float vignetteFadeSpeed;
    public float maxFadeValue;
    public float minFadeValue;
    public float waitingTime;

    public float waitingTimer;
    public float vignetteValue;
    public bool isVignetteActivated;
    public bool setFadeIn;
    public bool setFadeOut;
    public bool startWaitingTimer;

    public Image[] infectedScreens;
    public int currentIndex;
    public TraumaInducer screenShake;
    //public int lastIndex;

    // Start is called before the first frame update
    void Start()
    {
        //screenShake = GetComponent<TraumaInducer>();
        UIPlayerStatus.OnPlayerChangeStatus += ActivateMask;
        //isVignetteActivated = false;
        //lastIndex = -1;
        currentIndex = -1;

        for (int i = 0; i < infectedScreens.Length; i++)
        {
            infectedScreens[i].color = new Vector4(infectedScreens[i].color.r, infectedScreens[i].color.g, infectedScreens[i].color.b, 0);
        }

        DeactivateMask();
        ActivateMask(0);
        isVignetteActivated = true;
        setFadeIn = true;
        setFadeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isVignetteActivated)
        {
            if (setFadeIn)
            {
                vignetteValue += vignetteFadeSpeed * Time.deltaTime;
                infectedScreens[currentIndex].color = new Vector4(infectedScreens[currentIndex].color.r, infectedScreens[currentIndex].color.g, infectedScreens[currentIndex].color.b, vignetteValue);
                if (vignetteValue >= maxFadeValue)
                {
                    vignetteValue = maxFadeValue;
                    setFadeIn = false;
                    setFadeOut = true;
                }
            }

            if (setFadeOut)
            {
                vignetteValue -= vignetteFadeSpeed * Time.deltaTime;
                infectedScreens[currentIndex].color = new Vector4(infectedScreens[currentIndex].color.r, infectedScreens[currentIndex].color.g, infectedScreens[currentIndex].color.b, vignetteValue);
                if (vignetteValue <= minFadeValue)
                {
                    vignetteValue = minFadeValue;
                    setFadeOut = false;
                    startWaitingTimer = true;
                }
            }

            if (startWaitingTimer)
            {
                waitingTimer += Time.deltaTime;

                if (waitingTimer >= waitingTime)
                {
                    setFadeIn = true;
                    waitingTimer = 0;
                    startWaitingTimer = false;
                    
                }
            }

        }
    }

    public void ActivateMask(int index)
    {
        Debug.Log("Infected Index : " + index);
        Debug.Log("Infected Current Index : " + currentIndex);
        if (index != currentIndex)
        {
            DeactivateMask();
            currentIndex = index;
            StartCoroutine(screenShake.Shake());
        }

       // isVignetteActivated = true;
        /*setFadeIn = true;
        setFadeOut = false;*/
    }

    public void DeactivateMask()
    {
        //isVignetteActivated = false;
        /*setFadeIn = false;
        setFadeOut = false;*/
        if(currentIndex >= 0)
        {
            infectedScreens[currentIndex].color = new Vector4(infectedScreens[currentIndex].color.r, infectedScreens[currentIndex].color.g, infectedScreens[currentIndex].color.b, 0);
        }
        
    }

    private void OnDestroy()
    {
        UIPlayerStatus.OnPlayerChangeStatus -= ActivateMask;
    }
}
