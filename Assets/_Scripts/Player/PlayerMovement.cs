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

    private float inputHorizontal;
    private bool canMove = true;

    public bool CanMove { get => canMove; set => canMove = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    void Update()
    {
        if(!canMove) return;

        GetMovementInputs();
    }

    void GetMovementInputs()
    {
        inputHorizontal = Input.GetAxisRaw(Constants.INPUTS.HORIZONTAL);

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }

        Move();
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
        if(groundDetection == null)
        {
            Debug.LogWarning($"{nameof(groundDetection)} not asigned.");
            return false;
        }

        return Physics2D.Raycast(groundDetection.position, Vector2.down, distanceToTheGround, isJumpable);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 endPosition = groundDetection.position + Vector3.down * distanceToTheGround;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundDetection.position, endPosition);
    }
}
