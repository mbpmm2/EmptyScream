using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerStatus : MonoBehaviour
{
    [Header("Player Status Config")]
    public Vector2[] healthValue;
    public Color[] colorHealthStatus;
    public float[] readingSpeed;
    public GameObject[] healthStatusImages;

    [Header("Components Assign")]
    public UIBloodEffect blood;
    public UIScrollingTexture scroll;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI bpmText;
    public TextMeshProUGUI hpText;

    public int lastIndex;

    // Start is called before the first frame update
    void Start()
    {
        Player.OnPlayerChangeHP += CheckStatus;
        lastIndex = -1;
        for (int i = 0; i < healthStatusImages.Length; i++)
        {
            healthStatusImages[i].SetActive(false);
        }
        
        ApplyNewStatus(0);
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

    private void OnDestroy()
    {
        Player.OnPlayerChangeHP -= CheckStatus;
    }
}
