using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBloodSplat : MonoBehaviour
{
    public float activeTime;
    public float fadeSpeed;
    public float fadeSpeedOut;

    private bool isActive;
    private bool canFadeIn;
    private bool canFadeOut;
    private float activeTimer;

    private Image currentImage;
    private float currentAlpha;

    // Start is called before the first frame update
    void Start()
    {
        currentImage = GetComponent<Image>();
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            if (canFadeOut)
            {
                currentAlpha -= Time.deltaTime * fadeSpeed;
                currentImage.color = new Vector4(currentImage.color.r, currentImage.color.g, currentImage.color.b, currentAlpha);

                if (currentImage.color.a <= 0)
                {
                    currentAlpha = 0;
                    currentImage.color = new Vector4(currentImage.color.r, currentImage.color.g, currentImage.color.b, currentAlpha);
                    Deactivate();
                }
            }
            else
            {
                activeTimer += Time.deltaTime;

                if (activeTimer >= activeTime)
                {
                    activeTimer = 0;
                    canFadeOut = true;
                }
            }

            if (canFadeIn)
            {
                currentAlpha += Time.deltaTime * fadeSpeed;
                currentImage.color = new Vector4(currentImage.color.r, currentImage.color.g, currentImage.color.b, currentAlpha);

                if (currentImage.color.a >= 1)
                {
                    currentAlpha = 1;
                    currentImage.color = new Vector4(currentImage.color.r, currentImage.color.g, currentImage.color.b, currentAlpha);
                    canFadeIn = false;
                }
            }
        }
    }

    public void Activate()
    {
        isActive = true;
        canFadeIn = true;
    }

    public void Deactivate()
    {
        canFadeOut = false;
        canFadeIn = false;
        isActive = false;
        currentAlpha = 0;
        currentImage.color = new Vector4(currentImage.color.r, currentImage.color.g, currentImage.color.b, currentAlpha);
    }

    public bool IsBloodActive()
    {
        return isActive;
    }
}
