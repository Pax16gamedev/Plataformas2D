using System;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float bodyDamage = 20;
    [SerializeField] protected float patrolSpeed = 2;

    [Header("Puntuacion")]
    [SerializeField] protected int scoreValue = 100;
    public int ScoreValue => scoreValue;
    public bool IsOnEnemyDeathSubscribed { get; set; } = false; // Nueva propiedad para controlar suscripciones

    public event Action<EnemyBase> OnEnemyDeath;

    protected HealthSystem healthSystem;
    protected VisualDamageFeedback damageFeedback;

    protected void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        damageFeedback = GetComponent<VisualDamageFeedback>();
    }

    private void TriggerVisualFeedback(float dmg)
    {
        damageFeedback.TriggerFeedback();
    }

    private void OnEnable()
    {
        healthSystem.OnDamage += TriggerVisualFeedback;
    }

    private void OnDestroy()
    {
        healthSystem.OnDamage -= TriggerVisualFeedback;
        OnEnemyDeath?.Invoke(this);
        GameManager.Instance.IncreaseMonstersKilled();
    }
}
