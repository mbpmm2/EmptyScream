using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerStatus : MonoBehaviour
{
    [Header("General Config")]
    public float[] readingSpeed;

    [Header("Player Health Status Config")]
    public Vector2[] healthValue;
    public Color[] colorHealthStatus;
    public GameObject[] healthStatusImages;

    [Header("Player Sanity Status Config")]
    public Color[] colorSanityStatus;
    public GameObject[] sanityStatusImages;

    [Header("Components Assign Health")]
    public UIBloodEffect blood;
    public UIScrollingTexture scroll;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI bpmText;
    public TextMeshProUGUI hpText;

    [Header("Components Assign Sanity")]
    //public UIBloodEffect blood;
    public UIScrollingTexture scroll2;
    public TextMeshProUGUI sanityText;
    //public TextMeshProUGUI bpmText;
    public TextMeshProUGUI tierText;

    public int lastIndex;
    public int lastIndexSanity;

    // Start is called before the first frame update
    void Start()
    {
        Player.OnPlayerChangeHP += CheckStatus;
        Player.OnPlayerChangeSanity += UpdateSanityText;
        Player.OnPlayerChangeSanityTier += ApplyNewSanityStatus;
        lastIndex = -1;
        lastIndexSanity = -1;
        for (int i = 0; i < healthStatusImages.Length; i++)
        {
            healthStatusImages[i].SetActive(false);
            sanityStatusImages[i].SetActive(false);
        }
        
        ApplyNewStatus(0);
        ApplyNewSanityStatus(0);
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    public void CheckStatus(float hp)
    {
        healthText.text = "" + (int)hp;

        //Debug.Log("Test HP: " + hp + "   " + healthValue[1].y);

        if(hp <= healthValue[0].y)
        {
            ApplyNewStatus(0);
        }
        if(hp <= healthValue[1].y)
        {
            ApplyNewStatus(1);
        }
        if(hp <= healthValue[2].y)
        {
            ApplyNewStatus(2);
            blood.ActivateMask();
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
                //healthStatusImages[lastIndex].SetActive(false);
                sanityStatusImages[lastIndexSanity].SetActive(false);
            }

            // healthStatusImages[index].SetActive(true);
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
        }
    }

    private void OnDestroy()
    {
        Player.OnPlayerChangeHP -= CheckStatus;
        Player.OnPlayerChangeSanity -= UpdateSanityText;
        Player.OnPlayerChangeSanityTier -= ApplyNewSanityStatus;
    }
}
