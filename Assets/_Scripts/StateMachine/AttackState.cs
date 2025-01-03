using UnityEngine;

public class AttackState : State<EnemyController>
{
    private Transform target; // Doy por hecho que es el jugador o le vuelvo a hacer un ontrigger

    [SerializeField] float attackDistance = 1.5f; // Igual que stoppingDistance de ChaseState
    [SerializeField] float timeBetweenAttacks;

    private float timer;

    public override void OnEnterState(EnemyController enemyController)
    {
        base.OnEnterState(enemyController);
        timer = timeBetweenAttacks;
    }

    public override void OnUpdateState()
    {
        timer += Time.deltaTime;
        if(timer > timeBetweenAttacks)
        {
            print($"Ataque de {name}!");
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
        if(collision.TryGetComponent<Player>(out Player player))
        {
            target = player.transform;
        }
    }

}
