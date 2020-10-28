using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class UIPlayerStatus : MonoBehaviour
{

    [Header("Player Health Status Config")]
    public Gradient healthBarGradient;
    public GameObject defenseIcon;
    public float finePercentage;
    public float cautionPercentage;
    public float dangerPercentage;

    [Header("Components Assign Health")]
    public Slider healthBar;
    public Image fillBar;
    public UIBloodEffect blood;
    public Volume postProcess;
    private ColorAdjustments color;

    private Player player;
    private Image defenseImage;
    private bool canUpdateImmunityIcon;

    // Start is called before the first frame update
    void Start()
    {
        Player.OnDefenseTimerON += UpdateFillAmount;
        Player.OnPlayerChangeHP += CheckStatus;
        Player.OnDefenseStart += ActivateDefenseIcon;
        Player.OnDefenseStop += DeactivateDefenseIcon;
        player = GameManager.Get().playerGO.GetComponent<Player>();

        DeactivateDefenseIcon();

        ColorAdjustments colorAdjust;
        if (postProcess.profile.TryGet<ColorAdjustments>(out colorAdjust))
        {
            color = colorAdjust;
        }

        defenseImage = defenseIcon.GetComponent<Image>();

        healthBar.maxValue = player.health;
        healthBar.value = player.health;

        fillBar.color = healthBarGradient.Evaluate(1f);
    }

    private void UpdateFillAmount(float amount, float maxAmount)
    {
        defenseImage.fillAmount = 1 - (amount /maxAmount);
    }

    public void CheckStatus(float hp)
    {
        healthBar.value = hp;
        fillBar.color = healthBarGradient.Evaluate(healthBar.normalizedValue);

        if (hp / player.maxHealth <= (finePercentage * 0.01f) && hp / player.maxHealth > (cautionPercentage * 0.01f))
        {
            blood.DeactivateMask();
            color.saturation.value = 0.0f;
        }
        if (hp / player.maxHealth <= (cautionPercentage * 0.01f) && hp / player.maxHealth > (dangerPercentage * 0.01f))
        {
            blood.DeactivateMask();
            color.saturation.value = -20.0f;
        }
        if (hp / player.maxHealth <= (dangerPercentage * 0.01f))
        {
            blood.ActivateMask();
            color.saturation.value = -60.0f;
        }
    }

    private void ActivateDefenseIcon()
    {
        defenseIcon.SetActive(true);
    }

    private void DeactivateDefenseIcon()
    {
        defenseIcon.SetActive(false);
    }

    private void OnDestroy()
    {
        Player.OnDefenseTimerON -= UpdateFillAmount;
        Player.OnPlayerChangeHP -= CheckStatus;
        Player.OnDefenseStart -= ActivateDefenseIcon;
        Player.OnDefenseStop -= DeactivateDefenseIcon;
    }
}
