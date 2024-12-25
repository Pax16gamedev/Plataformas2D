using System.Collections;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform attackSpawnPoint;
    [SerializeField] float timeBetweenAttacks = 2;
    [SerializeField] float bodyDamage = 20;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(TriggerAttack());
    }

    IEnumerator TriggerAttack()
    {
        while (true)
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
}
