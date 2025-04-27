using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Header("Health settings")]
    [SerializeField]
    private float maxHealth = 100;
    private float currentHealth;

    [Header("UI")]
    [SerializeField]
    private HealthSlider healthSlider;


    void Start()
    {
        currentHealth = maxHealth;

        if(healthSlider != null)
        {
            healthSlider.SetMaxHealth(maxHealth);
        }
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if(healthSlider != null)
        {
            healthSlider.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if(healthSlider != null)
        {
            healthSlider.SetHealth(currentHealth);
        }
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
