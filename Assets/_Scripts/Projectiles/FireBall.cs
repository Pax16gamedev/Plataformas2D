using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] float fireForce = 8;
    [SerializeField] float lifeSpan = 3;
    [SerializeField] float damage = 30;
    [SerializeField] float knockbackForce = 30;

    [SerializeField] int maxBounces = 3;

    private int currentBounces = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb.AddForce(transform.right * fireForce, ForceMode2D.Impulse);
        Invoke(nameof(TriggerExplosion), lifeSpan);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(Constants.TAGS.GROUND))
        {
            currentBounces++;
            if(currentBounces >= maxBounces)
            {
                TriggerExplosion();
            }
        }

        if(collision.gameObject.CompareTag(Constants.TAGS.PLAYER_HITBOX))
        {
            collision.gameObject.GetComponent<HealthSystem>()?.TakeDamage(damage);
            TriggerExplosion();
            //ApplyKnockback(collision); // TODO: Revisar
        }
    }

    private void TriggerExplosion()
    {
        rb.velocity = Vector3.zero;
        rb.gravityScale = 0;

        animator.SetTrigger(Constants.ANIMATIONS.FIREBALL.EXPLODE_TRIGGER);
    }

    private void ApplyKnockback(Collision2D collision)
    {
        Rigidbody2D playerRb = collision.gameObject.GetComponentInParent<Rigidbody2D>();
        if(playerRb == null) return;

        // Determinar dirección del knockback (desde el objeto que colisiona)
        Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
        knockbackDirection = new Vector2(knockbackDirection.x, 0);

        // Invertimos la dirección para empujar en la direccion opuesta
        knockbackDirection = -knockbackDirection;

        // Aplicar la fuerza de knockback en la direccion opuesta
        playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        Debug.Log($"Knockback aplicado en dirección: {knockbackDirection}");
    }

    // Se ejecuta desde un evento de animacion
    private void DestroyFireBall()
    {
        Destroy(gameObject);
    }
}
