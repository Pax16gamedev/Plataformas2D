using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAnimation playerAnimation;

    [Header("Movement system")]
    [SerializeField] float speed = 12f;
    [SerializeField] float jumpForce = 30f;
    [SerializeField] float distanceToTheGround = 1.15f;
    [SerializeField] Transform groundDetection;
    [SerializeField] LayerMask isJumpable;

    [Header("Combat system")]
    [SerializeField] float damage = 20;
    [SerializeField] float attackRadius;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask isDamageable;

    private float inputHorizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    void Update()
    {
        GetInputs();
        Move();
    }

    void GetInputs()
    {
        inputHorizontal = Input.GetAxisRaw(Constants.INPUTS.HORIZONTAL);

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
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

    private bool IsGrounded()
    {
        
        return Physics2D.Raycast(groundDetection.position, Vector2.down, distanceToTheGround, isJumpable);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPoint.position, attackRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundDetection.position, Vector3.down * distanceToTheGround);
    }
}
