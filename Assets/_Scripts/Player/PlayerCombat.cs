using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat system")]
    [SerializeField] float damage = 20;
    [SerializeField] float attackRadius = 0.5f;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask isDamageable;

    private PlayerAnimation playerAnimation;
    private VisualDamageFeedback damageFeedback;
    private HealthSystem healthSystem;

    private void Awake()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        damageFeedback = GetComponentInChildren<VisualDamageFeedback>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            TriggerAttack();
        }

        if(Input.GetMouseButtonDown(1))
        {
            healthSystem.TakeDamage(1);
        }
    }

    private void TriggerAttack()
    {
        playerAnimation.TriggerAttack();
    }

    // Se ejecuta desde un evento de animacion
    public void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, isDamageable);
        foreach(Collider2D collider in colliders)
        {
            collider.GetComponent<HealthSystem>()?.TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si mi ataque atraviesa a un enemigo
        if(collision.CompareTag(Constants.TAGS.ENEMY))
        {
            collision.GetComponent<HealthSystem>()?.TakeDamage(damage);
        }
    }

    private void TriggerVisualFeedback(float dmg)
    {
        print($"Daño recibido {dmg}");
        damageFeedback.TriggerFeedback();
        CameraShakeManager.Instance.TriggerShake();
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
