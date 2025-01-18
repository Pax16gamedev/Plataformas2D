using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    Player player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
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
            player.transform.eulerAngles = Vector3.zero;
        }
        else if(movementInput < 0) // Izquierda
        {
            player.transform.eulerAngles = new Vector2(0, 180);
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

    public void Dash()
    {
        //print("Dashing animation!");
    }

    // Se ejecuta desde un evento de animacion
    public void AttackEvent() => player.AttackEvent();
}
