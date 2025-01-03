using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected Transform[] waypoints;
    [SerializeField] protected float patrolSpeed = 2;

    [SerializeField] protected float bodyDamage = 20;

    private Vector3 currentDestination;
    private int currentIndexDestination = 0;

    protected HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    protected virtual void Start()
    {
        currentDestination = waypoints[currentIndexDestination].position;
        StartCoroutine(Patrol());
    }

    protected IEnumerator Patrol()
    {
        while(true)
        {
            while(transform.position != currentDestination)
            {
                Move();
                yield return null; // = yield return new WaitForEndOfFrame();
            }
            NextWaypoint();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentIndexDestination].position, patrolSpeed * Time.deltaTime);
    }

    private void NextWaypoint()
    {
        currentIndexDestination++;
        if(currentIndexDestination >= waypoints.Length)
        {
            currentIndexDestination = 0;
        }
        currentDestination = waypoints[currentIndexDestination].position;
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
}
