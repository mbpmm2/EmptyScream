using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class UIFadeScreen : MonoBehaviour
{
    public float alphaValueSpeed;
    public GameObject finishPanel;
    public GameObject startPanel;

    public CanvasGroup canvas;
    private bool fadeInFinish = true;
    private bool fadeOutFinish = true;
    private float alphaValue;
    private FirstPersonController player;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Get())
        {
            player = GameManager.Get().playerGO.GetComponent<FirstPersonController>();
        }

        canvas.gameObject.SetActive(false);
        startPanel.SetActive(false);
        finishPanel.SetActive(false);
        StartFadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        if(!fadeInFinish)
        {
            UpdateFadeIn();
        }
        else if(!fadeOutFinish)
        {
            UpdateFadeOut();
        }
    }

    public void UpdateFadeIn()
    {
        if (!fadeInFinish)
        {
            alphaValue += Time.deltaTime * alphaValueSpeed;
            if (alphaValue >= 1)
            {
                startPanel.SetActive(false);
                player.GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(false);
                player.GetComponent<FirstPersonController>().enabled = false;
                alphaValue = 1;
                fadeInFinish = true;
                finishPanel.SetActive(true);
            }

            canvas.alpha = alphaValue;
        }

    }

    public void UpdateFadeOut()
    {
        if (!fadeOutFinish)
        {
            alphaValue -= Time.deltaTime * alphaValueSpeed;
            if (alphaValue <= 0)
            {
                player.GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(true);
                player.GetComponent<FirstPersonController>().enabled = true;
                alphaValue = 0;
                fadeOutFinish = true;
                startPanel.SetActive(true);
                canvas.gameObject.SetActive(false);
            }

            canvas.alpha = alphaValue;
        }

    }

    public void StartFadeIn()
    {
        canvas.gameObject.SetActive(true);
        finishPanel.SetActive(false);
        fadeInFinish = false;
        alphaValue = 0;
        canvas.alpha = alphaValue;
        player.GetComponent<FirstPersonController>().enabled = false;
    }

    public void StartFadeOut()
    {
        canvas.gameObject.SetActive(true);
        startPanel.SetActive(false);
        fadeOutFinish = false;
        alphaValue = 1;
        canvas.alpha = alphaValue;
        player.GetComponent<FirstPersonController>().enabled = false;
    }
}
