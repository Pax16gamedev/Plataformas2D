using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerAnimation playerAnimation;

    [Header("Combat system")]
    [SerializeField] float damage = 20;
    [SerializeField] float attackRadius = 0.5f;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask isDamageable;

    private void Awake()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            TriggerAttack();
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
}
