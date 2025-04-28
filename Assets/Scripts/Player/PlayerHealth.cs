using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{

    [Header("Health settings")]
    [SerializeField]
    private float maxHealth = 100;
    private float currentHealth;

    [Header("HealthBar")]
    [SerializeField]
    private HealthSlider healthSlider;

    [Header("Damage Overlay")]
    public Image damageOverlay;
    public float duration = 2f;
    public float overlayMaxAlpha = 0.4f;

    private float currentAlpha = 0f;

    void Start()
    {
        currentHealth = maxHealth;

        UpdateHealthUI();
        ResetOverlay();
    }

    void Update()
    {
        HandleOverlayFade();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        ShowDamageOverlay();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Time.timeScale = 0f;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if(healthSlider != null)
        {
            healthSlider.SetMaxHealth(maxHealth);
            healthSlider.SetHealth(currentHealth);
        }

    }

    private void ShowDamageOverlay()
    {
        currentAlpha = overlayMaxAlpha;
        UpdateOverlayColor();
    }

    private void HandleOverlayFade()
    {
        if(currentAlpha > 0)
        {
            currentAlpha -= Time.deltaTime / duration;
            UpdateOverlayColor();
        }
    }

    private void UpdateOverlayColor()
    {
        if(damageOverlay != null)
        {
            Color color = damageOverlay.color;
            color.a = currentAlpha;
            damageOverlay.color = color;
        }
    }

    private void ResetOverlay()
    {
        if(damageOverlay != null)
        {
            Color color = damageOverlay.color;
            color.a = 0f;
            damageOverlay.color = color;
        } 
    }
}
