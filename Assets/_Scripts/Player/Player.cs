using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerCombat playerCombatSystem;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerCombatSystem = GetComponent<PlayerCombat>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void AttackEvent()
    {
        playerCombatSystem.Attack();
    }

    public void CanMove(bool canMove)
    {
        playerMovement.CanMove = canMove;
    }

    public void CanAttack(bool canAttack)
    {
        playerCombatSystem.CanAttack = canAttack;
    }
}
