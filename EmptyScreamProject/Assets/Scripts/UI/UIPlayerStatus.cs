using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class UIPlayerStatus : MonoBehaviour
{
    public delegate void OnPlayerStatusAction(int tier);
    public static OnPlayerStatusAction OnPlayerChangeStatus;

    [System.Serializable]
    public struct HealthValue
    {
        public Vector2[] healthValue;
    }

    [Header("General Config")]
    public float[] readingSpeed;

    [Header("Player Health Status Config")]
    public HealthValue[] healthValues;
    public Color[] colorHealthStatus;
    public GameObject[] healthStatusImages;

    [Header("Player Sanity Status Config")]
    public GameObject immunityIcon;
    public Color[] colorSanityStatus;
    public GameObject[] sanityStatusImages;

    [Header("Components Assign Health")]
    public UIBloodEffect blood;
    public UIScrollingTexture scroll;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI bpmText;
    public TextMeshProUGUI hpText;
    public Volume postProcess;
    private ColorAdjustments color;

    [Header("Components Assign Sanity")]
    public UIScrollingTexture scroll2;
    public TextMeshProUGUI sanityText;
    public TextMeshProUGUI tierText;

    public int lastIndex;
    public int lastIndexSanity;

    // Start is called before the first frame update
    void Start()
    {
        Player.OnPlayerChangeHP += CheckStatus;
        Player.OnPlayerChangeSanity += UpdateSanityText;
        Player.OnPlayerChangeSanityTier += ApplyNewSanityStatus;
        Player.OnImmunityStart += ActivateImmunityIcon;
        Player.OnImmunityStop += DeactivateImmunityIcon;
        lastIndex = -1;
        lastIndexSanity = -1;
        for (int i = 0; i < healthStatusImages.Length; i++)
        {
            healthStatusImages[i].SetActive(false);
            sanityStatusImages[i].SetActive(false);
        }
        
        ApplyNewStatus(0);
        ApplyNewSanityStatus(0);
        DeactivateImmunityIcon();

        ColorAdjustments colorAdjust;
        if (postProcess.profile.TryGet<ColorAdjustments>(out colorAdjust))
        {
            color = colorAdjust;
        }
    }

    public void CheckStatus(float hp)
    {
        healthText.text = "" + (int)hp;

        if(hp <= healthValues[lastIndexSanity].healthValue[0].y && hp >= healthValues[lastIndexSanity].healthValue[0].x)
        {
            ApplyNewStatus(0);
            blood.DeactivateMask();
            color.saturation.value = 0.0f;
        }
        if(hp <= healthValues[lastIndexSanity].healthValue[1].y && hp >= healthValues[lastIndexSanity].healthValue[1].x)
        {
            ApplyNewStatus(1);
            blood.DeactivateMask();
            color.saturation.value = -20.0f;
        }
        if(hp <= healthValues[lastIndexSanity].healthValue[2].y && hp >= healthValues[lastIndexSanity].healthValue[2].x)
        {
            ApplyNewStatus(2);
            blood.ActivateMask();
            color.saturation.value = -60.0f;
        }
    }

    private void UpdateSanityText(float sanity)
    {
        sanityText.text = "" + sanity;
    }

    public void ApplyNewStatus(int index)
    {
        if(lastIndex != index)
        {
            if(lastIndex >= 0)
            {
                healthStatusImages[lastIndex].SetActive(false);
            }
            
            healthStatusImages[index].SetActive(true);

            for (int i = 0; i < healthStatusImages[index].transform.childCount; i++)
            {
                healthStatusImages[index].transform.GetChild(i).GetComponent<Image>().color = colorHealthStatus[index];
            }

            healthText.color = colorHealthStatus[index];
            bpmText.color = colorHealthStatus[index];
            hpText.color = colorHealthStatus[index];
            scroll.speed = readingSpeed[index];
            scroll.currentImage = healthStatusImages[index].GetComponent<RectTransform>();
            lastIndex = index;

        }
    }

    public void ApplyNewSanityStatus(float index)
    {
        if (lastIndexSanity != (int)index)
        {
            if (lastIndexSanity >= 0)
            {
                sanityStatusImages[lastIndexSanity].SetActive(false);
            }

            sanityStatusImages[(int)index].SetActive(true);

            for (int i = 0; i < sanityStatusImages[(int)index].transform.childCount; i++)
            {
                sanityStatusImages[(int)index].transform.GetChild(i).GetComponent<Image>().color = colorSanityStatus[(int)index];
            }

            sanityText.color = colorSanityStatus[(int)index];
            tierText.color = colorSanityStatus[(int)index];
            scroll2.speed = readingSpeed[(int)index];
            scroll2.currentImage = sanityStatusImages[(int)index].GetComponent<RectTransform>();
            tierText.text = "Tier " + ((int)index+1);

            lastIndexSanity = (int)index;

            if (OnPlayerChangeStatus != null)
            {
                OnPlayerChangeStatus(lastIndexSanity);
            }
        }
    }

    private void ActivateImmunityIcon()
    {
        immunityIcon.SetActive(true);
    }

    private void DeactivateImmunityIcon()
    {
        immunityIcon.SetActive(false);
    }

    private void OnDestroy()
    {
        Player.OnPlayerChangeHP -= CheckStatus;
        Player.OnPlayerChangeSanity -= UpdateSanityText;
        Player.OnPlayerChangeSanityTier -= ApplyNewSanityStatus;
        Player.OnImmunityStart -= ActivateImmunityIcon;
        Player.OnImmunityStop -= DeactivateImmunityIcon;
    }
}
