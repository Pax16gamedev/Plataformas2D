using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerMovement player;
    Animator anim;

    private void Awake()
    {
        player = GetComponentInParent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    public void Run(float movementInput)
    {
        FlipCharacter(movementInput);
        if(movementInput != 0)
        {
            anim.SetBool(Constants.ANIMATIONS.PLAYER.RUNNING_BOOL, true);
        }
        else
        {
            anim.SetBool(Constants.ANIMATIONS.PLAYER.RUNNING_BOOL, false);
        }
    }

    private void FlipCharacter(float movementInput)
    {
        if(movementInput > 0) // Derecha
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(movementInput < 0) // Izquierda
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public void Jump()
    {
        anim.SetTrigger(Constants.ANIMATIONS.PLAYER.JUMP_TRIGGER);
    }

    public void TriggerAttack()
    {
        anim.SetTrigger(Constants.ANIMATIONS.PLAYER.ATTACK_TRIGGER);
    }

    public void Attack() => player.Attack();
}
