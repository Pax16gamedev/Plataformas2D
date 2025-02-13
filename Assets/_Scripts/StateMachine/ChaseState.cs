using UnityEngine;

public class ChaseState : State<EnemyController>
{
    private Transform target; // Doy por hecho que es el jugador o le vuelvo a hacer un ontrigger

    [SerializeField] float chaseSpeed = 3;
    [SerializeField] float stoppingDistance = 1.5f;

    public override void OnEnterState(EnemyController enemyController)
    {
        base.OnEnterState(enemyController);
    }

    public override void OnUpdateState()
    {
        if(!target) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, target.position) <= stoppingDistance)
        {
            controller.ChangeState(controller.AttackState);
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
            LookAt(target);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if(collision.TryGetComponent<Player>(out Player player))
        //{
        //    controller.ChangeState(controller.PatrolState);
        //}

        if(collision.CompareTag(Constants.TAGS.PLAYER_DETECTION))
        {
            controller.ChangeState(controller.PatrolState);
        }
    }

    private void LookAt(Transform target)
    {
        if(target.position.x > transform.position.x) // Derecha
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(target.position.x < transform.position.x) // Izquierda
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
    }
}
