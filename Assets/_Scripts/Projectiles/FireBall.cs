using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] float fireForce = 8;
    [SerializeField] float lifeSpan = 3;
    [SerializeField] float damage = 30;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
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
        }
    }

    private void TriggerExplosion()
    {
        animator.SetTrigger(Constants.ANIMATIONS.FIREBALL.EXPLODE_TRIGGER);
    }

    // Se ejecuta desde un evento de animacion
    private void DestroyFireBall()
    {
        Destroy(gameObject);
    }
}