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

    private bool canAttack = true;

    public bool CanAttack { get => canAttack; set => canAttack = value; }

    private void Awake()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        damageFeedback = GetComponentInChildren<VisualDamageFeedback>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        UIManager.Instance.HealthBar.SetMaxHealth(healthSystem.MaxHealth);
    }

    private void Update()
    {
        if(!canAttack) return;

        if(Input.GetMouseButtonDown(0))
        {
            TriggerAttack();
        }

        if(Input.GetMouseButtonDown(1))
        {
            healthSystem.TakeDamage(10);
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
            HealthSystem enemyHealth = collider.GetComponent<HealthSystem>();

            if(enemyHealth == null)
            {
                Debug.LogWarning($"HealthSystem not found in {collider.name}");
                continue;
            }

            enemyHealth.TakeDamage(damage);

            var enemy = collider.GetComponent<EnemyBase>();
            if(enemy != null && !enemy.IsOnEnemyDeathSubscribed)
            {
                enemy.OnEnemyDeath += HandleEnemyDeath;
                enemy.IsOnEnemyDeathSubscribed = true;
            }
        }
    }

    private void HandleEnemyDeath(EnemyBase enemy)
    {
        GameManager.Instance.IncreaseScore(enemy.ScoreValue);
        enemy.OnEnemyDeath -= HandleEnemyDeath;
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
        damageFeedback.TriggerFeedback();
        CameraShakeManager.Instance.TriggerShake();
    }

    private void OnDamageTaken(float damage)
    {
        TriggerVisualFeedback(damage);
        UIManager.Instance.HealthBar.SetHealth(healthSystem.CurrentHealth);
    }

    private void OnEnable()
    {
        healthSystem.OnDamage += OnDamageTaken;
    }

    private void OnDestroy()
    {
        healthSystem.OnDamage -= OnDamageTaken;
    }

    
}
