using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float health;

    public float Health => health;

    public event Action OnDeath;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            OnDeath?.Invoke();
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}
