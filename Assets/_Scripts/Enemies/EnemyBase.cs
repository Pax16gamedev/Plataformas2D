using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float bodyDamage = 20;
    [SerializeField] protected float patrolSpeed = 2;

    protected HealthSystem healthSystem;
    private DamageFeedback damageFeedback;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        damageFeedback = GetComponent<DamageFeedback>();
    }

    private void TriggerVisualFeedback(float dmg)
    {
        print($"Daño recibido {dmg}");
        damageFeedback.TriggerFeedback();
    }

    private void OnEnable()
    {
        healthSystem.OnDamage += TriggerVisualFeedback;
    }

    private void OnDestroy()
    {
        healthSystem.OnDamage -= TriggerVisualFeedback;
    }
}
