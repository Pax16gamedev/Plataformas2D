using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAnimation playerAnimation;
    private PlayerVisual playerVisual;

    [Header("Movement system")]
    [SerializeField] float speed = 12f;
    [SerializeField] float jumpSpeed = 12f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashCooldownTimer = 0;

    private float initialGravityScale;

    [Header("Ground")]
    [SerializeField] float jumpForce = 30f;
    [SerializeField] float distanceToTheGround = 0.15f;
    [SerializeField] Transform groundDetection;
    [SerializeField] LayerMask isJumpable;

    [Header("Air time")]
    [SerializeField] private float coyoteTime = 0.25f;
    private float timeInAir;

    [Header("Jump Time")]
    [SerializeField] private float maxJumpTime = 0.2f;
    private float jumpTime = 0;

    private int maxJumps = 1;
    private int jumps = 0;

    private float inputHorizontal;
    private int lastDirection = 1; // 1 derecha, -1 izquierda

    private bool canMove = true;
    public bool CanMove { get => canMove; set => canMove = value; }   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerVisual = GetComponentInChildren<PlayerVisual>();

        initialGravityScale = rb.gravityScale;
    }

    private void Start()
    {
        dashCooldownTimer = dashCooldown;
        UIManager.Instance.DashBar.SetMaxDashDuration(dashCooldown);
    }

    void Update()
    {
        if(!canMove) return;

        CheckForJumpConditions();
        GetMovementInputs();
        HandleDashCooldown();
    }

    void GetMovementInputs()
    {
        inputHorizontal = Input.GetAxisRaw(Constants.INPUTS.HORIZONTAL);

        CheckDirection();

        if(!isDashing)
        {
            Move();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            TriggerJump();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer >= dashCooldown)
        {
            TriggerDash();
        }

    }

    private void CheckDirection()
    {
        if(inputHorizontal < 0)
        {
            lastDirection = -1;
        }
        else if(inputHorizontal > 0)
        {
            lastDirection = 1;
        }
    }

    void Move()
    {
        rb.velocity = new Vector2 (inputHorizontal * speed, rb.velocity.y);
        playerAnimation.Run(inputHorizontal);
    }

    void TriggerJump()
    {
        if(jumps >= maxJumps) return; // Evita doble salto // 2: No lo evita

        if(IsGrounded() || timeInAir < coyoteTime)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); // Reinicia la velocidad vertical para evitar acumulaciones.
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        playerAnimation.Jump();

        jumps++;
    }

    void CheckForJumpConditions()
    {
        if(IsGrounded())
        {
            timeInAir = 0;
            jumpTime = 0;
            jumps = 0;
        }
        else
        {
            timeInAir += Time.deltaTime;
        }
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

    private void HandleDashCooldown()
    {
        if(dashCooldownTimer >= dashCooldown) return;

        dashCooldownTimer += Time.deltaTime;
        UIManager.Instance.DashBar.SetDash(dashCooldownTimer);
    }

    private void TriggerDash()
    {
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        dashCooldownTimer = 0;

        UIManager.Instance.DashBar.SetDash(0);

        // Desactivo la gravedad mientras haces el dash
        rb.gravityScale = 0;
        rb.velocity = new Vector2(lastDirection * dashSpeed, 0);

        playerAnimation.Dash();
        playerVisual.ActivateDashTrail();

        yield return new WaitForSeconds(dashDuration);

        playerVisual.DeactivateDashTrail();

        // Restauro la gravedad y permito el movimiento normal
        rb.gravityScale = initialGravityScale;
        isDashing = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 endPosition = groundDetection.position + Vector3.down * distanceToTheGround;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundDetection.position, endPosition);
    }
}
