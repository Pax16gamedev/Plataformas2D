using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private Animator animator;

    private PatrolState patrolState;
    private AttackState attackState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        patrolState = GetComponent<PatrolState>();
        attackState = GetComponent<AttackState>();
    }

    private void OnEnable()
    {
        //patrolState.OnPatrolStateExit += TriggerAttackAnimation;
        attackState.OnAttack += TriggerAttackAnimation;
    }

    private void OnDisable()
    {
        //patrolState.OnPatrolStateExit -= TriggerAttackAnimation;
        attackState.OnAttack += TriggerAttackAnimation;
    }

    // Se ejecuta desde un evento de animacion
    private void TriggerAttackAnimation()
    {
        animator.SetTrigger(Constants.ANIMATIONS.BAT.ATTACK_BOOL);
    }

    private void OnDestroy()
    {
        GameManager.Instance.IncreaseMonstersKilled();
    }
}
