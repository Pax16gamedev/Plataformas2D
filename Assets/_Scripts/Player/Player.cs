using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerCombat playerCombatSystem;

    private void Awake()
    {
        playerCombatSystem = GetComponentInChildren<PlayerCombat>();
    }

    public void AttackEvent()
    {
        playerCombatSystem.Attack();
    }
}
