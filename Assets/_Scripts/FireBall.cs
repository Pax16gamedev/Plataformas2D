using UnityEngine;

public class FireBall : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float fireForce = 8;
    [SerializeField] float lifeSpan = 3;
    [SerializeField] float damage = 30;

    [SerializeField] int maxBounces = 3;

    private int currentBounces = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.AddForce(transform.right * fireForce, ForceMode2D.Impulse);
        Destroy(gameObject, lifeSpan);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.TAGS.PLAYER_HITBOX))
        {
            collision.GetComponent<HealthSystem>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(Constants.TAGS.GROUND))
        {
            currentBounces++;
            if(currentBounces >= maxBounces)
            {
                Destroy(gameObject);
            }
        }
    }
}
