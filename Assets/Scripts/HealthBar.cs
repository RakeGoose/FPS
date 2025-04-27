using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthSlider : MonoBehaviour
{

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI healthText;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        UpdateHealthText(health);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        UpdateHealthText(health);
    }

    private void UpdateHealthText(float health)
    {
        healthText.text = $"{health} / {slider.maxValue}";
    }
}
