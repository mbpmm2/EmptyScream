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

    public Image infectedScreen;
    public int currentIndex;
    public TraumaInducer screenShake;

    // Start is called before the first frame update
    void Start()
    {
        Syringe.OnSyringeUse += StartVeinsScreen;
        currentIndex = -1;

        infectedScreen.color = new Vector4(infectedScreen.color.r, infectedScreen.color.g, infectedScreen.color.b, 0);
        DeactivateMask();
        ActivateMask(0);
        isVignetteActivated = false;
        setFadeIn = false;
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
                infectedScreen.color = new Vector4(infectedScreen.color.r, infectedScreen.color.g, infectedScreen.color.b, vignetteValue);
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
                infectedScreen.color = new Vector4(infectedScreen.color.r, infectedScreen.color.g, infectedScreen.color.b, vignetteValue);
                if (vignetteValue <= minFadeValue)
                {
                    vignetteValue = minFadeValue;
                    setFadeOut = false;
                    startWaitingTimer = true;
                    isVignetteActivated = false;
                }
            }

            if (startWaitingTimer)
            {
                waitingTimer += Time.deltaTime;

                if (waitingTimer >= waitingTime)
                {
                    //setFadeIn = true;
                    waitingTimer = 0;
                    startWaitingTimer = false;
                    
                }
            }

        }
    }

    public void StartVeinsScreen(ItemCore.ItemType type)
    {
        isVignetteActivated = true;
        setFadeIn = true;
        setFadeOut = false;
        StartCoroutine(screenShake.Shake());
    }

    public void ActivateMask(int index)
    {
        if (index != currentIndex)
        {
            DeactivateMask();
            currentIndex = index;
            StartCoroutine(screenShake.Shake());
        }
    }

    public void DeactivateMask()
    {
        if(currentIndex >= 0)
        {
            infectedScreen.color = new Vector4(infectedScreen.color.r, infectedScreen.color.g, infectedScreen.color.b, 0);
        }
    }

    private void OnDestroy()
    {
        Syringe.OnSyringeUse -= StartVeinsScreen;
    }
}
