using System.Collections;
using UnityEngine;

public class Wizard : EnemyBase
{
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform attackSpawnPoint;
    [SerializeField] float timeBetweenAttacks = 2;

    private Animator animator;
    private Transform target;

    private void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!target) return;

        LookAt(target);
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

    private void StartAttacking()
    {
        StartCoroutine(TriggerAttack());
    }

    private void StopAttacking()
    {
        StopCoroutine(TriggerAttack());
    }

    IEnumerator TriggerAttack()
    {
        while(true)
        {
            animator.SetTrigger(Constants.ANIMATIONS.WIZARD.ATTACK_TRIGGER);
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    // Se llama desde un evento de animacion
    private void ThrowFireball()
    {
        Instantiate(fireballPrefab, attackSpawnPoint.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.TAGS.PLAYER_DETECTION))
        {
            StartAttacking();
            target = collision.GetComponentInParent<Player>().transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.TAGS.PLAYER_DETECTION))
        {
            StopAttacking();
            target = null;
        }
    }
}
