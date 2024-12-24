using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAnimation playerAnimation;

    [Header("Movement settings")]
    [SerializeField] float speed = 12f;
    [SerializeField] float jumpForce = 30f;

    [Header("Combat system")]
    [SerializeField] float damage = 20;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask isDamageable;

    private float inputHorizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        GetInputs();
        Move();
    }

    void GetInputs()
    {
        inputHorizontal = Input.GetAxisRaw(Constants.INPUTS.HORIZONTAL);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if(Input.GetMouseButtonDown(0))
        {
            TriggerAttack();
        }
    }

    void Move()
    {
        rb.velocity = new Vector2 (inputHorizontal * speed, rb.velocity.y);
        playerAnimation.Run(inputHorizontal);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        playerAnimation.Jump();
    }

    void TriggerAttack()
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
}
