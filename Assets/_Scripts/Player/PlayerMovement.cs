using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAnimation playerAnimation;

    [Header("Movement system")]
    [SerializeField] float speed = 12f;
    [SerializeField] float jumpSpeed = 12f;

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

        CheckForJumpConditions();
        GetMovementInputs();        
    }

    void GetMovementInputs()
    {
        inputHorizontal = Input.GetAxisRaw(Constants.INPUTS.HORIZONTAL);

        Move();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            TriggerJump();
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

    private void OnDrawGizmos()
    {
        Vector3 endPosition = groundDetection.position + Vector3.down * distanceToTheGround;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundDetection.position, endPosition);
    }
}
