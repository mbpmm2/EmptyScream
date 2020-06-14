using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBloodEffect : MonoBehaviour
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

    public Image criticalHealthVignette;
    public UIBloodSplat[] bloodsplats;
    public List<int> bloodsplatsUsed;

    // Start is called before the first frame update
    void Start()
    {
        Player.OnPlayerHurt += SetRandomBloodSplat;
        isVignetteActivated = false;
        DeactivateMask();
    }

    // Update is called once per frame
    void Update()
    {
        if(isVignetteActivated)
        {
            if(setFadeIn)
            {
                vignetteValue += vignetteFadeSpeed * Time.deltaTime;
                criticalHealthVignette.color = new Vector4(criticalHealthVignette.color.r, criticalHealthVignette.color.g, criticalHealthVignette.color.b, vignetteValue);
                if(vignetteValue >= maxFadeValue)
                {
                    vignetteValue = maxFadeValue;
                    setFadeIn = false;
                    setFadeOut = true;
                }
            }

            if(setFadeOut)
            {
                vignetteValue -= vignetteFadeSpeed * Time.deltaTime;
                criticalHealthVignette.color = new Vector4(criticalHealthVignette.color.r, criticalHealthVignette.color.g, criticalHealthVignette.color.b, vignetteValue);
                if (vignetteValue <= minFadeValue)
                {
                    vignetteValue = minFadeValue;
                    setFadeOut = false;
                    startWaitingTimer = true;
                }
            }

            if(startWaitingTimer)
            {
                waitingTimer += Time.deltaTime;

                if(waitingTimer >= waitingTime)
                {
                    setFadeIn = true;
                    waitingTimer = 0;
                    startWaitingTimer = false;
                }
            }
            
        }
    }

    public void SetRandomBloodSplat()
    {
        for (int i = 0; i < bloodsplats.Length; i++)
        {
            if(!bloodsplats[i].IsBloodActive())
            {
                bloodsplatsUsed.Add(i);
            }
        }

        bloodsplatsUsed.Sort();

        if(bloodsplatsUsed.Count > 0)
        {
            int newIndex = Random.Range(bloodsplatsUsed[0], bloodsplatsUsed[bloodsplatsUsed.Count - 1] + 1);

            bloodsplats[newIndex].Activate();
        }

        bloodsplatsUsed.Clear();
    }

    public void ActivateMask()
    {
        isVignetteActivated = true;
        setFadeIn = true;
        setFadeOut = false;
    }

    public void DeactivateMask()
    {
        isVignetteActivated = false;
        setFadeIn = false;
        setFadeOut = false;
        criticalHealthVignette.color = new Vector4(criticalHealthVignette.color.r, criticalHealthVignette.color.g, criticalHealthVignette.color.b, 0);
    }

    private void OnDestroy()
    {
        Player.OnPlayerHurt -= SetRandomBloodSplat;
    }
}
