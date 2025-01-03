using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State<EnemyController>
{
    [SerializeField] protected Transform[] route;
    [SerializeField] protected float patrolSpeed = 2;

    private List<Vector3> waypoints = new List<Vector3>();

    private Vector3 currentDestination;
    private int currentIndexDestination = 0;

    public override void OnEnterState(EnemyController enemyController)
    {
        base.OnEnterState(enemyController);
        foreach(Transform t in route)
        {
            waypoints.Add(t.position);
        }

        currentDestination = waypoints[currentIndexDestination];
    }

    public override void OnUpdateState()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentIndexDestination], patrolSpeed * Time.deltaTime);
        if(transform.position == currentDestination)
        {
            NextWaypoint();
        }
    }

    public override void OnExitState()
    {
        waypoints.Clear();
        currentIndexDestination = 0;
    }

    private void NextWaypoint()
    {
        currentIndexDestination++;
        if(currentIndexDestination >= waypoints.Count)
        {
            currentIndexDestination = 0;
        }
        currentDestination = waypoints[currentIndexDestination];
        LookAtDestination();
    }

    private void LookAtDestination()
    {
        if(currentDestination.x > transform.position.x)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(currentDestination.x < transform.position.x)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.TAGS.PLAYER_DETECTION))
        {
            controller.ChangeState(controller.ChaseState);
        }

        //if(collision.TryGetComponent<Player>(out Player player))
        //{
        //    controller.ChangeState(controller.ChaseState);
        //}
    }
}
