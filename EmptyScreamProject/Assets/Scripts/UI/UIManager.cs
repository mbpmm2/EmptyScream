using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float idleTime;
    public float fadeSpeed;

    public float idleTimer;
    public float fadeTimer;
    public bool canFadeOut;
    public bool canFadeIn;

    public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        UIInventory.OnUIActive += RestartTimer;
        Player.OnPlayerHurt += RestartTimer;
        Player.OnPlayerChangeHP2 += RestartTimer;
        canvasGroup.alpha = 0;
        RestartTimer();
    }

    // Update is called once per frame
    void Update()
    {

        if(canFadeOut)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
            }
        }
        else
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTime)
            {
                idleTimer = 0;
                canFadeOut = true;
            }
        }

        if(canFadeIn)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed * 2;
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
                canFadeIn = false;
            }
        }

    }

    public void RestartTimer()
    {
        if(canvasGroup.alpha < 1)
        {
            canFadeIn = true;
        }
        idleTimer = 0;
        canFadeOut = false;
    }

    private void OnDestroy()
    {
        Player.OnPlayerHurt -= RestartTimer;
        UIInventory.OnUIActive -= RestartTimer;
        Player.OnPlayerChangeHP2 -= RestartTimer;
    }
}
