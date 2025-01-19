using System;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    public event Action OnAttack;

    private Transform target; // Doy por hecho que es el jugador o le vuelvo a hacer un ontrigger

    [Header("Distance")]
    [SerializeField] float attackDistance = 1.5f; // Igual que stoppingDistance de ChaseState
    [SerializeField] float timeBetweenAttacks;

    [Header("Charge")]
    [SerializeField] private float chargeSpeed = 8f;
    [SerializeField] private float returnSpeed = 4f;
    [SerializeField] private float chargeDistance = 1.5f;

    [Header("Damage")]
    [SerializeField] private float attackDamage = 25;

    private float timer;

    private Vector2 initialPosition;
    private bool isCharging;
    private bool isReturning;

    public override void OnEnterState(EnemyController enemyController)
    {
        base.OnEnterState(enemyController);
        timer = timeBetweenAttacks;

        initialPosition = transform.position;
        isCharging = false;
        isReturning = false;
    }

    public override void OnUpdateState()
    {
        if(target == null) return;

        //timer += Time.deltaTime;
        //if(timer > timeBetweenAttacks)
        //{
        //    print($"Ataque de {name} desde machineState!");
        //    TriggerAttack();
        //    OnAttack?.Invoke();
        //    timer = 0;
        //}

        if(!isCharging && !isReturning)
        {
            timer += Time.deltaTime;
        }

        if(isCharging)
        {
            ChargeTowardsTarget();
        }
        else if(isReturning)
        {
            ReturnToInitialPosition();
        }
        else if(timer > timeBetweenAttacks)
        {
            StartCharge();
            timer = 0;
        }

        if(Vector3.Distance(transform.position, target.position) > attackDistance)
        {
            controller.ChangeState(controller.ChaseState);
        }
    }

    public override void OnExitState()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.TryGetComponent<Player>(out Player player))
        //{
        //    target = player.transform;
        //}

        if(collision.CompareTag(Constants.TAGS.PLAYER_DETECTION))
        {
            target = collision.gameObject.transform;
        }

        if(collision.gameObject.CompareTag(Constants.TAGS.PLAYER_HITBOX))
        {
            collision.GetComponent<HealthSystem>()?.TakeDamage(attackDamage);
        }
    }

    private void StartCharge()
    {
        if(target == null) return;

        isCharging = true;
        TriggerAttack();
    }

    private void ChargeTowardsTarget()
    {

        // Movimiento hacia el jugador
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * chargeSpeed * Time.deltaTime;

        // Finaliza la carga si alcanza la distancia o el objetivo
        if(Vector3.Distance(transform.position, target.position) <= chargeDistance)
        {
            isCharging = false;
            isReturning = true;
        }
    }

    private void TriggerAttack()
    {
        controller.Animator.SetTrigger(Constants.ANIMATIONS.BAT.ATTACK_BOOL);
    }

    private void ReturnToInitialPosition()
    {
        Vector3 direction = (initialPosition - (Vector2)transform.position).normalized;
        transform.position += direction * returnSpeed * Time.deltaTime;

        // Finaliza el regreso si alcanza la posición inicial
        if(Vector3.Distance(transform.position, initialPosition) < 0.1f)
        {
            isReturning = false;
        }
    }

}
