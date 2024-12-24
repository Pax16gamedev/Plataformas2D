using System.Collections;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float patrolSpeed = 2;

    [SerializeField] float bodyDamage = 20;

    private Vector3 currentDestination;
    private int currentIndexDestination = 0;

    void Start()
    {
        currentDestination = waypoints[currentIndexDestination].position;
        StartCoroutine(Patrol());
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentIndexDestination].position, patrolSpeed * Time.deltaTime);
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            while(transform.position != currentDestination)
            {
                Move();
                yield return null; // = yield return new WaitForEndOfFrame();
            }
            NextWaypoint();
        }
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
        else if(currentDestination.x > transform.position.x)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(Constants.TAGS.PLAYER_DETECTION))
        {
            print("Player detectado");
        }
        else if(collision.gameObject.CompareTag(Constants.TAGS.PLAYER_HITBOX))
        {
            collision.GetComponent<HealthSystem>().TakeDamage(bodyDamage);
        }
    }


}
