using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaminaBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI staminaText;

    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
        UpdateStaminaText(stamina);
    }

    public void SetStamina(float stamina)
    {
        slider.value = stamina;
        UpdateStaminaText(stamina);
    }

    private void UpdateStaminaText(float stamina)
    {
        staminaText.text = $"{Mathf.CeilToInt(stamina)} / {Mathf.CeilToInt(slider.maxValue)}";
    }
}
