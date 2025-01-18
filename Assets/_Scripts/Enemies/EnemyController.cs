using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;

    public Animator Animator => animator;

    private PatrolState patrolState;
    private ChaseState chaseState;
    private AttackState attackState;

    private State<EnemyController> currentState;

    public PatrolState PatrolState { get => patrolState; }
    public ChaseState ChaseState { get => chaseState; }
    public AttackState AttackState { get => attackState; }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        patrolState = GetComponent<PatrolState>();
        chaseState = GetComponent<ChaseState>();
        attackState = GetComponent<AttackState>();

        ChangeState(patrolState);
    }

    void Update()
    {
        if(!currentState) return;

        currentState.OnUpdateState();
    }

    public void ChangeState(State<EnemyController> newState)
    {
        if(currentState)
        {
            currentState.OnExitState();
        }

        currentState = newState;
        currentState.OnEnterState(this);
    }

    private void OnDestroy()
    {
        GameManager.Instance.IncreaseMonstersKilled();
    }
}
