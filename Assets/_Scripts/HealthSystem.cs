using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public event Action<float> OnDamage;
    public event Action<HealthSystem> OnDeath;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnDamage?.Invoke(damage);

        if(currentHealth <= 0)
        {
            OnDeath?.Invoke(this);
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}
